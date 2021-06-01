using Mocha.Dialogs;
using Mocha.Navigation;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MochaWPFTestApp.ViewModels
{
    class Page3ViewModel : BindableBase, INavigatable, IOnNavigatingToAsync
    {
        public Navigator Navigator { get; }

        private string _buttonCaption = "Start";
        private double _progress = 0;
        private string _label = $"Progress: 0%";
        private bool _isButtonEnabled = true;

        private DelegateCommand _startCommand;
        public DelegateCommand StartCommand => _startCommand ?? (_startCommand = new DelegateCommand(Start));

        public string ButtonCaption
        {
            get => _buttonCaption;
            set => SetProperty(ref _buttonCaption, value);
        }

        public double Progress
        {
            get => _progress;
            set => SetProperty(ref _progress, value);
        }

        public string Label
        {
            get => _label;
            set => SetProperty(ref _label, value);
        }

        public bool IsButtonEnabled
        {
            get => _isButtonEnabled;
            set => SetProperty(ref _isButtonEnabled, value);
        }


        public Page3ViewModel(NavigationService navigationService)
        {
            Navigator = new Navigator(this, navigationService);
        }

        private async void Start()
        {
            IsButtonEnabled = false;

            Progress<int> progress = new Progress<int>();
            progress.ProgressChanged += OnProgressChanged;

            await Task.Run(() => DoStuff(progress));

            //await Task.Run(() => DoStuff(new Progress<int>((p) =>
            //{
            //    Progress = p;
            //    Label = $"Progress {p}%";
            //})));

            IsButtonEnabled = true;

        }

        private void OnProgressChanged(object sender, int e)
        {
            Progress = e;
            Label = $"Progress {e}%";
        }

        private void DoStuff(IProgress<int> progress)
        {
            for (int i = 0; i < 100; i++)
            {
                System.Threading.Thread.Sleep(100);
                progress.Report(i);
            }
            progress.Report(100);
        }

        public async Task OnNavigatingToAsync(NavigationData navigationData, NavigationCancelEventArgs e)
        {
            e.Cancel = true;
            await Navigator.NavigateAsync(NavigationModules.Page2);
        }
    }
}
