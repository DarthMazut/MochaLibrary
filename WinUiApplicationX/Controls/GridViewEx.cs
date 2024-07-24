using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinUiApplicationX.Controls
{
    public class GridViewEx : GridView
    {
        public static readonly DependencyProperty SelectedItemsExProperty =
            DependencyProperty.Register(nameof(SelectedItemsEx), typeof(IList), typeof(GridViewEx), new PropertyMetadata(null, SelectedItemsExChanged));

        public IList? SelectedItemsEx
        {
            get => (IList)GetValue(SelectedItemsExProperty);
            set => SetValue(SelectedItemsExProperty, value);
        }

        public GridViewEx()
        {
            // When SelectionChanged notify bound collection if any.
            this.SelectionChanged += (s, e) =>
            {
                if (SelectedItemsEx is not null)
                {
                    foreach (object? item in e.AddedItems)
                    {
                        if (!SelectedItemsEx.Contains(item))
                        {
                            SelectedItemsEx.Add(item);
                        }
                    }

                    foreach (object? item in e.RemovedItems)
                    {
                        SelectedItemsEx.Remove(item);
                    }
                }
            };
        }

        private static void SelectedItemsExChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            // When SelectedItemsEx changes mirror its item to SelectedItems
            // and subscribe to collectionChanged if is observableCollection.
            if (d is GridViewEx thisControl)
            {
                if (e.OldValue is INotifyCollectionChanged previousObservableCollection)
                {
                    previousObservableCollection.CollectionChanged -= SelectedItemsExCollectionChanged;
                }

                if (e.NewValue is IList receivedList)
                {
                    thisControl.SelectedItems.Clear();
                    foreach (object item in receivedList)
                    {
                        thisControl.SelectedItems.Add(item);
                    }

                    if (receivedList is INotifyCollectionChanged observableCollection)
                    {
                        observableCollection.CollectionChanged += SelectedItemsExCollectionChanged;
                    }
                }
            }

            // Mirror changes to bounds observableCollection to SelectedItems.
            void SelectedItemsExCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
            {
                if (e.NewItems is not null)
                {
                    foreach (object? item in e.NewItems)
                    {
                        if (!thisControl.SelectedItems.Any(i => i.Equals(item)))
                        {
                            thisControl.SelectedItems.Add(item);
                        }
                    }
                }

                if (e.OldItems is not null)
                {
                    foreach (object? item in e.OldItems)
                    {
                        int indexToRemove = thisControl.SelectedItems.Select((i, index) => (i, index)).First(t => t.i.Equals(item)).index;
                        thisControl.SelectedItems.RemoveAt(indexToRemove);
                    }
                }

                if (e.Action == NotifyCollectionChangedAction.Reset)
                {
                    thisControl.SelectedItems.Clear();
                }
            }
        }
    }
}
