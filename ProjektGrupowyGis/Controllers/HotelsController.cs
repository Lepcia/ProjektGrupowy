using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProjektGrupowyGis.Models;
using ProjektGrupowyGis.DAL;
using System.Net;

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
            List<Hotel> hotels = _hotelsSqlExecutor.GetHotels();
            return View(hotels);
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

        public ActionResult Edit(int idHotel) {
            Hotel hotel = _hotelsSqlExecutor.FindHotelById(idHotel);
            return View(hotel);
        }

        [HttpPost]
        public ActionResult Edit(Hotel hotel)
        {
            _hotelsSqlExecutor.EditHotel(hotel.Id_Hotel, hotel.Name, hotel.FullAddress, hotel.Webpage, hotel.Phone, hotel.Lat, hotel.Lng);
            return RedirectToAction("Index");
        }
    }
}