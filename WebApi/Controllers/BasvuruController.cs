using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using WebApi.Models;
using WebApi.Utility;
using WepApi.DataAccessLayer;

namespace WebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BasvuruController : Controller
    {
        SqlDataBlock sql = new SqlDataBlock();

        [HttpGet("FirmaIlanGetir")]
        public IEnumerable<FirmaIlan> FirmaIlanGetir([FromBody]  SearchFilter SearchFilter)
        {
            try
            {
                DataTable dt = sql.ExecuteDataTable(CommandType.StoredProcedure, "sp_FirmaIlanGetir");

                var result = (from rw in dt.Select()
                              select new FirmaIlan
                              {
                                  FirmaId = Convert.ToInt32(rw["FirmaId"]),
                                  IlanAciklama = Convert.ToString(rw["IlanAciklama"]),
                                  Konum = Convert.ToString(rw["Konum"]),
                                  SonaErmeSuresi = Convert.ToDateTime(rw["SonaErmeSuresi"])
                              }).ToList();

                if (!string.IsNullOrEmpty(SearchFilter.searchString))
                {
                    result = result.Where(s => s.IlanAciklama.Contains(SearchFilter.searchString)
                                           || s.Konum.Contains(SearchFilter.searchString)).ToList();
                }

                switch (SearchFilter.sortOrder)
                {
                    case "name_desc":
                        result = result.OrderByDescending(s => s.IlanAciklama).ToList();
                        break;
                    case "Date":
                        result = result.OrderBy(s => s.SonaErmeSuresi).ToList();
                        break;
                    case "date_desc":
                        result = result.OrderByDescending(s => s.SonaErmeSuresi).ToList();
                        break;
                    default:
                        result = result.OrderBy(s => s.IlanAciklama).ToList();
                        break;
                }

                return result;
            }
            catch (Exception ex)
            {
                FileProccess.WriteLog(ex.Message);

                return null;
            }
        }

        [HttpPost("Basvuru")]
        public ActionResult Basvuru(IlanBasvuru basvuru)
        {
            try
            {
                //başvuru yapıldı mı kontrol?

                var k = sql.ExecuteDataTable(CommandType.Text, "SELECT * FROM IlanBasvuru WHERE IlanId = @IlanId AND CvId = @CvId",
                    new SqlParameter("@CvId", basvuru.CvId),
                    new SqlParameter("@IlanId", basvuru.IlanId)
                    );

                if (k.Rows.Count > 0)
                    return Ok("CV'ye daha önce başvuru yapılmış");

                var b = sql.ExecuteNonQuery(CommandType.StoredProcedure, "sp_Basvuru",
                    new SqlParameter("@Adi", basvuru.CvId),
                    new SqlParameter("@Adres", basvuru.IlanId)
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