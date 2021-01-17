using System.Collections.Generic;

namespace WebApi.Models
{
    public class CVKullanici
    {
        public int Id { get; set; }
        public int KullaniciId { get; set; }
        public string Meslegi { get; set; }
        public int CalismaSuresi { get; set; }
    }
}