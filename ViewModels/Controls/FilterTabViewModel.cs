using MochaCore.Utils;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.Controls
{
    public class FilterTabViewModel : BindableBase
    {
        private string? _expression = "test";
        private DelegateCommand? _applyFilterCommand;

        public ControlNotifier Notifier { get; } = new ControlNotifier();

        public string? Expression
        {
            get => _expression;
            set => SetProperty(ref _expression, value);
        }

        public DelegateCommand ApplyFilterCommand => _applyFilterCommand ??= new DelegateCommand(ApplyFilter);

        private void ApplyFilter()
        {
            Notifier.InvokeCommand("FilterAppliedCommand", null);
        }

        private void InvokeFilterAppliedCommand()
        {
            FilterAppliedCommandInvoker = !FilterAppliedCommandInvoker;
        }

        private bool _filterAppliedCommandInvoker;

        public bool FilterAppliedCommandInvoker
        {
            get => _filterAppliedCommandInvoker;
            set => SetProperty(ref _filterAppliedCommandInvoker, value);
        }

    }
}
