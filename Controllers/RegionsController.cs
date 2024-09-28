using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using aser_electrification.Data;
using aser_electrification.Models;

namespace aser_electrification.Controllers
{
    public class RegionsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RegionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Regions
        public async Task<IActionResult> Index(int pg=1)
        {
             List<Region> regions = _context.Regions.ToList();
            const int pageSize = 5;
            if(pg < 1)
                pg = 1;
            int recsCount = regions.Count();
            var pager = new Pager(recsCount,pg,pageSize);
            int resSkip = (pg - 1) * pageSize;
            var data = regions.Skip(resSkip).Take(pager.PageSize).ToList();
            this.ViewBag.Pager = pager;
            
           // return View(await _context.Regions.ToListAsync());
           return View(data);
        }

        // GET: Regions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var region = await _context.Regions
                .FirstOrDefaultAsync(m => m.ID_Region == id);
            if (region == null)
            {
                return NotFound();
            }

            return View(region);
        }

        // GET: Regions/Create
        public IActionResult Create()
        {
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID_Region,Nom_Region")] Region region)
        {
                _context.Add(region);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
          
        }

    
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var region = await _context.Regions.FindAsync(id);
            if (region == null)
            {
                return NotFound();
            }
            return View(region);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID_Region,Nom_Region")] Region region)
        {
            if (id != region.ID_Region)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(region);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RegionExists(region.ID_Region))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(region);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var region = await _context.Regions
                .FirstOrDefaultAsync(m => m.ID_Region == id);
            if (region == null)
            {
                return NotFound();
            }

            return View(region);
        }

   
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var region = await _context.Regions.FindAsync(id);
            if (region != null)
            {
                _context.Regions.Remove(region);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RegionExists(int id)
        {
            return _context.Regions.Any(e => e.ID_Region == id);
        }
    }
}
