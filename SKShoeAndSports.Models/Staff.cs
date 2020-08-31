using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SKShoeAndSports.Models
{
    public class Staff
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public string County { get; set; }
        public string Postcode { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsAuthorisedStaff { get; set; }
    }
}
