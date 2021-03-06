﻿using Mocha.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MochaWPF
{
    internal class SimpleDialogData : IDialog
    {
        public bool? DialogResult { get; set; }

        public object DialogValue { get; set; }

        public DialogParameters DialogParameters { get; set; } = new DialogParameters();

        public DialogBehaviors DialogBehaviors { get; set; } = new DialogBehaviors();
    }
}
