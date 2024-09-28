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
    public class VillagesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VillagesController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index(int pg = 1)
        {
            var villages = _context.Villages.Include("Commune");
            const int pageSize = 5;
            if (pg < 1)
                pg = 1;
            int recsCount = villages.Count();
            var pager = new Pager(recsCount, pg, pageSize);
            int resSkip = (pg - 1) * pageSize;
            var data = villages.Skip(resSkip).Take(pager.PageSize).ToList();
            this.ViewBag.Pager = pager;

          
            return View(data);
            
        }
       

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var village = await _context.Villages
                .Include(v => v.Commune)
                .FirstOrDefaultAsync(m => m.ID_Village == id);
            if (village == null)
            {
                return NotFound();
            }

            return View(village);
        }
        [HttpGet]
        public IActionResult Create()
        {
            LoadRegions();
            return View();
        }
        [NonAction]
        private void LoadRegions()
        {
            var commune = _context.Communes.ToList();
            ViewBag.Commune = new SelectList(commune, "ID_Commune", "Nom_Commune");
        }

        [HttpPost]
        public IActionResult Create(Village model)
        {

            _context.Villages.Add(model);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));

        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id != null)
            {
                NotFound();
            }
            LoadRegions();
            var village = _context.Villages.Find(id);
            return View(village);
        }
        [HttpPost]
        public IActionResult Edit(Village model)
        {
            ModelState.Remove("Commune");
            
                _context.Villages.Update(model);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
          
        }
        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id != null)
            {
                NotFound();
            }
            LoadRegions();
            var village = _context.Villages.Find(id);
            return View(village);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(Village model)
        {

            _context.Villages.Remove(model);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        private bool VillageExists(int id)
        {
            return _context.Villages.Any(e => e.ID_Village == id);
        }
    }
}
