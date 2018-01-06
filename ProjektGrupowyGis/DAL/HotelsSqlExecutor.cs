using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using ProjektGrupowyGis.Models;

namespace ProjektGrupowyGis.DAL
{
    public class HotelsSqlExecutor
    {
        private string _connectionString = ConfigurationManager.ConnectionStrings["Context"].ConnectionString;
        private IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["Context"].ConnectionString);

        public int CountHotels()
        {
            var sqlQuery = $"SELECT COUNT(*) FROM HOTELS";
            var count = db.Query<int>(sqlQuery).SingleOrDefault();
            return count;
        }

        public void AddHotel(Hotel hotel)
        {
            var sqlQuery = $"INSERT INTO HOTELS (PLACE_ID, NAME, FULLADDRESS, WEBPAGE, LATITUDE, LONTITUDE, GOOGLE_RATE, STREET_NUM, STREET, COUNTRY, POSTCODE, PHONE) " +
                           $"VALUES(@IdHotel, @Name, @FullAddress, @Webpage, @Lat, @Lng, @Rating," +
                           $"@StreetNum, @Street, @Country, @PostCode, @Phone); SELECT CAST(SCOPE_IDENTITY() as int) ";
            int id = db.Query<int>(sqlQuery, hotel).SingleOrDefault();
        }

        public List<Hotel> GetHotels()
        {
            var sqlQuery = $"SELECT * FROM HOTELS";
            List<Hotel> hotels = db.Query<Hotel>(sqlQuery).ToList();
            return hotels;
        }
    }
}