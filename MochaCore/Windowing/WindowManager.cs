using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MochaCore.Windowing
{

    public static class WindowManager
    {
        private static readonly Dictionary<string, Func<IWindowModule>> _builders = new();
        private static readonly List<IWindowModule> _createdModules = new();

        public static void RegisterWindow(string id, Func<IWindowModule> moduleBuilder)
        {
            if (_builders.ContainsKey(id))
            {
                throw new ArgumentException($"Module with id={id} was already registered.");
            }

            _builders.Add(id, moduleBuilder);
        }

        public static IWindowModule RetrieveWindow(string id)
        {
            if (!_builders.ContainsKey(id))
            {
                throw new ArgumentException($"Module with id={id} was never registered.");
            }

            IWindowModule module = _builders[id].Invoke();
            _createdModules.Add(module);
            module.Disposed += (s, e) => _createdModules.Remove(module);
            return module;
        }
    }
}
