// Copyright (c) 2014 Marc Lefkon (http://www.leftek.com)
// All rights reserved (https://github.com/mlefkon/PublicLibrariesCS/blob/master/UtilityLib/)
// Licensed under MIT License (MIT) (https://github.com/mlefkon/PublicLibrariesCS/blob/master/UtilityLib/license.txt).
using System;
using System.Diagnostics;
using System.Linq.Expressions;
using System.ComponentModel;

namespace UtilityLib {
    public static class TypeHelper<T> { // Methods to extract information about a type. 
        [DebuggerStepThrough]
        public static string MemberName<R>(Expression<Func<T,R>> expr) { 
            // Get text string of a Type's member names using the Type. 
            // Usage: string s = TypeHelper<MyClass>.GetName(x => x.Name); 
            var node = expr.Body as MemberExpression;
            if (object.ReferenceEquals(null, node)) throw new InvalidOperationException("Expression must be of member access");
            return node.Member.Name;
        }
        public static bool TestParse(string s) {  
            // Usage: TypeHelper<Int64>.TestParse("14");
            Type t = typeof(T);
            TypeConverter converter = TypeDescriptor.GetConverter(t);
            try { object testVal = converter.ConvertFromString(s);
                  return true;
            } catch { return false; }
        }
    }
}
