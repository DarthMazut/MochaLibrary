using System;

namespace MochaCore.Utils
{
    /// <summary>
    /// Provides arguments for <see cref="IBindingTargetController.ControlRequested"/> event.
    /// </summary>
    public class BindingTargetControlRequestedEventArgs : EventArgs
    {
        protected BindingTargetControlRequestedEventArgs(BindingTargetControlRequestType requestType, string? propertyName, object? propertyValue, string? commandName, object? commandParameter, string? animationName, string? visualStateName, object? extraData)
        {
            RequestType = requestType;
            PropertyName = propertyName;
            PropertyValue = propertyValue;
            CommandName = commandName;
            CommandParameter = commandParameter;
            AnimationName = animationName;
            VisualStateName = visualStateName;
            ExtraData = extraData;
        }

        /// <summary>
        /// Creates an instance of <see cref="BindingTargetControlRequestedEventArgs"/> for 
        /// <see cref="BindingTargetControlRequestType.SetDependencyProperty"/> request type.
        /// </summary>
        /// <param name="propertyName">Name of dependency property to be changed</param>
        /// <param name="newValue">New value for changing dependency proeprty.</param>
        public static BindingTargetControlRequestedEventArgs ChangeProperty(string propertyName, object? newValue)
            => new(BindingTargetControlRequestType.SetDependencyProperty, propertyName, newValue, null, null, null, null, null);

        /// <summary>
        /// Creates an instance of <see cref="BindingTargetControlRequestedEventArgs"/> for 
        /// <see cref="BindingTargetControlRequestType.InvokeCommand"/> request type.
        /// </summary>
        /// <param name="commandName">Name of command to be invoked.</param>
        public static BindingTargetControlRequestedEventArgs InvokeCommand(string commandName)
            => new(BindingTargetControlRequestType.InvokeCommand, null, null, commandName, null, null, null, null);

        // TODO

        /// <summary>
        /// The type of control request.
        /// </summary>
        public BindingTargetControlRequestType RequestType { get; }

        /// <summary>
        /// Name of dependency property to be changed.
        /// </summary>
        public string? PropertyName { get; }

        /// <summary>
        /// New value for changing dependency property.
        /// </summary>
        public object? PropertyValue { get; }

        /// <summary>
        /// Name of command to be invoked.
        /// </summary>
        public string? CommandName { get; }

        /// <summary>
        /// Parameter of command to be invoked.
        /// </summary>
        public object? CommandParameter { get; }

        /// <summary>
        /// Name of storyboard to be played. This can be either resource key or element name.
        /// </summary>
        public string? AnimationName { get; }

        /// <summary>
        /// Name of target visual state.
        /// </summary>
        public string? VisualStateName { get; }

        /// <summary>
        /// An extra data object which can be used be  UI's elemnt custom handler 
        /// of <see cref="IBindingTargetController.ControlRequested"/>
        /// </summary>
        public object? ExtraData { get; }
    }

    /// <summary>
    /// Describes possible actions which can be requested by <see cref="IBindingTargetController"/> implementation.
    /// </summary>
    public enum BindingTargetControlRequestType
    {
        /// <summary>
        /// Request for changing the value of a specified dependency property of UI object 
        /// which subscribes to <see cref="IBindingTargetController.ControlRequested"/>.
        /// </summary>
        SetDependencyProperty,

        /// <summary>
        /// Request for invoking a command implementation of UI object which subscribes to 
        /// <see cref="IBindingTargetController.ControlRequested"/>.
        /// </summary>
        InvokeCommand,

        /// <summary>
        /// Request for starting an animation's storyboard of UI object which subscribes to 
        /// <see cref="IBindingTargetController.ControlRequested"/>.
        /// </summary>
        PlayAnimation,

        /// <summary>
        /// Request for setting a visual state of UI object which subscribes to 
        /// <see cref="IBindingTargetController.ControlRequested"/>.
        /// </summary>
        SetVisualState
    }
}