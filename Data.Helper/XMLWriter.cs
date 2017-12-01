using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Web;
using Data.Helper.External.Models;
using System.IO;
using System.Xml;
using System.Xml.Linq;

namespace Data.Helper
{
    public class XMLWriter
    {

        public void InsertUser(string login, string name, string email, string password)
        {
            string path = HttpContext.Current.Server.MapPath("~/App_Data/Users.xml");
            if (!File.Exists(path)) {
                XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
                xmlWriterSettings.Indent = true;
                xmlWriterSettings.NewLineOnAttributes = true;
                using (XmlWriter xmlWriter = XmlWriter.Create("")) { }
            }
            else {
                XDocument xDocument = XDocument.Load(path);
                XElement root = xDocument.Element("Users");
                IEnumerable<XElement> users = root.Descendants("User");
                XElement lastUser = users.Last();
                int countUsers = users.Count()+1;
                lastUser.AddAfterSelf(
                    new XElement("User",
                        new XElement("UserId", countUsers),
                        new XElement("Login", login),
                        new XElement("Name", name),
                        new XElement("Email", email),
                        new XElement("Password", password)
                    ));
                xDocument.Save(path);
            }
        }
    }
}
