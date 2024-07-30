namespace MvcBurger.Entities
{
    public class Menu
    {
        public int Id { get; set; }
        public string Ad { get; set; }
        public double Fiyat { get; set; }
        public string? ResimAdi { get; set; }
        public ICollection<Siparis>? Siparisler { get; set; }

        //public List<MenuSiparis> MenuSiparisler { get; set; }
    }
}
