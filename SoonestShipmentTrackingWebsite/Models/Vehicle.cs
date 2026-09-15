using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SoonestShipmentTrackingWebsite.Models
{
    public class Vehicle
    {
        public int Id { get; set; }

        [StringLength(20)]
        public string PlateNumber { get; set; }
        
        [StringLength(50)]
        public string Model { get; set; }
        
        [StringLength(50)]
        public string Type { get; set; }
        
        [StringLength(50)]
        public string Status { get; set; }
    }
}