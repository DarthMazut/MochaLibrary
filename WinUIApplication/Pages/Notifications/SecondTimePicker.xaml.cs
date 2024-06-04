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
using System.Timers;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace WinUiApplication.Pages.Notifications
{
    public sealed partial class SecondTimePicker : UserControl
    {
        public static readonly DependencyProperty SelectedTimeProperty =
            DependencyProperty.Register(nameof(SelectedTime), typeof(DateTimeOffset), typeof(SecondTimePicker), new PropertyMetadata(default, SelectedTimeChanged));

        public DateTimeOffset SelectedTime
        {
            get { return (DateTimeOffset)GetValue(SelectedTimeProperty); }
            set { SetValue(SelectedTimeProperty, value); }
        }

        private static void SelectedTimeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DateTimeOffset now = (DateTimeOffset)e.NewValue;
            ((SecondTimePicker)d).xe_HourNumber.Value = now.Hour;
            ((SecondTimePicker)d).xe_MinuteNumber.Value = now.Minute;
            ((SecondTimePicker)d).xe_SecondNumber.Value = now.Second;

            ((SecondTimePicker)d).UpdateTimeWarning();
        }

        public SecondTimePicker()
        {
            InitializeComponent();
            Unloaded += (s, e) =>
            {
                TimerTick -= OnInternalTimerTick;
            };

            TimerTick += OnInternalTimerTick;
        }

        private void OnInternalTimerTick(object? sender, EventArgs e)
        {
            _ = DispatcherQueue.TryEnqueue(() =>
            {
                xe_CurrentTimeText.Text = DateTimeOffset.Now.ToString("HH:mm:ss");
                UpdateTimeWarning();
            });
        }

        private void TimeValueChanged(NumberBox sender, NumberBoxValueChangedEventArgs args)
        {
            if (double.IsNaN(xe_HourNumber.Value) || double.IsNaN(xe_MinuteNumber.Value) || double.IsNaN(xe_SecondNumber.Value))
            {
                xe_HourNumber.Value = SelectedTime.Hour;
                xe_MinuteNumber.Value = SelectedTime.Minute;
                xe_SecondNumber.Value = SelectedTime.Second;
                return;
            }

            SelectedTime = new DateTimeOffset(
                SelectedTime.Year,
                SelectedTime.Month,
                SelectedTime.Day,
                (int)xe_HourNumber.Value,
                (int)xe_MinuteNumber.Value,
                (int)xe_SecondNumber.Value,
                SelectedTime.Offset);
        }

        private void UpdateTimeWarning()
        {
            xe_TimeWarning.Visibility = SelectedTime <= DateTimeOffset.Now ? Visibility.Visible : Visibility.Collapsed;
        }


        #region SHARED_TIMER

        private static CancellationTokenSource? _timerTaskCts;

        private static event EventHandler? _timerTick;
        private static event EventHandler? TimerTick
        {
            add
            {
                if (_timerTick is null)
                {
                    StartTimer();
                }

                _timerTick += value;
            }

            remove
            {
                _timerTick -= value;
                if (_timerTick is null)
                {
                    StopTimer();
                }
            }
        }

        private static void StartTimer()
        {
            if (_timerTaskCts is null)
            {
                CancellationTokenSource cts = new();
                _timerTaskCts = cts;

                Task.Run(async () =>
                {
                    while (cts.IsCancellationRequested == false)
                    {
                        _timerTick?.Invoke(null, EventArgs.Empty);
                        await Task.Delay(100);
                    }
                }, _timerTaskCts.Token);
            }
        }

        private static void StopTimer()
        {
            _timerTaskCts?.Cancel();
            _timerTaskCts = null;
        }

        #endregion
    }
}
