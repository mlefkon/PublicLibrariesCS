// Copyright (c) 2014 Marc Lefkon (http://www.leftek.com)
// All rights reserved (https://github.com/mlefkon/PublicLibrariesCS/blob/master/UtilityLib/)
// Licensed under MIT License (MIT) (https://github.com/mlefkon/PublicLibrariesCS/blob/master/UtilityLib/license.txt).
using System.Windows;
using System.Diagnostics;

namespace UtilityLib {
    public static partial class ApplicationExtensions {
        public static bool IsDuplicateProcess(this System.Windows.Application app) {
            return (Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName).Length > 1);
        }    
        public static void SwitchToOtherAppProcess(this Application app) {
            Process thisProc = Process.GetCurrentProcess();
            if (Process.GetProcessesByName(thisProc.ProcessName).Length > 1) { // this process already exits
                foreach (Process otherProc in Process.GetProcessesByName(thisProc.ProcessName)) {
                    if (thisProc.Id != otherProc.Id) otherProc.RestoreWindow(); // find the other ID and restore the window.
                }
            }
        }
    }
}
