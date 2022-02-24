using MochaCore.DialogsEx;
using MochaCore.DialogsEx.Extensions;
using MochaCore.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WpfApplication
{
    public class MyDialogWindowViewModel : IDialog<DialogProperties>, INotifyPropertyChanged
    {
        private readonly AsyncProperty<string> _inputText;

        public MyDialogWindowViewModel()
        {
            _inputText = new(this, nameof(InputText));
        }

        public IDialogModule<DialogProperties> DialogModule { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;

        public string InputText 
        {
            get => _inputText.Get();
            set => _inputText.Set(value);
        }

        public ICommand YesCommand => new SimpleCommand((o) => 
        {
            DialogModule.Properties.CustomProperties.Add("test", InputText);
            (DialogModule as IDialogClose)?.Close();
        });

        public ICommand NoCommand => new SimpleCommand((o) =>
        {
            (DialogModule as IDialogClose)?.Close();
        });
    }
}
