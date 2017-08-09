using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Data;
using WebStore.Models;

namespace WebStore.Controllers
{
    public class StoreItemsController : Controller
    {
        private readonly StoreContext _context;

        public StoreItemsController(StoreContext context)
        {
            _context = context;    
        }

        // GET: StoreItems
        public async Task<IActionResult> Index()
        {
            var storeContext = _context.Products.Include(s => s.Catagory).Include(s => s.Company);
            return View(await storeContext.ToListAsync());
        }

        // GET: StoreItems/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var storeItem = await _context.Products
                .Include(s => s.Catagory)
                .Include(s => s.Company)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (storeItem == null)
            {
                return NotFound();
            }

            return View(storeItem);
        }

        // GET: StoreItems/Create
        public IActionResult Create()
        {
            ViewData["CatagoryId"] = new SelectList(_context.Catagories, "Id", "Id");
            ViewData["CompanyId"] = new SelectList(_context.Companies, "Id", "Name");
            return View();
        }

        // POST: StoreItems/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Price,CatagoryId,Description,CompanyId")] Product storeItem)
        {
            if (ModelState.IsValid)
            {
                _context.Add(storeItem);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewData["CatagoryId"] = new SelectList(_context.Catagories, "Id", "Id", storeItem.CatagoryId);
            ViewData["CompanyId"] = new SelectList(_context.Companies, "Id", "Name", storeItem.CompanyId);
            return View(storeItem);
        }

        // GET: StoreItems/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var storeItem = await _context.Products.SingleOrDefaultAsync(m => m.Id == id);
            if (storeItem == null)
            {
                return NotFound();
            }
            ViewData["CatagoryId"] = new SelectList(_context.Catagories, "Id", "Id", storeItem.CatagoryId);
            ViewData["CompanyId"] = new SelectList(_context.Companies, "Id", "Name", storeItem.CompanyId);
            return View(storeItem);
        }

        // POST: StoreItems/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Price,CatagoryId,Description,CompanyId")] Product storeItem)
        {
            if (id != storeItem.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(storeItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StoreItemExists(storeItem.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            ViewData["CatagoryId"] = new SelectList(_context.Catagories, "Id", "Id", storeItem.CatagoryId);
            ViewData["CompanyId"] = new SelectList(_context.Companies, "Id", "Name", storeItem.CompanyId);
            return View(storeItem);
        }

        // GET: StoreItems/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var storeItem = await _context.Products
                .Include(s => s.Catagory)
                .Include(s => s.Company)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (storeItem == null)
            {
                return NotFound();
            }

            return View(storeItem);
        }

        // POST: StoreItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var storeItem = await _context.Products.SingleOrDefaultAsync(m => m.Id == id);
            _context.Products.Remove(storeItem);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool StoreItemExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}
