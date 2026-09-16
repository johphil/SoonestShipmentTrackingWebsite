using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace SoonestShipmentTrackingWebsite.Models
{    public enum ShipmentStatus
    {
        Pending = 0,
        PickedUp = 1,
        InTransit = 2,
        ArrivedAtHub = 3,
        OutForDelivery = 4,
        Delivered = 5,
        FailedDelivery = 6,
        Returned = 7,

        // Added to support customer-initiated shipment creation + staff
        // approval workflow. Appended at the end (not inserted/renumbered)
        // so any shipments already stored with the existing int values
        // keep the same meaning.
        PendingApproval = 8,
        Cancelled = 9
    }
}