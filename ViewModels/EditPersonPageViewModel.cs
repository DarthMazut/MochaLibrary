﻿using MochaCore.Behaviours;
using MochaCore.DialogsEx;
using MochaCore.DialogsEx.Extensions;
using MochaCore.Navigation;
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
    public class EditPersonPageViewModel : BindableBase, INavigatable, IOnNavigatedTo
    {
        private string? _title;
        private string? _firstName;
        private string? _lastName;
        private AsyncProperty<string> _initials;
        private string? _city;
        private DateTimeOffset _birthday;
        private string? _imageSource;

        private DelegateCommand _goBackCommand;
        private DelegateCommand _editPictureCommand;

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

        public DelegateCommand GoBackCommand => _goBackCommand ??= new DelegateCommand(GoBack);
        public DelegateCommand EditPictureCommand => _editPictureCommand ??= new DelegateCommand(EditPicture);

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

        public void OnNavigatedTo(NavigationData navigationData)
        {
            if (navigationData.Data is Person person)
            {
                Title = "Edit Person";
                FirstName = person.FirstName;
                LastName = person.LastName;
                City = person.City;
                Birthday = person.Birthday;
                ImageSource = ResolveImageSource(person.ImageID);
            }
            else
            {
                Title = "Add Person";
            }
        }

        private string? ResolveImageSource(string? imageID)
        {
            if (imageID is null)
            {
                return null;
            }

            string localFolderPath = BehaviourManager.Recall<object, string>("GetLocalAppFolderPath").Execute(new object());
            return Path.Combine(localFolderPath, imageID) + ".png";
        }
    }
}
