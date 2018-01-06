using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProjektGrupowyGis.Models;
using ProjektGrupowyGis.DAL;

namespace ProjektGrupowyGis.Controllers
{
    public class HotelsController : Controller
    {
        private HotelsSqlExecutor _hotelsSqlExecutor;

        public HotelsController()
        {
            _hotelsSqlExecutor = new HotelsSqlExecutor();
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult EmptyHotelsDB()
        {
            int count =_hotelsSqlExecutor.CountHotels();
            var response = new { response = count };
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddHotel(Hotel hotel) {
            _hotelsSqlExecutor.AddHotel(hotel);
            var response = new { response = "ok" };
            return Json(response, JsonRequestBehavior.AllowGet);
        }
    }
}