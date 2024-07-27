using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Documents;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Markup;
using Microsoft.UI.Xaml.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace WinUiApplicationX.Controls
{
    [ContentProperty(Name = nameof(Content))]
    public sealed class PageRoot : Control
    {
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register(nameof(Title), typeof(string), typeof(PageRoot), new PropertyMetadata(null));
        public static readonly DependencyProperty GlyphProperty =
            DependencyProperty.Register(nameof(Glyph), typeof(string), typeof(PageRoot), new PropertyMetadata(null));
        public static readonly DependencyProperty ContentProperty =
            DependencyProperty.Register(nameof(Content), typeof(object), typeof(PageRoot), new PropertyMetadata(null));
        public static readonly DependencyProperty FooterProperty =
            DependencyProperty.Register(nameof(Footer), typeof(object), typeof(PageRoot), new PropertyMetadata(null));
        public static readonly DependencyProperty HeaderPaddingProperty =
            DependencyProperty.Register(nameof(HeaderPadding), typeof(Thickness), typeof(PageRoot), new PropertyMetadata(new Thickness()));
        public static readonly DependencyProperty HeaderMarginProperty =
            DependencyProperty.Register(nameof(HeaderMargin), typeof(Thickness), typeof(PageRoot), new PropertyMetadata(new Thickness()));
        public static readonly DependencyProperty HeaderBorderThicknessProperty =
            DependencyProperty.Register(nameof(HeaderBorderThickness), typeof(Thickness), typeof(PageRoot), new PropertyMetadata(new Thickness()));
        public static readonly DependencyProperty ContentScrollViewerPaddingProperty =
            DependencyProperty.Register(nameof(ContentScrollViewerPadding), typeof(Thickness), typeof(PageRoot), new PropertyMetadata(new Thickness()));
        public static readonly DependencyProperty HeaderBorderBrushProperty =
            DependencyProperty.Register(nameof(HeaderBorderBrush), typeof(Brush), typeof(PageRoot), new PropertyMetadata(null));
        public static readonly DependencyProperty HeaderStyleProperty =
            DependencyProperty.Register(nameof(HeaderStyle), typeof(Style), typeof(PageRoot), new PropertyMetadata(null));
        public static readonly DependencyProperty IconStyleProperty =
            DependencyProperty.Register(nameof(IconStyle), typeof(Style), typeof(PageRoot), new PropertyMetadata(null));

        public static readonly DependencyProperty ShowTabsProperty =
            DependencyProperty.Register(nameof(ShowTabs), typeof(bool), typeof(PageRoot), new PropertyMetadata(false, ShowTabsChanged));

        private static void ShowTabsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is PageRoot thisControl)
            {
                VisualStateManager.GoToState(thisControl, thisControl.ShowTabs ? "DisplayTabs" : "HideTabs", false);
            }
        }

        public string? Title
        {
            get => (string?)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }

        public string? Glyph
        {
            get { return (string?)GetValue(GlyphProperty); }
            set { SetValue(GlyphProperty, value); }
        }

        public object? Footer
        {
            get { return (object?)GetValue(FooterProperty); }
            set { SetValue(FooterProperty, value); }
        }

        public object? Content
        {
            get { return (object?)GetValue(ContentProperty); }
            set { SetValue(ContentProperty, value); }
        }

        public Thickness HeaderPadding
        {
            get { return (Thickness)GetValue(HeaderPaddingProperty); }
            set { SetValue(HeaderPaddingProperty, value); }
        }

        public Thickness HeaderMargin
        {
            get { return (Thickness)GetValue(HeaderMarginProperty); }
            set { SetValue(HeaderMarginProperty, value); }
        }

        public Thickness HeaderBorderThickness
        {
            get { return (Thickness)GetValue(HeaderBorderThicknessProperty); }
            set { SetValue(HeaderBorderThicknessProperty, value); }
        }

        public Thickness ContentScrollViewerPadding
        {
            get { return (Thickness)GetValue(ContentScrollViewerPaddingProperty); }
            set { SetValue(ContentScrollViewerPaddingProperty, value); }
        }

        public Brush HeaderBorderBrush
        {
            get { return (Brush)GetValue(HeaderBorderBrushProperty); }
            set { SetValue(HeaderBorderBrushProperty, value); }
        }

        public Style HeaderStyle
        {
            get { return (Style)GetValue(HeaderStyleProperty); }
            set { SetValue(HeaderStyleProperty, value); }
        }

        public Style IconStyle
        {
            get { return (Style)GetValue(IconStyleProperty); }
            set { SetValue(IconStyleProperty, value); }
        }

        public bool ShowTabs
        {
            get { return (bool)GetValue(ShowTabsProperty); }
            set { SetValue(ShowTabsProperty, value); }
        }

        public PageRoot()
        {
            this.DefaultStyleKey = typeof(PageRoot);
        }
    }
}
