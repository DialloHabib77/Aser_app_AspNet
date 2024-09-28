using aser_electrification.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace aser_electrification.Controllers
{
    public class StatistiquesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StatistiquesController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ActionResult> Index()
        {
            var model = new StatistiquesViewModel
            {
                TauxAccesGlobalParMenage = await CalculerTauxAccesGlobalParMenageAsync(),
                TauxCouvertureGlobalParVillage = await CalculerTauxCouvertureGlobalParVillageAsync(),
                StatistiquesParRegion = await CalculerStatistiquesParRegionAsync()
            };

            return View(model);
        }

        private async Task<double> CalculerTauxAccesGlobalParMenageAsync()
        {
            var latestRecensementDate = await _context.Recensements.MaxAsync(r => r.Date_Recensement);

            var totalMenages = await _context.Villages.SumAsync(v => v.Nombre_Menages);
            var menagesElectrifies = await _context.Recensements
                .Where(r => r.Date_Recensement == latestRecensementDate)
                .SumAsync(r => r.Nombre_Menages_Electrifies);

            return totalMenages > 0 ? (double)menagesElectrifies / totalMenages * 100 : 0;
        }

        private async Task<double> CalculerTauxCouvertureGlobalParVillageAsync()
        {
            var totalVillages = await _context.Villages.CountAsync();
            var villagesElectrifies = await _context.Villages.CountAsync(v => v.Statut_Electrification);

            return totalVillages > 0 ? (double)villagesElectrifies / totalVillages * 100 : 0;
        }

        private async Task<List<RegionStatViewModel>> CalculerStatistiquesParRegionAsync()
        {
            var latestRecensementDate = await _context.Recensements.MaxAsync(r => r.Date_Recensement);

            return await _context.Regions
                .Select(r => new RegionStatViewModel
                {
                    NomRegion = r.Nom_Region,
                    TauxAccesParMenage = r.Departements
                        .SelectMany(d => d.Communes)
                        .SelectMany(c => c.Villages)
                        .Sum(v => v.Nombre_Menages) > 0
                            ? (double)r.Departements
                                .SelectMany(d => d.Communes)
                                .SelectMany(c => c.Villages)
                                .SelectMany(v => v.Recensements)
                                .Where(rec => rec.Date_Recensement == latestRecensementDate)
                                .Sum(rec => rec.Nombre_Menages_Electrifies)
                            / r.Departements
                                .SelectMany(d => d.Communes)
                                .SelectMany(c => c.Villages)
                                .Sum(v => v.Nombre_Menages) * 100
                            : 0,
                    TauxCouvertureParVillage = r.Departements
                        .SelectMany(d => d.Communes)
                        .SelectMany(c => c.Villages)
                        .Any()
                            ? (double)r.Departements
                                .SelectMany(d => d.Communes)
                                .SelectMany(c => c.Villages)
                                .Count(v => v.Statut_Electrification)
                            / r.Departements
                                .SelectMany(d => d.Communes)
                                .SelectMany(c => c.Villages)
                                .Count() * 100
                            : 0
                })
                .ToListAsync();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
            base.Dispose(disposing);
        }
    }

    public class StatistiquesViewModel
    {
        public double TauxAccesGlobalParMenage { get; set; }
        public double TauxCouvertureGlobalParVillage { get; set; }
        public List<RegionStatViewModel> StatistiquesParRegion { get; set; }
    }

    public class RegionStatViewModel
    {
        public string NomRegion { get; set; }
        public double TauxAccesParMenage { get; set; }
        public double TauxCouvertureParVillage { get; set; }
    }
}