using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MochaCore.Dialogs;
using MochaCore.Dialogs.Extensions;
using MochaCore.Events;
using MochaCore.Events.Extensions;
using MochaCore.Navigation;
using MochaCore.Settings;
using ModelX;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace ViewModelsX.Pages
{
    public partial class SettingsPageViewModel : ObservableObject, INavigationParticipant, IOnNavigatedToAsync, IOnNavigatingFromAsync
    {
        private readonly ISettingsSectionProvider<PizzaRecipe> _settingsProvider;
        private readonly IEventProvider<AppClosingEventArgs> _appClosingEventProvider;

        private PizzaRecipe? _currentSettings;

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

        [ObservableProperty]
        private int _bakingTemp;

        [ObservableProperty]
        private double _rating;

        [ObservableProperty]
        private string? _notes;

        [ObservableProperty]
        private bool _isSaveEnabled;

        [ObservableProperty]
        private bool _isRestoreEnabled;

        public async Task OnNavigatedToAsync(OnNavigatedToEventArgs e)
        {
            try
            {
                PizzaRecipe settings = await _settingsProvider.LoadAsync();
                _currentSettings = settings;
                AssignValues(settings);
            }
            catch (IOException ex)
            {
                await PromptSomethingWentWrong(ex);
            }
        }

        public async Task OnNavigatingFromAsync(OnNavigatingFromEventArgs e)
        {
            if (CheckSettingsChanged())
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

        protected override void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            UpdateIsEnabled();
        }

        partial void OnSelectedToppingsChanged(ObservableCollection<Topping>? oldValue, ObservableCollection<Topping> newValue)
        {
            if (oldValue is not null)
            {
                oldValue.CollectionChanged -= ToppingsCollectionChanged;
            }

            newValue.CollectionChanged += ToppingsCollectionChanged;
        }

        private void ToppingsCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e) => UpdateIsEnabled();

        [RelayCommand]
        private async Task Save()
        {
            try
            {
                _currentSettings = CreateSettings();
                await _settingsProvider.SaveAsync(_currentSettings);
                UpdateIsEnabled();
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
                _currentSettings = await _settingsProvider.RestoreDefaultsAsync(SavingMode.ToOriginalSource);
                AssignValues(_currentSettings);
                UpdateIsEnabled();
            }
            catch (IOException ex)
            {
                await PromptSomethingWentWrong(ex);
            }
        }

        private void UpdateIsEnabled()
        {
            IsSaveEnabled = CheckSettingsChanged();
            IsRestoreEnabled = !CreateSettings().Equals(new PizzaRecipe());
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

        private bool CheckSettingsChanged() => _currentSettings?.Equals(CreateSettings()) == false;

        private void AssignValues(PizzaRecipe recipe)
        {
            PizzaStyle = recipe.Style;
            IsThickCrust = recipe.IsThickCrust;
            FlourType = recipe.FlourType;
            Flour = recipe.Flour;
            Water = recipe.Water;
            Yeast = recipe.Yeast;
            Salt = recipe.Salt;
            SelectedToppings = [.. recipe.Toppings];
            BakingTemp = recipe.BakingTemp;
            Rating = recipe.Rating;
            Notes = recipe.Notes;
        }

        private PizzaRecipe CreateSettings() => new()
        {
            Style = PizzaStyle,
            IsThickCrust = IsThickCrust,
            FlourType = FlourType,
            Flour = Flour,
            Water = Water,
            Yeast = Yeast,
            Salt = Salt,
            Toppings = [.. SelectedToppings],
            BakingTemp = BakingTemp,
            Rating = Rating,
            Notes = Notes
        };

        private async Task ApplicationClosing(AppClosingEventArgs e, IReadOnlyCollection<AsyncEventHandler> collection)
        {
            if (CheckSettingsChanged())
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
