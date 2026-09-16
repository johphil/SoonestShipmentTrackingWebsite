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
                case ShipmentStatus.PickedUp: return "badge-info";
                case ShipmentStatus.InTransit: return "badge-info";
                case ShipmentStatus.ArrivedAtHub: return "badge-info";
                case ShipmentStatus.OutForDelivery: return "badge-warning";
                case ShipmentStatus.Delivered: return "badge-success";
                case ShipmentStatus.FailedDelivery: return "badge-danger";
                case ShipmentStatus.Returned: return "badge-dark";
                case ShipmentStatus.PendingApproval: return "badge-warning";
                case ShipmentStatus.Cancelled: return "badge-danger";
                default: return "badge-pending";
            }
        }

        public static string Label(ShipmentStatus status)
        {
            switch (status)
            {
                case ShipmentStatus.PickedUp: return "Picked Up";
                case ShipmentStatus.InTransit: return "In Transit";
                case ShipmentStatus.ArrivedAtHub: return "Arrived at Hub";
                case ShipmentStatus.OutForDelivery: return "Out for Delivery";
                case ShipmentStatus.FailedDelivery: return "Failed Delivery";
                case ShipmentStatus.PendingApproval: return "Pending Approval";
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