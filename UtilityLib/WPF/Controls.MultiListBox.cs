// Copyright (c) 2014 Marc Lefkon (http://www.leftek.com)
// All rights reserved (https://github.com/mlefkon/PublicLibrariesCS/blob/master/UtilityLib/)
// Licensed under MIT License (MIT) (https://github.com/mlefkon/PublicLibrariesCS/blob/master/UtilityLib/license.txt).
using System.Collections;
using System.Windows;

namespace UtilityLib {
    public class MultiListBox {
        /// SelectedItems Attached Dependency Property
        public static readonly DependencyProperty SelectedItemsProperty = DependencyProperty.RegisterAttached("SelectedItems", typeof(SelectedCollection), typeof(MultiListBox),
                new FrameworkPropertyMetadata((SelectedCollection)null, new PropertyChangedCallback(OnSelectedItemsChanged)));
        public static SelectedCollection GetSelectedItems(DependencyObject d) {
            return (SelectedCollection)d.GetValue(SelectedItemsProperty);
        }
        public static void SetSelectedItems(DependencyObject d, SelectedCollection value) { 
            d.SetValue(SelectedItemsProperty, value);
        }
        private static void OnSelectedItemsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) { 
            var listBox = (System.Windows.Controls.ListBox)d;
            ReSetSelectedItems(listBox);
            listBox.SelectionChanged += delegate { ReSetSelectedItems(listBox); };
        }
        private static void ReSetSelectedItems(System.Windows.Controls.ListBox listBox) {
            SelectedCollection selectedItems = GetSelectedItems(listBox);
            selectedItems.Clear();
            if (listBox.SelectedItems != null) selectedItems.AddRange(listBox.SelectedItems);  
            selectedItems.RaiseNewSelection();
        }
        public class SelectedCollection : ArrayList { // use this instead of ObservableCollection for the SelectedItems because no AddRange() in ObservableCollection, thus an event would get sent for each Add().
            public event SignalEventHandler NewSelection;
            public void RaiseNewSelection() { if (NewSelection != null) NewSelection(); }
        }
    }
}
