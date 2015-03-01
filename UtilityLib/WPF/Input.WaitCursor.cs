// Copyright (c) 2014 Marc Lefkon (http://www.leftek.com)
// All rights reserved (https://github.com/mlefkon/PublicLibrariesCS/blob/master/UtilityLib/)
// Licensed under MIT License (MIT) (https://github.com/mlefkon/PublicLibrariesCS/blob/master/UtilityLib/license.txt).
using System;
using System.Windows.Input;

namespace UtilityLib {
    public class WaitCursor : IDisposable {
        private System.Windows.Input.Cursor _previousCursor;
        public WaitCursor() {
            _previousCursor = Mouse.OverrideCursor;
            Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
        }
        public void Dispose() {
            Mouse.OverrideCursor = _previousCursor;
        }
    }

}
