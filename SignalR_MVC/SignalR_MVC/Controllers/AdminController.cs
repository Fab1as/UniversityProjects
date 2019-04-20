using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Web;
using SignalR_MVC.Data;

namespace Socket_MVC_Identity.Controllers
{
    public class AdminController : Controller
    {
        private ApplicationDbContext _context;
        private UserManager<IdentityUser> _userManager;
        private RoleManager<IdentityRole> _roleManager;

        public AdminController(UserManager<IdentityUser> userManager, ApplicationDbContext dbContext, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _context = dbContext;
            _roleManager = roleManager;
        }

        public ActionResult Index()
        {
            return RedirectPermanent("~/home/index");
        }

        [Authorize(Roles = "admin")]
        public async Task<ActionResult> AddInChat()
        {
            //var user = await _userManager.FindByEmailAsync("qwe@gmail.com");
            //var admin = await _userManager.FindByEmailAsync("fab@gmail.com");
            //await _roleManager.CreateAsync(new IdentityRole { Name = "chatUser" });
            //await _roleManager.CreateAsync(new IdentityRole { Name = "admin" });
            //await _userManager.AddToRoleAsync(admin, "admin");
            //await _userManager.AddToRoleAsync(user, "chatUser");
            var users = _context.Users;
            var model = new List<IdentityUser>();
            foreach (var u in users)
            {
                bool inRole = await _userManager.IsInRoleAsync(u, "chatUser");
                if (!inRole)
                    model.Add(u);
            }
            return View(model);
        }
        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> AddInChat(string email)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(email);
                await _userManager.AddToRoleAsync(user, "chatUser");
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        [Authorize(Roles = "admin")]
        public async Task<ActionResult> RemoveFromChat()
        {
            var users = _context.Users;
            var model = new List<IdentityUser>();
            foreach (var user in users)
            {
                bool inRole = await _userManager.IsInRoleAsync(user, "chatUser");
                if (inRole)
                    model.Add(user);
            }
            return View(model);
        }
        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> RemoveFromChat(string name)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(name);
                await _userManager.RemoveFromRoleAsync(user, "chatUser");
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}