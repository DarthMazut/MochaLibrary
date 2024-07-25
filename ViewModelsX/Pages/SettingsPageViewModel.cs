using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MochaCore.Dialogs;
using MochaCore.Dialogs.Extensions;
using MochaCore.Events;
using MochaCore.Events.Extensions;
using MochaCore.Navigation;
using MochaCore.Settings;
using ModelX;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ViewModelsX.Pages
{
    public partial class SettingsPageViewModel : ObservableObject, INavigationParticipant, IOnNavigatedToAsync, IOnNavigatingFromAsync
    {
        private readonly ISettingsSectionProvider<PizzaRecipe> _settingsProvider;
        private readonly IEventProvider<AppClosingEventArgs> _appClosingEventProvider;

        public INavigator Navigator { get; } = MochaCore.Navigation.Navigator.Create();

        public SettingsPageViewModel()
        {
            _settingsProvider = SettingsManager.Retrieve<PizzaRecipe>("Settings");
            _appClosingEventProvider = AppEventManager.RequestEventProvider<AppClosingEventArgs>("AppClosing");

            _appClosingEventProvider.SubscribeAsync(new AsyncEventHandler<AppClosingEventArgs>(ApplicationClosing));
        }

        [ObservableProperty]
        private PizzaStyle _pizzaStyle;

        [ObservableProperty]
        private bool _isThickCrust;

        [ObservableProperty]
        private FlourType _flourType;

        [ObservableProperty]
        private double _flour;

        [ObservableProperty]
        private double _water;

        [ObservableProperty]
        private double _yeast;

        [ObservableProperty]
        private double _salt;

        [ObservableProperty]
        private ObservableCollection<Topping> _selectedToppings = [];

        public async Task OnNavigatedToAsync(OnNavigatedToEventArgs e)
        {
            try
            {
                PizzaRecipe settings = await _settingsProvider.LoadAsync(LoadingMode.FromOriginalSource);
                PizzaStyle = settings.Style;
                IsThickCrust = settings.IsThickCrust;
                FlourType = settings.FlourType;
                Flour = settings.Flour;
                Water = settings.Water;
                Yeast = settings.Yeast;
                Salt = settings.Salt;
                SelectedToppings = [..settings.Toppings];
                //BakingTemp = settings.BakingTemp;
                //Rating = settings.Rating;
                //Notes = settings.Notes;
            }
            catch (IOException ex)
            {
                await PromptSomethingWentWrong(ex);
            }
        }

        public async Task OnNavigatingFromAsync(OnNavigatingFromEventArgs e)
        {
            if (await CheckSettingsChanged())
            {
                bool? result = await PromptDiscardDialog();
                if (result is not true)
                {
                    e.Cancel = true;
                    return;
                }
            }

            _appClosingEventProvider.UnsubscribeAsync(ApplicationClosing);
        }

        [RelayCommand]
        private async Task Save()
        {
            try
            {
                await _settingsProvider.UpdateAsync(s =>
                {
                    s.Style = PizzaStyle;
                    s.IsThickCrust = IsThickCrust;
                    s.FlourType = FlourType;
                    s.Flour = Flour;
                    s.Water = Water;
                    s.Yeast = Yeast;
                    s.Salt = Salt;
                    s.Toppings = [..SelectedToppings];
                    //s.BakingTemp = BakingTemp;
                    //s.Rating = Rating;
                    //s.Notes = Notes;

                }, LoadingMode.FromOriginalSource, SavingMode.ToOriginalSource);
            }
            catch (IOException ex)
            {
                await PromptSomethingWentWrong(ex);
            }
        }

        [RelayCommand]
        private async Task Restore()
        {
            try
            {
                PizzaRecipe settings =  await _settingsProvider.RestoreDefaultsAsync(SavingMode.ToOriginalSource);
                PizzaStyle = settings.Style;
                IsThickCrust = settings.IsThickCrust;
                FlourType = settings.FlourType;
                Flour = settings.Flour;
                Water = settings.Water;
                Yeast = settings.Yeast;
                Salt = settings.Salt;
                SelectedToppings = [..settings.Toppings];
                //BakingTemp = settings.BakingTemp;
                //Rating = settings.Rating;
                //Notes = settings.Notes;
            }
            catch (IOException ex)
            {
                await PromptSomethingWentWrong(ex);
            }
        }

        private Task PromptSomethingWentWrong(Exception ex)
        {
            using IDialogModule<StandardMessageDialogProperties> dialog
                    = DialogManager.RetrieveDialog<StandardMessageDialogProperties>("MessageDialog");

            dialog.Properties = new()
            {
                Title = "Something went wrong",
                Message = ex.Message,
                ConfirmationButtonText = "Oh no!",
            };

            return dialog.ShowModalAsync(Navigator.Module.View);
        }

        private Task<bool?> PromptDiscardDialog()
        {
            using IDialogModule<StandardMessageDialogProperties> dialog
                    = DialogManager.RetrieveDialog<StandardMessageDialogProperties>("MessageDialog");

            dialog.Properties = new()
            {
                Title = "Discard changes?",
                Message = "Changes were made to the current settings, but no \"Save\" button was pressed." +
                "\n\nDo you want to leave anyway and discard the changes?",
                ConfirmationButtonText = "Leave & discard changes",
                DeclineButtonText = "Stay on current page"
            };

            return dialog.ShowModalAsync(Navigator.Module.View);
        }

        private async Task<bool> CheckSettingsChanged()
        {
            PizzaRecipe currentSettings = await _settingsProvider.LoadAsync();
            return
                currentSettings.Style != PizzaStyle ||
                currentSettings.IsThickCrust != IsThickCrust ||
                currentSettings.FlourType != FlourType ||
                currentSettings.Flour != Flour ||
                currentSettings.Water != Water ||
                currentSettings.Yeast != Yeast ||
                currentSettings.Salt != Salt ||
                currentSettings.Toppings.SequenceEqual(SelectedToppings) == false;
                //currentSettings.BakingTemp != BakingTemp;
                //currentSettings.Rating != Rating;
                //currentSettings.Notes != Notes;
        }

        private async Task ApplicationClosing(AppClosingEventArgs e, IReadOnlyCollection<AsyncEventHandler> collection)
        {
            if (await CheckSettingsChanged())
            {
                bool? result = await PromptDiscardDialog();
                if (result is not true)
                {
                    e.Cancel = true;
                }
            }
        }
    }
}
