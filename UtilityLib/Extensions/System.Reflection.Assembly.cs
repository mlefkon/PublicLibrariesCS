// Copyright (c) 2014 Marc Lefkon (http://www.leftek.com)
// All rights reserved (https://github.com/mlefkon/PublicLibrariesCS/blob/master/UtilityLib/)
// Licensed under MIT License (MIT) (https://github.com/mlefkon/PublicLibrariesCS/blob/master/UtilityLib/license.txt).
using System;
using System.Reflection;
using System.Security.Cryptography;
using System.IO;

namespace UtilityLib {
    public static partial class AssemblyExtensions {
        public static DateTime BuildTimestamp(this Assembly TargetAssembly) {
            string filePath = TargetAssembly.Location; // System.Reflection.Assembly.GetCallingAssembly().Location;
            const int c_PeHeaderOffset = 60;
            const int c_LinkerTimestampOffset = 8;
            byte[] b = new byte[2048];
            System.IO.Stream s = null;
            try {
                s = new System.IO.FileStream(filePath, System.IO.FileMode.Open, System.IO.FileAccess.Read);
                s.Read(b, 0, 2048);
            } finally {
                if (s != null) s.Close();
            }
            int i = System.BitConverter.ToInt32(b, c_PeHeaderOffset);
            int secondsSince1970 = System.BitConverter.ToInt32(b, i + c_LinkerTimestampOffset);
            DateTime dt = new DateTime(1970, 1, 1, 0, 0, 0);
            dt = dt.AddSeconds(secondsSince1970);
            dt = dt.AddHours(TimeZone.CurrentTimeZone.GetUtcOffset(dt).Hours);
            return dt;
        }
        public static string AssemblyMD5(this Assembly TargetAssembly) {
            using (var md5 = MD5.Create()) {
                Uri uri = new Uri( TargetAssembly.CodeBase ); // Assembly.GetExecutingAssembly().CodeBase
                using (var stream = File.OpenRead( uri.AbsolutePath )) {
                    return Convert.ToBase64String( md5.ComputeHash(stream) );
                }
            }
        }

    }
}
