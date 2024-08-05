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
        private static readonly string _tabsStateKey = "Tabs";
        private static readonly string _contentStateKey = "Normal";

        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register(nameof(Title), typeof(string), typeof(PageRoot), new PropertyMetadata(null));
        public static readonly DependencyProperty GlyphProperty =
            DependencyProperty.Register(nameof(Glyph), typeof(string), typeof(PageRoot), new PropertyMetadata(null));
        public static readonly DependencyProperty ContentProperty =
            DependencyProperty.Register(nameof(Content), typeof(object), typeof(PageRoot), new PropertyMetadata(null));
        public static readonly DependencyProperty FooterProperty =
            DependencyProperty.Register(nameof(Footer), typeof(object), typeof(PageRoot), new PropertyMetadata(null));
        public static readonly DependencyProperty HeaderStyleProperty =
            DependencyProperty.Register(nameof(HeaderStyle), typeof(Style), typeof(PageRoot), new PropertyMetadata(null));
        public static readonly DependencyProperty IconStyleProperty =
            DependencyProperty.Register(nameof(IconStyle), typeof(Style), typeof(PageRoot), new PropertyMetadata(null));
        public static readonly DependencyProperty TabsProperty =
            DependencyProperty.Register(nameof(Tabs), typeof(IList<object>), typeof(PageRoot), new PropertyMetadata(null));
        public static readonly DependencyProperty SelectedTabProperty =
            DependencyProperty.Register(nameof(SelectedTab), typeof(object), typeof(PageRoot), new PropertyMetadata(null));
        

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

        public IList<object> Tabs
        {
            get { return (IList<object>)GetValue(TabsProperty); }
            set { SetValue(TabsProperty, value); }
        }

        public object? SelectedTab
        {
            get { return GetValue(SelectedTabProperty); }
            set { SetValue(SelectedTabProperty, value); }
        }

        public PageRoot()
        {
            this.DefaultStyleKey = typeof(PageRoot);
            Tabs = [];
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            VisualStateManager.GoToState(this, Tabs.Count == 0 ? _contentStateKey : _tabsStateKey, false);
        }
    }
}
