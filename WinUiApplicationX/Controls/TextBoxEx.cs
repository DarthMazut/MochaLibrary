using Microsoft.UI.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Imaging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Core;

namespace WinUiApplicationX.Controls
{
    public class TextBoxEx : TextBox
    {
        private static readonly string[] _validImageExtensions = [".jpg", ".png", ".gif", ".bmp", ".svg"];
        private Button? _gazeButton;
        private Button? _commandButton;
        private Image? _previewImage;

        #region DEPENDENCY PROPERTIES

        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register(nameof(Command), typeof(ICommand), typeof(TextBoxEx), new PropertyMetadata(null, (d, e) =>
            {
                if (d is TextBoxEx thisControl)
                {
                    if (thisControl._commandButton is not null)
                    {
                        thisControl._commandButton.Visibility = e.NewValue is null ? Visibility.Collapsed : Visibility.Visible;
                    }
                }
            }));

        public static readonly DependencyProperty CommandButtonGlyphProperty =
            DependencyProperty.Register(nameof(CommandButtonGlyph), typeof(string), typeof(TextBoxEx), new PropertyMetadata("\uED25"));

        public string CommandButtonGlyph
        {
            get => (string)GetValue(CommandButtonGlyphProperty);
            set => SetValue(CommandButtonGlyphProperty, value);
        }

        public ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

        #endregion

        public TextBoxEx()
        {
            this.DefaultStyleKey = typeof(TextBoxEx);
            RegisterPropertyChangedCallback(TextProperty, OnTextChanged);
            RegisterPropertyChangedCallback(FocusStateProperty, OnFocusChanged);
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _gazeButton = (Button)GetTemplateChild("GazeButton");
            _commandButton = (Button)GetTemplateChild("CommandButton");
            _previewImage = (Image)GetTemplateChild("PreviewImage");

            _commandButton.Visibility = Command is null ? Visibility.Collapsed : Visibility.Visible;

            HandlePointerOver();
        }

        private void OnFocusChanged(DependencyObject sender, DependencyProperty dp) => HandleGazeButton(false);

        private void OnTextChanged(DependencyObject sender, DependencyProperty dp) => HandleGazeButton(true);

        private void HandleGazeButton(bool handleImageSource)
        {
            if (_gazeButton is null || _previewImage is null)
            {
                return;
            }

            string currentText = Text;
            bool isProperImage = CheckIsImageExtension(currentText, out bool isSvg) && File.Exists(currentText);
            bool shouldDisplayGazeButton = isProperImage && FocusState != FocusState.Unfocused;
            _gazeButton.Visibility = shouldDisplayGazeButton ? Visibility.Visible : Visibility.Collapsed;
            if (isProperImage && handleImageSource)
            {
                _previewImage.Source = isSvg ?
                    new SvgImageSource(new Uri(currentText)) :
                    new BitmapImage(new Uri(currentText));
            }
        }

        private bool CheckIsImageExtension(string path, out bool isSvg)
        {
            string ext = Path.GetExtension(path);
            isSvg = ext == _validImageExtensions[^1];
            return _validImageExtensions.Contains(Path.GetExtension(path));
        }

        private void HandlePointerOver()
        {
            if (_gazeButton is null || _commandButton is null)
            {
                return;
            }

            _gazeButton.PointerEntered += (s, e) =>
            {
                ProtectedCursor = InputSystemCursor.Create(InputSystemCursorShape.Arrow);
            };
            _gazeButton.PointerExited += (s, e) =>
            {
                ProtectedCursor = InputSystemCursor.Create(InputSystemCursorShape.IBeam);
            };

            _commandButton.PointerEntered += (s, e) =>
            {
                ProtectedCursor = InputSystemCursor.Create(InputSystemCursorShape.Arrow);
            };
            _commandButton.PointerExited += (s, e) =>
            {
                ProtectedCursor = InputSystemCursor.Create(InputSystemCursorShape.IBeam);
            };
        }
    }
}
