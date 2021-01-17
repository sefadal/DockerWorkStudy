using System;

namespace WebApi.Models
{
    public class FirmaIlan
    {
        public int Id { get; set; }
        public int FirmaId { get; set; }
        public string IlanAciklama { get; set; }
        public string Konum { get; set; }
        public DateTime SonaErmeSuresi { get; set; }
    }

    public class SearchFilter
    {
        public string searchString { get; set; }
        public string sortOrder { get; set; }
    }
}
