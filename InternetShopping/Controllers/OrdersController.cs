
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InternetShopping.Models;
using InternetShopping.Data;
using Microsoft.AspNetCore.JsonPatch;


namespace InternetShopping.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class OrdersController : ControllerBase
    {
        private readonly InternetShoppingDbContext _context;
      
        
        public OrdersController(InternetShoppingDbContext context)
        {
            _context = context;
        }

       
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
        {
            return await _context.Orders
                .AsNoTracking()
                .Include(or=>or.OrderDetail)
                .ToListAsync();
        }

       
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(int id)
        {
           
            
            var order = await _context.Orders
                .AsNoTracking()
                .Include(or=>or.OrderDetail)
                .SingleOrDefaultAsync(or=>or.Id==id);

            if (order == null)
            {
                return NotFound("Order is not found.");
            }

            return order;
        }

       
       
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrder(int id, Order order)
        {
            if (id != order.Id)
            {
                return BadRequest("IDs do not match.");
            }

            var ord = await _context.Orders
                .AsNoTracking()
                .SingleOrDefaultAsync(or=>or.Id==id);
            
            if (ord == null)
            {
                return NotFound("Error. Order is absent");
            }

            if (order.Status != StatusValues.Registered)
            {
                ModelState.AddModelError("Operational error", "Order is not registered.");
                return BadRequest(ModelState);
            }


           

            int goodQuantity = 0;
            decimal orderSum = 0;

            if (order.OrderDetail is not null)
                foreach (OrderDetail orderDetail in order.OrderDetail)
            {

                goodQuantity += orderDetail.Quantity;
                if (goodQuantity > 10)
                    return BadRequest("Quantity exceeds the limit");



            



                    orderSum += orderDetail.Quantity * orderDetail.Good.Price;

                if (orderSum > 15000)
                    return BadRequest("Sum exceeds the limit");

            }



            _context.Entry(order).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!OrderExists(id))
                {
                    return NotFound("Oder is not found.");
                }
                else
                {
                    return BadRequest("Error update the database.");
                }
            }

            return NoContent();
        }

      
        [HttpPost]
      
        public async Task<ActionResult<Order>> PostOrder(Order order)
        {
          
           if(!ModelState.IsValid  )
            {
                return BadRequest("It is model error.");
            }

            if (order.Status != StatusValues.Registered)
            {
                ModelState.AddModelError("Operational error", "Order is not registered.");
                return BadRequest(ModelState);
            }

            int goodQuantity = 0;
            decimal orderSum = 0;




            if (order.OrderDetail is not null)
            foreach (OrderDetail orderDetail in order.OrderDetail)
            {

                goodQuantity += orderDetail.Quantity;
                if (goodQuantity > 10)
                    return BadRequest("Quantity exceeds the limit");



                var good = await _context.Goods.FindAsync(orderDetail.GoodId);
                    
                
                if (good ==null)
                {
                    ModelState.AddModelError("Operational error", "Good is null.");
                    return BadRequest(ModelState);
                }

                             
                orderSum += orderDetail.Quantity * good.Price;

                if (orderSum > 15000)
                    return BadRequest("Sum exceeds the limit");

            }

                                


            try
            {
                _context.Orders.Add(order);
                await _context.SaveChangesAsync();
            }
            catch(DbUpdateException ex)
            {
                return BadRequest(ex.Message);
            }
            catch
            {
                return BadRequest("Error. Try again.");
            }

            return CreatedAtAction(nameof(GetOrder), new { id = order.Id }, order);
        }

      
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
           
            if(id<1)
            {
                
                return BadRequest("Id isn't correct.");
            }
            
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound("Order isn't found.");
            }

            if(order.Status!=StatusValues.Registered)
            {
                ModelState.AddModelError("Operational error", "Order isn't registered.");
                return BadRequest(ModelState); 
            }

            try
            {
                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();
            }
             catch(DbUpdateException ex)
            {
                return BadRequest(ex.Message);
            }
            catch
            {
                return BadRequest("Error update the database.");
            }

            return NoContent();
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.Id == id);
        }

        [HttpPatch("{id}")]
        
        public async Task<IActionResult> JsonPatchWithModelState(int id, [FromBody] JsonPatchDocument<Order> patchDoc)
        {
            

            
            if (patchDoc != null && id>0)
            {
               
                var order = await _context.Orders
                    .Include(or => or.OrderDetail)
                    .SingleOrDefaultAsync(or=>or.Id==id);
                    

                if(order == null)
                {
                    return NotFound("Order isn't found.");
                }


                if (order.Status != StatusValues.Registered)
                    return BadRequest("Operation is forbidden.");

                int goodQuantity = 0;
                decimal orderSum = 0;
                if (order.OrderDetail is not null)
                    foreach (OrderDetail orderDetail in order.OrderDetail)
                    {

                        goodQuantity += orderDetail.Quantity;
                        if (goodQuantity > 10)
                            return BadRequest("Quantity exceeds the limit");



                        var good = await _context.Goods.FindAsync(orderDetail.GoodId);
                       


                        if (good == null)
                        {
                            ModelState.AddModelError("Operational error", "Good is null.");
                            return BadRequest(ModelState);
                        }


                        orderSum += orderDetail.Quantity * good.Price;

                        if (orderSum > 15000)
                            return BadRequest("Sum exceeds the limit");

                    }



                try
                {


                    patchDoc.ApplyTo(order, ModelState);
                    if (!ModelState.IsValid)
                    {
                        return BadRequest(ModelState);
                    }
                }
                catch
                {
                    return BadRequest("Please, enter the correct order.");
                }
                    
                
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch(DbUpdateException ex)
                {
                    return BadRequest(ex.Message);
                }
                catch
                {
                    return BadRequest("Error update the database.");
                }

                return new ObjectResult(order);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
    }
}
