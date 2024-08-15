using MochaCore.Dialogs;
using MochaCore.Dialogs.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
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

            module.Opening += ModuleOpening;
            module.Closed += ModuleClosed;
            module.Disposed += ModuleDisposed;
            
            UpdateLog($"Module created [{Type}][{Name}]");
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public string? Title
        {
            get
            {
                GetModuleValues(out string? title, out _, out _, out _, out _);
                return title;
            }
            set => SetModuleValues(true, false, false, false, value, default, default, []); 
        }

        public string? InitialDirectory
        {
            get
            {
                GetModuleValues(out _, out string? initialDirectory, out _, out _, out _);
                return initialDirectory;
            }
            set => SetModuleValues(false, true, false, false, default, value, default, []);
        }

        public IReadOnlyList<string?> SelectedPaths
        {
            get
            {
                GetModuleValues(out _, out _, out string?[] selectedPaths, out _, out _);
                return [.. selectedPaths];
            }
        }

        public bool Multiselection
        {
            get
            {
                GetModuleValues(out _, out _, out _, out bool multiselection, out _);
                return multiselection;
            }
            set => SetModuleValues(false, false, true, false, default, default, value, []);
        }

        public IList<ExtensionFilter> Filters
        {
            get
            {
                GetModuleValues(out _, out _, out _, out _, out IList<ExtensionFilter> filters);
                return filters;
            }
            set => SetModuleValues(false, false, false, true, default, default, default, value);
        }

        public string Name { get; }

        public SystemDialogType Type { get; }

        public IDialogModule CoreModule { get; }

        public string Log => _log;

        public async Task<bool?> SafeShow(object? host)
        {
            try
            {
                bool? result = await CoreModule.ShowModalAsync(host);
                UpdateLog($"Selected path(s): {Environment.NewLine}\t {string.Join($"{Environment.NewLine}\t", SelectedPaths)}");
                UpdateLog($"Dialog result: {result}");
                return result;
            }
            catch (Exception ex) // Do not catch general exceptions :O
            {
                UpdateLog(ex.ToString());
            }

            return null;
        }

        public void Dispose()
        {
            CoreModule.Dispose();
            CoreModule.Opening -= ModuleOpening;
            CoreModule.Closed -= ModuleClosed;
            CoreModule.Disposed -= ModuleDisposed;
        }

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

        private void ModuleOpening(object? sender, EventArgs e) => UpdateLog("Event invoked [OPENING]");

        private void ModuleClosed(object? sender, EventArgs e) => UpdateLog("Event invoked [CLOSED]");

        private void ModuleDisposed(object? sender, EventArgs e) => UpdateLog("Event invoked [DISPOSED]");

        private void GetModuleValues(
            out string? title,
            out string? initalDirectory,
            out string?[] selectedPaths,
            out bool multiselection,
            out IList<ExtensionFilter> filters)
        {
            switch (Type)
            {
                case SystemDialogType.SaveDialog:
                    IDialogModule<SaveFileDialogProperties> saveModule = (IDialogModule<SaveFileDialogProperties>)CoreModule;
                    title = saveModule.Properties.Title;
                    initalDirectory = saveModule.Properties.InitialDirectory;
                    selectedPaths = [saveModule.Properties.SelectedPath];
                    multiselection = default;
                    filters = saveModule.Properties.Filters;
                    break;
                case SystemDialogType.OpenDialog:
                    IDialogModule<OpenFileDialogProperties> openModule = (IDialogModule<OpenFileDialogProperties>)CoreModule;
                    title = openModule.Properties.Title;
                    initalDirectory = openModule.Properties.InitialDirectory;
                    selectedPaths = [..openModule.Properties.SelectedPaths];
                    multiselection = openModule.Properties.MultipleSelection;
                    filters = openModule.Properties.Filters;
                    break;
                case SystemDialogType.BrowseDialog:
                    IDialogModule<BrowseFolderDialogProperties> browseModule = (IDialogModule<BrowseFolderDialogProperties>)CoreModule;
                    title = browseModule.Properties.Title;
                    initalDirectory = browseModule.Properties.InitialDirectory;
                    selectedPaths = [browseModule.Properties.SelectedPath];
                    multiselection = default;
                    filters = [];
                    break;
                default:
                    throw new InvalidOperationException($"Type {Type} is not supported.");
            }
        }

        private void SetModuleValues(
            bool setTitle,
            bool setInitialDirectory,
            bool setMultiselection,
            bool setFilters,
            string? title,
            string? initialDirectory,
            bool multiselection,
            IList<ExtensionFilter> filters)
        {
            switch (Type)
            {
                case SystemDialogType.SaveDialog:
                    IDialogModule<SaveFileDialogProperties> saveModule = (IDialogModule<SaveFileDialogProperties>)CoreModule;
                    if (setTitle) saveModule.Properties.Title = title;
                    if (setInitialDirectory) saveModule.Properties.InitialDirectory = initialDirectory;
                    if (setFilters) saveModule.Properties.Filters = filters;
                    break;
                case SystemDialogType.OpenDialog:
                    IDialogModule<OpenFileDialogProperties> openModule = (IDialogModule<OpenFileDialogProperties>)CoreModule;
                    if (setTitle) openModule.Properties.Title = title;
                    if (setInitialDirectory) openModule.Properties.InitialDirectory = initialDirectory;
                    if (setMultiselection) openModule.Properties.MultipleSelection = multiselection;
                    if (setFilters) openModule.Properties.Filters = filters;
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
