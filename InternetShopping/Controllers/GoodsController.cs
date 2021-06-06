
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InternetShopping.Models;
using InternetShopping.Data;
namespace InternetShopping.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class GoodsController : ControllerBase
    {
        private readonly InternetShoppingDbContext _context;

        public GoodsController(InternetShoppingDbContext context)
        {
            _context = context;
        }

       
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Good>>> GetGoods()
        {
            return await _context.Goods
                .AsNoTracking()
                .ToListAsync();
        }

        
        [HttpGet("{id}")]
        public async Task<ActionResult<Good>> GetGood(int id)
        {
            var good = await _context.Goods
                       .FindAsync(id);

            if (good == null)
            {
                return NotFound("Good isn't found.");
            }

            return good;
        }

        
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGood(int id, Good good)
        {
            if (id != good.Id)
            {
                return BadRequest("Ids doesn't match.");
            }

            

            _context.Entry(good).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!GoodExists(id))
                {
                    return NotFound("Good isn't found");
                }
                else
                {
                    return BadRequest(ex.Message);
                }
            }
            catch
            {
                return BadRequest("Error update the database.");
            }

            return NoContent();
        }

        
        [HttpPost]
        public async Task<ActionResult<Good>> PostGood(Good good)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            
            try
            {
                _context.Goods.Add(good);
                await _context.SaveChangesAsync();
            }
            catch(DbUpdateException ex)
            {
                return BadRequest(ex.Message);
            }
            catch 
            {
                return BadRequest("Error update the database");
            }

            return CreatedAtAction(nameof(GetGood), new { id = good.Id }, good);
        }

      
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGood(int id)
        {
         
            var good =await  _context.Goods.FindAsync(id);

            if (good == null)
            {
                return NotFound("Good isn't found");
            }
            
            try
            {
                _context.Goods.Remove(good);
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

        private bool GoodExists(int id)
        {
            return _context.Goods.Any(e => e.Id == id);
        }
    }
}
