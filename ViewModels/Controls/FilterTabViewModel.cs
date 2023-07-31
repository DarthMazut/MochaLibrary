using MochaCore.Utils;
using Model;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.Controls
{
    public class FilterTabViewModel : BindableBase, IBindingTargetController
    {
        private PersonFilterValue _selectedFilter;
        private string? _expression = "test";
        private bool _matchWholeWords;
        private bool _containsExpression;
        private DelegateCommand? _applyFilterCommand;
        private DelegateCommand? _removeFilterCommand;

        public event EventHandler<BindingTargetControlRequestedEventArgs>? ControlRequested;

        public FilterTabViewModel()
        {
            SelectedFilter = FilterValues.First();
        }

        public PersonFilterValue SelectedFilter
        {
            get => _selectedFilter;
            set => SetProperty(ref _selectedFilter, value);
        }

        public string? Expression
        {
            get => _expression;
            set => SetProperty(ref _expression, value);
        }

        public bool MatchWholeWords 
        {
            get => _matchWholeWords;
            set => SetProperty(ref _matchWholeWords, value); 
        }

        public bool ContainsExpression
        {
            get => _containsExpression;
            set => SetProperty(ref _containsExpression, value);
        }

        public IList<PersonFilterValue> FilterValues { get; } = ((PersonFilterValue[])Enum.GetValues(typeof(PersonFilterValue))).ToList();

        public DelegateCommand ApplyFilterCommand => _applyFilterCommand ??= new DelegateCommand(ApplyFilter);

        public DelegateCommand RemoveFilterCommand => _removeFilterCommand ??= new DelegateCommand(RemoveFilter);

        private void RemoveFilter()
        {
            ControlRequested?.Invoke(this, BindingTargetControlRequestedEventArgs.SetProperty("Filter", null));
            ControlRequested?.Invoke(this, BindingTargetControlRequestedEventArgs.InvokeCommand("FilterRemovedCommand"));
        }

        private void ApplyFilter()
        {
            PersonFilter createdFilter = CreateFilter();
            ControlRequested?.Invoke(this, BindingTargetControlRequestedEventArgs.SetProperty("Filter", createdFilter));
            ControlRequested?.Invoke(this, BindingTargetControlRequestedEventArgs.InvokeCommand("FilterAppliedCommand", createdFilter));
        }

        private PersonFilter CreateFilter()
        {
            return new PersonFilter()
            {
                FilterValue = SelectedFilter,
                Expression = Expression,
                MatchWholeWords = MatchWholeWords,
                Contains = ContainsExpression
            };
        }
    }
}
