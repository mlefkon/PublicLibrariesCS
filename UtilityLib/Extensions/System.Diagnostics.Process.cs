// Copyright (c) 2014 Marc Lefkon (http://www.leftek.com)
// All rights reserved (https://github.com/mlefkon/PublicLibrariesCS/blob/master/UtilityLib/)
// Licensed under MIT License (MIT) (https://github.com/mlefkon/PublicLibrariesCS/blob/master/UtilityLib/license.txt).
using System;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace UtilityLib {
    public enum ShowWindowConstants { SW_HIDE = 0, SW_SHOWNORMAL = 1, SW_SHOWMINIMIZED = 2, SW_SHOWMAXIMIZED = 3, SW_SHOWNOACTIVATE = 4, SW_SHOW = 5, SW_MINIMIZE = 6, SW_SHOWMINNOACTIVE = 7, SW_SHOWNA = 8, SW_RESTORE = 9, SW_SHOWDEFAULT = 10, SW_FORCEMINIMIZE = 11, SW_MAX = 11 }
    public static partial class ProcessExtensions {
        [DllImport("User32.dll")] public static extern int ShowWindowAsync(IntPtr hWnd , int swCommand);

        public static void RestoreWindow(this Process proc) { // eg: Process.GetCurrentProcess().RestoreWindow();
            try { ShowWindowAsync(proc.MainWindowHandle, (int)ShowWindowConstants.SW_SHOWMINIMIZED); // try{} just in case process doesn't have a main window.
                  ShowWindowAsync(proc.MainWindowHandle, (int)ShowWindowConstants.SW_RESTORE);
            } catch (Exception) { }
        }
    }
}
