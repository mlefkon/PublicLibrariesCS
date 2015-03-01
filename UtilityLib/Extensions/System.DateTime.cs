// Copyright (c) 2014 Marc Lefkon (http://www.leftek.com)
// All rights reserved (https://github.com/mlefkon/PublicLibrariesCS/blob/master/UtilityLib/)
// Licensed under MIT License (MIT) (https://github.com/mlefkon/PublicLibrariesCS/blob/master/UtilityLib/license.txt).
using System;

namespace UtilityLib {
    public static partial class DateTimeExtensions {
        public static DateTime TruncateSeconds(this DateTime Timestamp, long Resolution) { // for use when saving DateTime2 types in SQL Server
            return new DateTime(Timestamp.Ticks - (Timestamp.Ticks % (TimeSpan.TicksPerSecond / (int)Math.Pow(10,Resolution) )), Timestamp.Kind);
        }
        public static DateTime StartOfSecond(this DateTime Timestamp, int Resolution = 1) { return new DateTime(Timestamp.Year, Timestamp.Month, Timestamp.Day, Timestamp.Hour, Timestamp.Minute, 0+Resolution*(int)(Timestamp.Second/Resolution)); }
        public static DateTime StartOfMinute(this DateTime Timestamp, int Resolution = 1) { return new DateTime(Timestamp.Year, Timestamp.Month, Timestamp.Day, Timestamp.Hour, 0+Resolution*(int)(Timestamp.Minute/Resolution), 0               ); }
        public static DateTime StartOfHour  (this DateTime Timestamp, int Resolution = 1) { return new DateTime(Timestamp.Year, Timestamp.Month, Timestamp.Day, 0+Resolution*(int)(Timestamp.Hour/Resolution), 0,                0               ); }
        public static DateTime StartOfDay   (this DateTime Timestamp, int Resolution = 1) { return new DateTime(Timestamp.Year, Timestamp.Month, 1+Resolution*(int)((Timestamp.Day-1)/Resolution), 0,          0,                0               ); }
        public static DateTime StartOfMonth (this DateTime Timestamp, int Resolution = 1) { return new DateTime(Timestamp.Year, 1+Resolution*(int)((Timestamp.Month-1)/Resolution), 1,             0,          0,                0               ); }
        public static DateTime StartOfYear  (this DateTime Timestamp, int Resolution = 1) { return new DateTime(0+Resolution*(int)(Timestamp.Year/Resolution), 1,                   1,             0,          0,                0               ); }

        public static DateTime StartOfNextSecond(this DateTime Timestamp, int Resolution = 1) { return Timestamp.StartOfSecond(Resolution).AddSeconds(Resolution); }
        public static DateTime StartOfNextMinute(this DateTime Timestamp, int Resolution = 1) { return Timestamp.StartOfMinute(Resolution).AddMinutes(Resolution); }
        public static DateTime StartOfNextHour  (this DateTime Timestamp, int Resolution = 1) { return Timestamp.StartOfHour  (Resolution).AddHours  (Resolution); }
        public static DateTime StartOfNextDay   (this DateTime Timestamp, int Resolution = 1) { return Timestamp.StartOfDay   (Resolution).AddDays   (Resolution); }
        public static DateTime StartOfNextMonth (this DateTime Timestamp, int Resolution = 1) { return Timestamp.StartOfMonth (Resolution).AddMonths (Resolution); }
        public static DateTime StartOfNextYear  (this DateTime Timestamp, int Resolution = 1) { return Timestamp.StartOfYear  (Resolution).AddYears  (Resolution); }

        public static DateTime EndOfSecond(this DateTime Timestamp, int Resolution = 1) { return Timestamp.StartOfNextSecond(Resolution).AddSeconds(-1); }
        public static DateTime EndOfMinute(this DateTime Timestamp, int Resolution = 1) { return Timestamp.StartOfNextMinute(Resolution).AddMinutes(-1); }
        public static DateTime EndOfHour  (this DateTime Timestamp, int Resolution = 1) { return Timestamp.StartOfNextHour  (Resolution).AddHours  (-1); }
        public static DateTime EndOfDay   (this DateTime Timestamp, int Resolution = 1) { return Timestamp.StartOfNextDay   (Resolution).AddDays   (-1); }
        public static DateTime EndOfMonth (this DateTime Timestamp, int Resolution = 1) { return Timestamp.StartOfNextMonth (Resolution).AddMonths (-1); }
        public static DateTime EndOfYear  (this DateTime Timestamp, int Resolution = 1) { return Timestamp.StartOfNextYear  (Resolution).AddYears  (-1); }

        public static DateTime RoundToSecond(this DateTime Timestamp, int Resolution = 1) { DateTime startOf = Timestamp.StartOfSecond(Resolution); return startOf.AddSeconds( (Timestamp - startOf).TotalMilliseconds < 500*Resolution ? 0 : Resolution); }
        public static DateTime RoundToMinute(this DateTime Timestamp, int Resolution = 1) { DateTime startOf = Timestamp.StartOfMinute(Resolution); return startOf.AddMinutes( (Timestamp - startOf).TotalSeconds      < 30*Resolution  ? 0 : Resolution); }
        public static DateTime RoundToHour  (this DateTime Timestamp, int Resolution = 1) { DateTime startOf = Timestamp.StartOfHour  (Resolution); return startOf.AddHours  ( (Timestamp - startOf).TotalMinutes      < 30*Resolution  ? 0 : Resolution); }
        public static DateTime RoundToDay   (this DateTime Timestamp, int Resolution = 1) { DateTime startOf = Timestamp.StartOfDay   (Resolution); return startOf.AddDays   ( (Timestamp - startOf).TotalHours        < 12*Resolution  ? 0 : Resolution); }
        public static DateTime RoundToMonth (this DateTime Timestamp, int Resolution = 1) { DateTime startOf = Timestamp.StartOfMonth (Resolution); return startOf.AddMonths ( (Timestamp - startOf).TotalDays         < (startOf.AddMonths(Resolution) - startOf).TotalDays / 2  ? 0 : Resolution);  }
        public static DateTime RoundToYear  (this DateTime Timestamp, int Resolution = 1) { DateTime startOf = Timestamp.StartOfYear  (Resolution); return startOf.AddYears  ( (Timestamp - startOf).TotalDays         < (startOf.AddYears (Resolution) - startOf).TotalDays / 2  ? 0 : Resolution); }
    }
}
