using MochaCore.Behaviours;
using MochaCore.DialogsEx;
using MochaCore.DialogsEx.Extensions;
using MochaCore.Navigation;
using MochaCore.Settings;
using MochaCore.Utils;
using Model;
using Prism.Commands;
using Prism.Mvvm;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public class EditPersonPageViewModel : BindableBase, INavigatable, IOnNavigatedToAsync
    {
        private string? _title;
        private string? _firstName;
        private string? _lastName;
        private AsyncProperty<string> _initials;
        private string? _city;
        private DateTimeOffset _birthday;
        private string? _imageSource;

        private Person? _editingPerson;

        private DelegateCommand _goBackCommand;
        private DelegateCommand _editPictureCommand;
        private DelegateCommand _applyCommand;

        public EditPersonPageViewModel()
        {
            Navigator = new Navigator(this, NavigationServices.MainNavigationService);
            _initials = new(this, nameof(Initials));
            _initials.SynchronizedProperties.Add(nameof(FirstName));
            _initials.SynchronizedProperties.Add(nameof(LastName));
        }

        public Navigator Navigator { get; }

        public string? Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        public string? FirstName
        {
            get => _firstName;
            set => SetProperty(ref _firstName, value);
        }

        public string? LastName
        {
            get => _lastName;
            set => SetProperty(ref _lastName, value);
        }

        public string? Initials
        {
            get => $"{FirstName?.FirstOrDefault()}{LastName?.FirstOrDefault()}";
        }

        public string? City
        {
            get => _city;
            set => SetProperty(ref _city, value);
        }

        public DateTimeOffset Birthday
        {
            get => _birthday;
            set => SetProperty(ref _birthday, value);
        }

        public string? ImageSource 
        {
            get => _imageSource;
            set => SetProperty(ref _imageSource, value);
        }

        public Func<string, bool> ValidationFunction => (t) => 
        {
            return !string.IsNullOrWhiteSpace(t);
        };

        public DelegateCommand GoBackCommand => _goBackCommand ??= new DelegateCommand(GoBack);
        public DelegateCommand EditPictureCommand => _editPictureCommand ??= new DelegateCommand(EditPicture);
        public DelegateCommand ApplyCommand => _applyCommand ??= new DelegateCommand(Apply);

        private async void Apply()
        {
            Person? person = _editingPerson;
            if (person is null)
            {
                person = new(FirstName, LastName);
            }
            else
            {
                person.FirstName = FirstName;
                person.LastName = LastName;
            }
            
            person.City = City;
            person.Birthday = Birthday;

            ISettingsSection<ApplicationSettings> settingsSection = ApplicationSettings.Section;
            ApplicationSettings applicationSettings = await settingsSection.LoadAsync();

            if (ImageSource is not null)
            {
                PersonImageType? imageType = PersonImageTypeExtensions.ResolvePathExtension(ImageSource);
                if (imageType is not null)
                {
                    person.ImageType = imageType;
                    try
                    {
                        File.Copy(ImageSource, Path.Combine(applicationSettings.ImagesPath, person.ImageName), true);
                    }
                    catch (IOException ex) when (ex.HResult == -2147024864) { }
                }
            }

            if (_editingPerson is null)
            {
                await settingsSection.UpdateAsync(settings => settings.People.Add(person));
            }
            else
            {
                await settingsSection.UpdateAsync(settings =>
                {
                    int currentIndex = settings.People.FindIndex(p => p.Guid == person.Guid);
                    settings.People.RemoveAt(currentIndex);
                    settings.People.Insert(currentIndex, person);
                });
            }

            _ = await Navigator.NavigateAsync(Pages.PeoplePage.GetNavigationModule());
        }

        private async void GoBack()
        {
            _ = await Navigator.NavigateAsync(Pages.PeoplePage.GetNavigationModule());
        }

        private async void EditPicture()
        {
            ICustomDialogModule<DialogProperties> editPictureDialog = Dialogs.EditPictureDialog.Module;
            if (ImageSource is not null)
            {
                editPictureDialog.Properties.CustomProperties.Add("SelectedImage", ImageSource);
            }

            await editPictureDialog.ShowModalAsync(this);
            if (editPictureDialog.Properties.CustomProperties.ContainsKey("SelectedImage"))
            {
                ImageSource = editPictureDialog.Properties.CustomProperties["SelectedImage"] as string;
            }
        }

        public async Task OnNavigatedToAsync(NavigationData navigationData)
        {
            if (navigationData.Data is Person person)
            {
                Title = "Edit Person";
                FirstName = person.FirstName;
                LastName = person.LastName;
                City = person.City;
                Birthday = person.Birthday ?? new DateTimeOffset();
                ImageSource = await ResolveImageSource(person.ImageName);
                
                _editingPerson = person;
            }
            else
            {
                Title = "Add Person";
            }
        }

        private async Task<string?> ResolveImageSource(string? imageName)
        {
            if (imageName is null)
            {
                return null;
            }

            ISettingsSection<ApplicationSettings> settingsSection = ApplicationSettings.Section;
            ApplicationSettings settings = await settingsSection.LoadAsync();

            return Path.Combine(settings.ImagesPath, imageName);
        }
    }
}
