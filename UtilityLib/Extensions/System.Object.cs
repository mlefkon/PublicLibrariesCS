// Copyright (c) 2014 Marc Lefkon (http://www.leftek.com)
// All rights reserved (https://github.com/mlefkon/PublicLibrariesCS/blob/master/UtilityLib/)
// Licensed under MIT License (MIT) (https://github.com/mlefkon/PublicLibrariesCS/blob/master/UtilityLib/license.txt).
using System;
using System.Diagnostics;
using System.IO;
using System.Linq.Expressions;
using System.Runtime.Serialization.Formatters.Binary;

namespace UtilityLib {
    public static partial class ObjectExtensions {
        public static T ThrowIfNull<T>(this T ThisObj, string variableName) where T : class {
            if (ThisObj == null) throw new NullReferenceException(string.Format("Value is Null: {0}", variableName));
            return ThisObj;
        }
        public static T DeepClone<T>(this T ThisObj) {
            using (MemoryStream stream = new MemoryStream()) {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, ThisObj);
                stream.Position = 0;
                return (T) formatter.Deserialize(stream);
            }
        }
        [DebuggerStepThrough]
        public static string MemberName<T, R>(this T ThisObj, Expression<Func<T, R>> expr) {  // Get text string of a Type's member names using an instance. Usage: var a = new Animal(); a.MemberName(x => x.Status);
            var node = expr.Body as MemberExpression;
            if (object.ReferenceEquals(null, node)) throw new InvalidOperationException("Expression must be of member access");
            return node.Member.Name;
        }
    }
}
