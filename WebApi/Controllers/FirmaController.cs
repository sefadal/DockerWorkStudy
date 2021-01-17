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
    public class FirmaController : Controller
    {
        SqlDataBlock sql = new SqlDataBlock();

        [HttpPost("FirmaBilgileri")]
        public ActionResult FirmaBilgileri(Firma firma)
        {
            try
            {
                var b = sql.ExecuteNonQuery(CommandType.StoredProcedure, "sp_FirmaBilgileri",
                    new SqlParameter("@Adi", firma.Adi),
                    new SqlParameter("@Adres", firma.Adres)
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

        [HttpPost("FirmaSil")]
        public ActionResult FirmaSil(int id)
        {
            try
            {
                //firma ilan bilgileri var mı kontrol?

                var k = sql.ExecuteDataTable(CommandType.Text, "SELECT * FROM dbo.FirmaIlan INNER JOIN dbo.IlanBasvuru ON IlanBasvuru.IlanId = FirmaIlan.Id WHERE FirmaId = @FirmaId",
                 new SqlParameter("@FirmaId", id)
                 );

                if (k.Rows.Count > 0)
                    return Ok("Firmanın ilanlarına başvuru yapılmış silinemez!");

                var b = sql.ExecuteNonQuery(CommandType.StoredProcedure, "sp_FirmaSil",
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

        [HttpPost("IlanOlustur")]
        public ActionResult IlanOlustur(FirmaIlan ilan)
        {
            try
            {
                var b = sql.ExecuteNonQuery(CommandType.StoredProcedure, "sp_IlanOlustur",
                    new SqlParameter("@FirmaId", ilan.FirmaId),
                    new SqlParameter("@IlanAciklama", ilan.IlanAciklama),
                    new SqlParameter("@Konum", ilan.Konum),
                    new SqlParameter("@SonaErmeSuresi", ilan.SonaErmeSuresi)
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

        [HttpPost("IlanSil")]
        public ActionResult IlanSil(int id)
        {
            try
            {
                //başvuru var mı kontrol?

                var k = sql.ExecuteDataTable(CommandType.Text, "SELECT * FROM IlanBasvuru WHERE IlanId = @IlanId",
                 new SqlParameter("@IlanId", id)
                 );

                if (k.Rows.Count > 0)
                    return Ok("İlana başvuru yapılmış silinemez");

                var b = sql.ExecuteNonQuery(CommandType.StoredProcedure, "sp_IlanSil",
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