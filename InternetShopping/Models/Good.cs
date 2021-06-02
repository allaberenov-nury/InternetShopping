using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace InternetShopping.Models
{
    public class Good
    {
        public int Id { get; set; }
        
        [MaxLength(50)]
        [Required]
        public string Name { get; set; }
        [Column(TypeName = "decimal(6, 2)")]
        public decimal Price { get; set; }

       
       
        public List<OrderDetail> OrderDetails { get; set; }

    }
}
