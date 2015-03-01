// Copyright (c) 2014 Marc Lefkon (http://www.leftek.com)
// All rights reserved (https://github.com/mlefkon/PublicLibrariesCS/blob/master/UtilityLib/)
// Licensed under MIT License (MIT) (https://github.com/mlefkon/PublicLibrariesCS/blob/master/UtilityLib/license.txt).
using System;
using System.Windows;

namespace UtilityLib {
    public static class TooltipHelper {
        public static void ShowTooltipInMsgBox(object sender, EventArgs e = null) {
            if (!(sender is FrameworkElement)) throw new Exception("Invalid Tooltip element."); 
            System.Windows.MessageBox.Show(((FrameworkElement)sender).ToolTip.ToString(), "Help", MessageBoxButton.OK, MessageBoxImage.Question);
        }
    }
}
