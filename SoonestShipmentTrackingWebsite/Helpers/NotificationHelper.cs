using SoonestShipmentTrackingWebsite.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace SoonestShipmentTrackingWebsite.Helpers
{
    // A single customer-facing notification built from a shipment history event.
    public class ShipmentNotificationItem
    {
        public int ShipmentId { get; set; }
        public string ControlNumber { get; set; }
        public ShipmentStatus Status { get; set; }
        public string Location { get; set; }
        public DateTime Timestamp { get; set; }
        public bool IsUnread { get; set; }

        public string Message
        {
            get
            {
                var text = ControlNumber + " is now " + StatusDisplayHelper.Label(Status).ToLowerInvariant();
                if (!string.IsNullOrWhiteSpace(Location))
                    text += " - " + Location;
                return text;
            }
        }

        public string TimeAgo
        {
            get
            {
                var span = DateTime.Now - Timestamp;
                if (span.TotalMinutes < 1) return "Just now";
                if (span.TotalMinutes < 60) return (int)span.TotalMinutes + "m ago";
                if (span.TotalHours < 24) return (int)span.TotalHours + "h ago";
                if (span.TotalDays < 7) return (int)span.TotalDays + "d ago";
                return Timestamp.ToString("MMM d, yyyy");
            }
        }
    }

    public static class NotificationHelper
    {
        private const string SeenSessionKey = "NotifSeenAt";

        // Pulls the customer's latest shipment history events (newest first) and
        // flags any newer than the last time the customer opened the bell.
        public static List<ShipmentNotificationItem> GetNotifications(string userId, int take = 15)
        {
            var seenAt = HttpContext.Current.Session[SeenSessionKey] as DateTime?;

            using (var db = new ApplicationDbContext())
            {
                var events = db.ShipmentHistories
                    .Where(h => h.Shipment.CustomerId == userId)
                    .OrderByDescending(h => h.Timestamp)
                    .Take(take)
                    .Select(h => new
                    {
                        h.ShipmentId,
                        ControlNumber = h.Shipment.ControlNumber,
                        h.Status,
                        h.Location,
                        h.Timestamp
                    })
                    .ToList();

                return events.Select(e => new ShipmentNotificationItem
                {
                    ShipmentId = e.ShipmentId,
                    ControlNumber = e.ControlNumber,
                    Status = e.Status,
                    Location = e.Location,
                    Timestamp = e.Timestamp,
                    IsUnread = !seenAt.HasValue || e.Timestamp > seenAt.Value
                }).ToList();
            }
        }

        public static int UnreadCount(List<ShipmentNotificationItem> items)
        {
            return items == null ? 0 : items.Count(i => i.IsUnread);
        }

        // Called when the customer opens the bell so the badge count clears.
        public static void MarkAllSeen()
        {
            HttpContext.Current.Session[SeenSessionKey] = DateTime.Now;
        }
    }
}
