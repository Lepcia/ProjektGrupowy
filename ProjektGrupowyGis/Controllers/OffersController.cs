using PagedList;
using ProjektGrupowyGis.DAL;
using ProjektGrupowyGis.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProjektGrupowyGis.Controllers
{
    public class OffersController : Controller
    {
        private OffersSqlExecutor _offersSqlExecutor;
        private AccountSqlExecutor _accountSqlExecutor;
        private HotelsSqlExecutor _hotelsSqlExecutor;

        public OffersController()
        {
            _offersSqlExecutor = new OffersSqlExecutor();
            _accountSqlExecutor = new AccountSqlExecutor();
            _hotelsSqlExecutor = new HotelsSqlExecutor();
        }

        public ActionResult Index(OffersSearch search, int? page, string name, string hotelName, string description, string dateFrom,
            string dateTo, string peopleFrom, string peopleTo, string priceFrom, string priceTo)
        {
            ViewBag.CurrentSearch = new OffersSearch();
            ViewBag.empty = null;
            if (search != null)
            {
                page = 1;
                ViewBag.CurrentSearch = search;
            }
            else {
                ViewBag.CurrentSearch.nameSearch = name;
                ViewBag.CurrentSearch.hotelName = hotelName;
                ViewBag.CurrentSearch.descriptionSearch = description;
                ViewBag.CurrentSearch.dateFrom = dateFrom;
                ViewBag.CurrentSearch.dateTo = dateTo;
                ViewBag.CurrentSearch.peopleFrom = peopleFrom;
                ViewBag.CurrentSearch.peopleTo = peopleTo;
                ViewBag.CurrentSearch.priceFrom = priceFrom;
                ViewBag.CurrentSearch.priceTo = priceTo;
                search = ViewBag.CurrentSearch;
            }

            int pageSize = 10;
            int pageNumber = (page ?? 1);
            var user = HttpContext.User.Identity;
            bool canEdit = user.Name == "Admin" ? true : false;

            var offers = _offersSqlExecutor.FilterOffers(search);
            OffersModel model = new OffersModel { Offers = offers.ToPagedList<Offer>(pageNumber, pageSize), CanEdit = canEdit, Search = search };
            return View(model);            
        }

        public ActionResult AddOffer(Offer offer)
        {
            _offersSqlExecutor.AddOffer(offer);
            var response = new { response = "ok" };
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Edit(int idOffer)
        {
            if (User.Identity.Name == "Admin")
            {
                Offer offer = _offersSqlExecutor.FindOfferById(idOffer);
                IEnumerable<SelectListItem> hotels = _hotelsSqlExecutor.GetHotelsForSelect();
                OfferEditModel model = new OfferEditModel { hotels = hotels, offer = offer, SelectedHotelId = offer.ID_HOTEL };
                return View(model);
            }
            else return RedirectToAction("Index");
        }

        public ActionResult Create()
        {
            if (User.Identity.Name == "Admin")
            {
                Offer offer = new Offer();
                IEnumerable<SelectListItem> hotels = _hotelsSqlExecutor.GetHotelsForSelect();
                OfferEditModel model = new OfferEditModel { hotels = hotels, offer = offer, SelectedHotelId = offer.ID_HOTEL };
                return View(model);
            }
            else return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Create(Offer offer)
        {
            if (User.Identity.Name == "Admin")
            {
                _offersSqlExecutor.AddOffer(offer);
                return RedirectToAction("Index");
            }
            else return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Edit(Offer offer)
        {
            if (User.Identity.Name == "Admin")
            {
                _offersSqlExecutor.EditOffer(offer);
                return RedirectToAction("Index");
            }
            else return RedirectToAction("Index");
        }
    }
}