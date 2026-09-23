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
        InTransit = 1,
        OutForDelivery = 2,
        Delivered = 3,
        FailedDelivery = 4,
        Returned = 5,
        Cancelled = 6
    }
}