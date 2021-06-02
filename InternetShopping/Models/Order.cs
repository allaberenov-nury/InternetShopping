using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace InternetShopping.Models
{
    public enum StatusValues
    {
        Registered,
        Formed,
        Executed,
        Cancelled
    }
    public class Order
    {
      //  [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        public DateTime RegisterDate { get; set; }
        
        [MaxLength(150)]
        [Required]
        public string FullName { get; set; }
        public StatusValues Status { get; set; }
      
        public List<OrderDetail> OrderDetail { get; set; } 
     }
}
