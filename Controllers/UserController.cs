using CMSApplication.Data;
using CMSApplication.Models;
using CMSApplication.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMSApplication.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        public UserController(ApplicationDbContext context, RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task<IActionResult> ListUsers()
        {
            var users = await _userManager.Users.OrderBy(u => u.FirstName).ToListAsync();
            return View(users);
        }

        public async Task<IActionResult> DisplayUser(string Id)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == Id);
            var city = _context.Cities.FirstOrDefault(c => c.Id == user.CityId);
            var country = _context.Countries.FirstOrDefault(c => c.Id == city.CountryId);
            var userRoles = await _userManager.GetRolesAsync(user);
            
            
            
            
            //var roles = _context.UserRoles.Where(u => u.UserId == Id).Select(r => r.RoleId).ToList();
            
            return View(new DisplayUserViewModel
            {
                applicationUser = user,
                City = city,
                Country = country,
                Roles = userRoles
            }) ;
        }
    }
}
