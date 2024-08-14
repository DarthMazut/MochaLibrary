using MochaCore.Dialogs;
using MochaCore.Dialogs.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelX.Dialogs
{
    public class SystemDialog : IDisposable, INotifyPropertyChanged
    {
        private string _log = string.Empty;

        public SystemDialog(IDialogModule module) : this(module, null) { }

        public SystemDialog(IDialogModule module, string? name)
        {
            Type = ResolveType(module) ?? throw new ArgumentException($"Module {module.GetType()} is not supported.");

            CoreModule = module;
            Name = name ?? $"{Type} Module - {Path.GetRandomFileName()}";
            UpdateLog($"Module created [{Type}][{Name}]");
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public string? Title
        {
            get
            {
                GetModuleValues(out string? title, out _);
                return title;
            }
            set => SetModuleValues(true, false, value, default); 
        }

        public string? InitialDirectory
        {
            get
            {
                GetModuleValues(out _, out string? initialDirectory);
                return initialDirectory;
            }
            set => SetModuleValues(false, true, default, value);
        }

        public string Name { get; }

        public SystemDialogType Type { get; }

        public IDialogModule CoreModule { get; }

        public string Log => _log;

        public async Task<bool?> SafeShow(object? host)
        {
            try
            {
                return await CoreModule.ShowModalAsync(host);
            }
            catch (Exception ex) // Do not catch general exceptions :O
            {
                UpdateLog(ex.ToString());
            }

            return null;
        }

        public void Dispose() => CoreModule.Dispose();

        public static SystemDialogType? ResolveType(IDialogModule module)
            => module switch
            {
                IDialogModule<SaveFileDialogProperties> => SystemDialogType.SaveDialog,
                IDialogModule<OpenFileDialogProperties> => SystemDialogType.OpenDialog,
                IDialogModule<BrowseFolderDialogProperties> => SystemDialogType.BrowseDialog,
                _ => null
            };

        private void UpdateLog(string log)
        {
            _log += $"{log}{Environment.NewLine}";
            PropertyChanged?.Invoke(this, new(nameof(Log)));
        }

        private void GetModuleValues(out string? title, out string? initalDirectory)
        {
            switch (Type)
            {
                case SystemDialogType.SaveDialog:
                    IDialogModule<SaveFileDialogProperties> saveModule = (IDialogModule<SaveFileDialogProperties>)CoreModule;
                    title = saveModule.Properties.Title;
                    initalDirectory = saveModule.Properties.InitialDirectory;
                    break;
                case SystemDialogType.OpenDialog:
                    IDialogModule<OpenFileDialogProperties> openModule = (IDialogModule<OpenFileDialogProperties>)CoreModule;
                    title = openModule.Properties.Title;
                    initalDirectory = openModule.Properties.InitialDirectory;
                    break;
                case SystemDialogType.BrowseDialog:
                    IDialogModule<BrowseFolderDialogProperties> browseModule = (IDialogModule<BrowseFolderDialogProperties>)CoreModule;
                    title = browseModule.Properties.Title;
                    initalDirectory = browseModule.Properties.InitialDirectory;
                    break;
                default:
                    throw new InvalidOperationException($"Type {Type} is not supported.");
            }
        }

        private void SetModuleValues(
            bool setTitle,
            bool setInitialDirectory,
            string? title,
            string? initialDirectory)
        {
            switch (Type)
            {
                case SystemDialogType.SaveDialog:
                    IDialogModule<SaveFileDialogProperties> saveModule = (IDialogModule<SaveFileDialogProperties>)CoreModule;
                    if (setTitle) saveModule.Properties.Title = title;
                    if (setInitialDirectory) saveModule.Properties.InitialDirectory = initialDirectory;
                    break;
                case SystemDialogType.OpenDialog:
                    IDialogModule<OpenFileDialogProperties> openModule = (IDialogModule<OpenFileDialogProperties>)CoreModule;
                    if (setTitle) openModule.Properties.Title = title;
                    if (setInitialDirectory) openModule.Properties.InitialDirectory = initialDirectory;
                    break;
                case SystemDialogType.BrowseDialog:
                    IDialogModule<BrowseFolderDialogProperties> browseModule = (IDialogModule<BrowseFolderDialogProperties>)CoreModule;
                    if (setTitle) browseModule.Properties.Title = title;
                    if (setInitialDirectory) browseModule.Properties.InitialDirectory = initialDirectory;
                    break;
                default:
                    throw new InvalidOperationException($"Type {Type} is not supported.");
            }
        }
    }

    public enum SystemDialogType
    {
        SaveDialog,
        OpenDialog,
        BrowseDialog
    }
}
