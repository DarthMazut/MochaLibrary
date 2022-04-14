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
    public class FilterTabViewModel : BindableBase
    {
        private string _selectedFilter;
        private string? _expression = "test";
        private bool _matchWholeWords;
        private bool _containsExpression;
        private DelegateCommand? _applyFilterCommand;
        private DelegateCommand? _removeFilterCommand;

        public FilterTabViewModel()
        {
            SelectedFilter = FilterValues.First();
        }

        public ControlNotifier Notifier { get; } = new ControlNotifier();

        public string SelectedFilter
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

        public IList<string> FilterValues { get; } = Enum.GetNames(typeof(PersonFilterValue));

        public DelegateCommand ApplyFilterCommand => _applyFilterCommand ??= new DelegateCommand(ApplyFilter);

        public DelegateCommand RemoveFilterCommand => _removeFilterCommand ??= new DelegateCommand(RemoveFilter);

        private void RemoveFilter()
        {
            Notifier.SetDependencyPropertyValue("Filter", null);
            Notifier.InvokeCommand("FilterRemovedCommand", null);
        }

        private void ApplyFilter()
        {
            PersonFilter createdFilter = CreateFilter();
            Notifier.SetDependencyPropertyValue("Filter", createdFilter);
            Notifier.InvokeCommand("FilterAppliedCommand", createdFilter);
        }

        private PersonFilter CreateFilter()
        {
            return new PersonFilter()
            {
                FilterValue = Enum.Parse<PersonFilterValue>(SelectedFilter),
                Expression = Expression,
                MatchWholeWords = MatchWholeWords,
                Contains = ContainsExpression
            };
        }
    }
}
