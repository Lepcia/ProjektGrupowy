using ProjektGrupowyGis.DAL;
using ProjektGrupowyGis.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace ProjektGrupowyGis.API
{
    public class EmailService
    {
        private static MailAddress _ourMail = new MailAddress("jojojoooo@wp.pl", "Booking GIS Hotele");
        private static SmtpClient _smptClient = new SmtpClient("smtp.wp.pl", 587)
        {
            DeliveryMethod = SmtpDeliveryMethod.Network,
            UseDefaultCredentials = false,
            EnableSsl = false,
            Credentials = new System.Net.NetworkCredential("jojojoooo", "12345678!")
        };

        public static void SendEmail(string clientAddress, string messageSubject, string messageBody)
        {
            MailMessage mail = new MailMessage();
            mail.To.Add(new MailAddress(clientAddress));
            mail.From = _ourMail;
            mail.Subject = messageSubject;
            mail.Body = messageBody;
            _smptClient.Send(mail);
        }

        public static void SendWelcomeEmail(string clientAddress)
        {
            string welcomeSubject = "Witamy w naszej wyszukiwarce hoteli!";
            string messageBody = "Dziekujemy za rejestracje w naszym portalu. Liczymy, ze znajdziesz to czego szukasz. Pozdrawiamy, zespol GIS";
            SendEmail(clientAddress, welcomeSubject, messageBody);
        }

        public static void SendReservationEmail(string clientAddress, UserReservationFullData userReservation)
        {
            string messageSubject = $"Potwierdzenie rezerwacji nr {userReservation.ID_USER_RESERVATION} w hotelu {userReservation.HOTEL_NAME}";
            string messageBody = $"Witaj {new AccountSqlExecutor().GetUserById(userReservation.ID_USER).Name}! \n" +
                                 $"Potwierdzamy złożoną rezerwację oraz przedstawiamy jej szczegóły:\n" +
                                 $"Nazwa hotelu: {userReservation.HOTEL_NAME}\n" +
                                 $"Wybrana oferta: {userReservation.NAME}, {userReservation.DESCRIPTION}\n" +
                                 $"Szczegóły rezerwacji:\nZameldowanie: {userReservation.DATE_START}, Wymeldowanie: {userReservation.DATE_END}\nCena: {userReservation.PRICE}zł\n" +
                                 $"Liczba gości: {userReservation.GUESTS}\n" +
                                 $"Pozdrawiamy, zespół GIS";

            SendEmail(clientAddress, messageSubject, messageBody);
        }

        public static void SendReservationCancelarEmail(string clientAddress, UserReservationFullData userReservation)
        {
            string messageSubject = $"Potwierdzenie anulowania rezerwacji nr {userReservation.ID_USER_RESERVATION} w hotelu {userReservation.HOTEL_NAME}";
            string messageBody = $"Witaj {new AccountSqlExecutor().GetUserById(userReservation.ID_USER).Name}! \n" +
                                 $"Potwierdzamy anulowanie rezerwacji.\nJest nam przykro, lecz mamy nadzieję, że jesteś zadowolony z naszych usług!\n" +
                                 $"Pozdrawiamy, zespół GIS";

            SendEmail(clientAddress, messageSubject, messageBody);
        }
    }
}