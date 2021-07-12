using CMSApplication.Data;
using CMSApplication.Models;
using CMSApplication.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace CMSApplication.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ManagerController : Controller
    {
        private UserManager<ApplicationUser> _userManager;
        private ApplicationDbContext _context;
        
        public ManagerController(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }
        
        [HttpGet]
        public async Task<IActionResult> UpdateUser(string id)
        {
            ApplicationUser user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{id}'");
            }

            var city = _context.Cities.FirstOrDefault(c => c.Id == user.CityId);
            var hasRole = await _userManager.IsInRoleAsync(user, "Admin");

            return View(new DisplayUserViewModel {
                applicationUser = user,
                Cities = _context.Cities.ToList(),
                isAdmin = hasRole
            }
            );
        }

        [HttpPost]
        public async Task<IActionResult> UpdateUser(DisplayUserViewModel model)
        {
            var id = model.applicationUser.Id;
            model.Cities = _context.Cities.ToList();
            ApplicationUser user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                user.FirstName = model.applicationUser.FirstName;
                user.LastName = model.applicationUser.LastName;
                user.Email = model.applicationUser.Email;
                user.CityId = model.applicationUser.CityId;
                var isInRole = await _userManager.IsInRoleAsync(user, "Admin");

                if (!isInRole && model.isAdmin) await _userManager.AddToRoleAsync(user, "Admin");
                else if (isInRole && !model.isAdmin) await _userManager.RemoveFromRoleAsync(user, "Admin");

                if(model.Password != null && model.Password != "")
                {
                    var passwordValidator = new PasswordValidator<ApplicationUser>();
                    var pV = await passwordValidator.ValidateAsync(_userManager, user, model.Password);
                    if (pV.Succeeded) 
                    { 
                        PasswordHasher<ApplicationUser> pH = new PasswordHasher<ApplicationUser>();
                        user.PasswordHash = pH.HashPassword(user, model.Password);
                    }
                    else
                    {
                        Errors(pV);
                        return View(model);
                    }
                }
                

                IdentityResult result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                    return RedirectToAction("ListUsers", "User");
                else
                    Errors(result);
            }

            return View(model);
        }

        public async Task<IActionResult> DeleteUser(string Id)
        {
            ApplicationUser user = await _userManager.FindByIdAsync(Id);
            if (user != null)
            {
                IdentityResult result = await _userManager.DeleteAsync(user);
                if (!result.Succeeded)
                    Errors(result);
            }

            return RedirectToAction("ListUsers", "User");
        }

        [HttpGet]
        public async Task<IActionResult> ManageCities(int? id = null, string Msg = null)
        {
            if (id == null)
            {
                id = _context.Cities.First().Id;
            }
            var cities =await _context.Cities.Where(c => c.CountryId == id).ToListAsync();
            var countries =await  _context.Countries.ToListAsync();
            return View(new CitySelectViewModel
            {
                Country = countries.FirstOrDefault(c => c.Id == id),
                Cities = cities,
                Countries = countries,
                Message = Msg
            });
        }

        [HttpPost]
        public async Task<IActionResult> ManageCities(CitySelectViewModel model)
        {
            var cities =await _context.Cities.Where(c => c.CountryId == model.Country.Id).ToListAsync();
            var countries = await _context.Countries.ToListAsync();
            
            return View(new CitySelectViewModel
            {
                Country = model.Country,
                Cities = cities,
                Countries = countries
            });
        }

        [HttpGet]
        public async Task<IActionResult> DisplayCity(int Id)
        {
            
            var city = await _context.Cities.FirstOrDefaultAsync(c => c.Id == Id);
            var country = await _context.Countries.FirstOrDefaultAsync(c => c.Id == city.CountryId);
            var countries = await _context.Countries.ToListAsync();

            return View(new CitySelectViewModel 
            {
                City = city,
                Country = country,
                Countries = countries
            });
        }

        [HttpPost]
        public async Task<IActionResult> UpdateCity(CitySelectViewModel model)
        {
            var updateCity = await _context.Cities.FirstOrDefaultAsync(c => c.Id == model.City.Id);
            if(updateCity != null)
            {
                updateCity.CityName = model.City.CityName;
                updateCity.CountryId = model.City.CountryId;

                await _context.SaveChangesAsync();
            }
            
            return RedirectToAction("ManageCities", new { id = model.City.CountryId });
        }

        [HttpGet]
        public async Task<IActionResult> AddCity(int CountryId)
        {
            var country = await _context.Countries.FirstOrDefaultAsync(c => c.Id == CountryId);

            return View(new CitySelectViewModel
            {
                Country = country
            });
        }

        [HttpPost]
        public async Task<IActionResult> AddCity(CitySelectViewModel model)
        {
            var newCity = new City
            {
                CityName = model.City.CityName,
                CountryId = model.Country.Id
            };
            await _context.Cities.AddAsync(newCity);
            await _context.SaveChangesAsync();

            return RedirectToAction("ManageCities", new {id = model.Country.Id });
        }

        public async Task<IActionResult> DeleteCity(int Id)
        {
            
            var city = await _context.Cities.FirstOrDefaultAsync(c => c.Id == Id);
            var country = await _context.Countries.FirstOrDefaultAsync(c => c.Id == city.CountryId);
            if (_context.Users.Any(c => c.CityId == Id))
            {
                string msg = "Could not remove the city. There are users connected.";
                return RedirectToAction("ManageCities", new { id = country.Id, Msg = msg });
            }
            _context.Cities.Remove(city);
            await _context.SaveChangesAsync();
            
            return RedirectToAction("ManageCities", new { id = country.Id});
        }
        [HttpGet]
        public async Task<IActionResult> EditCountry(int Id)
        {
            var country = await _context.Countries.FirstOrDefaultAsync(c => c.Id == Id);

            return View(country);
        }
        [HttpPost]
        public async Task<IActionResult> EditCountry(Country country)
        {
            var updateCountry = await _context.Countries.FirstOrDefaultAsync(c => c.Id == country.Id);
            if(updateCountry != null)
            {
                updateCountry.CountryName = country.CountryName;
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("ManageCities", new { id = country.Id });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteCountry(int Id)
        {
            var country = await _context.Countries.FirstOrDefaultAsync(c => c.Id == Id);
            if(_context.Cities.Any(c => c.CountryId == Id))
            {
                string msg = "Could not remove the country. There are cities connected.";
                return RedirectToAction("ManageCities", new { id = country.Id, Msg = msg });
            }
            _context.Countries.Remove(country);
            await _context.SaveChangesAsync();

            return RedirectToAction("ManageCities");

        }

        [HttpGet]
        public IActionResult AddCountry()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddCountry(Country country)
        {
            var newCountry = new Country
            {
                CountryName = country.CountryName
            };

            await _context.Countries.AddAsync(newCountry);
            await _context.SaveChangesAsync();

            return RedirectToAction("ManageCities", new { id = newCountry.Id });
        }

        private void Errors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
        }
    }
}
