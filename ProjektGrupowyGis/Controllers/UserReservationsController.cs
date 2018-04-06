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
    public class UserReservationsController : Controller
    {
        private AccountSqlExecutor _accountSqlExecutor;
        private ReservationsSqlExecutor _reservationsSqlExecutor;

        public UserReservationsController()
        {
            _accountSqlExecutor = new AccountSqlExecutor();
            _reservationsSqlExecutor = new ReservationsSqlExecutor();
        }

        public ActionResult Index(UserReservationSearch search, int? page, string offerName, string hotelName, string dateFrom,
            string dateTo, string peopleFrom, string peopleTo, string priceFrom, string priceTo)
        {
            ViewBag.CurrentSearch = new UserReservationSearch();
            ViewBag.empty = null;
            if (search != null)
            {
                page = 1;
                ViewBag.CurrentSearch = search;
            }
            else
            {
                ViewBag.CurrentSearch.offerName = offerName;
                ViewBag.CurrentSearch.hotelName = hotelName;
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
            var idUser = _accountSqlExecutor.GetUserId(user.Name);

            var reservations = _reservationsSqlExecutor.FilterUserReservations(idUser, search);
            UserReservationModel model = new UserReservationModel { Reservations = reservations.ToPagedList<UserReservationFullData>(pageNumber, pageSize), Search = search };

            return View(model);
        }

        public ActionResult Create(int idOffer)
        {
            UserReservationModel model = new UserReservationModel();
            return View(model);
        }
    }
}