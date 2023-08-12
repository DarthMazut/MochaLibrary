using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MochaCore.Windowing
{
    /// <summary>
    /// Handles registering and retrieval of <see cref="IWindowModule"/> objects.
    /// Provides methods for tracking modules in use.
    /// </summary>
    public static class WindowManager
    {
        private static readonly Dictionary<string, Func<IWindowModule>> _builders = new();
        private static readonly Dictionary<string, List<IWindowModule>> _createdModules = new();

        /// <summary>
        /// Registers <see cref="IWindowModule"/> builder delegate.
        /// </summary>
        /// <param name="id">Unique identifier of registering builder delegate</param>
        /// <param name="moduleBuilder">Builder delegate.</param>
        public static void RegisterWindow(string id, Func<IWindowModule> moduleBuilder)
        {
            if (_builders.ContainsKey(id))
            {
                throw new ArgumentException($"Module with id={id} was already registered.");
            }

            _builders.Add(id, moduleBuilder);
        }

        /// <summary>
        /// Returns registered implementation of <see cref="IWindowModule"/> with specified id.
        /// </summary>
        /// <param name="id">Identifier of module which implementation is to be retrieved.</param>
        public static IWindowModule RetrieveWindow(string id)
        {
            if (!_builders.ContainsKey(id))
            {
                throw new ArgumentException($"Module with id={id} was never registered.");
            }

            IWindowModule module = _builders[id].Invoke();
            TrackModule(id, module);
            return module;
        }

        /// <summary>
        /// Returns a <see cref="IReadOnlyCollection{T}"/> of instantiated modules.
        /// </summary>
        public static IReadOnlyCollection<IWindowModule> GetCreatedModules()
            => _createdModules.Values.SelectMany(l => l).ToList().AsReadOnly();

        /// <summary>
        /// Returns a <see cref="IReadOnlyCollection{T}"/> of instantiated modules.
        /// </summary>
        /// <param name="id">Identifer of the modules to be included.</param>
        public static IReadOnlyCollection<IWindowModule> GetCreatedModules(string id)
        {
            if (!_builders.ContainsKey(id))
            {
                throw new ArgumentException($"Module with id={id} was never registered.");
            }

            if (!_createdModules.ContainsKey(id))
            {
                return Enumerable.Empty<IWindowModule>().ToList().AsReadOnly();
            }

            return _createdModules[id].ToList().AsReadOnly();
        }

        /// <summary>
        /// Returns a <see cref="IReadOnlyCollection{T}"/> of modules which associated windows are currently open.
        /// </summary>
        public static IReadOnlyCollection<IWindowModule> GetOpenedModules()
            => _createdModules.Values.SelectMany(l => l).Where(m => m.IsOpen).ToList().AsReadOnly();

        /// <summary>
        /// Returns a <see cref="IReadOnlyCollection{T}"/> of modules which associated windows are currently open.
        /// </summary>
        /// <param name="id">Identifer of the modules to be included.</param>
        public static IReadOnlyCollection<IWindowModule> GetOpenedModules(string id)
        {
            if (!_builders.ContainsKey(id))
            {
                throw new ArgumentException($"Module with id={id} was never registered.");
            }

            if (!_createdModules.ContainsKey(id))
            {
                return Enumerable.Empty<IWindowModule>().ToList().AsReadOnly();
            }

            return _createdModules[id].Where(m => m.IsOpen).ToList().AsReadOnly();
        }

        /// <summary>
        /// Searches for <see cref="IWindowModule"/> instance associated with given <see cref="IWindowAware"/> object.
        /// </summary>
        /// <returns>Found instance or <see langword="null"/> if not such was found.</returns>
        public static IWindowModule? FindCorrespondingWindowModule(IWindowAware dataContext)
            => _createdModules.Values.SelectMany(l => l).FirstOrDefault(m => m.DataContext == dataContext);

        private static void TrackModule(string id, IWindowModule module)
        {
            if (_createdModules.ContainsKey(id))
            {
                _createdModules[id].Add(module);
            }
            else
            {
                _createdModules.Add(id, new List<IWindowModule>() { module });
            }

            module.Disposed += ModuleDisposed;
            void ModuleDisposed(object? sender, EventArgs e)
            {
                UntrackModule(id, module);
                module.Disposed -= ModuleDisposed;
            }
        }

        private static void UntrackModule(string id, IWindowModule module)
        {
            _createdModules[id].Remove(module);
        }
    }
}
