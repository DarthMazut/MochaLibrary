using MochaCore.DialogsEx;
using MochaCore.DialogsEx.Extensions;
using MochaCore.Utils;
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

        private DelegateCommand<string?> _closeCommand;
        private DelegateCommand _searchFolderCommand;

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
            set => SetProperty(ref _imageSource, value);
        }

        public DelegateCommand<string?> CloseCommand => _closeCommand;
        public DelegateCommand SearchFolderCommand => _searchFolderCommand;

        public EditPictureDialogViewModel()
        {
            _closeCommand = new DelegateCommand<string?>(Close);
            _searchFolderCommand = new DelegateCommand(FindImage);
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
            if (await selectFileDialog.ShowModalAsync(this) is true)
            {
                ImageSource = selectFileDialog.Properties.SelectedPaths.FirstOrDefault();
            }

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
