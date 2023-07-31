
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Animation;
using MochaCore.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MochaWinUI.Utils
{
    /// <summary>
    /// Provides an easy way for a <see cref="Control"/> to listen and execute action requested by 
    /// <see cref="IBindingTargetController.ControlRequested"/> event.
    /// </summary>
    public static class BindingControl
    {
        /// <summary>
        /// Registers a default handler for <see cref="IBindingTargetController.ControlRequested"/> event.
        /// By default the <see cref="FrameworkElement.DataContextProperty"/> is expected to provide
        /// <see cref="IBindingTargetController"/> implementation.
        /// </summary>
        /// <typeparam name="T">The type of subscribing control.</typeparam>
        /// <param name="host">
        /// A <see cref="Control"/> that listens to and executes actions requested by <see cref="IBindingTargetController"/> instance.
        /// </param>
        /// <returns>
        /// An <see cref="IDisposable"/> instance that represents the subscription to the 
        /// <see cref="IBindingTargetController.ControlRequested"/> event. Dispose this 
        /// instance to unregister the control from listening to control requests.
        /// </returns>
        public static IDisposable RegisterContextProperty<T>(T host) where T : Control
            => RegisterContextProperty(host, Control.DataContextProperty);

        /// <summary>
        /// Registers a default handler for <see cref="IBindingTargetController.ControlRequested"/> event.
        /// </summary>
        /// <typeparam name="T">The type of subscribing control.</typeparam>
        /// <param name="host">
        /// A <see cref="Control"/> that listens to and executes actions requested by <see cref="IBindingTargetController"/> instance.
        /// </param>
        /// <param name="property">
        /// A dependency property which references implementation of <see cref="IBindingTargetController"/>.
        /// </param>
        /// <returns>
        /// An <see cref="IDisposable"/> instance that represents the subscription to the 
        /// <see cref="IBindingTargetController.ControlRequested"/> event. Dispose this 
        /// instance to unregister the control from listening to control requests.
        /// </returns>
        public static IDisposable RegisterContextProperty<T>(T host, DependencyProperty property) where T : Control
        {
            long subscriptionId = host.RegisterPropertyChangedCallback(property, (s, dp) =>
            {
                if (host.GetValue(dp) is IBindingTargetController controller)
                {
                    controller.ControlRequested += (s, e) =>
                    {
                        if (e.RequestType == BindingTargetControlRequestType.SetDependencyProperty)
                        {
                            DependencyProperty? dp = GetDependencyPropertyByName(e.PropertyName!, typeof(T));
                            if (dp is not null)
                            {
                                host.SetValue(dp, e.PropertyValue);
                            }

                            return;
                        }

                        if (e.RequestType == BindingTargetControlRequestType.InvokeCommand)
                        {
                            DependencyProperty? dp = GetDependencyPropertyByName(e.CommandName!, typeof(T));
                            if (dp is not null)
                            {
                                if (host.GetValue(dp) is ICommand command)
                                {
                                    command.Execute(e.CommandParameter);
                                }
                            }
  
                            return;
                        }

                        if (e.RequestType == BindingTargetControlRequestType.PlayAnimation)
                        {
                            _ = host.Resources.TryGetValue(e.AnimationName, out object? foundElement);
                            foundElement ??= host.FindName(e.AnimationName);

                            if (foundElement is Storyboard storyboard)
                            {
                                storyboard.Begin();
                            }

                            return;
                        }

                        if (e.RequestType == BindingTargetControlRequestType.SetVisualState)
                        {
                            VisualStateManager.GoToState(host, e.VisualStateName, false);
                        }
                    };
                }
            });

            return new BindingControlSubscription(host, property, subscriptionId);
        }

        /// <summary>
        /// Registers a custom handler for <see cref="IBindingTargetController.ControlRequested"/> event.
        /// </summary>
        /// <typeparam name="T">The type of subscribing control.</typeparam>
        /// <param name="host">
        /// A <see cref="Control"/> that listens to and executes actions requested by <see cref="IBindingTargetController"/> instance.
        /// </param>
        /// <param name="property">
        /// A dependency property which references implementation of <see cref="IBindingTargetController"/>.
        /// </param>
        /// <param name="customHandler">Handler for <see cref="IBindingTargetController.ControlRequested"/> event.</param>
        /// <returns>
        /// An <see cref="IDisposable"/> instance that represents the subscription to the 
        /// <see cref="IBindingTargetController.ControlRequested"/> event. Dispose this 
        /// instance to unregister the control from listening to control requests.
        /// </returns>
        public static IDisposable RegisterContextProperty<T>(T host, DependencyProperty property, EventHandler<BindingTargetControlRequestedEventArgs> customHandler) where T : Control
        {
            long subscriptionId = host.RegisterPropertyChangedCallback(property, (s, dp) =>
            {
                if (host.GetValue(dp) is IBindingTargetController controller)
                {
                    controller.ControlRequested += (s, e) =>
                    {
                        customHandler?.Invoke(s, e);
                    };
                }
            });

            return new BindingControlSubscription(host, property, subscriptionId);
        }

        /// <summary>
        /// Retrieve a dependency property by its name using reflection
        /// </summary>
        /// <param name="propertyName">Dependency property name (without *Property* suffix).</param>
        /// <param name="ownerType">Type of property host.</param>
        public static DependencyProperty? GetDependencyPropertyByName(string propertyName, Type ownerType)
        {
            FieldInfo? fieldInfo = ownerType.GetField(propertyName + "Property", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
            return fieldInfo?.GetValue(null) as DependencyProperty;
        }

        private class BindingControlSubscription : IDisposable
        {
            private readonly FrameworkElement _host;
            private readonly DependencyProperty _dp;
            private readonly long _subscriptionId;

            public BindingControlSubscription(FrameworkElement host, DependencyProperty dp, long subscriptionId)
            {
                _host = host;
                _dp = dp;
                _subscriptionId = subscriptionId;
            }

            public void Dispose()
            {
                _host.UnregisterPropertyChangedCallback(_dp, _subscriptionId);
            }
        }
    }
}
