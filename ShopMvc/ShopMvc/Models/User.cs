using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopMvc.Models
{
    public class User : IdentityUser
    {
        public string NickName { get; set; }
        public int PurchasesCount { get; set; }
    }
}
