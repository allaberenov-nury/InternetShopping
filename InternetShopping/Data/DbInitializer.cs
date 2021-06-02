using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InternetShopping.Models;

namespace InternetShopping.Data
{
    public static class DbInitializer
    {
        public static void Initialize(InternetShoppingDbContext context)
        {
            if (context.Goods.Any())
            {
                return;   // DB has been seeded
            }

          
            var good1 = new Good { Name = "Pen", Price = 3500.0m };
            var good2 = new Good { Name = "Book", Price = 5500.0m };
            var good3 = new Good { Name = "Glue", Price = 500.0m };

            var orders = new Order[]
            {
                 new Order{  FullName="Petrova S.", RegisterDate=DateTime.Parse("11.02.2021"), Status=StatusValues.Registered,
                     OrderDetail=new List<OrderDetail>
                    {
                       new OrderDetail{ Quantity=3,  Good=good1 },
                       new OrderDetail{ Quantity=2, Good=good2 }
                    }
                 },

                 new Order{  FullName="Karokov. N.", RegisterDate=DateTime.Parse("12.02.2021"), Status=StatusValues.Cancelled,
                     OrderDetail=new List<OrderDetail>
                    {
                       new OrderDetail{ Quantity=1,  Good=good2 },
                       new OrderDetail{ Quantity=2, Good=good3 }
                    }
                 },
                 new Order{  FullName="Mihalov. K.", RegisterDate=DateTime.Parse("12.02.2020"), Status=StatusValues.Registered,
                     OrderDetail=new List<OrderDetail>
                    {
                       new OrderDetail{ Quantity=4,  Good=good1 },
                       new OrderDetail{ Quantity=3, Good=good3 }
                    }
                 },
            };

           
            context.Orders.AddRange(orders);
            context.SaveChanges();

        }
    }
}
