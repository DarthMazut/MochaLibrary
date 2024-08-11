using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation.Collections;

namespace MochaWinUI.Utils.Xaml
{
    public static class ParentsExtension
    {
        public static ParentAnchorCollection? GetAnchors(DependencyObject obj)
            => (ParentAnchorCollection)obj.GetValue(AnchorsProperty);

        

        public static void SetAnchors(DependencyObject obj, ParentAnchorCollection value)
            => obj.SetValue(AnchorsProperty, value);

        public static readonly DependencyProperty AnchorsProperty =
            DependencyProperty.RegisterAttached(
                "Anchors",
                typeof(ParentAnchorCollection),
                typeof(ParentsExtension),
                new PropertyMetadata(null, OnAnchorsChanged));

        private static void OnAnchorsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is FrameworkElement host)
            {
                if (host.IsLoaded)
                {
                    TrySetParents(host);
                }
                else
                {
                    host.Loaded += OnHostLoaded;
                }
            }
        }

        private static void OnHostLoaded(object sender, RoutedEventArgs e)
        {
            if (sender is FrameworkElement host)
            {
                host.Loaded -= OnHostLoaded;
                TrySetParents(host);
            }
        }

        private static void TrySetParents(FrameworkElement host)
        {
            if (GetAnchors(host) is IList<ParentAnchor> anchors)
            {
                foreach (ParentAnchor anchor in anchors)
                {
                    _ = anchor.TrySetParent(host);
                }
            }
        }

        private static DependencyObjectCollection CreateNewDependencyCollection(DependencyObject obj)
        {
            DependencyObjectCollection dpCollection = new();
            dpCollection.VectorChanged += VectorChanged;
            return dpCollection;

            void VectorChanged(IObservableVector<DependencyObject> sender, IVectorChangedEventArgs e)
            {
                //dpCollection.VectorChanged -= VectorChanged;
                if (obj is FrameworkElement host)
                {
                    if (host.IsLoaded)
                    {
                        TrySetParents(host);
                    }
                    else
                    {
                        host.Loaded += OnHostLoaded;
                    }
                }
            }
        }
    }

    public class ParentAnchorCollection : List<ParentAnchor>
    {

    }

    public class ParentAnchor : DependencyObject
    {
        public static readonly DependencyProperty ParentProperty =
            DependencyProperty.Register(nameof(Parent), typeof(FrameworkElement), typeof(ParentAnchor), new PropertyMetadata(null));
        public static readonly DependencyProperty ParentTypeProperty =
            DependencyProperty.Register(nameof(ParentType), typeof(Type), typeof(ParentAnchor), new PropertyMetadata(null));
        public static readonly DependencyProperty ParentNameProperty =
            DependencyProperty.Register(nameof(ParentName), typeof(string), typeof(ParentAnchor), new PropertyMetadata(null));

        public FrameworkElement? Parent
        {
            get { return (FrameworkElement)GetValue(ParentProperty); }
            set { SetValue(ParentProperty, value); }
        }

        public Type? ParentType
        {
            get { return (Type)GetValue(ParentTypeProperty); }
            set { SetValue(ParentTypeProperty, value); }
        }

        public string ParentName
        {
            get { return (string)GetValue(ParentNameProperty); }
            set { SetValue(ParentNameProperty, value); }
        }

        public bool TrySetParent(FrameworkElement host)
        {
            bool wasSet = true;
            if (ParentName is not null)
            {
                Parent = ParentResolver.FindParentElement(host, ParentName);
            }
            else if (ParentType is not null)
            {
                Parent = ParentResolver.FindParentElement(host, ParentType);
            }
            else
            {
                wasSet = false;
            }

            return wasSet;
        }
    }
}
