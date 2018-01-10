using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjektGrupowyGis.Models
{
    public class Rate
    {
        public int ID_HOTEL { get; set; }
        public int ID_USER { get; set; }
        public int RATE { get; set; }
    }

    public class RatePost {
        public string userLogin { get; set; }
        public int hotelId { get; set; }
        public int rate { get; set; }
    }
}