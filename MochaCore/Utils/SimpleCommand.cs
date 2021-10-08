using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MochaCore.Utils
{
    /// <summary>
    /// Provides simplified implementation of <see cref="ICommand"/> interface
    /// which can be executed always.
    /// </summary>
    public class SimpleCommand : ICommand
    {
        private Action<object?> _commandAction;
        
        /// <inheritdoc/>
        public event EventHandler? CanExecuteChanged;

        /// <inheritdoc/>
        public bool CanExecute(object? parameter) => true;

        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleCommand"/> class.
        /// </summary>
        /// <param name="commandAction">Delegate to be invoked when this command is executed.</param>
        public SimpleCommand(Action<object?> commandAction)
        {
            _commandAction = commandAction;
        }

        /// <inheritdoc/>
        public void Execute(object? parameter)
        {
            _commandAction?.Invoke(parameter);
        }
    }
}
