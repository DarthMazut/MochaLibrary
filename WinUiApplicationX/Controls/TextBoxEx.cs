using Microsoft.UI.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core;

namespace WinUiApplicationX.Controls
{
    public class TextBoxEx : TextBox
    {
        public static readonly DependencyProperty DeleteButtonGlyphProperty =
            DependencyProperty.Register(nameof(DeleteButtonGlyph), typeof(string), typeof(TextBoxEx), new PropertyMetadata("\uE894"));

        public static readonly DependencyProperty GazeButtonGlyphProperty =
            DependencyProperty.Register(nameof(GazeButtonGlyph), typeof(string), typeof(TextBoxEx), new PropertyMetadata("\uE7B3"));

        public string DeleteButtonGlyph
        {
            get => (string)GetValue(DeleteButtonGlyphProperty);
            set => SetValue(DeleteButtonGlyphProperty, value);
        }

        public string GazeButtonGlyph
        {
            get => (string)GetValue(GazeButtonGlyphProperty);
            set => SetValue(GazeButtonGlyphProperty, value);
        }

        public TextBoxEx()
        {
            this.DefaultStyleKey = typeof(TextBoxEx);
            ProtectedCursor = InputSystemCursor.Create(InputSystemCursorShape.Arrow);
        }
    }
}
