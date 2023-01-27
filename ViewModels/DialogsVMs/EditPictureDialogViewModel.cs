using MochaCore.Dialogs;
using MochaCore.Dialogs.Extensions;
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

namespace ViewModels.DialogsVMs
{
    public class EditPictureDialogViewModel : BindableBase, ICustomDialog<DialogProperties>
    {
        private string? _imageSource;
        private bool _isSelectedPathLegit;
        private bool _isLoadingImage;

        private DelegateCommand<string?> _closeCommand;
        private DelegateCommand _searchFolderCommand;
        private DelegateCommand _imageOpenedCommand;

        public CustomDialogControl<DialogProperties> DialogControl { get; } = new();

        public string? ImageSource
        {
            get => _imageSource;
            set => SetProperty(ref _imageSource, value);
        }

        public bool IsSelectedPathLegit 
        {
            get => _isSelectedPathLegit;
            set => SetProperty(ref _isSelectedPathLegit, value);
        }

        public bool IsLoadingImage
        {
            get => _isLoadingImage;
            set => SetProperty(ref _isLoadingImage, value);
        }

        public DelegateCommand<string?> CloseCommand => _closeCommand;

        public DelegateCommand SearchFolderCommand => _searchFolderCommand;

        public DelegateCommand ImageOpenedCommand => _imageOpenedCommand;

        public EditPictureDialogViewModel()
        {
            _closeCommand = new DelegateCommand<string?>(Close);
            _searchFolderCommand = new DelegateCommand(FindImage);
            _imageOpenedCommand = new DelegateCommand(ImageOpened);

            DialogControl.SubscribeOnDialogOpened(Opened);
        }

        private void Opened()
        {
            if (DialogControl.Properties.CustomProperties.ContainsKey("SelectedImage"))
            {
                ImageSource = (string)DialogControl.Properties.CustomProperties["SelectedImage"];
            }
        }

        private async void FindImage()
        {
            IDialogModule<OpenFileDialogProperties> selectFileDialog = Dialogs.SelectFileDialog.Module;
            selectFileDialog.Properties.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyVideos);
            selectFileDialog.Properties.Filters.Add(new ExtensionFilter("Images", new List<string> { "jpg", "jpeg", "png" }));

            if (await selectFileDialog.ShowModalAsync(this) is true)
            {
                IsLoadingImage = true;
                IsSelectedPathLegit = false;
                await Task.Yield();
                ImageSource = selectFileDialog.Properties.SelectedPaths.FirstOrDefault();
            }
        }

        private void ImageOpened()
        {
            bool isExtensionSupported = PersonImageTypeExtensions.ResolvePathExtension(ImageSource) is not null;
            IsSelectedPathLegit = isExtensionSupported;
            IsLoadingImage = false;
        }

        private void Close(string? newPath)
        {
            if (newPath is not null)
            {
                DialogControl.Properties.CustomProperties["SelectedImage"] = newPath;
            }

            DialogControl.Close(true);
        }
    }
}
