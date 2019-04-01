using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using SignalR_MVC.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalR_MVC.Hubs
{
    public class ChatHub : Hub
    {

        ApplicationDbContext _context;

        public ChatHub()
        {
            _context = new ApplicationDbContext(ApplicationDbContext.Configure());
        }

        public async Task SendMessage(string user, string message)
        { 
            if (checkForRole(user))
            {
                await Clients.All.SendAsync("ReceiveMessage", user, message);
            }
        }


        private bool checkForRole(string name)
        {
            var userId = _context.Users.FirstOrDefault(x => x.UserName == name).Id;
            var roleId = _context.Roles.FirstOrDefault(x => x.Name == "chatUser").Id;
            if (_context.UserRoles.Any(x => x.RoleId == roleId && x.UserId == userId))
                return true;
            return false;
        }
    }
}
