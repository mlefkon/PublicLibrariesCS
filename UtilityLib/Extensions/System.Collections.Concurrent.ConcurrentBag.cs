// Copyright (c) 2014 Marc Lefkon (http://www.leftek.com)
// All rights reserved (https://github.com/mlefkon/PublicLibrariesCS/blob/master/UtilityLib/)
// Licensed under MIT License (MIT) (https://github.com/mlefkon/PublicLibrariesCS/blob/master/UtilityLib/license.txt).
using System.Collections.Concurrent;

namespace UtilityLib {
    public static partial class ConcurrentBagExtensions {
        public static bool TryClear<TItem>(this ConcurrentBag<TItem> bag) {
            TItem result;
            while (bag.TryTake(out result));
            return bag.IsEmpty;
        }
    }
}
