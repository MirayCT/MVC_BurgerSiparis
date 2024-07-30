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
using MvcBurger.Models;

namespace MvcBurger.Controllers
{
   // [Authorize(Roles ="Yonetici")]
    public class MenuController : Controller
    {
        private readonly MvcBurgerContext _context;

        public MenuController(MvcBurgerContext context)
        {
            _context = context;
        }

        // GET: Menu
        public async Task<IActionResult> Index()
        {
              return _context.Menuler != null ? 
                          View(await _context.Menuler.ToListAsync()) :
                          Problem("Entity set 'MvcBurgerContext.Menuler'  is null.");
        }

        // GET: Menu/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Menuler == null)
            {
                return NotFound();
            }

            var menu = await _context.Menuler
                .FirstOrDefaultAsync(m => m.Id == id);
            if (menu == null)
            {
                return NotFound();
            }

            return View(menu);
        }

        // GET: Menu/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MenuViewModel menuViewModel)
        {
            if (ModelState.IsValid)
            {
                Menu menu = new Menu();
                if(menuViewModel.ResimAdi != null)
                {
                    var dosyaAdi = menuViewModel.ResimAdi.FileName;
                    var konum = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Resimler", dosyaAdi);

                    //Ekleme için akış ortamı oluşturalım
                    var akisOrtami = new FileStream(konum, FileMode.Create);

                    //Resmi kaydet
                    menuViewModel.ResimAdi.CopyTo(akisOrtami);

                    //ortamı kapat
                    akisOrtami.Close();

                    menu.ResimAdi = dosyaAdi;

                }

                menu.Ad = menuViewModel.Ad;
                menu.Fiyat = menuViewModel.Fiyat;

                _context.Add(menu);
                await _context.SaveChangesAsync();     
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Menu/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Menuler == null)
            {
                return NotFound();
            }

            var menu = await _context.Menuler.FindAsync(id);
            TempData["id"] = menu.Id;
            if (menu == null)
            {
                return NotFound();
            }

            MenuViewModel menuViewModel = new MenuViewModel();
            menuViewModel.Ad = menu.Ad;
            menuViewModel.Fiyat = menu.Fiyat;

            return View(menuViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(MenuViewModel menuViewModel)
        {
            var guncellenecekMenu = _context.Menuler.FirstOrDefault(m => m.Id == (int)TempData["id"]);
            if (menuViewModel.ResimAdi != null)
            {
                var dosyaAdi = menuViewModel.ResimAdi.FileName;
                var konum = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Resimler", dosyaAdi);

                //Ekleme için akış ortamı oluşturalım
                var akisOrtami = new FileStream(konum, FileMode.Create);

                //Resmi kaydet
                menuViewModel.ResimAdi.CopyTo(akisOrtami);

                //ortamı kapat
                akisOrtami.Close();

                guncellenecekMenu.ResimAdi = dosyaAdi;

            }
            guncellenecekMenu.Ad = menuViewModel.Ad;
            guncellenecekMenu.Fiyat = menuViewModel.Fiyat;

            _context.Menuler.Update(guncellenecekMenu);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Menu/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Menuler == null)
            {
                return NotFound();
            }

            var menu = await _context.Menuler
                .FirstOrDefaultAsync(m => m.Id == id);
            if (menu == null)
            {
                return NotFound();
            }

            return View(menu);
        }

        // POST: Menu/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Menuler == null)
            {
                return Problem("Entity set 'MvcBurgerContext.Menuler'  is null.");
            }
            var menu = await _context.Menuler.FindAsync(id);
            if (menu != null)
            {
                ResimSil(menu);
                _context.Menuler.Remove(menu);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MenuExists(int id)
        {
          return (_context.Menuler?.Any(e => e.Id == id)).GetValueOrDefault();
        }
        public void ResimSil(Menu menu) 
        {
         
            var resmiKullananBaskaVarMi = _context.Menuler.Any(u => u.ResimAdi == menu.ResimAdi && u.Id != menu.Id);
          

            if (!resmiKullananBaskaVarMi)
            {
                var dosya = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Resimler", menu.ResimAdi);            
                System.IO.File.Delete(dosya);
            }
        }
    }
}
