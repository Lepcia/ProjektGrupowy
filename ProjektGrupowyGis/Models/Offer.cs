using PagedList;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProjektGrupowyGis.Models
{
    public class Offer
    {
        public int? ID_OFFER { get; set; }
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
        [Display(Name = "People from")]
        public int PEOPLE_FROM { get; set; }
        [Display(Name = "People to")]
        public int PEOPLE_TO { get; set; }
        [Display(Name = "Booked")]
        public bool BOOKED { get; set; }
    }

    public class OffersModel
    {
        public IPagedList<Offer> Offers { get; set; }
        public OffersSearch Search { get; set; }
        public bool CanEdit { get; set; }
        public int? page { get; set; }
    }

    public class OfferEditModel
    {
        public Offer offer { get; set; }

        [Display(Name = "Hotel Name")]
        public string SelectedHotelId { get; set; }
        public IEnumerable<SelectListItem> hotels { get; set; }
    }

    public class OffersSearch
    {
        public string nameSearch { get; set; }
        public string hotelName { get; set; }
        public string descriptionSearch { get; set; }
        public string dateFrom { get; set; }
        public string dateTo { get; set; }
        public string priceFrom { get; set; }
        public string priceTo { get; set; }
        public string peopleFrom { get; set; }
        public string peopleTo { get; set; }
    }

}