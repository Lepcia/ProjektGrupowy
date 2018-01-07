using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjektGrupowyGis.Models
{
    public class Hotel
    {
        public string Place_Id { get; set; }
        public string Name { get; set; }
        public string FullAddress { get; set; }
        public string Webpage { get; set; }
        public float Rating { get; set; }
        public string Lat { get; set; }
        public string Lng { get; set; }
        public int? Street_Num { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string PostCode { get; set; }
        public string Phone { get; set; }
    }

    public class SearchByRadius
    {
        public int Radius { get; set; }
        public string Lat { get; set; }
        public string Lng { get; set; }
    }
}