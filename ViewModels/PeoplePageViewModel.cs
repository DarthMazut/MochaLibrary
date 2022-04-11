using MochaCore.Navigation;
using Model;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.Controls;

namespace ViewModels
{
    public class PeoplePageViewModel : BindableBase, INavigatable, IOnNavigatedTo
    {
        private bool _isLoadingPeople;
        private bool _isPeopleListEmpty;
        private bool _isFilterOpen;
        private ObservableCollection<Person> _people = new();
        private PersonFilter? _currentFilter;

        private List<Person> _modelData = new()
        {
            new Person("John", "Doe", "Calafiornia", new DateTime(1999, 6, 6), "xxx"),
            new Person("Jane", "Doe", "Santa Monica", new DateTime(2001, 2, 16), "yyy"),
            new Person("Dupa", "Dupa", "Dupa", new DateTime(1995, 1, 17), "zzz")
        };

        private DelegateCommand<Person> _removePersonCommand;
        private DelegateCommand _arrangePeopleCommand;
        private DelegateCommand _addPerson;
        private DelegateCommand _openFilterCommand;
        private DelegateCommand _applyFilterCommand;


        public PeoplePageViewModel()
        {
            Navigator = new(this, NavigationServices.MainNavigationService);
            _people.CollectionChanged += (s, e) => IsPeopleListEmpty = !People.Any();
        }

        public Navigator Navigator { get; }

        public FilterTabViewModel FilterTabViewModel { get; } = new FilterTabViewModel();

        public bool IsLoadingPeople 
        {
            get => _isLoadingPeople;
            set => SetProperty(ref _isLoadingPeople, value);
        }

        public ObservableCollection<Person> People
        {
            get => _people;
            set => SetProperty(ref _people, value, () => IsPeopleListEmpty = !People.Any());
        }

        public bool IsPeopleListEmpty
        {
            get => _isPeopleListEmpty;
            set => SetProperty(ref _isPeopleListEmpty, value);
        }

        public bool IsFilterOpen 
        {
            get => _isFilterOpen;
            set => SetProperty(ref _isFilterOpen, value);
        }

        public PersonFilter? CurrentFilter
        {
            get => _currentFilter;
            set => SetProperty(ref _currentFilter, value);
        }

        public DelegateCommand<Person> RemovePersonCommand => _removePersonCommand ?? (_removePersonCommand = new DelegateCommand<Person>(RemovePerson));

        public DelegateCommand ArrangePeopleCommand => _arrangePeopleCommand ?? (_arrangePeopleCommand = new DelegateCommand(ArrangePeople));

        public DelegateCommand AddPersonCommand => _addPerson ?? (_addPerson = new DelegateCommand(AddPerson));

        public DelegateCommand OpenFilterCommand => _openFilterCommand ??= new DelegateCommand(() => IsFilterOpen = true);

        public DelegateCommand ApplyFilterCommand => _applyFilterCommand ??= new DelegateCommand(() =>
        {

        });

        public void OnNavigatedTo(NavigationData navigationData)
        {
            IsLoadingPeople = true;

            foreach (Person person in _modelData)
            {
                People.Add(person);
            }

            IsLoadingPeople = false;
        }

        private void ArrangePeople()
        {
            var orderedList = People.OrderBy(p => p.FirstName).ToList();
            People = new(orderedList);
        }

        private void RemovePerson(Person person)
        {
            People.Remove(person);
        }

        private void AddPerson()
        {
            People.Add(new Person("Dick"));
        }
    }
}
