// Copyright (c) 2014 Marc Lefkon (http://www.leftek.com)
// All rights reserved (https://github.com/mlefkon/PublicLibrariesCS/blob/master/UtilityLib/)
// Licensed under MIT License (MIT) (https://github.com/mlefkon/PublicLibrariesCS/blob/master/UtilityLib/license.txt).
using System;
using System.Globalization;
using System.Diagnostics;
using System.Windows;
using System.Windows.Data;

namespace UtilityLib {
    // XAML Functions
    [ValueConversion(typeof(bool), typeof(Visibility))]
    public class VisibleOrCollapseConverter : IValueConverter {
        public object Convert    (object value, Type targetType, object parameter, CultureInfo culture) { return (bool)value ? Visibility.Visible : Visibility.Collapsed; }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) { throw new InvalidOperationException("VisibleOrCollapseConverter can only be used with Mode=OneWay."); }
    }
    [ValueConversion(typeof(bool), typeof(Visibility))]
    public class VisibleOrHiddenConverter : IValueConverter {
        public object Convert    (object value, Type targetType, object parameter, CultureInfo culture) { return (bool)value ? Visibility.Visible : Visibility.Hidden; }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) { throw new InvalidOperationException("VisibleOrHiddenConverter can only be used with Mode=OneWay."); }
    }
    [ValueConversion(typeof(bool), typeof(bool))]
    public class NotConverter : IValueConverter {
        public object Convert    (object value, Type targetType, object parameter, CultureInfo culture) { return !(bool)value; }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) { return !(bool)value; }
    }
    public class IsNullConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) { return (value == null); }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) { throw new InvalidOperationException("IsNullConverter can only be used with Mode=OneWay."); }
    }
    public class IsNotNullConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) { return (value != null); }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) { throw new InvalidOperationException("IsNotNullConverter can only be used with Mode=OneWay."); }
    }
    public class DebugConverter : IValueConverter { // This converter does nothing except breaking the debugger into the convert method
        // usage: <Window.Resources><Util:DebugConverter x:Key="debugConverter" /></Window.Resources>
        //        <TextBlock Text="{Binding ElementName=stack, Path=ActualWidth, Converter={StaticResource debugConverter}}" />
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) { Debugger.Break(); return value; } 
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) { Debugger.Break(); return value; }
    }

}
