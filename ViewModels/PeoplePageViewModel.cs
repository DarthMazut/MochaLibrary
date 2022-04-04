using MochaCore.Navigation;
using Model;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public class PeoplePageViewModel : INavigatable
    {
        private DelegateCommand<Person> _removePersonCommand;

        public PeoplePageViewModel()
        {
            Navigator = new(this, NavigationServices.MainNavigationService);
        }

        public Navigator Navigator { get; }

        public ObservableCollection<Person> People { get; } = new()
        {
            new Person("John", "Doe", "Calafiornia", new DateTime(1999, 6, 6)),
            new Person("Jane", "Doe", "Santa Monica", new DateTime(2001, 2, 16)),
            new Person("Dupa")
        };

        public DelegateCommand<Person> RemovePersonCommand => _removePersonCommand ?? (_removePersonCommand = new DelegateCommand<Person>(RemovePerson));

        private void RemovePerson(Person person)
        {
            People.Remove(person);
        }
    }
}
