// Copyright (c) 2014 Marc Lefkon (http://www.leftek.com)
// All rights reserved (https://github.com/mlefkon/PublicLibrariesCS/blob/master/UtilityLib/)
// Licensed under MIT License (MIT) (https://github.com/mlefkon/PublicLibrariesCS/blob/master/UtilityLib/license.txt).
using System;

namespace UtilityLib {
    public static partial class TypeExtensions {
        public static bool IsNumericType(this Type t) {
            if (t == typeof(byte) || t == typeof(Int16) || t == typeof(Int32) || t == typeof(Int64) ||
                t == typeof(float) || t == typeof(Single) || t == typeof(double) || t == typeof(decimal) ) return true;
            return false;
        }

    }
}
