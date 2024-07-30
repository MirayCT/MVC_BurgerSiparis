using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MvcBurger.Areas.Identity.Data;
using MvcBurger.Entities;

namespace MvcBurger.Controllers
{
    //[Authorize(Roles ="Yonetici")]
    public class EkstraMalzemeController : Controller
    {
        private readonly MvcBurgerContext _context;

        public EkstraMalzemeController(MvcBurgerContext context)
        {
            _context = context;
        }

        //EkstraMalzeme Listeleme
        public async Task<IActionResult> Index()
        {
              return _context.EkstraMalzemeler != null ? 
                          View(await _context.EkstraMalzemeler.ToListAsync()) :
                          Problem("Entity set 'MvcBurgerContext.EkstraMalzemeler'  is null.");
        }

        // GET: EkstraMalzeme/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.EkstraMalzemeler == null)
            {
                return NotFound();
            }

            var ekstraMalzeme = await _context.EkstraMalzemeler
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ekstraMalzeme == null)
            {
                return NotFound();
            }

            return View(ekstraMalzeme);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Adi,Fiyat")] EkstraMalzeme ekstraMalzeme)
        {
            if (ModelState.IsValid)
            {
                _context.Add(ekstraMalzeme);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(ekstraMalzeme);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.EkstraMalzemeler == null)
            {
                return NotFound();
            }

            var ekstraMalzeme = await _context.EkstraMalzemeler.FindAsync(id);
            if (ekstraMalzeme == null)
            {
                return NotFound();
            }
            return View(ekstraMalzeme);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Adi,Fiyat")] EkstraMalzeme ekstraMalzeme)
        {
            if (id != ekstraMalzeme.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ekstraMalzeme);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EkstraMalzemeExists(ekstraMalzeme.Id))
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
            return View(ekstraMalzeme);
        }

        // GET: EkstraMalzeme/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.EkstraMalzemeler == null)
            {
                return NotFound();
            }

            var ekstraMalzeme = await _context.EkstraMalzemeler
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ekstraMalzeme == null)
            {
                return NotFound();
            }

            return View(ekstraMalzeme);
        }

        // POST: EkstraMalzeme/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.EkstraMalzemeler == null)
            {
                return Problem("Entity set 'MvcBurgerContext.EkstraMalzemeler'  is null.");
            }
            var ekstraMalzeme = await _context.EkstraMalzemeler.FindAsync(id);
            if (ekstraMalzeme != null)
            {
                _context.EkstraMalzemeler.Remove(ekstraMalzeme);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EkstraMalzemeExists(int id)
        {
          return (_context.EkstraMalzemeler?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
