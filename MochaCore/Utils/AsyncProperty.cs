using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MochaCore.Dispatching;

#nullable disable
namespace MochaCore.Utils
{
    /// <summary>
    /// Represents a property backing field with the ability to start an asynchronous 
    /// operation whenever its value changes. It also allows to set an asynchronous callback
    /// which will be executed after async operation completes successfully.
    /// This class is <see cref="INotifyPropertyChanged"/> compatible.
    /// </summary>
    /// <typeparam name="T">Type of corresponding property.</typeparam>
    public class AsyncProperty<T> : IDisposable
    {
        private delegate ref MulticastDelegate GetEventFieldValue(object obj);

        private readonly INotifyPropertyChanged _host;
        private readonly string _propertyName;

        private T _initialValue;
        private T _internalValue;

        private GetEventFieldValue _multicastDelegateGetter;
        private CancellationTokenSource _cts;

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncProperty{T}"/> class.
        /// </summary>
        /// <param name="host">Host of this property. Pass <see langword="this"/> here.</param>
        /// <param name="propertyName">Name of corresponding property.</param>
        public AsyncProperty(INotifyPropertyChanged host, string propertyName)
        {
            _host = host;
            _propertyName = propertyName;

            host.PropertyChanged += (s, e) =>
            {
                if (SynchronizedProperties.Any())
                {
                    if (SynchronizedProperties.Contains(e.PropertyName))
                    {
                        if (e.PropertyName == _propertyName)
                        {
                            throw new InvalidOperationException("You cannot synchronize property with itself!");
                        }
                        NotifyPropertyChangeViaReflection();
                    }
                }
            };
        }

        /// <summary>
        /// Specifies how long it will take before <see cref="CancellationToken"/> within 
        /// <see cref="PropertyChangedOperation"/> is automatically cancelled. 
        /// </summary>
        public TimeSpan PropertyChangedOperationTimeout { get; init; } = Timeout.InfiniteTimeSpan;

        /// <summary>
        /// Fires whenever <see cref="AsyncOperation"/> is cancelled successfully.
        /// </summary>
        public event EventHandler<AsyncPropertyChangedEventArgs<T>> AsyncOperationCancelled;

        /// <summary>
        /// Returns an initial value for represented property.
        /// </summary>
        public T InitialValue
        {
            get => _initialValue;
            init
            {
                _initialValue = value;
                _internalValue = value;
            }
        }

        /// <summary>
        /// Allows to define an operation that is executed whenever value of this object changes.
        /// Provided <see cref="CancellationToken"/> will be cancelled when new value has been assigned
        /// to this object, before currently running <see cref="PropertyChangedOperation"/> is completed.
        /// </summary>
        public Func<CancellationToken, AsyncPropertyChangedEventArgs<T>, Task> PropertyChangedOperation { get; init; }

        /// <summary>
        /// Provides names of properties which this <see cref="AsyncProperty{T}"/> is synchronized with.
        /// When an <see cref="AsyncProperty{T}"/> is synchronized with another property it gets updated 
        /// (<see cref="INotifyPropertyChanged.PropertyChanged"/>) whenever that property changes.
        /// </summary>
        public ICollection<string> SynchronizedProperties { get; } = new List<string>();

        /// <summary>
        /// Returns value of this object.
        /// </summary>
        public T Get()
        {
            return _internalValue;
        }

        /// <summary>
        /// Sets a new value for this object. If <see cref="PropertyChangedOperation"/> is defined it will be 
        /// executed after new value is set. If async operation is currently running it will be requested to cancel. 
        /// </summary>
        /// <param name="value">New value for this object.</param>
        public bool Set(T value)
        {
            if (EqualityComparer<T>.Default.Equals(value, _internalValue))
            {
                return false;
            }

            T previousValue = _internalValue;
            _internalValue = value;

            NotifyPropertyChangeViaReflection();

            if (PropertyChangedOperation is not null)
            {
                CancelAsyncOperation();
                _cts = new CancellationTokenSource();
                _ = HandlePropertyChangedOperation(_cts, new AsyncPropertyChangedEventArgs<T>(_host, previousValue, value));
            }

            return true;
        }

        /// <summary>
        /// Tries to cancel currently active asynchronous operation initiated by this <see cref="AsyncProperty{T}"/>.
        /// </summary>
        public void CancelAsyncOperation()
        {
            try
            {
                _cts?.Cancel();
            }
            catch (ObjectDisposedException) { }
        }

        /// <summary>
        /// Most of the time <see cref="AsyncProperty{T}"/> object will reside inside your VM.
        /// Whenever you no longer need this VM (eg. you navigating to other page, closing a dialog etc.)
        /// you should call <see cref="Dispose"/> on all of your VM's <see cref="AsyncProperty{T}"/> objects.
        /// </summary>
        public void Dispose()
        {
            CancelAsyncOperation();
            GC.SuppressFinalize(this);
        }

        private async Task HandlePropertyChangedOperation(CancellationTokenSource cts, AsyncPropertyChangedEventArgs<T> e)
        {
            try
            {
                if (AsyncOperationCancelled is not null)
                {
                    cts.Token.Register(() => AsyncOperationCancelled.Invoke(this, e));
                }

                List<Task> tasks = new() { PropertyChangedOperation.Invoke(cts.Token, e) };
                CancellationTokenSource timeoutCts = new();
                if (PropertyChangedOperationTimeout != Timeout.InfiniteTimeSpan)
                {
                    tasks.Add(Task.Delay(PropertyChangedOperationTimeout, timeoutCts.Token));
                }

                Task firstCompleted = await Task.WhenAny(tasks);
                if (firstCompleted != tasks.First())
                {
                    e.MarkTimedOut();
                }

                cts.Cancel();
                timeoutCts.Cancel();

                await tasks.First();
            }
            finally
            {
                cts.Dispose();
            }
        }

        private void NotifyPropertyChangeViaReflection()
        {
            _multicastDelegateGetter ??= CreateMulticastDelegateGetter();

            MulticastDelegate inpcDelegate = _multicastDelegateGetter.Invoke(_host);
            if (inpcDelegate is not null)
            {
                (inpcDelegate as PropertyChangedEventHandler).Invoke(_host, new PropertyChangedEventArgs(_propertyName));
            }
        }

        private GetEventFieldValue CreateMulticastDelegateGetter()
        {
            const BindingFlags bf = BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly;
            Type hostType = _host.GetType();

            FieldInfo fi = GetPropertyChangedFiled(hostType);
            _ = fi ?? throw new MissingFieldException(hostType.Name, nameof(INotifyPropertyChanged.PropertyChanged));

            string s_name = "__refget_" + hostType.Name + "_fi_" + fi.Name;

            DynamicMethod dm = new(s_name, typeof(MulticastDelegate), new[] { typeof(object) }, hostType, true);
            dm.GetType().GetField("m_returnType", bf).SetValue(dm, typeof(MulticastDelegate).MakeByRefType());

            var il = dm.GetILGenerator();
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldflda, fi);
            il.Emit(OpCodes.Ret);

            return (GetEventFieldValue)dm.CreateDelegate(typeof(GetEventFieldValue));
        }

        private FieldInfo GetPropertyChangedFiled(Type t)
        {
            Type currentType = t;
            FieldInfo fieldInfo = null;

            while (fieldInfo is null && currentType is not null)
            {
                fieldInfo = currentType.GetField(nameof(INotifyPropertyChanged.PropertyChanged), BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);
                currentType = currentType.BaseType;
            }

            return fieldInfo;
        }

    }
}
