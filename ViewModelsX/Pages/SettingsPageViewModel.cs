using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MochaCore.Navigation;
using MochaCore.Settings;
using ModelX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ViewModelsX.Pages
{
    public partial class SettingsPageViewModel : ObservableObject, INavigationParticipant, IOnNavigatedToAsync
    {
        private readonly ISettingsSectionProvider<Settings> _settingsProvider;

        public INavigator Navigator { get; } = MochaCore.Navigation.Navigator.Create();

        public SettingsPageViewModel()
        {
            _settingsProvider = SettingsManager.Retrieve<Settings>("Settings");
        }

        [ObservableProperty]
        private bool _isSwitched;

        [ObservableProperty]
        private SettingsOptionType _dropDownSelectedItem = SettingsOptionType.Option1Enum;

        [ObservableProperty]
        private string? _text;

        [ObservableProperty]
        private string? _password;

        [ObservableProperty]
        private string? _cryptoText;

        public async Task OnNavigatedToAsync(OnNavigatedToEventArgs e)
        {
            try
            {
                Settings settings = await _settingsProvider.LoadAsync(LoadingMode.FromOriginalSource);
                IsSwitched = settings.Switch1;
                DropDownSelectedItem = settings.OptionType;
                Text = settings.Text;
                CryptoText = settings.CryptoText;
            }
            catch (IOException)
            {
                // TODO: handle
                throw;
            }
        }

        [RelayCommand]
        private async Task Save()
        {
            try
            {
                await _settingsProvider.UpdateAsync(s =>
                {
                    s.Switch1 = IsSwitched;
                    s.OptionType = DropDownSelectedItem;
                    s.Text = Text;
                }, LoadingMode.FromOriginalSource, SavingMode.ToOriginalSource);
            }
            catch (IOException)
            {
                // TODO: handle
                throw;
            }
        }

        [RelayCommand]
        private async Task Restore()
        {
            try
            {
                await _settingsProvider.RestoreDefaultsAsync(SavingMode.ToOriginalSource);
            }
            catch (IOException)
            {
                // TODO: handle
                throw;
            }
        }

        [RelayCommand]
        private async Task Decrypt()
        {
            if (CryptoText is null || Password is null)
            {
                return;
            }

            CryptoText = Decrypt(CryptoText, Password);
        }

        [RelayCommand]
        private async Task Encrypt()
        {
            if (CryptoText is null || Password is null)
            {
                return;
            }

            string encryptedText = Encrypt(CryptoText, Password);
            await _settingsProvider.UpdateAsync(s => s.CryptoText = encryptedText);
        }

        public static string Encrypt(string plainText, string password)
        {
            using (Aes aes = Aes.Create())
            {
                // Derive a key and IV from the password
                using (Rfc2898DeriveBytes keyGenerator = new Rfc2898DeriveBytes(password, Encoding.UTF8.GetBytes("SaltIsGoodForYou")))
                {
                    aes.Key = keyGenerator.GetBytes(32); // AES-256 key
                    aes.IV = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16];
                }

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, aes.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        using (StreamWriter streamWriter = new StreamWriter(cryptoStream))
                        {
                            streamWriter.Write(plainText);
                        }
                    }
                    return Convert.ToBase64String(memoryStream.ToArray());
                }
            }
        }

        public static string Decrypt(string cipherText, string password)
        {
            try
            {
                using (Aes aes = Aes.Create())
                {
                    // Derive a key and IV from the password
                    using (Rfc2898DeriveBytes keyGenerator = new Rfc2898DeriveBytes(password, Encoding.UTF8.GetBytes("SaltIsGoodForYou")))
                    {
                        aes.Key = keyGenerator.GetBytes(32); // AES-256 key
                        aes.IV = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16];  // AES block size is 16 bytes
                    }

                    using (MemoryStream memoryStream = new MemoryStream(Convert.FromBase64String(cipherText)))
                    {
                        using (CryptoStream cryptoStream = new CryptoStream(memoryStream, aes.CreateDecryptor(), CryptoStreamMode.Read))
                        {
                            using (StreamReader streamReader = new StreamReader(cryptoStream))
                            {
                                return streamReader.ReadToEnd();
                            }
                        }
                    }
                }
            }
            catch (CryptographicException)
            {
                // Return corrupted data
                byte[] corruptedBytes = Convert.FromBase64String(cipherText);
                string corruptedString = Encoding.UTF8.GetString(corruptedBytes);
                return corruptedString;
            }
        }
    }
}
