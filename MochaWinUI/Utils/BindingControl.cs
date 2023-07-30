
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Animation;
using MochaCore.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MochaWinUI.Utils
{
    /// <summary>
    /// Allows to setup <see cref="DependencyObject"/> to listen for <see cref="IBindingTargetController"/>
    /// requests.
    /// </summary>
    public static class BindingControl
    {
        public static void RegisterContextProperty<T>(T host) where T : FrameworkElement
            => RegisterContextProperty(host, FrameworkElement.DataContextProperty);

        public static void RegisterContextProperty<T>(T host, DependencyProperty property) where T : FrameworkElement
        {
            host.RegisterPropertyChangedCallback(property, (s, dp) =>
            {
                if (host.GetValue(dp) is IBindingTargetController controller)
                {
                    controller.BindingTargetControlRequested += (s, e) =>
                    {
                        if (e.RequestType == BindingTargetControlRequestType.PlayAnimation)
                        {
                            object? foundElement = null;
                            foundElement = host.Resources.TryGetValue(e.TargetName, out foundElement);
                            foundElement ??= host.FindName(e.TargetName);

                            if (foundElement is Storyboard storyboard)
                            {
                                storyboard.Begin();
                            }
                        }

                        DependencyProperty? dp = GetDependencyPropertyByName(e.TargetName, typeof(T));
                        if (dp is null)
                        {
                            return;
                        }

                        if (e.RequestType == BindingTargetControlRequestType.SetDependencyProperty)
                        {
                            host.SetValue(dp, e.Parameter);
                            return;
                        }

                        if (e.RequestType == BindingTargetControlRequestType.InvokeCommand)
                        {
                            object? value = host.GetValue(dp);
                            if (value is ICommand command)
                            {
                                command.Execute(e.Parameter);
                            }
                            return;
                        }
                    };
                }
            });
        }

        public static void RegisterContextProperty(DependencyObject host, DependencyProperty property, EventHandler<BindingTargetControlRequestedEventArgs> customHandler)
        {
            host.RegisterPropertyChangedCallback(property, (s, dp) =>
            {
                if (host.GetValue(dp) is IBindingTargetController controller)
                {
                    controller.BindingTargetControlRequested += (s, e) =>
                    {
                        customHandler?.Invoke(s, e);
                    };
                }
            });
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
    }
}
