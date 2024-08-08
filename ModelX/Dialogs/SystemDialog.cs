using MochaCore.Dialogs;
using MochaCore.Dialogs.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelX.Dialogs
{
    public class SystemDialog
    {
        protected SystemDialog(IDialogModule module)
        {
            Module = module;
            Type = ResolveType(module) ?? throw new ArgumentException("Invalid module");
            Name = $"{Type} Module - {Path.GetRandomFileName()}";
        }

        public string Name { get; set; }

        public SystemDialogType Type { get; }

        public IDialogModule Module { get; }

        public static SystemDialog FromModule(IDialogModule<SaveFileDialogProperties> saveDialogModule) => new(saveDialogModule);

        public static SystemDialog FromModule(IDialogModule<OpenFileDialogProperties> openDialogModule) => new(openDialogModule);

        public static SystemDialog FromModule(IDialogModule<BrowseFolderDialogProperties> browseDialogModule) => new(browseDialogModule);

        public static SystemDialogType? ResolveType(IDialogModule module)
            => module switch
            {
                IDialogModule<SaveFileDialogProperties> => SystemDialogType.SaveDialog,
                IDialogModule<OpenFileDialogProperties> => SystemDialogType.OpenDialog,
                IDialogModule<BrowseFolderDialogProperties> => SystemDialogType.BrowseDialog,
                _ => null
            };
    }

    public enum SystemDialogType
    {
        SaveDialog,
        OpenDialog,
        BrowseDialog
    }
}
