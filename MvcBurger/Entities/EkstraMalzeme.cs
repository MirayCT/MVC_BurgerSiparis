namespace MvcBurger.Entities
{
    public class EkstraMalzeme
    {
        public int Id { get; set; }
        public string Adi { get; set; }
        public double Fiyat { get; set; }
        //public List<MenuSiparis> MenuSiparisler { get; set; }
        public ICollection<Siparis>? Siparisler { get; set; }
    }
}
