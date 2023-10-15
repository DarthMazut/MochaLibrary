using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MochaCore.Windowing.Extensions.DI
{
    /// <summary>
    /// Provides implementation of <see cref="IWindowFactory"/>.
    /// </summary>
    public class WindowFactory : IWindowFactory
    {
        /// <inheritdoc/>
        public IBaseWindowModule? FindCorrespondingWindowModule(IBaseWindowAware dataContext)
            => WindowManager.FindCorrespondingWindowModule(dataContext);

        /// <inheritdoc/>
        public IReadOnlyCollection<IBaseWindowModule> GetCreatedModules()
            => WindowManager.GetCreatedModules();

        /// <inheritdoc/>
        public IReadOnlyCollection<IBaseWindowModule> GetCreatedModules(string id)
            => WindowManager.GetCreatedModules(id);

        /// <inheritdoc/>
        public IReadOnlyCollection<IBaseWindowModule> GetOpenedModules()
            => WindowManager.GetOpenedModules();

        /// <inheritdoc/>
        public IReadOnlyCollection<IBaseWindowModule> GetOpenedModules(string id)
            => WindowManager.GetOpenedModules(id);

        /// <inheritdoc/>
        public IWindowModule RetrieveWindow(string id)
            => WindowManager.RetrieveWindow(id);

        /// <inheritdoc/>
        public IWindowModule<T> RetrieveWindow<T>(string id) where T : class, new()
            => WindowManager.RetrieveWindow<T>(id);

        /// <inheritdoc/>
        public IBaseWindowModule RetrieveBaseWindow(string id)
            => WindowManager.RetrieveBaseWindow(id);

        /// <inheritdoc/>
        public IBaseWindowModule<T> RetrieveBaseWindow<T>(string id) where T : class, new()
            => WindowManager.RetrieveBaseWindow<T>(id);
    }
}
