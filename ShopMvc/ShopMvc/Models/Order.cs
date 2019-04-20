using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ShopMvc.Models
{
    public class Order 
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime OrderPlacedTime { get; set; }

        [StringLength(255)]
        [Required]
        public string FirstName { get; set; }

        [StringLength(255)]
        [Required]
        public string LastName { get; set; }

        [StringLength(255)]
        [Required]
        public string Address { get; set; }

        [StringLength(255)]
        [Required]
        public string City { get; set; }

        [StringLength(255)]
        [Required]
        public string Country { get; set; }

        [StringLength(6)]
        [Required]
        public string ZipCode { get; set; }

        [Required]
        public string UserId { get; set; }

        public ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
