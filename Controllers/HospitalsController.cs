using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using lab5.Models;
using Microsoft.EntityFrameworkCore;

namespace lab5.Controllers
{
    public class HospitalsController : Controller
    {
        private readonly Context cntx;

        public HospitalsController(Context context)
        {
            cntx = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await cntx.Hospitals.ToListAsync());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Address,Phones")] HospitalModel hospitalModel)
        {
            if (ModelState.IsValid)
            {
                cntx.Add(hospitalModel);
                await cntx.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(hospitalModel);
        }

        public async Task<IActionResult> Edit(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hospitalModel = await cntx.Hospitals.FindAsync(id);
            if (hospitalModel == null)
            {
                return NotFound();
            }
            return View(hospitalModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Address,Phones")] HospitalModel hospitalModel)
        {
            if (id != hospitalModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    cntx.Update(hospitalModel);
                    await cntx.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HospitalModelExists(hospitalModel.Id))
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
            return View(hospitalModel);
        }
        public async Task<IActionResult> Details(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hospitalModel = await cntx.Hospitals
                .FirstOrDefaultAsync(m => m.Id == id);
            if (hospitalModel == null)
            {
                return NotFound();
            }

            return View(hospitalModel);
        }


        public async Task<IActionResult> Delete(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hospitalModel = await cntx.Hospitals
                .FirstOrDefaultAsync(m => m.Id == id);
            if (hospitalModel == null)
            {
                return NotFound();
            }

            return View(hospitalModel);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var hospitalModel = await cntx.Hospitals.FindAsync(id);
            cntx.Hospitals.Remove(hospitalModel);
            await cntx.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HospitalModelExists(int id)
        {
            return cntx.Hospitals.Any(e => e.Id == id);
        }
    }
}
