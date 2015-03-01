// Copyright (c) 2014 Marc Lefkon (http://www.leftek.com)
// All rights reserved (https://github.com/mlefkon/PublicLibrariesCS/blob/master/UtilityLib/)
// Licensed under MIT License (MIT) (https://github.com/mlefkon/PublicLibrariesCS/blob/master/UtilityLib/license.txt).
using System;
using System.Threading;

namespace UtilityLib {
    // References:
    //   www.codeproject.com/Articles/98461/Enforcing-Single-Instance-WPF-Applications
    //   www.codeproject.com/Articles/32908/C-Single-Instance-App-With-the-Ability-To-Restore
    //   bobpowell.net/singleinstance.aspx
    //   www.simple-talk.com/dotnet/.net-framework/creating-tray-applications-in-.net-a-practical-guide/
    /* Notes:
         A couple ways of doing this:
         - check process list (cons: possible dup proc name, race condition, doesn't work for console(?)
         - use Mutex
         - use Semaphore
     */
    public sealed class SingletonApplicationEnforcer { // This class allows restricting the number of executables in execution, to one.
        readonly string _appUniqueId;  // AppUniqueId = usedfor naming the EventWaitHandle
        Thread thread;
       
        public SingletonApplicationEnforcer(string AppUniqueId) { 
            _appUniqueId = AppUniqueId;
        }
        public event EventHandler DuplicateInstanceDetected; // Use this event to respond to DupInstance opening... like restore window or create new window if app lives in SystemTray.
            private void RaiseDuplicateInstanceDetected() { if (DuplicateInstanceDetected != null) DuplicateInstanceDetected(this, new EventArgs()); }
        public bool IsDuplicateInstance() { // Determines if this application instance is not the singleton instance.　If this application is not the singleton, then it should exit.
            bool createdNew;
            EventWaitHandle argsWaitHandle = new EventWaitHandle( false, EventResetMode.AutoReset, "DupWaitHandle_" + _appUniqueId, out createdNew);
            GC.KeepAlive(argsWaitHandle);
            if (createdNew) { // This is the main, or singleton application. A thread is created to service RaiseDuplicateInstanceDetected(), repeatedly, each time the argsWaitHandle is Set by a non-singleton application instance. 
                thread = new Thread(() => { while (true) {
                                                argsWaitHandle.WaitOne();
                                                RaiseDuplicateInstanceDetected();
                                            }
                                          });
                thread.IsBackground = true;
                thread.Start();
            } else { // Is Non-singleton instance, so should signal via Semaphore.
                argsWaitHandle.Set();
            }
            return !createdNew;
        }
    }
}
