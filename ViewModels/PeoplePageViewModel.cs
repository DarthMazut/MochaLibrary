﻿using MochaCore.DialogsEx;
using MochaCore.DialogsEx.Extensions;
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
        private DelegateCommand<Person> _moreInfoCommand;
        private DelegateCommand _addPerson;
        private DelegateCommand _openFilterCommand;
        private DelegateCommand<PersonFilter> _applyFilterCommand;
        private DelegateCommand _filterRemovedCommand;


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

        public DelegateCommand<Person> MoreInfoCommand => _moreInfoCommand ?? (_moreInfoCommand = new DelegateCommand<Person>(MoreInfo));

        public DelegateCommand AddPersonCommand => _addPerson ?? (_addPerson = new DelegateCommand(AddPerson));

        public DelegateCommand OpenFilterCommand => _openFilterCommand ??= new DelegateCommand(() => IsFilterOpen = true);

        public DelegateCommand<PersonFilter> ApplyFilterCommand => _applyFilterCommand ??= new DelegateCommand<PersonFilter>(ApplyFilter);
        
        public DelegateCommand FilterRemovedCommand => _filterRemovedCommand ??= new DelegateCommand(RemoveFilter);

        public void OnNavigatedTo(NavigationData navigationData)
        {
            IsLoadingPeople = true;

            foreach (Person person in _modelData)
            {
                People.Add(person);
            }

            IsLoadingPeople = false;
        }

        private async void MoreInfo(Person person)
        {
            IDialogModule<StandardMessageDialogProperties> dialog = DialogManager.GetDialog<StandardMessageDialogProperties>("MoreInfoDialog");

            dialog.Properties.Title = $"Here you will see details for {person.FullName} contact...";
            dialog.Properties.Message = "But currently this feature is under construction ;)";
            dialog.Properties.ConfirmationButtonText = "I'll stay calm!";
            await dialog.ShowModalAsync(this);
        }

        private void RemovePerson(Person person)
        {
            People.Remove(person);
        }

        private async void AddPerson()
        {
            await Navigator.NavigateAsync(Pages.EditPersonPage.GetNavigationModule());
        }

        private async void ApplyFilter(PersonFilter filter)
        {
            CurrentFilter = filter;
            IsFilterOpen = false;
            IsLoadingPeople = true;
            People = new ObservableCollection<Person>(await ResolveFilter());
            IsLoadingPeople = false;
        }

        private async void RemoveFilter()
        {
            CurrentFilter = null;
            IsFilterOpen = false;
            People = new ObservableCollection<Person>(await ResolveFilter());
        }

        private Task<List<Person>> ResolveFilter()
        {
            if (CurrentFilter is null)
            {
                return Task.FromResult(_modelData);
            }

            return Task.Run(async () => 
            {
                await Task.Delay(1500);
                return People.Where(p => CurrentFilter.CheckPerson(p)).ToList();
            });
        }
    }
}
