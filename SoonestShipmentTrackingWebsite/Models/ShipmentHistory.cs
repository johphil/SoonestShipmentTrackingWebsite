using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SoonestShipmentTrackingWebsite.Models
{
    public class ShipmentHistory
    {
        public int Id { get; set; }

        public int ShipmentId { get; set; }

        public virtual Shipment Shipment { get; set; }

        public ShipmentStatus Status { get; set; }

        [StringLength(150)]
        public string Location { get; set; }

        [StringLength(500)]
        public string Notes { get; set; }

        public DateTime Timestamp { get; set; }

        public ShipmentHistory()
        {
            Timestamp = DateTime.Now;
        }
    }
}