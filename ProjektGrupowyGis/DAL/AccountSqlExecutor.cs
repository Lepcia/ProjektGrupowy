using ProjektGrupowyGis.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Dapper;


namespace ProjektGrupowyGis.DAL
{
    public class AccountSqlExecutor
    {
        private string _connectionString = ConfigurationManager.ConnectionStrings["Context"].ConnectionString;
        private IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["Context"].ConnectionString);

        public User CheckIfLoginTaken(string login)
        {
            var sqlQuery = $"SELECT * FROM USERS WHERE LOGIN = '{login}'";
            var user = db.Query<User>(sqlQuery).SingleOrDefault();
            return user;
        }

        public User CheckPassword(string login, string password)
        {
            var sqlQuery = $"SELECT * FROM USERS WHERE LOGIN = '{login}' AND PASSWORD = '{password}'";
            User user = db.Query<User>(sqlQuery).SingleOrDefault();
            return user;
        }

        public int AddUser(User user)
        {
            var sqlQuery = $"INSERT INTO USERS(LOGIN,NAME,EMAIL,PASSWORD) values(@Login,@Name,@Email,@Password); SELECT CAST(SCOPE_IDENTITY() as int)";
            var id = db.Query<int>(sqlQuery, user).SingleOrDefault();

            return id;
        }

        public int GetUserId(string login) {
            var sqlQuery = $"SELECT ID_USER FROM USERS WHERE LOGIN = '{login}'";
            var id = db.Query<int>(sqlQuery).SingleOrDefault();
            return id;
        }

        public User GetUserById(int idUser)
        {
            var sqlQuery = $"SELECT * FROM USERS WHERE ID_USER = '{idUser}'";
            User user = db.Query<User>(sqlQuery).SingleOrDefault();
            return user;
        }

        public string GetUserEmail(int idUser)
        {
            var sqlQuery = $"SELECT EMAIL FROM USERS WHERE ID_USER = '{idUser}'";
            string email = db.Query<string>(sqlQuery).SingleOrDefault();
            return email;
        }
        
    }
}