using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SoonestShipmentTrackingWebsite.Models
{
    public class Shipment
    {
        public int Id { get; set; }

        [Required, StringLength(20)]
        public string ControlNumber { get; set; }

        [Required]
        public string CustomerId { get; set; }
        public virtual ApplicationUser Customer { get; set; }

        [Required, StringLength(100)]
        public string SenderName { get; set; }
        [StringLength(250)]
        public string SenderAddress { get; set; }

        [Required, StringLength(100)]
        public string RecipientName { get; set; }
        [Required, StringLength(250)]
        public string RecipientAddress { get; set; }
        [Required, StringLength(100)]
        public string DestinationCity { get; set; }

        [Phone]
        public string RecipientPhone { get; set; }
        [EmailAddress]
        public string RecipientEmail { get; set; }

        [StringLength(150)]
        public string PackageDescription { get; set; }
        public decimal? WeightKg { get; set; }

        public ShipmentStatus CurrentStatus { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime? EstimatedDeliveryDate { get; set; }
        public DateTime UpdatedDate { get; set; }

        public virtual ICollection<ShipmentHistory> History { get; set; }

        public Shipment()
        {
            History = new List<ShipmentHistory>();
            CreatedDate = DateTime.Now;
            UpdatedDate = DateTime.Now;
            CurrentStatus = ShipmentStatus.Pending;
        }
    }
}