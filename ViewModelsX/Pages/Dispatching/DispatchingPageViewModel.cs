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
        private bool _isBusy;

        [ObservableProperty]
        private string? _logText;

        [RelayCommand]
        private async Task HasThreadAccess()
        {
            LogText = string.Empty;
            await MarkStart("HasThreadAccess:");

            Log($"UI thread: {_dispatcher.HasThreadAccess}");
            await Task.Run(() => Log($"Task thread: {_dispatcher.HasThreadAccess}"));

            MarkDone();
        }

        [RelayCommand]
        private async Task RunOnMainThread()
        {
            LogText = string.Empty;
            await MarkStart("RunOnMainThread:");

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
                MarkDone();
            });
        }

        [RelayCommand]
        private async Task RunAsyncOnMainThread()
        {
            LogText = string.Empty;
            await MarkStart("RunAsyncOnMainThread:");

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
                MarkDone();
            });
        }

        [RelayCommand]
        private async Task RunOnMainThreadAwaitable()
        {
            LogText = string.Empty;
            await MarkStart("RunOnMainThreadAsync:");
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

                    MarkDone();
                });
                DoWork(3000);
            });
        }

        [RelayCommand]
        private async Task RunAsyncOnMainThreadAwaitable()
        {
            LogText = string.Empty;
            await MarkStart("RunAsyncOnMainThreadAsync:");
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

                    MarkDone();
                });
                DoWork(3000);
            });
        }

        [RelayCommand]
        private async Task EnqueueOnMainThread()
        {
            LogText = string.Empty;
            await MarkStart("EnqueueOnMainThread:");
            Log("Starting new task");

            _ = Task.Run(() =>
            {
                DoWork(3000);
                Log("Enqueuing on main thread");
                _dispatcher.EnqueueOnMainThread(() =>
                {
                    Log("Starting enqueued work");
                    DoWork(3000);
                    MarkDone();
                });
                DoWork(3000);
                Log("Working thread is done");
            });

            Log("Starting work on UI thread");
            DoWork(9000);
        }

        [RelayCommand]
        private async Task EnqueueAsyncOnMainThread()
        {
            LogText = string.Empty;
            await MarkStart("EnqueueAsyncOnMainThread:");
            Log("Starting new task");

            _ = Task.Run(() =>
            {
                DoWork(3000);
                Log("Enqueuing on main thread");
                _dispatcher.EnqueueOnMainThread(async () =>
                {
                    Log("Starting enqueued work");
                    Log("Awaiting work on UI thread: 3s");
                    await Task.Run(() =>
                    {
                        DoWork(3000);
                    });
                    Log("Work awaited on UI thread: 3s");
                    MarkDone();
                });
                DoWork(3000);
                Log("Working thread is done");
            });

            Log("Starting work on UI thread");
            DoWork(9000);
        }

        [RelayCommand]
        private async Task Yield()
        {
            LogText = string.Empty;
            await MarkStart("Yield:");

            Log("Enqueuing on Main thread");
            _dispatcher.EnqueueOnMainThread(() =>
            {
                Log("Starting enqueued work");
                DoWork(1000);
            });
            DoWork(3000);
            Log("Yield");
            await _dispatcher.Yield();
            DoWork(2000);

            MarkDone();
        }

        private void DoWork(int msDuration)
        {
            Log($"Doing work: {msDuration} ms");
            Thread.Sleep(msDuration);
            Log($"Work done: {msDuration} ms");
        }

        private void Log(string message)
        {
            int id = Environment.CurrentManagedThreadId;
            string threadName = id == 1 ? "UI" : $"#{id}";
            string timestamp = DateTimeOffset.Now.ToString("HH: mm: ss: fff");
            _dispatcher.EnqueueOnMainThread(() => LogText += $"[Thread {threadName}][{timestamp}]: {message}" + Environment.NewLine);
        }

        private async Task MarkStart(string title)
        {
            IsBusy = true;
            LogText += title + Environment.NewLine + "------------------------------" + Environment.NewLine;
            await _dispatcher.Yield();
        }

        private void MarkDone() => _dispatcher.EnqueueOnMainThread(() =>
        {
            LogText += Environment.NewLine + "DONE!";
            IsBusy = false;
        });
    }
}
