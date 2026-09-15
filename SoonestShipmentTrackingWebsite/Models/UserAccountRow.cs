using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SoonestShipmentTrackingWebsite.Models
{
    // Row shown on Admin/UserAccounts.aspx.
    public class UserAccountRow
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string CurrentRole { get; set; }
        public bool IsCurrentUser { get; set; }
    }
}