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
            var sqlQuery = $"INSERT INTO HOTELS (PLACE_ID, NAME, FULLADDRESS, WEBPAGE, LAT, LNG, GOOGLE_RATE, STREET_NUM, STREET, COUNTRY, CITY, POSTCODE, PHONE) " +
                           $"VALUES(@Place_Id, @Name, @FullAddress, @Webpage, REPLACE(@Lat, ',', '.'), REPLACE(@Lng, ',', '.'), @Rating," +
                           $"@Street_Num, @Street, @Country, @City, @PostCode, @Phone); SELECT CAST(SCOPE_IDENTITY() as int) ";
            int id = db.Query<int>(sqlQuery, hotel).SingleOrDefault();
        }

        public List<Hotel> GetHotels()
        {
            var sqlQuery = $"SELECT HOTELS.*, (SELECT coalesce(AVG(HOTEL_RATES.RATE), 0) FROM HOTEL_RATES WHERE HOTEL_RATES.ID_HOTEL = HOTELS.ID_HOTEL) AS AVG_RATE FROM HOTELS";
            List<Hotel> hotels = db.Query<Hotel>(sqlQuery).ToList();
            return hotels;
        }

        public List<Hotel> GetHotelsWithUserRate(int idUser) {
            var sqlQuery = $"SELECT HOTELS.*, (SELECT coalesce(AVG(HOTEL_RATES.RATE), 0) FROM HOTEL_RATES WHERE HOTEL_RATES.ID_HOTEL = HOTELS.ID_HOTEL) AS AVG_RATE," +
                "(select coalesce(rate, 0) from HOTEL_RATES where ID_HOTEL = HOTELS.ID_HOTEL and ID_USER = " + idUser + " ) as USER_RATE FROM HOTELS";
            List<Hotel> hotels = db.Query<Hotel>(sqlQuery).ToList();
            return hotels;
        }

        public List<Hotel> GetHotelsByRadius(int radius, string lat, string lng)
        {
            var sqlQuery = $"DECLARE @SOURCE GEOGRAPHY = 'POINT({lat} {lng})'; DECLARE @radius int = '{radius}'" +
                           "SELECT * FROM HOTELS WHERE @SOURCE.STDistance('POINT(' + CONVERT(NVARCHAR(50), LAT) + ' ' + CONVERT(NVARCHAR(50),LNG) + ')') <= @radius";
            List<Hotel> hotels = db.Query<Hotel>(sqlQuery).ToList();
            return hotels;
        }

        public Hotel FindHotelById(int id) {
            var sqlQuery = $"SELECT *,  (SELECT AVG(HOTEL_RATES.RATE) FROM HOTEL_RATES WHERE HOTEL_RATES.ID_HOTEL = HOTELS.ID_HOTEL) AS AVG_RATE FROM HOTELS WHERE ID_HOTEL = '{id}'";
            Hotel hotel = db.Query<Hotel>(sqlQuery).SingleOrDefault();
            return hotel;
        }

        public void EditHotel(string idHotel, string name, string fulladdress, string webpage, string phone, string lat, string lng)
        {
            var sqlQuery = $"UPDATE HOTELS SET NAME = '{name}', FULLADDRESS = '{fulladdress}', WEBPAGE = '{webpage}', PHONE = '{phone}', LAT = '{lat}', LNG = '{lng}' WHERE ID_HOTEL = '{idHotel}';"+
                "SELECT * FROM HOTELS WHERE ID_HOTEL = " + idHotel;
            Hotel hotel = db.Query<Hotel>(sqlQuery).SingleOrDefault();
        }

        public void RateHotel(string login, int idHotel, int rate)
        {
            var sqlQuery = $"DECLARE @count int, @id_user int; SELECT @id_user = ID_USER FROM USERS WHERE LOGIN = '{login}'; SELECT @count = COUNT(*) FROM HOTEL_RATES WHERE ID_USER = @id_user AND ID_HOTEL = '{idHotel}';" +
                "IF @COUNT > 0 BEGIN UPDATE HOTEL_RATES SET RATE = " + rate + " WHERE ID_USER = @id_user AND ID_HOTEL = " + idHotel + " END ELSE BEGIN INSERT INTO HOTEL_RATES (ID_USER, ID_HOTEL, RATE) " +
                "VALUES(@id_user, " + idHotel + ", " + rate + "); END";
            db.Query(sqlQuery); 
        }
    }
}