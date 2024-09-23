﻿using CommunityToolkit.Mvvm.ComponentModel;
using MochaCore.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModelsX.Pages.Windowing.Dialogs
{
    public class WindowingPageOpenWindowDialogViewModel : ObservableObject, ICustomDialog<WindowingPageOpenWindowDialogProperties>
    {
        public ICustomDialogControl<WindowingPageOpenWindowDialogProperties> DialogControl { get; }
            = new CustomDialogControl<WindowingPageOpenWindowDialogProperties>();

        public string Test => "Hello";
    }
}
