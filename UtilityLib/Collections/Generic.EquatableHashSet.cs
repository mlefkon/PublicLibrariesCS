// Copyright (c) 2014 Marc Lefkon (http://www.leftek.com)
// All rights reserved (https://github.com/mlefkon/PublicLibrariesCS/blob/master/UtilityLib/)
// Licensed under MIT License (MIT) (https://github.com/mlefkon/PublicLibrariesCS/blob/master/UtilityLib/license.txt).
using System;
using System.Collections.Generic;

namespace UtilityLib {
    public class EquatableHashSet<T> : HashSet<T> {
        public EquatableHashSet() {}
        public EquatableHashSet(IEnumerable<T> Items) : base(Items) {} // this constructor will ignore duplicates in Items.
        public override string ToString() { return String.Join("|", this); }
        public override int GetHashCode() { return this.ToString().GetHashCode(); } // override for equivalency.
        public override bool Equals(object obj) { 
            EquatableHashSet<T> other = obj as EquatableHashSet<T>;
            if (other == null || this.Count != other.Count) return false;
            foreach (T item in this) if (!other.Contains(item)) return false;
            return true; 
        }
    }
}
