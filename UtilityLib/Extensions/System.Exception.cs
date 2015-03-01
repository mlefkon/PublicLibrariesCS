// Copyright (c) 2014 Marc Lefkon (http://www.leftek.com)
// All rights reserved (https://github.com/mlefkon/PublicLibrariesCS/blob/master/UtilityLib/)
// Licensed under MIT License (MIT) (https://github.com/mlefkon/PublicLibrariesCS/blob/master/UtilityLib/license.txt).
using System;
using System.Text;

namespace UtilityLib {
    public static partial class ExceptionExtensions {
        public static string MessageInnermost(this Exception ex) {
            while (ex.InnerException != null) ex = ex.InnerException;
            return ex.Message;
        }
        public static string MessageFull(this Exception ex, string MsgSeparator = null) {
            if (MsgSeparator == null) MsgSeparator = Environment.NewLine + Environment.NewLine;
            StringBuilder msg = new StringBuilder("");
            while (ex != null) {
                msg.Append(msg.Length == 0 ? "" : MsgSeparator).Append(ex.Message);
                ex = ex.InnerException;   
            }
            return msg.ToString();
        } 
    }
}
