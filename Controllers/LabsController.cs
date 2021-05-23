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
    public class LabsController : Controller
    {
        private readonly Context cntx;

        public LabsController(Context context)
        {
            cntx = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await cntx.Labs.ToListAsync());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Address")] LabModel labModel)
        {
            if (ModelState.IsValid)
            {
                cntx.Add(labModel);
                await cntx.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(labModel);
        }

        public async Task<IActionResult> Edit(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var labModel = await cntx.Labs.FindAsync(id);
            if (labModel == null)
            {
                return NotFound();
            }
            return View(labModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Address")] LabModel labModel)
        {
            if (id != labModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    cntx.Update(labModel);
                    await cntx.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LabModelExists(labModel.Id))
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
            return View(labModel);
        }

        public async Task<IActionResult> Details(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var labModel = await cntx.Labs
                .FirstOrDefaultAsync(m => m.Id == id);
            if (labModel == null)
            {
                return NotFound();
            }

            return View(labModel);
        }

        public async Task<IActionResult> Delete(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var labModel = await cntx.Labs
                .FirstOrDefaultAsync(m => m.Id == id);
            if (labModel == null)
            {
                return NotFound();
            }

            return View(labModel);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var labModel = await cntx.Labs.FindAsync(id);
            cntx.Labs.Remove(labModel);
            await cntx.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LabModelExists(int id)
        {
            return cntx.Labs.Any(e => e.Id == id);
        }
    }
}
