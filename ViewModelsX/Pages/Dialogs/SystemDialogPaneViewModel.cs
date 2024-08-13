﻿using CommunityToolkit.Mvvm.ComponentModel;
using ModelX.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModelsX.Pages.Dialogs
{
    public partial class SystemDialogPaneViewModel : ObservableObject
    {
        [ObservableProperty]
        private SystemDialog? _selectedDialog;
    }
}
