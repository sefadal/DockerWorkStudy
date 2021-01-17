using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Data;
using System.Data.SqlClient;
using WebApi.Models;
using WebApi.Utility;
using WepApi.DataAccessLayer;

namespace WebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class KullaniciCVController : Controller
    {
        SqlDataBlock sql = new SqlDataBlock();

        [HttpPost("CVOlustur")]
        public ActionResult CVOlustur(CVKullanici cv)
        {
            try
            {
                //cv var mı kontrol?

                var k = sql.ExecuteDataTable(CommandType.Text, "SELECT * FROM CVKullanici WHERE KullaniciId = @KullaniciId",
                    new SqlParameter("@KullaniciId", cv.KullaniciId)
                    );

                if (k.Rows.Count > 0)
                    return Ok("Kullanıcının CV bilgileri mevcuttur");

                var b = sql.ExecuteNonQuery(CommandType.StoredProcedure, "sp_CvOlustur",
                    new SqlParameter("@CalismaSuresi", cv.CalismaSuresi),
                    new SqlParameter("@KullaniciId", cv.KullaniciId),
                    new SqlParameter("@Meslegi", cv.Meslegi)
                    ) > 0;

                if (b)
                    return Ok();
            }
            catch (Exception ex)
            {
                FileProccess.WriteLog(ex.Message);

                return BadRequest();
            }

            return BadRequest();
        }

        [HttpPost("CVSil")]
        public ActionResult CVSil(int id)
        {
            try
            {
                //eğitim bilgileri, çalışma, başvuru bilgileri var mı kontrol?

                var k = sql.ExecuteDataTable(CommandType.Text, "SELECT * FROM CVEgitim WHERE CvId = @CvId",
                  new SqlParameter("@CvId", id)
                  );

                if (k.Rows.Count > 0)
                    return Ok("Kullanıcının eğitim bilgileri mevcuttur silinemez");

                var k2 = sql.ExecuteDataTable(CommandType.Text, "SELECT * FROM CVCalisma WHERE CvId = @CvId",
                  new SqlParameter("@CvId", id)
                  );

                if (k2.Rows.Count > 0)
                    return Ok("Kullanıcının çalışma bilgileri mevcuttur silinemez");

                var k3 = sql.ExecuteDataTable(CommandType.Text, "SELECT * FROM IlanBasvuru WHERE CvId = @CvId",
                  new SqlParameter("@CvId", id)
                  );

                if (k3.Rows.Count > 0)
                    return Ok("Kullanıcının firma başvuru bilgileri mevcuttur silinemez");


                var b = sql.ExecuteNonQuery(CommandType.StoredProcedure, "sp_CvSil",
                    new SqlParameter("@Id", id)
                    ) > 0;

                if (b)
                    return Ok();
            }
            catch (Exception ex)
            {
                FileProccess.WriteLog(ex.Message);

                return BadRequest();
            }

            return BadRequest();
        }

        [HttpPost("CVEgitimOlustur")]
        public ActionResult CVEgitimOlustur(CVEgitim cv)
        {
            try
            {
                var b = sql.ExecuteNonQuery(CommandType.StoredProcedure, "sp_CvEgitimOlustur",
                    new SqlParameter("@Adi", cv.Adi),
                    new SqlParameter("@BaslangicTarihi", cv.BaslangicTarihi),
                    new SqlParameter("@BitisTarihi", cv.BitisTarihi),
                    new SqlParameter("@CvId", cv.CvId),
                    new SqlParameter("@EgitimDurumu", cv.EgitimDurumu),
                    new SqlParameter("@Notu", cv.Notu)
                    ) > 0;

                if (b)
                    return Ok();
            }
            catch (Exception ex)
            {
                FileProccess.WriteLog(ex.Message);

                return BadRequest();
            }

            return BadRequest();
        }

        [HttpPost("CVEgitimSil")]
        public ActionResult CVEgitimSil(int id)
        {
            try
            {
                var b = sql.ExecuteNonQuery(CommandType.StoredProcedure, "sp_CvEgitimSil",
                    new SqlParameter("@Id", id)
                    ) > 0;

                if (b)
                    return Ok();
            }
            catch (Exception ex)
            {
                FileProccess.WriteLog(ex.Message);

                return BadRequest();
            }

            return BadRequest();
        }

        [HttpPost("CVCalismaOlustur")]
        public ActionResult CVCalismaOlustur(CVCalisma cv)
        {
            try
            {
                var b = sql.ExecuteNonQuery(CommandType.StoredProcedure, "sp_CvCalismaOlustur",
                    new SqlParameter("@Aciklama", cv.Aciklama),
                    new SqlParameter("@BaslangicTarihi", cv.BaslangicTarihi),
                    new SqlParameter("@BitisTarihi", cv.BitisTarihi),
                    new SqlParameter("@CvId", cv.CvId),
                    new SqlParameter("@FirmaAdi", cv.FirmaAdi),
                    new SqlParameter("@Pozisyon", cv.Pozisyon)
                    ) > 0;

                if (b)
                    return Ok();
            }
            catch (Exception ex)
            {
                FileProccess.WriteLog(ex.Message);

                return BadRequest();
            }

            return BadRequest();
        }

        [HttpPost("CVCalismaSil")]
        public ActionResult CVCalismaSil(int id)
        {
            try
            {
                var b = sql.ExecuteNonQuery(CommandType.StoredProcedure, "sp_CvCalismaSil",
                    new SqlParameter("@Id", id)
                    ) > 0;

                if (b)
                    return Ok();
            }
            catch (Exception ex)
            {
                FileProccess.WriteLog(ex.Message);

                return BadRequest();
            }

            return BadRequest();
        }
    }
}