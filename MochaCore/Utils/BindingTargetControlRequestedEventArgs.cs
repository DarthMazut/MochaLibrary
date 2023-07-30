using System;

namespace MochaCore.Utils
{
    /// <summary>
    /// Provides arguments for <see cref="IBindingTargetController.BindingTargetControlRequested"/> event.
    /// </summary>
    public class BindingTargetControlRequestedEventArgs : EventArgs
    {
        protected BindingTargetControlRequestedEventArgs(BindingTargetControlRequestType requestType, string targetName, object? parameter)
        {
            RequestType = requestType;
            TargetName = targetName;
            Parameter = parameter;
        }

        /// <summary>
        /// Creates a control request event argument for changing the value of a specified dependency property.
        /// </summary>
        /// <param name="propertyName">The name of the dependency property to be changed.</param>
        /// <param name="newValue">The new value to set for the dependency property.</param>
        public static BindingTargetControlRequestedEventArgs SetProperty(string propertyName, object? newValue)
            => new BindingTargetControlRequestedEventArgs(BindingTargetControlRequestType.SetDependencyProperty, propertyName, newValue);

        /// <summary>
        /// Creates a control request event argument for invoking a command implementation.
        /// </summary>
        /// <param name="commandName">The name of the command to be invoked.</param>
        public static BindingTargetControlRequestedEventArgs InvokeCommand(string commandName)
            => new BindingTargetControlRequestedEventArgs(BindingTargetControlRequestType.InvokeCommand, commandName, null);

        /// <summary>
        /// Creates a control request event argument for invoking a command implementation.
        /// </summary>
        /// <param name="commandName">The name of the command to be invoked.</param>
        /// <param name="commandParameter">The parameter to pass to the command implementation.</param>
        public static BindingTargetControlRequestedEventArgs InvokeCommand(string commandName, object? commandParameter)
            => new BindingTargetControlRequestedEventArgs(BindingTargetControlRequestType.InvokeCommand, commandName, commandParameter);

        /// <summary>
        /// Creates a control request event argument for starting an animation's storyboard.
        /// </summary>
        /// <param name="animationName">The name of the animation to be played.</param>
        public static BindingTargetControlRequestedEventArgs PlayAnimation(string animationName)
            => new BindingTargetControlRequestedEventArgs(BindingTargetControlRequestType.PlayAnimation, animationName, null);

        /// <summary>
        /// Type of control request.
        /// </summary>
        public BindingTargetControlRequestType RequestType { get; }

        /// <summary>
        /// The name of the element targeted by the request.
        /// </summary>
        public string TargetName { get; }

        /// <summary>
        /// Parameter of control request.
        /// </summary>
        public object? Parameter { get; }
    }

    /// <summary>
    /// Describes the type of control request.
    /// </summary>
    public enum BindingTargetControlRequestType
    {
        /// <summary>
        /// Request for changing the value of a specified dependency property.
        /// </summary>
        SetDependencyProperty,

        /// <summary>
        /// Request for invoking a command implementation.
        /// </summary>
        InvokeCommand,

        /// <summary>
        /// Request for starting an animation's storyboard.
        /// </summary>
        PlayAnimation
    }
}