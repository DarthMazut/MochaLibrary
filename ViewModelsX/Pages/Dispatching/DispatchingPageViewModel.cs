using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MochaCore.Dispatching;
using MochaCore.Navigation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModelsX.Pages.Dispatching
{
    public partial class DispatchingPageViewModel : ObservableObject, INavigationParticipant
    {
        private readonly IDispatcher _dispatcher = DispatcherManager.GetMainThreadDispatcher();

        public INavigator Navigator { get; } = MochaCore.Navigation.Navigator.Create();

        [ObservableProperty]
        private string? _logText;

        [RelayCommand]
        private void RunOnMainThread()
        {
            LogText = string.Empty;
            LogTitle("RunOnMainThread:");

            Log("Starting new task");

            _ = Task.Run(() =>
            {
                Log("Side task started");
                Log("Doing work on task thread: 3s");
                Thread.Sleep(3000);
                Log("Work done on task thread: 3s");
                Log("Running on main thread");
                _dispatcher.RunOnMainThread(() =>
                {
                    Log("Switched to UI thread");
                    Log("Doing work on UI thread: 3s");
                    Thread.Sleep(3000);
                    Log("Work done on UI thread: 3s");

                });
                Log("Switch to task thread");
                Log("Doing work on task thread: 3s");
                Thread.Sleep(3000);
                Log("Work done on task thread: 3s");
                LogDone();
            });
        }

        [RelayCommand]
        private void RunAsyncOnMainThread()
        {
            LogText = string.Empty;
            LogTitle("RunAsyncOnMainThread:");

            Log("Starting new task");

            _ = Task.Run(() =>
            {
                Log("Side task started");
                Log("Doing work on task thread: 3s");
                Thread.Sleep(3000);
                Log("Work done on task thread: 3s");
                Log("Running on main thread");
                _dispatcher.RunOnMainThread(async () =>
                {
                    Log("Switched to UI thread");
                    Log("Awaiting work on UI thread: 3s");
                    await Task.Run(() =>
                    {
                        Log("Doing work on working thread: 3s");
                        Thread.Sleep(3000);
                        Log("Work done on working thread: 3s");
                    });
                    Log("Work awaited on UI thread: 3s");

                });
                Log("Switch to task thread");
                Log("Doing work on task thread: 3s");
                Thread.Sleep(3000);
                Log("Work done on task thread: 3s");
                LogDone();
            });
        }

        [RelayCommand]
        private void RunOnMainThreadAsync()
        {
            LogText = string.Empty;
            LogTitle("RunOnMainThreadAsync:");
            Log("Starting new task");

            _ = Task.Run(async () =>
            {
                Log("Side task started");
                Log("Doing work on task thread: 3s");
                Thread.Sleep(3000);
                Log("Work done on task thread: 3s");
                Log("Running on main thread, awaited");
                await _dispatcher.RunOnMainThreadAsync(() =>
                {
                    Log("Switched to UI thread");
                    Log("Doing work on UI thread: 3s");
                    Thread.Sleep(3000);
                    Log("Work done on UI thread: 3s");
                });
                Log("Switch to task thread");
                Log("Doing work on task thread: 3s");
                Thread.Sleep(3000);
                Log("Work done on task thread: 3s");
                Log("Running on main thread, NOT awaited");
                _ = _dispatcher.RunOnMainThreadAsync(() =>
                {
                    Log("Switched to UI thread");
                    Log("Doing work on UI thread: 6s");
                    Thread.Sleep(6000);
                    Log("Work done on UI thread: 6s");

                    LogDone();
                });
                Log("Doing work on task thread: 3s");
                Thread.Sleep(3000);
                Log("Work done on task thread: 3s");
            });
        }

        private void Log(string message)
        {
            int id = Thread.CurrentThread.ManagedThreadId;
            string threadName = id == 1 ? "UI" : $"#{id}";
            _dispatcher.EnqueueOnMainThread(() => LogText += $"[Thread {threadName}]: {message}" + Environment.NewLine);
        }

        private void LogTitle(string message)
            => _dispatcher.EnqueueOnMainThread(() => LogText += message + Environment.NewLine +
                "------------------------------" + Environment.NewLine);

        private void LogDone() => _dispatcher.EnqueueOnMainThread(() => LogText += Environment.NewLine + "DONE!");
    }
}
