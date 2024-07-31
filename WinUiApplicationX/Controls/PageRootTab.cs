using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Markup;

namespace WinUiApplicationX.Controls
{
    [ContentProperty(Name = nameof(Content))]
    public class PageRootTab : DependencyObject
    {
        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.Register(nameof(Header), typeof(object), typeof(PageRootTab), new PropertyMetadata(null));

        public static readonly DependencyProperty ContentProperty =
            DependencyProperty.Register(nameof(Content), typeof(object), typeof(PageRootTab), new PropertyMetadata(null));

        public object? Content
        {
            get { return (object?)GetValue(ContentProperty); }
            set { SetValue(ContentProperty, value); }
        }

        public object? Header
        {
            get { return (object?)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }
    }
}
