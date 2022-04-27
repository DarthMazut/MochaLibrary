using MochaCore.DialogsEx;
using MochaCore.DialogsEx.Extensions;
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

        private ICustomDialogModule<DialogProperties> _dialogModule;
        public ICustomDialogModule<DialogProperties> DialogModule 
        {
            get => _dialogModule;
            set
            {
                _dialogModule = value;
                DialogModule.Opened += Opened;
            }
        }

        public string? ImageSource
        {
            get => _imageSource;
            set => SetProperty(ref _imageSource, value, OnImageSourceChanged);
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
        }

        private void Opened(object? sender, EventArgs e)
        {
            if (DialogModule.Properties.CustomProperties.ContainsKey("SelectedImage"))
            {
                ImageSource = (string)DialogModule.Properties.CustomProperties["SelectedImage"];
            }
        }

        private async void FindImage()
        {
            IDialogModule<OpenFileDialogProperties> selectFileDialog = Dialogs.SelectFileDialog.Module;
            selectFileDialog.Properties.Filters.Add(new ExtensionFilter("Images", new List<string> { "jpg", "jpeg", "png" }));
            if (await selectFileDialog.ShowModalAsync(this) is true)
            {
                ImageSource = selectFileDialog.Properties.SelectedPaths.FirstOrDefault();
            }

        }

        private void OnImageSourceChanged()
        {
            IsLoadingImage = true;
            IsSelectedPathLegit = false;
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
                DialogModule.Properties.CustomProperties["SelectedImage"] = newPath;
            }

            DialogModule.Close();
        }
    }
}
