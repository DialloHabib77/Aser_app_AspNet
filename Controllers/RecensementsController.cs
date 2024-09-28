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
    public class RecensementsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RecensementsController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index(int pg = 1, int? regionId = null, int? departementId = null, int? communeId = null)
        {
            IQueryable<Recensement> recensements = _context.Recensements
                .Include(r => r.Village)
                .ThenInclude(v => v.Commune)
                .ThenInclude(c => c.Departement)
                .ThenInclude(d => d.Region);

            if (regionId.HasValue)
            {
                recensements = recensements.Where(r => r.Village.Commune.Departement.Region.ID_Region == regionId);
            }

            if (departementId.HasValue)
            {
                recensements = recensements.Where(r => r.Village.Commune.Departement.ID_Departement == departementId);
            }

            if (communeId.HasValue)
            {
                recensements = recensements.Where(r => r.Village.Commune.ID_Commune == communeId);
            }

            const int pageSize = 5;
            if (pg < 1)
                pg = 1;

            int recsCount = await recensements.CountAsync();
            var pager = new Pager(recsCount, pg, pageSize);
            int resSkip = (pg - 1) * pageSize;

            var data = await recensements.Skip(resSkip).Take(pager.PageSize).ToListAsync();

            ViewBag.Pager = pager;

            // Charger les listes pour les filtres
            ViewBag.Regions = new SelectList(await _context.Regions.ToListAsync(), "ID_Region", "Nom_Region");
            ViewBag.Departements = new SelectList(new List<Departement>(), "ID_Departement", "Nom_Departement");
            ViewBag.Communes = new SelectList(new List<Commune>(), "ID_Commune", "Nom_Commune");

            return View(data);
        }
        [HttpGet]
        public async Task<JsonResult> GetDepartements(int regionId)
        {
            var departements = await _context.Departements
                .Where(d => d.ID_Region == regionId)
                .Select(d => new { id = d.ID_Departement, name = d.Nom_Departement })
                .ToListAsync();
            return Json(departements);
        }
        [HttpGet]
        public async Task<JsonResult> GetCommunes(int departementId)
        {
            var communes = await _context.Communes
                .Where(c => c.ID_Departement == departementId)
                .Select(c => new { id = c.ID_Commune, name = c.Nom_Commune })
                .ToListAsync();
            return Json(communes);
        }


        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recensement = await _context.Recensements
                .Include(r => r.Village)
                .FirstOrDefaultAsync(m => m.ID_Recensement == id);
            if (recensement == null)
            {
                return NotFound();
            }

            return View(recensement);
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
            var village = _context.Villages.ToList();
            ViewBag.Village = new SelectList(village, "ID_Village", "Nom_Village");
        }

        [HttpPost]
        public IActionResult Create(Recensement model)
        {

            _context.Recensements.Add(model);
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
            var recensement = _context.Recensements.Find(id);
            return View(recensement);
        }
        [HttpPost]
        public IActionResult Edit(Recensement model)
        {
            ModelState.Remove("Village");
            
            _context.Recensements.Update(model);
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
            var recensement = _context.Recensements.Find(id);
            return View(recensement);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(Recensement model)
        {

            _context.Recensements.Remove(model);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        private bool RecensementExists(int id)
        {
            return _context.Recensements.Any(e => e.ID_Recensement == id);
        }
    }
}
