using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using Dapper;
using System.Data;
using System.Data.SqlClient;
using ProjektGrupowyGis.Models;

namespace ProjektGrupowyGis.DAL
{
    public class SqlExecutor
    {
        private string _connectionString = ConfigurationManager.ConnectionStrings["AccountContext"].ConnectionString;
        private IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["AccountContext"].ConnectionString);

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
    }
}