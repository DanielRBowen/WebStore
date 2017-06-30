using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Tax.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Tax.Controllers
{
    [Route("api/[controller]")]
    public class TaxController : Controller
    {
        private readonly TaxContext _context;

        public TaxController(TaxContext context)
        {
            _context = context;

            if (_context.TaxZones.Count() == 0)
            {
                _context.TaxZones.Add(new TaxZone { ZipCode = 84037, TaxRate = (decimal)0.1 });
                _context.SaveChanges();
            }
        }


        // GET: api/values
        [HttpGet("{zipcode}", Name = "GetTaxByZip" )]   
        public IActionResult GetTaxByZip(decimal zipcode)
        {
            var taxZone = _context.TaxZones.FirstOrDefault(t => t.ZipCode == zipcode);
              
            if (taxZone == null)
            {
                return NotFound();
            }
            return new ObjectResult(taxZone);
        }

        [HttpPost]
        public IActionResult Create([FromBody] TaxZone taxZone)
        {
            if (taxZone == null)
            {
                return BadRequest();
            }

            _context.TaxZones.Add(taxZone);
            _context.SaveChanges();

            return CreatedAtRoute("Get", new { id = taxZone.Id }, taxZone);
        }

        [HttpPut("{id}")]
        public IActionResult Update(long id, [FromBody] TaxZone taxZone)
        {
            if (taxZone == null || taxZone.Id != id)
            {
                return BadRequest();
            }

            var tax = _context.TaxZones.FirstOrDefault(t => t.Id == id);
            if (tax == null)
            {
                return NotFound();
            }

            tax.ZipCode = taxZone.ZipCode;
            tax.TaxRate = taxZone.TaxRate;

            _context.TaxZones.Update(tax);
            _context.SaveChanges();
            return new NoContentResult();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            var tax = _context.TaxZones.First(t => t.Id == id);
            if (tax == null)
            {
                return NotFound();
            }

            _context.TaxZones.Remove(tax);
            _context.SaveChanges();
            return new NoContentResult();
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
