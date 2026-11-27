using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SoonestShipmentTrackingWebsite.Models
{
    public class Rider
    {
        public Rider()
        {
            IsActive = true;
            DateCreated = Helpers.TimeHelper.Now;
            DateModified = Helpers.TimeHelper.Now;
        }

        [Key]
        public int RiderId { get; set; }

        [Required, StringLength(100)]
        public string FullName { get; set; }

        [Required, StringLength(256), EmailAddress]
        [Index("IX_Rider_Email", IsUnique = true)]
        public string Email { get; set; }

        [Required, StringLength(20)]
        [RegularExpression(@"^\+?[0-9]{10,15}$", ErrorMessage = "Enter a valid mobile number.")]
        public string MobileNumber { get; set; }

        [Required, StringLength(100)]
        [Index("IX_Rider_LicenseNumber", IsUnique = true)]
        public string LicenseNumber { get; set; }

        [Required, StringLength(250)]
        public string Address { get; set; }

        public bool IsActive { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime DateModified { get; set; }
    }
}
