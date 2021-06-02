using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InternetShopping.Models
{
   
    public class OrderDetail
    {
        public int Id { get; set; }
        public int? GoodId { get; set; }
        public Good Good { get; set; }
        public int OrderId { get; set; }
        public Order Order { get; set; }
        public int Quantity { get; set; }


    }
    
}
