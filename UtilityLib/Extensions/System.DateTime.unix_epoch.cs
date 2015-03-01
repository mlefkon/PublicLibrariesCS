// Copyright (c) 2014 Marc Lefkon (http://www.leftek.com)
// All rights reserved (https://github.com/mlefkon/PublicLibrariesCS/blob/master/UtilityLib/)
// Licensed under MIT License (MIT) (https://github.com/mlefkon/PublicLibrariesCS/blob/master/UtilityLib/license.txt).
using System;

namespace UtilityLib {
    public static partial class UnixEpochExtensions {
        private static readonly DateTime UnixEpochZeroUTC = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        //private static readonly double OleAutomationEpochAsJulianDay = 2415018.5;

        // Use both extensions to convert epoch back and forth for consistency because it uses the UTC value as the intermediary.
        public static Int32 ToUnixEpochUtcInt32(this DateTime Timestamp, bool AutoCorrectToMinMax = false) { 
            double totalSecs = (Timestamp.ToUniversalTime() - UnixEpochZeroUTC).TotalSeconds;
            if (AutoCorrectToMinMax) {
                if      (totalSecs < Int32.MinValue) totalSecs = Int32.MinValue;
                else if (totalSecs > Int32.MaxValue) totalSecs = Int32.MaxValue;
            }
            return Convert.ToInt32(totalSecs);
        }
        public static UInt32 ToUnixEpochUtcUInt32(this DateTime Timestamp, bool AutoCorrectToMinMax = false) { 
            double totalSecs = (Timestamp.ToUniversalTime() - UnixEpochZeroUTC).TotalSeconds;
            if (AutoCorrectToMinMax) {
                if      (totalSecs < UInt32.MinValue) totalSecs = UInt32.MinValue;
                else if (totalSecs > UInt32.MaxValue) totalSecs = UInt32.MaxValue;
            }
            return Convert.ToUInt32(totalSecs);
        }
        public static DateTime UnixEpochUtcToDateTime(this Int32 UnixEpochUTC) {
            return DateTime.SpecifyKind(UnixEpochZeroUTC.AddSeconds(UnixEpochUTC), DateTimeKind.Utc).ToLocalTime();
        }
        public static DateTime UnixEpochUtcToDateTime(this UInt32 UnixEpochUTC) {
            return DateTime.SpecifyKind(UnixEpochZeroUTC.AddSeconds(UnixEpochUTC), DateTimeKind.Utc).ToLocalTime();
        }
    }
}
