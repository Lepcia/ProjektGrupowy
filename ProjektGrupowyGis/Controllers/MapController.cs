using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProjektGrupowyGis.DAL;
using ProjektGrupowyGis.Models;

namespace ProjektGrupowyGis.Controllers
{
    public class MapController : Controller
    {
        private HotelsSqlExecutor _hotelsSqlExecutor;

        public MapController() {
            _hotelsSqlExecutor = new HotelsSqlExecutor();
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetHotelsByRadius(SearchByRadius search)
        {
            List<Hotel> hotels = _hotelsSqlExecutor.GetHotelsByRadius(search.Radius, search.Lat, search.Lng);
            return Json(hotels, JsonRequestBehavior.AllowGet);
        }
    }
}