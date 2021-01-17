using System;

namespace WebApi.Models
{
    public class CVCalisma
    {
        public int Id { get; set; }
        public int CvId { get; set; }
        public string FirmaAdi { get; set; }
        public string Pozisyon { get; set; }
        public string Aciklama { get; set; }
        public DateTime BaslangicTarihi { get; set; }
        public DateTime BitisTarihi { get; set; }
    }
}
