using MochaCore.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MochaCoreWinUI.Settings
{
    public class ApplicationSettingsSection<T> : ISettingsSection<T> where T : class, new()
    {
        public T Load()
        {
            throw new NotImplementedException();
        }

        public Task<T> LoadAsync()
        {
            throw new NotImplementedException();
        }

        public void RestoreDefaults()
        {
            throw new NotImplementedException();
        }

        public Task RestoreDefaultsAsync()
        {
            throw new NotImplementedException();
        }

        public void Save(T settings)
        {
            throw new NotImplementedException();
        }

        public Task SaveAsync(T settings)
        {
            throw new NotImplementedException();
        }

        public void Update(Action<T> updateAction)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Action<T> updateAction)
        {
            throw new NotImplementedException();
        }
    }
}
