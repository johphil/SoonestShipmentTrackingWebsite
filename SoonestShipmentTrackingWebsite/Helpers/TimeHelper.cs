using System;

namespace SoonestShipmentTrackingWebsite.Helpers
{
    /// <summary>
    /// Provides the current date/time in GMT+8 (Philippines/Singapore/China time),
    /// regardless of the server's local timezone. The server is hosted in Europe,
    /// so all recorded timestamps should use this helper instead of DateTime.Now.
    /// </summary>
    public static class TimeHelper
    {
        private static readonly TimeSpan Gmt8Offset = TimeSpan.FromHours(8);

        /// <summary>Current date/time in GMT+8.</summary>
        public static DateTime Now => DateTime.UtcNow.Add(Gmt8Offset);

        /// <summary>Converts the given DateTime (assumed UTC if Kind is Utc or Unspecified) to GMT+8.</summary>
        public static DateTime ToGmt8(DateTime value)
        {
            var utc = value.Kind == DateTimeKind.Local ? value.ToUniversalTime() : DateTime.SpecifyKind(value, DateTimeKind.Utc);
            return utc.Add(Gmt8Offset);
        }
    }
}
