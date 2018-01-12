using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using PagedList;

namespace ProjektGrupowyGis.Models
{
    public class Hotel
    {
        public string Id_Hotel { get; set; }
        public string Place_Id { get; set; }
        public string Name { get; set; }
        public string FullAddress { get; set; }
        public string Webpage { get; set; }
        public float Rate { get; set; }
        public float Google_Rate { get; set; }
        [Display(Name="Latitude")]
        public string Lat { get; set; }
        [Display(Name="Longtitude")]
        public string Lng { get; set; }
        public int? Street_Num { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string PostCode { get; set; }
        public string Phone { get; set; }
        [DisplayFormat(DataFormatString ="{0:0.#}")]
        public float Avg_Rate { get; set; } = 0;
        public int User_Rate { get; set; } = 0;
        public int RatesCount { get; set; } = 0;
    }

    public class SearchByRadius
    {
        public int Radius { get; set; }
        public string Lat { get; set; }
        public string Lng { get; set; }
    }

    public class HotelsModel
    {
        public IPagedList<Hotel> Hotels { get; set; }
        public HotelsSearch Search { get; set; }
        public bool CanRate { get; set; } = false;
        public bool CanEdit { get; set; } = false;
        public int? page { get; set; }

    }

    public class HotelsSearch
    {
        public string nameSearch { get; set; }
        public string addressSearch { get; set; }
        public string googleRateFrom { get; set; }
        public string googleRateTo { get; set; }
        public string usersRateFrom { get; set; }
        public string usersRateTo { get; set; }
        public string yourRateFrom { get; set; }
        public string yourRateTo { get; set; }
    }

}