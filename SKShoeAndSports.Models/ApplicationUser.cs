using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SKShoeAndSports.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public string Name { get; set; }
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public string County { get; set; }
        public string Postcode { get; set; }

        public int? StaffId { get; set; }

        [ForeignKey("StaffId")]
        public Staff Staff { get; set; }

        [NotMapped]
        public string Role { get; set; }

    }
}
