// Copyright (c) 2014 Marc Lefkon (http://www.leftek.com)
// All rights reserved (https://github.com/mlefkon/PublicLibrariesCS/blob/master/UtilityLib/)
// Licensed under MIT License (MIT) (https://github.com/mlefkon/PublicLibrariesCS/blob/master/UtilityLib/license.txt).
using System;

namespace UtilityLib {
    public enum TimeSpanPart {Milliseconds, Seconds, Minutes, Hours, Days, Weeks, Months, Years};
    public struct TimeSpanPartAmount {
        public TimeSpanPart Part;
        public int Amount;
        public TimeSpanPartAmount(TimeSpanPart Part, int Amount) { this.Part = Part; this.Amount = Amount; }
    }
    public static partial class TimeSpanExtensions {
        public static TimeSpanPartAmount Largest(this TimeSpan Span) {
            return Span.Days    > 365 ? new TimeSpanPartAmount(TimeSpanPart.Years,   (int)(Span.Days/365)) :
                   Span.Days    > 30  ? new TimeSpanPartAmount(TimeSpanPart.Months,  (int)(Span.Days/30 )) :
                   Span.Days    > 7   ? new TimeSpanPartAmount(TimeSpanPart.Weeks,   (int)(Span.Days/7  )) :
                   Span.Days    > 1   ? new TimeSpanPartAmount(TimeSpanPart.Days,    Span.Days) :
                   Span.Hours   > 1   ? new TimeSpanPartAmount(TimeSpanPart.Hours,   Span.Hours) : 
                   Span.Minutes > 1   ? new TimeSpanPartAmount(TimeSpanPart.Minutes, Span.Minutes) : 
                                        new TimeSpanPartAmount(TimeSpanPart.Seconds, Span.Seconds) ;
        }
        public static String ToStringLargest(this TimeSpan Span) {
            TimeSpanPartAmount largest = Span.Largest();
            return largest.Amount + " " + largest.Part.ToString().ToLower() + (largest.Amount > 1 ? "s" : "");
        }
    }
}
