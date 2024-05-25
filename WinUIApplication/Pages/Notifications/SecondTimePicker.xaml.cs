using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace WinUiApplication.Pages.Notifications
{
    public sealed partial class SecondTimePicker : UserControl
    {
        private CancellationTokenSource? _timerTaskCts;

        public SecondTimePicker()
        {
            this.InitializeComponent();

            ActivateTimeer();
            Unloaded += (s, e) => DeactivateTimer();
        }

        private void ActivateTimeer()
        {
            if (_timerTaskCts is null)
            {
                CancellationTokenSource cts = new();
                _timerTaskCts = cts;

                Task.Run(async () =>
                {
                    while (cts.IsCancellationRequested == false)
                    {
                        _ = DispatcherQueue.TryEnqueue(() =>
                        {
                            xe_CurrentTimeText.Text = DateTimeOffset.Now.ToString("HH:mm:ss");
                        });

                        await Task.Delay(100);
                    }
                }, _timerTaskCts.Token);
            }      
        }

        private void DeactivateTimer()
        {
            _timerTaskCts?.Cancel();
            _timerTaskCts = null;
        }
    }
}
