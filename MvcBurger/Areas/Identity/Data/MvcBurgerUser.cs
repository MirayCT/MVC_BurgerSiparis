using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using MvcBurger.Entities;

namespace MvcBurger.Areas.Identity.Data;

// Add profile data for application users by adding properties to the MvcBurgerUser class
public class MvcBurgerUser : IdentityUser
{
    public string Ad { get; set; }
    public string Soyad { get; set; }
    public ICollection<Siparis>? Siparisler { get; set; }
}

