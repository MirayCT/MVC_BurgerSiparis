using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MvcBurger.Areas.Identity.Data;
using MvcBurger.Entities;
using static MvcBurger.Enums.Buyuluk;

namespace MvcBurger.Controllers
{
    //[Authorize(Roles ="Musteri")]
    public class SiparisController : Controller
    {
        private readonly MvcBurgerContext _context;
        private readonly UserManager<MvcBurgerUser> _userManager;

        public SiparisController(MvcBurgerContext context, UserManager<MvcBurgerUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Siparis
        public async Task<IActionResult> Index()
        {
            //Tüm siparisler
            ViewBag.Menuler = await _context.Menuler.ToListAsync();
            ViewBag.EkstraMalzemeler = await _context.EkstraMalzemeler.ToListAsync();


            var allUsers = await _userManager.Users
                .Include(u => u.Siparisler).ThenInclude(s => s.Menuler)
                .Include(u => u.Siparisler).ThenInclude(s => s.EkstraMalzemeler)
                .ToListAsync();
            
            var userId = _userManager.GetUserId(HttpContext.User);
            //Anlık kullanıcı
            var user = allUsers.FirstOrDefault(u => u.Id == userId);
            ViewBag.FullName = user.Ad + " " + user.Soyad;
            //Kullanıcının kendi siparisleri
            ViewBag.KullaniciSiparis = user.Siparisler;
            return View();
            
        }

        // GET: Siparis/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Siparisler == null)
            {
                return NotFound();
            }

            var siparis = await _context.Siparisler
                .FirstOrDefaultAsync(m => m.Id == id);
            if (siparis == null)
            {
                return NotFound();
            }

            return View(siparis);
        }

        // GET: Siparis/Create
        public IActionResult Create()
        {
            ViewBag.Menuler1 = _context.Menuler.ToList();
            ViewBag.EkstraMalzemeler1 = _context.EkstraMalzemeler.ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int menuId, int extraId, string size, int quantity, double totalPrice)
        {
            var allUsers = await _userManager.Users.Include(u => u.Siparisler).ToListAsync();

            var userId = _userManager.GetUserId(HttpContext.User);
            //Anlık kullanıcı
            var user = allUsers.FirstOrDefault(u => u.Id == userId);

            var selectedMenu = _context.Menuler.FirstOrDefault(m => m.Id == menuId);
            var selectedExtra = _context.EkstraMalzemeler.FirstOrDefault(e => e.Id == extraId);
      
            Siparis siparis = new Siparis()
            {
                Buyukluk = (Buyukluk)System.Enum.Parse(typeof(Buyukluk), size),
                SiparisSayisi = quantity,
                Menuler = new List<Menu>(),
                EkstraMalzemeler = new List<EkstraMalzeme>(),


            };
            siparis.Menuler.Add(selectedMenu);
            siparis.EkstraMalzemeler.Add(selectedExtra);
            siparis.ToplamFiyat = totalPrice;
            user.Siparisler.Add(siparis);
            await _userManager.UpdateAsync(user);


            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> Update(int? id)
        {
            ViewBag.Menuler2 = _context.Menuler.ToList();
            ViewBag.EkstraMalzemeler2 = _context.EkstraMalzemeler.ToList();

            if (id == null || _context.Siparisler == null)
            {
                return NotFound();
            }

            var siparis = await _context.Siparisler.FindAsync(id);
            if (siparis == null)
            {
                return NotFound();
            }
            return View(siparis);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int siparisId, int menuId, int extraId, string size, int quantity, double totalPrice)
        {
            var allUsers = await _userManager.Users
                .Include(u => u.Siparisler).ThenInclude(s => s.Menuler)
                .Include(u => u.Siparisler).ThenInclude(s => s.EkstraMalzemeler)
                .ToListAsync();

            List<Menu> secilenMenuler = new List<Menu>();
            List<EkstraMalzeme> secilenEkstraMalzemeler = new List<EkstraMalzeme>();
 
            var userId = _userManager.GetUserId(HttpContext.User);
            // Anlık kullanıcı
            var user = allUsers.FirstOrDefault(u => u.Id == userId);

            var siparis = user.Siparisler.FirstOrDefault(s => s.Id == siparisId);

            if (siparis == null)
            {
                return NotFound();
            }

            var selectedMenu = _context.Menuler.FirstOrDefault(m => m.Id == menuId);
            secilenMenuler.Add(selectedMenu);

            var selectedExtra = _context.EkstraMalzemeler.FirstOrDefault(e => e.Id == extraId);
            secilenEkstraMalzemeler.Add(selectedExtra);

            // Yeni menü ve ekstra malzemeleri ekle
            siparis.Menuler = secilenMenuler;
            siparis.EkstraMalzemeler = secilenEkstraMalzemeler;

            // Yeni boyut ve miktarı ayarla
            siparis.Buyukluk = (Buyukluk)Enum.Parse(typeof(Buyukluk), size);
            siparis.SiparisSayisi = quantity;

            // Yeni toplam fiyatı hesapla ve ayarla
            siparis.ToplamFiyat = totalPrice;

            // Kullanıcıyı güncelle
            await _userManager.UpdateAsync(user);

            return RedirectToAction("Index");
        }

        // GET: Siparis/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Siparisler == null)
            {
                return NotFound();
            }

            var siparis = await _context.Siparisler
                .FirstOrDefaultAsync(m => m.Id == id);
            if (siparis == null)
            {
                return NotFound();
            }

            return View(siparis);
        }

        // POST: Siparis/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
        

            List<Menu> secilenMenuler = new List<Menu>();
            List<EkstraMalzeme> secilenEkstraMalzemeler = new List<EkstraMalzeme>();

            var userId = _userManager.GetUserId(HttpContext.User);
            // Anlık kullanıcı
            

            var siparis = _context.Siparisler.Include(s => s.Menuler).Include(e => e.EkstraMalzemeler).FirstOrDefault(s => s.Id == id);

            if (_context.Siparisler == null)
            {
                return Problem("Entity set 'MvcBurgerContext.Siparisler'  is null.");
            }

            if (siparis != null)
            {
                _context.Siparisler.Remove(siparis);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SiparisExists(int id)
        {
          return (_context.Siparisler?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        //private double SiparisToplamHesapla(Siparis siparis)
        //{
        //    double totalPrice = 0;

        //    if (siparis.Menuler != null)
        //    {
        //        foreach (var menu in siparis.Menuler)
        //        {
        //            // Menü fiyatı boyuta göre hesaplanıyor
        //            switch (siparis.Buyukluk)
        //            {
        //                case Buyukluk.Kucuk:
        //                    totalPrice += menu.Fiyat * siparis.SiparisSayisi;
        //                    break;
        //                case Buyukluk.Orta:
        //                    totalPrice += (menu.Fiyat * siparis.SiparisSayisi) + 50;
        //                    break;
        //                case Buyukluk.Buyuk:
        //                    totalPrice += (menu.Fiyat * siparis.SiparisSayisi) + 100;
        //                    break;
        //                default:
        //                    break;
        //            }
        //        }
        //    }

        //    if (siparis.EkstraMalzemeler != null)
        //    {
        //        foreach (var ekstra in siparis.EkstraMalzemeler)
        //        {
        //            totalPrice += ekstra.Fiyat;
        //        }
        //    }

        //    return totalPrice;
        //}
    }
}
