using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Dapper;
using ProjektGrupowyGis.Models;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace ProjektGrupowyGis.DAL
{
    public class OffersSqlExecutor
    {
        private string _connectionString = ConfigurationManager.ConnectionStrings["Context"].ConnectionString;
        private IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["Context"].ConnectionString);

        public void AddOffer(Offer offer)
        {
            var sqlQuery = $"INSERT INTO OFFERS (NAME, ID_HOTEL, DESCRIPTION, DATE_START, DATE_END, PRICE, PEOPLE_FROM, PEOPLE_TO) " +
                $"VALUES (@NAME, @ID_HOTEL, @DESCRIPTION, @DATE_START, @DATE_END, @PRICE, @PEOPLE_FROM, @PEOPLE_TO); " +
                $"SELECT CAST(SCOPE_IDENTITY() AS INT) ";
            int id = db.Query<int>(sqlQuery, offer).SingleOrDefault();
        }

        public void EditOffer(Offer offer)
        {
            var sqlQuery = $"UPDATE OFFERS SET NAME = @NAME, ID_HOTEL = @ID_HOTEL, DESCRIPTION = @DESCRIPTION, DATE_START = @DATE_START, " +
                $"DATE_END = @DATE_END, PRICE = @PRICE, PEOPLE_FROM = @PEOPLE_FROM, PEOPLE_TO = @PEOPLE_TO " +
                $"WHERE ID_OFFER = @ID_OFFER; SELECT * FROM OFFERS WHERE ID_OFFER = @ID_OFFER";
            Offer offerE = db.Query<Offer>(sqlQuery, offer).SingleOrDefault();
        }

        public List<Offer> GetOffers()
        {
            var sqlQuery = $"SELECT OFFERS.*, HOTELS.NAME AS HOTEL_NAME FROM OFFERS INNER JOIN HOTELS ON HOTELS.ID_HOTEL = OFFERS.ID_HOTEL";
            List<Offer> offers = db.Query<Offer>(sqlQuery).ToList();
            return offers;
        }

        public Offer FindOfferById(int id)
        {
            var sqlQuery = $"SELECT OFFERS.*, HOTELS.NAME AS HOTEL_NAME FROM OFFERS INNER JOIN HOTELS ON HOTELS.ID_HOTEL = OFFERS.ID_HOTEL" +
                $" WHERE ID_OFFER = '{id}'";
            Offer offer = db.Query<Offer>(sqlQuery).SingleOrDefault();
            return offer;
        }

        public List<Offer> FilterOffers(OffersSearch search)
        {
            search.dateFrom = string.IsNullOrEmpty(search.dateFrom) ? "1999-01-01" : search.dateFrom;
            search.dateTo = string.IsNullOrEmpty(search.dateTo) ? "2100-01-01" : search.dateTo;
            search.peopleFrom = string.IsNullOrEmpty(search.peopleFrom) ? "1" : search.peopleFrom;
            search.peopleTo = string.IsNullOrEmpty(search.peopleTo) ? "20" : search.peopleTo;
            search.priceFrom = string.IsNullOrEmpty(search.priceFrom) ? "0" : search.priceFrom;
            search.priceTo = string.IsNullOrEmpty(search.priceTo) ? "100000" : search.priceTo;

            var sqlQuery = $"SELECT OFFERS.*, HOTELS.NAME AS HOTEL_NAME, (SELECT COUNT(1) FROM USER_RESERVATIONS WHERE USER_RESERVATIONS.ID_OFFER = OFFERS.ID_OFFER) AS BOOKED " + 
                $" FROM OFFERS INNER JOIN HOTELS ON HOTELS.ID_HOTEL = OFFERS.ID_HOTEL" +
                $" WHERE OFFERS.NAME LIKE '%" + search.nameSearch + "%' AND OFFERS.DESCRIPTION LIKE '%" + search.descriptionSearch + "%' AND HOTELS.NAME LIKE '%" + search.hotelName + "%'" +
                $" AND OFFERS.DATE_START BETWEEN @dateFrom AND @dateTo AND OFFERS.DATE_END BETWEEN @dateFrom AND @dateTo AND OFFERS.PRICE BETWEEN @priceFrom AND @priceTo " +
                $" AND OFFERS.PEOPLE_FROM >= @peopleFrom and OFFERS.PEOPLE_TO <= @peopleTo";
            List<Offer> offers = db.Query<Offer>(sqlQuery, search).ToList();
            return offers;
        }
    }
}