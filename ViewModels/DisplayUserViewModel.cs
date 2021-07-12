using CMSApplication.Data;
using CMSApplication.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CMSApplication.ViewModels
{
    public class DisplayUserViewModel
    {
        public ApplicationUser applicationUser { get; set; }
        [Display(Name ="City")]
        public IList<City> Cities { get; set; }
        public City City { get; set; }
        public Country Country { get; set; }
        
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public bool isAdmin { get; set; }
        public string StatusMessage { get; set; }
        public IList<string> Roles { get; set; }

    }
}
