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

            CryptoText = await DecryptTextAsync(CryptoText, Password);
        }

        [RelayCommand]
        private async Task Encrypt()
        {
            if (CryptoText is null || Password is null)
            {
                return;
            }

            string encryptedText = await EncryptTextAsync(CryptoText, Password);
            await _settingsProvider.UpdateAsync(s => s.CryptoText = encryptedText);
        }

        private static async Task<string> EncryptTextAsync(string plainText, string password)
        {
            using Aes aes = Aes.Create();
            using Rfc2898DeriveBytes keyGenerator = new(password, Encoding.UTF8.GetBytes("SaltIsGoodForYou"), 1000, HashAlgorithmName.SHA1);
            aes.Key = keyGenerator.GetBytes(32);
            aes.IV = keyGenerator.GetBytes(16);

            using MemoryStream memoryStream = new();
            memoryStream.Write(aes.IV, 0, aes.IV.Length);
            using CryptoStream cryptoStream = new(memoryStream, aes.CreateEncryptor(), CryptoStreamMode.Write);
            using StreamWriter streamWriter = new(cryptoStream);
            await streamWriter.WriteAsync(plainText);
            return Convert.ToBase64String(memoryStream.ToArray());
        }

        private static Task<string> DecryptTextAsync(string cipherText, string password)
        {
            using Aes aes = Aes.Create();
            using Rfc2898DeriveBytes keyGenerator = new(password, Encoding.UTF8.GetBytes("SaltIsGoodForYou"), 1000, HashAlgorithmName.SHA1);

            aes.Key = keyGenerator.GetBytes(32);
            aes.IV = keyGenerator.GetBytes(16);

            using MemoryStream memoryStream = new(Convert.FromBase64String(cipherText));
            using CryptoStream cryptoStream = new(memoryStream, aes.CreateDecryptor(), CryptoStreamMode.Read);
            using StreamReader streamReader = new(cryptoStream);
            return streamReader.ReadToEndAsync();
        }


    }
}
