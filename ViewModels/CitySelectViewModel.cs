using CMSApplication.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CMSApplication.ViewModels
{
    public class CitySelectViewModel
    {
        [Display(Name ="Country")]
        public Country Country { get; set; }
        public City City { get; set; }
        public IList<City> Cities { get; set; }
        public IList<Country> Countries { get; set; }
        public string Message { get; set; }
    }
}
