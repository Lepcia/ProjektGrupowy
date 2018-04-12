using Dapper;
using ProjektGrupowyGis.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ProjektGrupowyGis.DAL
{
    public class ReservationsSqlExecutor
    {
        private string _connectionString = ConfigurationManager.ConnectionStrings["Context"].ConnectionString;
        private IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["Context"].ConnectionString);

        public int AddReservation(UserReservation reservation)
        {
            var sqlQuery = $"INSERT INTO USER_RESERVATIONS (ID_OFFER, ID_USER, RESERVATION_DATE, GUESTS)" +
                           $"VALUES(@ID_OFFER, @ID_USER, @RESERVATION_DATE, @GUESTS); SELECT CAST(SCOPE_IDENTITY() AS INT)";
            int id = db.Query<int>(sqlQuery, reservation).SingleOrDefault();
            return id;
        }

        public List<Guest> GetGuestsToReservation(int idReservation)
        {
            var sqlQuery = $"SELECT * FROM GUESTS WHERE ID_USER_RESERVATION = '{idReservation}'";

            List<Guest> guests = db.Query<Guest>(sqlQuery).ToList();
            return guests;
        }

        public void AddGuestToReservation(int idReservation, Guest guest)
        {
            var sqlQuery = $"INSERT INTO GUESTS (ID_USER_RESERVATION, FIRST_NAME, LAST_NAME) " +
                $"VALUES('{idReservation}', @FIRST_NAME, @LAST_NAME)";

            db.Query<Guest>(sqlQuery, guest);
        }

        public List<UserReservationFullData> GetAllReservations()
        {
            var sqlQuery = $"SELECT USER_RESERVATIONS.*, OFFERS.ID_HOTEL, OFFERS.NAME, OFFERS.DESCRIPTION, OFFERS.DATE_START," +
                $"OFFERS.DATE_END, OFFERS.PRICE, OFFERS.PEOPLE_FROM, OFFERS.PEOPLE_TO, HOTELS.NAME AS HOTEL_NAME " +
                $"FROM USER_RESERVATIONS INNER JOIN OFFERS ON USER_RESERVATIONS.ID_OFFER = OFFERS.ID_OFFER " +
                $"INNER JOIN HOTELS ON OFFERS.ID_HOTEL = HOTELS.ID_HOTEL";
            List<UserReservationFullData> reservations = db.Query<UserReservationFullData>(sqlQuery).ToList();

            foreach (var r in reservations) {
                r.GuestsList = GetGuestsToReservation(r.ID_USER_RESERVATION);
            }

            return reservations; 
        }

        public List<UserReservationFullData> GetUserReservations(int idUser)
        {
            var sqlQuery = $"SELECT USER_RESERVATIONS.*, OFFERS.ID_HOTEL, OFFERS.NAME, OFFERS.DESCRIPTION, OFFERS.DATE_START," +
                $"OFFERS.DATE_END, OFFERS.PRICE, OFFERS.PEOPLE_FROM, OFFERS.PEOPLE_TO, HOTELS.NAME AS HOTEL_NAME " +
                $"FROM USER_RESERVATIONS INNER JOIN OFFERS ON USER_RESERVATIONS.ID_OFFER = OFFERS.ID_OFFER " +
                $"INNER JOIN HOTELS ON OFFERS.ID_HOTEL = HOTELS.ID_HOTEL WHERE ID_USER = '{idUser}'";

            List<UserReservationFullData> reservations = db.Query<UserReservationFullData>(sqlQuery).ToList();

            foreach (var r in reservations)
            {
                r.GuestsList = GetGuestsToReservation(r.ID_USER_RESERVATION);
            }

            return reservations;
        }

        public List<UserReservationFullData> FilterUserReservations(int idUser, UserReservationSearch search)
        {
            search.dateFrom = string.IsNullOrEmpty(search.dateFrom) ? "1999-01-01" : search.dateFrom;
            search.dateTo = string.IsNullOrEmpty(search.dateTo) ? "2100-01-01" : search.dateTo;
            search.guestsFrom = string.IsNullOrEmpty(search.guestsFrom) ? "1" : search.guestsFrom;
            search.guestsTo = string.IsNullOrEmpty(search.guestsTo) ? "20" : search.guestsTo;
            search.priceFrom = string.IsNullOrEmpty(search.priceFrom) ? "0" : search.priceFrom;
            search.priceTo = string.IsNullOrEmpty(search.priceTo) ? "100000" : search.priceTo;

            var sqlQuery = $"SELECT USER_RESERVATIONS.*, OFFERS.*, HOTELS.NAME AS HOTEL_NAME FROM USER_RESERVATIONS INNER JOIN OFFERS ON USER_RESERVATIONS.ID_OFFER = OFFERS.ID_OFFER " +
                $" INNER JOIN HOTELS ON OFFERS.ID_HOTEL = HOTELS.ID_HOTEL" +
                $" WHERE OFFERS.NAME LIKE '%" + search.offerName + "%' AND HOTELS.NAME LIKE '%" + search.hotelName + "%'" +
                $" AND OFFERS.DATE_START BETWEEN @dateFrom AND @dateTo AND OFFERS.DATE_END BETWEEN @dateFrom AND @dateTo AND OFFERS.PRICE BETWEEN @priceFrom AND @priceTo " +
                $" AND USER_RESERVATIONS.GUESTS >= @guestsFrom and USER_RESERVATIONS.GUESTS <= @guestsTo AND USER_RESERVATIONS.ID_USER = '{idUser}'";

            List<UserReservationFullData> reservations = db.Query<UserReservationFullData>(sqlQuery, search).ToList();

            foreach (var r in reservations)
            {
                r.GuestsList = GetGuestsToReservation(r.ID_USER_RESERVATION);
            }

            return reservations;
        }

        public void DeleteUserReservation(int idUserReservation)
        {
            var sqlQuery = $"DELETE FROM USER_RESERVATIONS WHERE ID_USER_RESERVATION = '{idUserReservation}'";
            db.Query<int>(sqlQuery);
        }
    }
}