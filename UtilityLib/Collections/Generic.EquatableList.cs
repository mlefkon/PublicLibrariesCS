// Copyright (c) 2014 Marc Lefkon (http://www.leftek.com)
// All rights reserved (https://github.com/mlefkon/PublicLibrariesCS/blob/master/UtilityLib/)
// Licensed under MIT License (MIT) (https://github.com/mlefkon/PublicLibrariesCS/blob/master/UtilityLib/license.txt).
using System;
using System.Collections.Generic;

namespace UtilityLib {
    public class EquatableList<T> : List<T> {
        public EquatableList() {}
        public EquatableList(IEnumerable<T> Items) : base(Items) {}
        public override string ToString() { return String.Join("|", this); }
        public override int GetHashCode() { return this.ToString().GetHashCode(); } // override for equivalency.
        public override bool Equals(object obj) { 
            EquatableList<T> other = obj as EquatableList<T>;
            if (other == null || this.Count != other.Count) return false;
            for (int i = 0; i < this.Count; i++) if (!other[i].Equals(this[i])) return false;
            return true; 
        }
    }
}
