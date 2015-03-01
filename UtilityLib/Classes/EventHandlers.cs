// Copyright (c) 2014 Marc Lefkon (http://www.leftek.com)
// All rights reserved (https://github.com/mlefkon/PublicLibrariesCS/blob/master/UtilityLib/)
// Licensed under MIT License (MIT) (https://github.com/mlefkon/PublicLibrariesCS/blob/master/UtilityLib/license.txt).

namespace UtilityLib {
    public delegate void SignalEventHandler(); // no EventArgs.
    public delegate void DataEventHandler<T1>(T1 Arg1); // like the framework EventHandler<T> without first [sender] parameter.
    public delegate void DataEventHandler<T1 ,T2>(T1 Arg1, T2 Arg2);
    public delegate void DataEventHandler<T1 ,T2, T3>(T1 Arg1, T2 Arg2, T3 Arg3);
    public delegate void ItemChangedEventHandler<T>(T OldVal, T NewVal);
    public delegate void ItemRemovedEventHandler<T>(T DelVal);
}
