// Copyright (c) 2014 Marc Lefkon (http://www.leftek.com)
// All rights reserved (https://github.com/mlefkon/PublicLibrariesCS/blob/master/UtilityLib/)
// Licensed under MIT License (MIT) (https://github.com/mlefkon/PublicLibrariesCS/blob/master/UtilityLib/license.txt).
using System;
using System.Windows.Input;
using System.Diagnostics;

namespace UtilityLib {
    // Implements the ICommand interface. Thanks to Josh Smith for this code: msdn.microsoft.com/en-us/magazine/dd419663.aspx
    // Usage: public ICommand MyCommandNameToBindToFromXAML { get { return new RelayCommand( param => { /* Execute Code */ }, param => {return /* bool */;} ) } }
    //   Then in XAML: XAMLCommandProperty="{Binding MyCommandNameToBindToFromXAML}"
    public class RelayCommand : ICommand {
        readonly Action<object> _execute;
        readonly Predicate<object> _canExecute;

        public RelayCommand(Action<object> execute) : this(execute, null) {}
        public RelayCommand(Action<object> execute, Predicate<object> canExecute) {
            if (execute == null) throw new ArgumentNullException("execute");
            _execute = execute;
            _canExecute = canExecute;
        }
        [DebuggerStepThrough]
        public bool CanExecute(object parameter) {
            return _canExecute == null ? true : _canExecute(parameter);
        }
        public void RaiseCanExecuteChanged() { if (CanExecuteChanged != null) System.Windows.Application.Current.Dispatcher.Invoke( () => { CanExecuteChanged(this, new EventArgs()); } ); }
            public event EventHandler CanExecuteChanged;
        public void Execute(object parameter) { _execute(parameter); }
    }

}
