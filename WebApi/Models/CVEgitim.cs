using System;

namespace WebApi.Models
{
    public class CVEgitim
    {
        public int Id { get; set; }
        public int CvId { get; set; }
        public string EgitimDurumu { get; set; }
        public string Adi { get; set; }
        public int Notu { get; set; }
        public DateTime BaslangicTarihi { get; set; }
        public DateTime BitisTarihi { get; set; }
    }
}
