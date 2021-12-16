using MochaCore.Dispatching;
using MochaCore.Navigation;
using MochaCore.Utils;
using System;
using System.ComponentModel;
using System.Threading.Tasks;

namespace MochaCoreWinUITestApp.ViewModels
{
    public class Page1ViewModel : INavigatable, INotifyPropertyChanged
    {
        private static readonly Random _rnd = new();

        private readonly DynamicProperty<string> _asyncOperationResult = new(nameof(AsyncOperationResult));
        private readonly DynamicProperty<int> _selectedIndex = new(nameof(SelectedIndex))
        {
            PropertyChangedCallback = (e) => (e.Host as Page1ViewModel).AsyncOperationResult = $"PropertyChangedCallback executed! ({e.NewValue+1})",
            AsyncOperation = async (token, e) =>
            {
                await Task.Delay(5000, token);
                return new object();
            },
            AsyncOperationCallback = (result, e) => (e.Host as Page1ViewModel).AsyncOperationResult = $"Async callback finished ({e.NewValue+1})",
            Dispatcher = DispatcherManager.GetMainThreadDispatcher()
        };

        public Page1ViewModel()
        {
            Navigator = new(this, NavigationServices.MainNavigationService);

            _ = Task.Run(async () =>
            {
                try
                {
                    for (int i = 0; i < 100; i++)
                    {
                        DispatcherManager.GetMainThreadDispatcher().EnqueueOnMainThread(() =>
                        {
                            SelectedIndex = i % 10;
                        });
                        await Task.Delay(100);
                    }
                }
                catch (Exception ex)
                {

                    throw;
                }
            });
        }

        public string AsyncOperationResult
        {
            get => _asyncOperationResult.Get();
            set => _asyncOperationResult.Set(this, value);
        }

        public int SelectedIndex
        {
            get => _selectedIndex.Get();
            set => _selectedIndex.Set(this, value);
        }

        private int _selectedIndexOld;
        public int SelectedIndexOld
        {
            get => _selectedIndexOld;
            set
            {
                if (value != _selectedIndexOld)
                {
                    _selectedIndexOld = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedIndexOld)));
                }
            }

        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public Navigator Navigator { get; }


    }
}
