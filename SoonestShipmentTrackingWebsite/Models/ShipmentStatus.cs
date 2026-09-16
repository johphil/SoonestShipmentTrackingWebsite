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
        OutForDelivery = 1,
        Delivered = 2,
        FailedDelivery = 3,
        Returned = 4
    }
}