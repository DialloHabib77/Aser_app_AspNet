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
    public class DepartementsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DepartementsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(int pg= 1)
        {
            var departements = _context.Departements.Include("Region");
            const int pageSize = 5;
            if (pg < 1)
                pg = 1;
            int recsCount = departements.Count();
            var pager = new Pager(recsCount, pg, pageSize);
            int resSkip = (pg - 1) * pageSize;
            var data = departements.Skip(resSkip).Take(pager.PageSize).ToList();
            this.ViewBag.Pager = pager;

     
            return View(data);

            //var departements = _context.Departements.Include("Region");
           // return View(departements);
        }

       
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var departement = await _context.Departements
                .Include(d => d.Region)
                .FirstOrDefaultAsync(m => m.ID_Departement == id);
            if (departement == null)
            {
                return NotFound();
            }

            return View(departement);
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
            var region = _context.Regions.ToList();
            ViewBag.Region = new SelectList(region, "ID_Region", "Nom_Region");
        }
       
        [HttpPost]
        public IActionResult Create(Departement model)
        {
           
            _context.Departements.Add(model);
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
            var departement = _context.Departements.Find(id);
            return View(departement);
        }
        [HttpPost]
        public IActionResult Edit(Departement model)
        {
            ModelState.Remove("Region");
           
            _context.Departements.Update(model);
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
            var departement = _context.Departements.Find(id);
            return View(departement);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(Departement model)
        {
          
            _context.Departements.Remove(model);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        private bool DepartementExists(int id)
        {
            return _context.Departements.Any(e => e.ID_Departement == id);
        }
    }
}
