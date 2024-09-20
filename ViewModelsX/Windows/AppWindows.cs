using MochaCore.Windowing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModelsX.Windows
{
    public static class AppWindows
    {
        public static AppWindow<IWindowModule> MainWindow { get; } = new("MainWindow");

        public static AppWindow<IWindowModule<WindowingGeneralWindowProperties>> WindowingGeneralWindow { get; } = new("WindowingGeneralWindow");
    }

    public class AppWindow<T>(string id) where T : IBaseWindowModule
    {
        public string Id { get; } = id;

        public T Module => (T)WindowManager.RetrieveWindow(Id);
    }
}
