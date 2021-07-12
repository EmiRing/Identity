using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CMSApplication.Models
{
    public class ApplicationUser: IdentityUser
    {
        [PersonalData]
        [Display(Name = "First name")]
        public string FirstName { get; set; }
        [PersonalData]
        [Display(Name = "Last name")]
        public string LastName { get; set; }
        [PersonalData]
        [Display(Name = "City")]
        public int CityId { get; set; }
        public City City { get; set; }

        [NotMapped]
        [Display(Name = "Full name")]
        public string FullName
        {
            get
            {
                return LastName == null ? FirstName : FirstName + " " + LastName;
            }
        }
    }
}
