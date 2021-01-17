using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using WepApi.Models;
using WepApi.Helpers;

namespace WebApi.Services
{
    public interface IKullaniciService
    {
        Kullanici Authenticate(string kullaniciAdi, string sifre);
        IEnumerable<Kullanici> GetAll();
        IEnumerable<Kullanici> Insert(Kullanici user);
        bool IsUserExist(Kullanici user);
    }

    public class UserService : IKullaniciService
    {
        private List<Kullanici> _users = new List<Kullanici>
        {
            new Kullanici { Id = 1, Ad = "Sefa Melih", Soyad = "Dal", KullaniciAdi = "sefadal", Sifre = "1234" },
        };

        private readonly AppSettings _appSettings;

        public UserService(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }

        public Kullanici Authenticate(string kullaniciAdi, string sifre)
        {
            var user = _users.SingleOrDefault(x => x.KullaniciAdi == kullaniciAdi && x.Sifre == sifre);

            if (user == null)
                return null;

            var tokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            user.Token = tokenHandler.WriteToken(token);

            user.Sifre = null;

            return user;
        }

        public IEnumerable<Kullanici> GetAll()
        {
            return _users.Select(x =>
            {
                x.Sifre = null;
                return x;
            });
        }

        public bool IsUserExist(Kullanici user)
        {
            bool isExist;

            var userName = user.KullaniciAdi.ToLower();
            isExist = _users.Any(n => n.KullaniciAdi == user.KullaniciAdi.ToLower());

            return isExist;
        }

        public IEnumerable<Kullanici> Insert(Kullanici user)
        {
            _users.Add(user);
            return _users;
        }
    }
}