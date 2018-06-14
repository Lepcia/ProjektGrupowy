using PagedList;
using ProjektGrupowyGis.API;
using ProjektGrupowyGis.DAL;
using ProjektGrupowyGis.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ProjektGrupowyGis.Controllers
{
    public class UserReservationsController : Controller
    {
        private AccountSqlExecutor _accountSqlExecutor;
        private ReservationsSqlExecutor _reservationsSqlExecutor;
        private OffersSqlExecutor _offerSqlExecutor;

        public UserReservationsController()
        {
            _accountSqlExecutor = new AccountSqlExecutor();
            _reservationsSqlExecutor = new ReservationsSqlExecutor();
            _offerSqlExecutor = new OffersSqlExecutor();
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
            Offer offer = _offerSqlExecutor.FindOfferById(idOffer);

            List<SelectListItem> guestNumberList = new List<SelectListItem>();
            for (int i = offer.PEOPLE_FROM; i <= offer.PEOPLE_TO; i++)
            {
                guestNumberList.Add(new SelectListItem { Text = i.ToString(), Value = i.ToString() });
            }
            var user = HttpContext.User.Identity;
            var idUser = _accountSqlExecutor.GetUserId(user.Name);

            UserReservationEdit model = new UserReservationEdit { Offer = offer, Reservation = new UserReservationFullData(), GuestsSelect = guestNumberList, IdUser = idUser};
            
            return View(model);
        }

        public ActionResult Delete(int idReservation)
        {
            var reservation = _reservationsSqlExecutor.GetReservationById(idReservation);
            EmailService.SendReservationCancelarEmail(new AccountSqlExecutor().GetUserById(reservation.ID_USER).Email, reservation);
            _reservationsSqlExecutor.DeleteUserReservation(idReservation);
            
            return RedirectToAction("Index", "UserReservations");
        }

        [HttpPost]
        public ActionResult Create(UserReservationEdit model)
        {
            List<Guest> selectedGuests = model.Guests.Where(g => g.FIRST_NAME != null).ToList();
            UserReservation reservation = new UserReservation { ID_OFFER = model.Offer.ID_OFFER, ID_USER = model.IdUser, RESERVATION_DATE = DateTime.Now, GUESTS = selectedGuests.Count};
            int idReservation = _reservationsSqlExecutor.AddReservation(reservation);

            foreach (Guest guest in selectedGuests)
            {
                _reservationsSqlExecutor.AddGuestToReservation(idReservation, guest);
            }
            
            UserReservationFullData reservationFullData = _reservationsSqlExecutor.GetReservationById(idReservation);
            User user = _accountSqlExecutor.GetUserById(model.IdUser);
            EmailService.SendReservationEmail(user.Email, reservationFullData);
            return RedirectToAction("Index");
        }

    }
}