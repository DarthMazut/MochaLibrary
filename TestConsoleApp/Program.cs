using MochaCore.Utils;
using Prism.Mvvm;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;

namespace TestConsoleApp
{
    public class VM : BindableBase
    {
        private readonly AsyncProperty<string> _myDynamicProperty;

        public VM()
        {
            _myDynamicProperty = new(this, nameof(MyDynamicProperty));
            //_myDynamicProperty.SynchronizedProperties.Add(nameof(MyPrismProperty));
        }

        public string MyDynamicProperty
        {
            get => _myDynamicProperty.Get();
            set => _myDynamicProperty.Set(value);
        }

        private string _myPrismProperty;
        public string MyPrismProperty
        {
            get => _myPrismProperty;
            set => SetProperty(ref _myPrismProperty, value);
        }
    }

    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Press any key to start:");
            _ = Console.ReadKey(false);
            VM vm = new();
            vm.PropertyChanged += (s, e) =>
            {

            };

            //for (int i = 0; i < 5; i++)
            //{
            //    Task.Run(() =>
            //    {
            //        for (int i = 0; i < 1_000; i++)
            //        {
            //            vm.MyProperty = i.ToString();
            //        }
            //    });
            //}

            Stopwatch stopwatch = new();
            stopwatch.Start();
            for (int i = 0; i < 50_000_000; i++)
            {
                vm.MyDynamicProperty = i.ToString();
            }
            stopwatch.Stop();
            Console.WriteLine($"Result: {stopwatch.Elapsed}");
            Console.WriteLine("Press any key to exit...");
            _ = Console.ReadKey(false);
        }
    }
}
