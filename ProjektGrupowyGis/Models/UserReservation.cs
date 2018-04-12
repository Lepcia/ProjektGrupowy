using PagedList;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProjektGrupowyGis.Models
{
    public class UserReservation
    {
        public int? ID_USER_RESERVATION { get; set; }
        public int? ID_OFFER { get; set; }
        public int? ID_USER { get; set; }
        public DateTime RESERVATION_DATE { get; set; }
        public int? GUESTS { get; set; }
    }

    public class UserReservationFullData
    {
        public int ID_USER_RESERVATION { get; set; }
        public int? ID_OFFER { get; set; }
        public int? ID_USER { get; set; }
        [Display(Name = "Guests")]
        public int GUESTS { get; set; }
        [Display(Name = "Reservation date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime RESERVATION_DATE { get; set; }
        public string ID_HOTEL { get; set; }
        [Display(Name = "Hotel Name")]
        public string HOTEL_NAME { get; set; }
        [Display(Name = "Offer Name")]
        public String NAME { get; set; }
        [Display(Name = "Description")]
        public String DESCRIPTION { get; set; }
        [Display(Name = "Start date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime DATE_START { get; set; }
        [Display(Name = "End date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime DATE_END { get; set; }
        [Display(Name = "Price")]
        public float PRICE { get; set; }
        public List<Guest> GuestsList { get; set; }
    }

    public class UserReservationModel
    {
        public IPagedList<UserReservationFullData> Reservations { get; set; }
        public UserReservationSearch Search { get; set; }
        public int? page { get; set; }
    }

    public class Guest
    {
        public int? ID_GUEST { get; set; }
        public int? ID_USER_RESERVATION { get; set; }
        public string FIRST_NAME { get; set; }
        public string LAST_NAME { get; set; }
        public bool CHILD { get; set; }
    }

    public class UserReservationSearch
    {
        public string hotelName { get; set; }
        public string offerName { get; set; }
        public string dateFrom { get; set; }
        public string dateTo { get; set; }
        public string priceFrom { get; set; }
        public string priceTo { get; set; }
        public string guestsFrom { get; set; }
        public string guestsTo { get; set; }
    }

    public class UserReservationEdit
    { 
        public int IdUser { get; set; }
        public Offer Offer { get; set; }
        public UserReservationFullData Reservation { get; set; }
        public List<Guest> Guests { get; set; }
        public List<SelectListItem> GuestsSelect { get; set; }
    }
}