using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ShopMvc.Models
{
    public class OrderDetail 
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string LiquidName { get; set; }
        public int Qty { get; set; }
        public decimal Price { get; set; }

        public int OrderId { get; set; }
        public Order Order { get; set; }
    }
}
