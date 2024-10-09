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
                DoWork(3000);
                Log("Running on main thread");
                _dispatcher.RunOnMainThread(() =>
                {
                    Log("Switched to UI thread");
                    DoWork(3000);

                });
                Log("Switch to task thread");
                DoWork(3000);
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
                DoWork(3000);
                Log("Running on main thread");
                _dispatcher.RunOnMainThread(async () =>
                {
                    Log("Switched to UI thread");
                    Log("Awaiting work on UI thread: 3s");
                    await Task.Run(() =>
                    {
                        DoWork(3000);
                    });
                    Log("Work awaited on UI thread: 3s");

                });
                Log("Switch to task thread");
                DoWork(3000);
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
                DoWork(3000);
                Log("Running on main thread, awaited");
                await _dispatcher.RunOnMainThreadAsync(() =>
                {
                    Log("Switched to UI thread");
                    DoWork(3000);
                });
                Log("Switch to task thread");
                DoWork(3000);
                Log("Running on main thread, NOT awaited");
                _ = _dispatcher.RunOnMainThreadAsync(() =>
                {
                    Log("Switched to UI thread");
                    DoWork(6000);

                    LogDone();
                });
                DoWork(3000);
            });
        }

        [RelayCommand]
        private void RunAsyncOnMainThreadAsync()
        {
            LogText = string.Empty;
            LogTitle("RunAsyncOnMainThreadAsync:");
            Log("Starting new task");

            _ = Task.Run(async () =>
            {
                Log("Side task started");
                DoWork(3000);
                Log("Running on main thread, awaited");
                await _dispatcher.RunOnMainThreadAsync(async () =>
                {
                    Log("Switched to UI thread");
                    Log("Awaiting work on UI thread: 3s");
                    await Task.Run(() =>
                    {
                        DoWork(3000);
                    });
                    Log("Work awaited on UI thread: 3s");
                });
                Log("Switch to task thread");
                DoWork(3000);
                Log("Running on main thread, NOT awaited");
                _ = _dispatcher.RunOnMainThreadAsync(async () =>
                {
                    Log("Switched to UI thread");
                    Log("Awaiting work on UI thread: 6s");
                    await Task.Run(() =>
                    {
                        DoWork(6000);
                    });
                    Log("Work awaited on UI thread: 6s");

                    LogDone();
                });
                DoWork(3000);
            });
        }

        private void DoWork(int msDuration)
        {
            Log($"Doing work: {msDuration} ms");
            Thread.Sleep(3000);
            Log($"Work done: {msDuration} ms");
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
