using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MochaCore.Windowing.Extensions.DI
{
    /// <summary>
    /// Provides selected functionalities of static <see cref="WindowManager"/> class.
    /// Use this interface when choosing DI approach.
    /// </summary>
    public interface IWindowFactory
    {
        /// <summary>
        /// Returns registered implementation of <see cref="IBaseWindowModule"/> with specified id.
        /// </summary>
        /// <param name="id">Identifier of module which implementation is to be retrieved.</param>
        public IBaseWindowModule RetrieveBaseWindow(string id);

        /// <summary>
        /// Returns registered implementation of <see cref="IBaseWindowModule{T}"/> with specified id.
        /// </summary>
        /// <typeparam name="T">Type of module properties.</typeparam>
        /// <param name="id">Identifier of module which implementation is to be retrieved.</param>
        public IBaseWindowModule<T> RetrieveBaseWindow<T>(string id) where T : class, new();

        /// <summary>
        /// Returns registered implementation of <see cref="IWindowModule"/> with specified id.
        /// </summary>
        /// <param name="id">Identifier of module which implementation is to be retrieved.</param>
        public IWindowModule RetrieveWindow(string id);

        /// <summary>
        /// Returns registered implementation of <see cref="IWindowModule{T}"/> with specified id.
        /// </summary>
        /// <typeparam name="T">Type of module properties.</typeparam>
        /// <param name="id">Identifier of module which implementation is to be retrieved.</param>
        public IWindowModule<T> RetrieveWindow<T>(string id) where T : class, new();

        /// <summary>
        /// Returns a <see cref="IReadOnlyCollection{T}"/> of instantiated modules.
        /// </summary>
        public IReadOnlyCollection<IBaseWindowModule> GetCreatedModules();

        /// <summary>
        /// Returns a <see cref="IReadOnlyCollection{T}"/> of instantiated modules.
        /// </summary>
        /// <param name="id">Identifer of the modules to be included.</param>
        public IReadOnlyCollection<IBaseWindowModule> GetCreatedModules(string id);

        /// <summary>
        /// Returns a <see cref="IReadOnlyCollection{T}"/> of modules which associated windows are currently open.
        /// </summary>
        public IReadOnlyCollection<IBaseWindowModule> GetOpenedModules();

        /// <summary>
        /// Returns a <see cref="IReadOnlyCollection{T}"/> of modules which associated windows are currently open.
        /// </summary>
        /// <param name="id">Identifer of the modules to be included.</param>
        public IReadOnlyCollection<IBaseWindowModule> GetOpenedModules(string id);

        /// <summary>
        /// Searches for <see cref="IBaseWindowModule"/> instance associated with given <see cref="IBaseWindowAware"/> object.
        /// </summary>
        /// <returns>Found instance or <see langword="null"/> if not such was found.</returns>
        public IBaseWindowModule? FindCorrespondingWindowModule(IBaseWindowAware dataContext);
    }
}
