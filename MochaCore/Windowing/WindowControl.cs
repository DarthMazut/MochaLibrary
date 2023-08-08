using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MochaCore.Windowing
{
    public class WindowControl : IWindowControl
    {
        private IWindowModule? _module;
        private bool _isInitialized = false;

        public void Initialize(IWindowModule module)
        {
            _module = module;
            _isInitialized = true;
        }

        public void Uninitialize()
        {
            _module = null;
            _isInitialized = false;
        }

        /// <inheritdoc/>
        public void Close()
        {
            InitializationGuard();

            _module!.Close();
        }

        private void InitializationGuard()
        {
            if (!_isInitialized)
            {
                throw new InvalidOperationException($"");
            }
        }
    }
}
