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
            bool canBook = user.Name != "" ? true : false;

            var offers = _offersSqlExecutor.FilterOffers(search);
            OffersModel model = new OffersModel { Offers = offers.ToPagedList<Offer>(pageNumber, pageSize), CanEdit = canEdit, CanBook = canBook, Search = search };
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

        public ActionResult GenerateOffers()
        {
            if (User.Identity.Name == "Admin")
            {
                List<string> hotelsIds = _hotelsSqlExecutor.GetAllHotelsIds();
                string[] names = getOfferNames();
                string[] descriptions = getOfferDescriptions();

                Random rand = new Random();

                foreach (string hotelId in hotelsIds)
                {
                    int countOfHotelRooms = rand.Next(15, 51);
                    for (int i = 0; i < countOfHotelRooms; i++)
                    {
                        int typeOfRoom = rand.Next(0, 7);
                        // room data
                        string name = names[typeOfRoom];
                        string description = descriptions[typeOfRoom];
                        int price = generatePrice(typeOfRoom);
                        int[,] peopleInRoom = generateHotelRoomCount(typeOfRoom);

                        DateTime thisDay = DateTime.Today;
                        DateTime startDay = thisDay.AddDays(rand.Next(1, 60));
                        DateTime endDay = startDay.AddDays(rand.Next(2, 15));

                        Offer offer = new Offer { ID_HOTEL = hotelId, PRICE = price, DATE_START = startDay, DATE_END = endDay,
                            DESCRIPTION = description, NAME = name, PEOPLE_FROM = peopleInRoom[0, 0], PEOPLE_TO = peopleInRoom[1, 0],BOOKED = false };

                        _offersSqlExecutor.AddOffer(offer);
                    }
                }

            }
            return RedirectToAction("Index");
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

        private int generatePrice(int hotelType)
        {
            Random rand = new Random();
            int cost;

            switch (hotelType)
            {
                case 0:
                    {
                        cost = rand.Next(150, 301);
                        break;
                    }
                case 1:
                    {
                        cost = rand.Next(300, 601);
                        break;
                    }
                case 2:
                    {
                        cost = rand.Next(600, 1001);
                        break;
                    }
                case 3:
                    {
                        cost = rand.Next(400, 601);
                        break;
                    }
                case 4:
                    {
                        cost = rand.Next(300, 501);
                        break;
                    }
                case 5:
                    {
                        cost = rand.Next(100, 201);
                        break;
                    }
                case 6:
                    {
                        cost = rand.Next(800, 1201);
                        break;
                    }
                default:
                    {
                        cost = cost = rand.Next(200, 1201);
                        break;
                    }
            }
            return cost;
        }

        private int[,] generateHotelRoomCount(int hotelType)
        {
            Random rand = new Random();
            int[,] count;

            switch (hotelType)
            {
                case 0:
                    {
                        count = new int[,] { { 1 }, { 3 } };
                        break;
                    }
                case 1:
                    {
                        count = new int[,] { { 1 }, { 3 } };
                        break;
                    }
                case 2:
                    {
                        count = new int[,] { { 2 }, { 2 } };
                        break;
                    }
                case 3:
                    {
                        count = new int[,] { { 1 }, { 5 } };
                        break;
                    }
                case 4:
                    {
                        count = new int[,] { { 1 }, { 1 } };
                        break;
                    }
                case 5:
                    {
                        count = new int[,] { { 1 }, { 3 } };
                        break;
                    }
                case 6:
                    {
                        count = new int[,] { { 2 }, { 2 } };
                        break;
                    }
                default:
                    {
                        count = new int[,] { { 1 }, { 2 } };
                        break;
                    }
            }

            return count;
        }

        private string[] getOfferNames()
        {
            string[] names = {
                    "Basic",
                    "Premium",
                    "Dla dwojga",
                    "Wypoczynek",
                    "Biznes",
                    "Budget",
                    "Romantyczny czas"
                };
            return names;
        }

        private string[] getOfferDescriptions()
        {
            string[] descriptions = {
                    "Podstawowy pokój w przystępnej cenie",
                    "Luksusowy apartament klasy premium, barek w cenie",
                    "Luksusowy apartament dla dwojga z łożem małżeńskim",
                    "Oferta skierowana dla klientów nastawionych na wypoczynek, w pakiecie zestaw spa i open bar",
                    "Pokój klasy biznes",
                    "Przystepna oferta dla każdego klienta",
                    "Apartament dla dwojga z pakietem spa oraz romantyczną kolacją"
                             };
            return descriptions;
        }
    }
}