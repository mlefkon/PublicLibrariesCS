
namespace System {
    public enum DateTimePart {Millisecond, Second, Minute, Hour, Day, Week, Month, Year};
    public struct DateTimePartAmount {
        public DateTimePart Part;
        public int Amount;
        public DateTimePartAmount(DateTimePart Part, int Amount) { this.Part = Part; this.Amount = Amount; }
    }
    public static partial class TimeSpanExtensions {
        public static DateTimePartAmount Largest(this TimeSpan Span) {
            return Span.Days    > 365 ? new DateTimePartAmount(DateTimePart.Year,   (int)(Span.Days/365)) :
                   Span.Days    > 30  ? new DateTimePartAmount(DateTimePart.Month,  (int)(Span.Days/30 )) :
                   Span.Days    > 7   ? new DateTimePartAmount(DateTimePart.Week,   (int)(Span.Days/7  )) :
                   Span.Days    > 1   ? new DateTimePartAmount(DateTimePart.Day,    Span.Days) :
                   Span.Hours   > 1   ? new DateTimePartAmount(DateTimePart.Hour,   Span.Hours) : 
                   Span.Minutes > 1   ? new DateTimePartAmount(DateTimePart.Minute, Span.Minutes) : 
                                        new DateTimePartAmount(DateTimePart.Second, Span.Seconds) ;
        }
        public static String ToStringLargest(this TimeSpan Span) {
            DateTimePartAmount largest = Span.Largest();
            return largest.Amount + " " + largest.Part.ToString().ToLower() + (largest.Amount > 1 ? "s" : "");
        }
    }
}
