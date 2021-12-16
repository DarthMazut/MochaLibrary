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
    public class DynamicProperty<T>
    {
        private delegate ref MulticastDelegate GetEventFieldValue(object obj);
        
        private readonly SemaphoreSlim _semaphore = new(1);
        private readonly string _propertyName;
        
        private T _initialValue;
        private T _internalValue;
        private INotifyPropertyChanged _host;
        private GetEventFieldValue _multicastDelegateGetter;
        private CancellationTokenSource _cts;

        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicProperty{T}"/> class.
        /// </summary>
        /// <param name="propertyName">Name of linked property.</param>
        public DynamicProperty(string propertyName)
        {
            _propertyName = propertyName;
        }

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
        /// Provides a delegate which is invoked whenever value of this object changes.
        /// </summary>
        public Action<DynamicPropertyChangedEventArgs<T>> PropertyChangedCallback { get; init; }

        /// <summary>
        /// Provides a definition of asynchronous operation initiated whenever value of this object changes.
        /// </summary>
        public Func<CancellationToken, DynamicPropertyChangedEventArgs<T>, Task<object>> AsyncOperation { get; init; }

        /// <summary>
        /// A delegate which is invoked after asynchronous operation is completed successfully.
        /// </summary>
        public Action<object, DynamicPropertyChangedEventArgs<T>> AsyncOperationCallback { get; init; }

        /// <summary>
        /// Gets or initializes <see cref="IDispatcher"/> associated with this <see cref="DynamicProperty{T}"/>.
        /// If this property is set (not null) the <see cref="AsyncOperationCallback"/> delegate is invoked via provided
        /// <see cref="IDispatcher"/> object.
        /// </summary>
        public IDispatcher Dispatcher { get; init; }

        /// <summary>
        /// Returns value of this object.
        /// </summary>
        public T Get()
        {
            return _internalValue;
        }

        /// <summary>
        /// Sets a new value for this object. 
        /// If <see cref="AsyncOperation"/> is defined (not null) it will be started after new value is set.
        /// If <see cref="PropertyChangedCallback"/> is defined it will be invoked after value changed.
        /// If <see cref="AsyncOperationCallback"/> is defined it will be executed after async operation is completed successfully.
        /// If async operation is currently running it will be cancelled and async callback won't be executed (if still possible).
        /// </summary>
        /// <param name="caller">Host of this property. <para>Pass <see langword="this"/> here.</para></param>
        /// <param name="value">New value fot this object.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public bool Set(INotifyPropertyChanged caller, T value)
        {
            _ = caller ?? throw new ArgumentNullException(nameof(caller));

            _semaphore.Wait();
            try
            {
                if (EqualityComparer<T>.Default.Equals(value, _internalValue))
                {
                    return false;
                }

                T previousValue = _internalValue;

                _host = caller;
                _internalValue = value;

                if (AsyncOperation is not null)
                {
                    CancelAsyncOperation();

                    _cts = new CancellationTokenSource();
                    _ = HandleAsyncOperation(_cts, previousValue, value);
                }

                NotifyPropertyChangeViaReflection();
                PropertyChangedCallback?.Invoke(new DynamicPropertyChangedEventArgs<T>(_host, previousValue, value));
            }
            finally
            {
               _semaphore.Release();
            }

            return true;
        }

        /// <summary>
        /// Tries to cancel currently active asynchronous operation initiated by this <see cref="DynamicProperty{T}"/>.
        /// </summary>
        public void CancelAsyncOperation()
        {
            try
            {
                _cts?.Cancel();
            }
            catch (ObjectDisposedException) { }
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

        private async Task HandleAsyncOperation(CancellationTokenSource cts, T previousValue, T newValue)
        {
            try
            {
                object result = await AsyncOperation.Invoke(cts.Token, new DynamicPropertyChangedEventArgs<T>(_host, previousValue, newValue));

                if (cts.IsCancellationRequested)
                {
                    return;
                }

                await _semaphore.WaitAsync();

                if (cts.IsCancellationRequested)
                {
                    cts.Dispose();
                    _semaphore.Release();
                    return;
                }

                if (Dispatcher is not null)
                {
                    Dispatcher.EnqueueOnMainThread(() =>
                    {
                        AsyncOperationCallback!.Invoke(result, new DynamicPropertyChangedEventArgs<T>(_host, previousValue, newValue));
                    });
                }
                else
                {
                    AsyncOperationCallback!.Invoke(result, new DynamicPropertyChangedEventArgs<T>(_host, previousValue, newValue));
                }
            }
            finally
            {
                cts.Dispose();
                _semaphore.Release();
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
