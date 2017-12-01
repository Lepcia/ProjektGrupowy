using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Data;
using Data.Helper.External.Models;

namespace Data.Helper
{
    public class XMLReader
    {
        public List<Users> GetListOfUsers()
        {
            string xmlData = HttpContext.Current.Server.MapPath("~/App_Data/Users.xml");
            DataSet ds = new DataSet();
            ds.ReadXml(xmlData);

            List<Users> users = new List<Users>();
            users = (from rows in ds.Tables[0].AsEnumerable()
                     select new Users
                     {
                         UserId = Convert.ToInt32(rows[0].ToString()),
                         Login = rows[1].ToString(),
                         Name = rows[2].ToString(),
                         Email = rows[3].ToString(),
                         Password = rows[4].ToString()
                     }).ToList();

            return users;
        }

        public Users GetUserById(int id)
        {
            string xmlData = HttpContext.Current.Server.MapPath("~/App_Data/Users.xml");
            DataSet ds = new DataSet();
            ds.ReadXml(xmlData);

            Users user = new Users();
            user = (from rows in ds.Tables[0].AsEnumerable()
                    select new Users
                    {
                        UserId = Convert.ToInt32(rows[0].ToString()),
                        Login = rows[1].ToString(),
                        Name = rows[2].ToString(),
                        Email = rows[3].ToString(),
                        Password = rows[4].ToString()
                    }).Where(x => x.UserId == id).FirstOrDefault();

            return user;
        }

        public Users GetUserByLoginAndPassword(int id, string password)
        {
            string xmlData = HttpContext.Current.Server.MapPath("~/App_Data/Users.xml");
            DataSet ds = new DataSet();
            ds.ReadXml(xmlData);

            Users user = (from rows in ds.Tables[0].AsEnumerable()
                    select new Users
                    {
                        UserId = Convert.ToInt32(rows[0].ToString()),
                        Login = rows[1].ToString(),
                        Name = rows[2].ToString(),
                        Email = rows[3].ToString(),
                        Password = rows[4].ToString()
                    }).Where(x => x.UserId == id && x.Password == password).FirstOrDefault();

            return user;
        }

        public bool CheckIfLoginTaken(string login)
        {
            string xmlData = HttpContext.Current.Server.MapPath("~/App_Data/Users.xml");
            DataSet ds = new DataSet();
            ds.ReadXml(xmlData);

            Users user = (from rows in ds.Tables[0].AsEnumerable()
                          select new Users
                          {
                              UserId = Convert.ToInt32(rows[0].ToString()),
                              Login = rows[1].ToString(),
                              Name = rows[2].ToString(),
                              Email = rows[3].ToString(),
                              Password = rows[4].ToString()
                          }).Where(x => x.Login == login).FirstOrDefault();
            bool isTaken = user == null ? false : true;
            return isTaken;
        }
    }
}
