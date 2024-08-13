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
    /// <summary>
    /// Allows to retrieve parent of selected element of given type or name.
    /// <para>Example usage:</para>
    /// <code>
    /// &lt;Button 
    /// Command="{Binding Parent.DataContext.ParentCommand, ElementName=ButtonParent}"
    /// Content={Binding Text}&gt;
    ///     &lt;local:ParentService.Anchors&gt;
    ///         &lt;local:ParentAnchorCollection&gt;
    ///             &lt;local:ParentAnchor x:Name="ButtonParent" Type="Page"/&gt;
    ///         &lt;/local:ParentAnchorCollection&gt;
    ///     &lt;/local:ParentService.Anchors&gt;
    /// &lt;/Button&gt;
    /// </code>
    /// </summary>
    public static class ParentService
    {
        /// <summary>
        /// Allows to set a collection of <see cref="ParentAnchor"/> objects on <see cref="FrameworkElement"/> items.
        /// </summary>
        public static readonly DependencyProperty AnchorsProperty =
            DependencyProperty.RegisterAttached(
                "Anchors",
                typeof(ParentAnchorCollection),
                typeof(ParentService),
                new PropertyMetadata(null, OnAnchorsChanged));

        /// <summary>
        /// Allows to set a single <see cref="ParentAnchor"/> object on <see cref="FrameworkElement"/> item.
        /// </summary>
        /// <remarks>
        /// Often you need to reference only a single parent element from particual control.
        /// This property allows to reduce verbosity of <see cref="AnchorsProperty"/>, allowing 
        /// to ommit <see cref="ParentAnchorCollection"/> declaration.
        /// </remarks>
        public static readonly DependencyProperty AnchorProperty =
            DependencyProperty.RegisterAttached(
                "Anchor",
                typeof(ParentAnchor),
                typeof(ParentService),
                new PropertyMetadata(null, OnAnchorsChanged));

        public static ParentAnchorCollection? GetAnchors(DependencyObject obj)
            => (ParentAnchorCollection)obj.GetValue(AnchorsProperty);

        public static void SetAnchors(DependencyObject obj, ParentAnchorCollection value)
            => obj.SetValue(AnchorsProperty, value);


        public static ParentAnchor GetAnchor(DependencyObject obj)
            => (ParentAnchor)obj.GetValue(AnchorProperty);

        public static void SetAnchor(DependencyObject obj, ParentAnchor value)
            => obj.SetValue(AnchorProperty, value);

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
            if (GetAnchor(host) is ParentAnchor singleAnchor)
            {
                singleAnchor.TrySetParent(host);
            }

            if (GetAnchors(host) is IList<ParentAnchor> anchors)
            {
                foreach (ParentAnchor anchor in anchors)
                {
                    anchor.TrySetParent(host);
                }
            }
        }
    }

    /// <summary>
    /// Provides a typed wrapper around <see cref="List{T}"/> of <see cref="ParentAnchor"/> type.
    /// </summary>
    public class ParentAnchorCollection : List<ParentAnchor>
    {

    }

    /// <summary>
    /// Provides a bindable <see cref="Parent"/> property, effectively addressing the absence of <i>RelativeSource AncestorType</i> in WinUI.
    /// Use the <see cref="ParentName"/> and <see cref="ParentType"/> properties to specify the criteria for searching the parent.
    /// </summary>
    public class ParentAnchor : DependencyObject

    {
        /// <summary>
        /// Provides the found parent object, if any.
        /// </summary>
        public static readonly DependencyProperty ParentProperty =
            DependencyProperty.Register(nameof(Parent), typeof(FrameworkElement), typeof(ParentAnchor), new PropertyMetadata(null));

        /// <summary>
        /// Type of the parent object to be found.
        /// </summary>
        public static readonly DependencyProperty ParentTypeProperty =
            DependencyProperty.Register(nameof(ParentType), typeof(Type), typeof(ParentAnchor), new PropertyMetadata(null));

        /// <summary>
        /// Name of the parent object to be found.
        /// </summary>
        public static readonly DependencyProperty ParentNameProperty =
            DependencyProperty.Register(nameof(ParentName), typeof(string), typeof(ParentAnchor), new PropertyMetadata(null));

        /// <summary>
        /// Provides the found parent object, if any.
        /// </summary>
        public FrameworkElement? Parent
        {
            get { return (FrameworkElement)GetValue(ParentProperty); }
            set { SetValue(ParentProperty, value); }
        }

        /// <summary>
        /// Type of the parent object to be found.
        /// </summary>
        public Type? ParentType
        {
            get { return (Type)GetValue(ParentTypeProperty); }
            set { SetValue(ParentTypeProperty, value); }
        }

        /// <summary>
        /// Name of the parent object to be found.
        /// </summary>
        public string ParentName
        {
            get { return (string)GetValue(ParentNameProperty); }
            set { SetValue(ParentNameProperty, value); }
        }

        /// <summary>
        /// Searches for the first occurrence of a parent with the specified name (<see cref="ParentName"/>) 
        /// or type (<see cref="ParentType"/>) (the name takes precedence over the type). The found object 
        /// is assigned to the <see cref="Parent"/> property.
        /// </summary>
        /// <param name="host">Object whose parent is to be found.</param>
        /// <returns><see langword="True"/> if the parent object was found and set, otherwise <see langword="false"/>.</returns>
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
