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
    public class CommunesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CommunesController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(int pg=1)
        {
            var communes = _context.Communes.Include("Departement");
            const int pageSize = 5;
            if (pg < 1)
                pg = 1;
            int recsCount = communes.Count();
            var pager = new Pager(recsCount, pg, pageSize);
            int resSkip = (pg - 1) * pageSize;
            var data = communes.Skip(resSkip).Take(pager.PageSize).ToList();
            this.ViewBag.Pager = pager;

            return View(data);
            //var communes = _context.Communes.Include("Departement");
            //return View(communes);
        }
       

    
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var commune = await _context.Communes
                .Include(c => c.Departement)
                .FirstOrDefaultAsync(m => m.ID_Commune == id);
            if (commune == null)
            {
                return NotFound();
            }

            return View(commune);
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
            var departement = _context.Departements.ToList();
            ViewBag.Departement = new SelectList(departement, "ID_Departement", "Nom_Departement");
        }

        [HttpPost]
        public IActionResult Create(Commune model)
        {

            _context.Communes.Add(model);
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
            var commune = _context.Communes.Find(id);
            return View(commune);
        }
        [HttpPost]
        public IActionResult Edit(Commune model)
        {
                _context.Communes.Update(model);
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
            var commune = _context.Communes.Find(id);
            return View(commune);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(Commune model)
        {

            _context.Communes.Remove(model);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        private bool CommuneExists(int id)
        {
            return _context.Communes.Any(e => e.ID_Commune == id);
        }
    }
}
