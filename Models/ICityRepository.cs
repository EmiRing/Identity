using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMSApplication.Models
{
    interface ICityRepository
    {
        IEnumerable<City> AllCities { get; }
        int GetCityId(string CityName);

    }
}
