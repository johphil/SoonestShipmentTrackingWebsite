using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SoonestShipmentTrackingWebsite.Models
{
    public class Branch
    {
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string BranchName { get; set; }

        [Required, StringLength(250)]
        public string Address { get; set; }

        [Required, StringLength(100)]
        public string City { get; set; }

        [Phone, StringLength(30)]
        public string ContactNumber { get; set; }

        [EmailAddress, StringLength(150)]
        public string Email { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }

        public Branch()
        {
            IsActive = true;
            CreatedDate = DateTime.Now;
            UpdatedDate = DateTime.Now;
        }
    }
}