using SoonestShipmentTrackingWebsite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SoonestShipmentTrackingWebsite.Helpers
{
    public static class StatusDisplayHelper
    {
        public static string BadgeClass(ShipmentStatus status)
        {
            switch (status)
            {
                case ShipmentStatus.Pending: return "badge-pending";
                case ShipmentStatus.OutForDelivery: return "badge-warning";
                case ShipmentStatus.Delivered: return "badge-success";
                case ShipmentStatus.FailedDelivery: return "badge-danger";
                case ShipmentStatus.Returned: return "badge-dark";
                default: return "badge-pending";
            }
        }

        public static string Label(ShipmentStatus status)
        {
            switch (status)
            {
                case ShipmentStatus.OutForDelivery: return "Out for Delivery";
                case ShipmentStatus.FailedDelivery: return "Failed Delivery";
                default: return status.ToString();
            }
        }

        // Returns a ready-to-render <span> badge, used from .aspx inline
        // data-binding expressions: <%# StatusDisplayHelper.Badge(...) %>
        public static string Badge(ShipmentStatus status)
        {
            return "<span class=\"status-badge " + BadgeClass(status) + "\">" + Label(status) + "</span>";
        }
    }
}