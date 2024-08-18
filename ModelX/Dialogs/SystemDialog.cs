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
            Name = string.IsNullOrWhiteSpace(name) ? $"{Type} Module - {Path.GetRandomFileName()}" : name;

            module.Opening += ModuleOpening;
            module.Closed += ModuleClosed;
            module.Disposed += ModuleDisposed;
            
            UpdateLog($"Module created [{Type}][{Name}]");
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public string? Title
        {
            get => GetModuleValues().Title;
            set => SetModuleValues(new ModuleValues()
            {
                Title = value
            }); 
        }

        public string? InitialDirectory
        {
            get => GetModuleValues().InitialDirectory;
            set => SetModuleValues(new ModuleValues()
            {
                InitialDirectory = value
            });
        }

        public IReadOnlyList<string?> SelectedPaths
        {
            get => GetModuleValues().SelectedPaths;
        }

        public bool Multiselection
        {
            get => GetModuleValues().Multiselection;
            set => SetModuleValues(new ModuleValues()
            {
                Multiselection = value
            });
        }

        public IList<ExtensionFilter> Filters
        {
            get => GetModuleValues().Filters;
            set => SetModuleValues(new ModuleValues()
            {
                Filters = value
            });
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
                UpdateLog($"Dialog result: {result?.ToString() ?? "null"}");
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

        private ModuleValues GetModuleValues()
        {
            string? title = null;
            string? initalDirectory = null;
            string?[] selectedPaths = [];
            bool multiselection = false;
            IList<ExtensionFilter> filters = [];

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

            return new ModuleValues()
            {
                Title = title,
                InitialDirectory = initalDirectory,
                SelectedPaths = selectedPaths,
                Multiselection = multiselection,
                Filters = filters
            };
        }

        private void SetModuleValues(ModuleValues values)
        {
            switch (Type)
            {
                case SystemDialogType.SaveDialog:
                    IDialogModule<SaveFileDialogProperties> saveModule = (IDialogModule<SaveFileDialogProperties>)CoreModule;
                    if (values.SetTitle) saveModule.Properties.Title = values.Title;
                    if (values.SetInitialDirectory) saveModule.Properties.InitialDirectory = values.InitialDirectory;
                    if (values.SetFilters) saveModule.Properties.Filters = values.Filters;
                    break;
                case SystemDialogType.OpenDialog:
                    IDialogModule<OpenFileDialogProperties> openModule = (IDialogModule<OpenFileDialogProperties>)CoreModule;
                    if (values.SetTitle) openModule.Properties.Title = values.Title;
                    if (values.SetInitialDirectory) openModule.Properties.InitialDirectory = values.InitialDirectory;
                    if (values.SetMultiselection) openModule.Properties.MultipleSelection = values.Multiselection;
                    if (values.SetFilters) openModule.Properties.Filters = values.Filters;
                    break;
                case SystemDialogType.BrowseDialog:
                    IDialogModule<BrowseFolderDialogProperties> browseModule = (IDialogModule<BrowseFolderDialogProperties>)CoreModule;
                    if (values.SetTitle) browseModule.Properties.Title = values.Title;
                    if (values.SetInitialDirectory) browseModule.Properties.InitialDirectory = values.InitialDirectory;
                    break;
                default:
                    throw new InvalidOperationException($"Type {Type} is not supported.");
            }
        }

        private class ModuleValues
        {
            private readonly bool _setTitle;
            private readonly bool _setInitialDirectory;
            private readonly bool _setFilters;
            private readonly bool _setMultiselection;
            private readonly bool _setSelectedPaths;
            private readonly string? _title;
            private readonly string? _initialDirectory;
            private readonly bool _multiselection;
            private readonly IList<ExtensionFilter> _filters = [];
            private readonly string?[] _selectedPaths = [];

            public bool SetTitle => _setTitle;
            public bool SetInitialDirectory => _setInitialDirectory;
            public bool SetFilters => _setFilters;
            public bool SetMultiselection => _setMultiselection;
            public bool SetSelectedPaths => _setSelectedPaths;

            public string? Title
            { 
                get => _title;
                init
                {
                    _setTitle = true;
                    _title = value;
                }
            }

            public string? InitialDirectory
            { 
                get => _initialDirectory;
                init
                {
                    _setInitialDirectory = true;
                    _initialDirectory = value;
                }
            }

            public bool Multiselection
            { 
                get => _multiselection;
                init
                {
                    _setMultiselection = true;
                    _multiselection = value;
                }
            }

            public IList<ExtensionFilter> Filters
            { 
                get => _filters;
                init
                {
                    _setFilters = true;
                    _filters = value;
                }
            }

            public string?[] SelectedPaths
            {
                get => _selectedPaths;
                init
                {
                    _setSelectedPaths = true;
                    _selectedPaths = value;
                }
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
