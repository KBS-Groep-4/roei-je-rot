using RoeiJeRot.Logic.Config;
using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;


namespace RoeiJeRot.Logic.Services
{
    public interface IMailService
    {
        void SendConfirmation(string email, string firstName, DateTime datum, TimeSpan tijd);
        void SendCancelConfirmation(string email, string firstName, DateTime datum);
        void SendCancelMail(string email, string firstName, DateTime datum);

    }
    public class MailService : IMailService
    {
        private MailAddress fromAddress = new MailAddress("roeijerot@gmail.com", "Roeivereniging Roei-je-Rot");
        private string userName;
        private string passWord;
        

        public MailService(IConfig config)
        {
            userName = config.Email;
            passWord = config.Secret;
        }
        public void SendConfirmation(string email, string firstName, DateTime datum, TimeSpan tijd)
        {
            try
            {
                MailMessage mail = new MailMessage();
                mail.From = fromAddress;
                mail.To.Add(new MailAddress(email, firstName));
                mail.Subject = "Bevestiging boot reservatie";
                mail.Body = $"Beste {firstName}" + Environment.NewLine + Environment.NewLine +
                               $"Je ontvangt deze mail omdat je een reservering hebt geplaatst voor een boot op {datum.ToString("d")} voor {tijd.TotalMinutes} minuten. In de bijlage vind u de afspraak die u kunt toevoegen aan uw eigen agenda." +
                               Environment.NewLine + Environment.NewLine +
                               "We wens u veel roei plezier!" + Environment.NewLine + Environment.NewLine +
                               "Met vriendelijke groeten," + Environment.NewLine +
                               Environment.NewLine +
                               "Roeivereniging Roei-je-Rot";

                //ics generator
                StringBuilder str = new StringBuilder();
                str.AppendLine("BEGIN:VCALENDAR");
                str.AppendLine("PRODID:-//Reservering boot");
                str.AppendLine("VERSION:2.0");
                str.AppendLine("METHOD:REQUEST");
                str.AppendLine("BEGIN:VEVENT");
                str.AppendLine(string.Format("DTSTART:{0:yyyyMMddTHHmmss}", datum));
                str.AppendLine(string.Format("DTSTAMP:{0:yyyyMMddTHHmmss}", DateTime.UtcNow));
                str.AppendLine(string.Format("DTEND:{0:yyyyMMddTHHmmss}", datum + tijd));
                str.AppendLine("LOCATION: Vereneging Roei-je-Rot");
                str.AppendLine(string.Format("UID:{0}", Guid.NewGuid()));
                str.AppendLine(string.Format("DESCRIPTION: Reservering Roei-je-Rot"));
                str.AppendLine(string.Format("X-ALT-DESC;FMTTYPE=text/html:{0}", mail.Body));
                str.AppendLine(string.Format("SUMMARY: Reservering Roei-je-Rot"));
                str.AppendLine(string.Format("ORGANIZER:MAILTO:{0}", mail.From.Address));

                str.AppendLine(string.Format("ATTENDEE;CN=\"{0}\";RSVP=TRUE:mailto:{1}", mail.To[0].DisplayName,
                    mail.To[0].Address));

                str.AppendLine("BEGIN:VALARM");
                str.AppendLine("TRIGGER:-PT15M");
                str.AppendLine("ACTION:DISPLAY");
                str.AppendLine("DESCRIPTION:Reminder");
                str.AppendLine("END:VALARM");
                str.AppendLine("END:VEVENT");
                str.AppendLine("END:VCALENDAR");

                var calendarBytes = Encoding.UTF8.GetBytes(str.ToString());
                MemoryStream ms = new MemoryStream(calendarBytes);
                mail.Attachments.Add(new Attachment(ms, "Reservering.ics", "text/calendar "));

                try
                {
                    using (var smtpClient = new SmtpClient("smtp.gmail.com", 587))
                    {
                        smtpClient.UseDefaultCredentials = false;
                        smtpClient.Credentials = new NetworkCredential()
                        {
                            UserName = userName,
                            Password = passWord,
                        };
                        smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                        smtpClient.EnableSsl = true;


                        //smtpClient.Send("targetemail@targetdomain.xyz", "myemail@gmail.com", "Account verification", body);
                        smtpClient.Send(mail);
                    }
                }
                catch (Exception e)
                {
                    Console.Error.WriteLine("{0}: {1}", e.ToString(), e.Message);
                }
            }
            catch
            {
                Console.Error.WriteLine("Error while sending mail.");
            }
        }
        public void SendCancelConfirmation(string email, string firstName, DateTime datum)
        {
            MailMessage mail = new MailMessage();
            mail.From = fromAddress;
            mail.To.Add(new MailAddress(email, firstName));
            mail.Subject = "Bevestiging reservering annuleren";
            mail.Body = $"Beste {firstName}" + Environment.NewLine + Environment.NewLine +
                           $"Je ontvangt deze mail omdat je een reservering hebt geanuleerd voor een boot op {datum.ToString("d")}." +
                           Environment.NewLine + Environment.NewLine +
                           "We hopen u nog een keer weer te zien bij onze vereniging!" + Environment.NewLine + Environment.NewLine +
                           "Met vriendelijke groeten," + Environment.NewLine +
                           Environment.NewLine +
                           "Roeivereniging Roei-je-Rot";
            try
            {
                using (var smtpClient = new SmtpClient("smtp.gmail.com", 587))
                {
                    smtpClient.UseDefaultCredentials = false;
                    smtpClient.Credentials = new NetworkCredential()
                    {
                        UserName = userName,
                        Password = passWord,
                    };
                    smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtpClient.EnableSsl = true;

                    //smtpClient.Send("targetemail@targetdomain.xyz", "myemail@gmail.com", "Account verification", body);
                    smtpClient.Send(mail);
                }
            }
            catch (Exception e)
            {
                Console.Error.WriteLine("{0}: {1}", e.ToString(), e.Message);
            }       
        }
        public void SendCancelMail(string email, string firstName, DateTime datum)
        {
            MailMessage mail = new MailMessage();
            mail.From = fromAddress;
            mail.To.Add(new MailAddress(email, firstName));
            mail.Subject = "Uw reservering is geannuleerd";
            mail.Body = $"Beste {firstName}" + Environment.NewLine + Environment.NewLine +
                           $"Je ontvangt deze mail, omdat je reservering op {datum.ToString("d")} helaas niet door kan gaan in verband met een beschadigde boot. Alle boten van hetzelfde type zijn voor die dag al gereserveerd" +
                           Environment.NewLine + Environment.NewLine +
                           "We verzoeken u om een nieuwe reservering te maken voor een andere datum. Excuus voor het ongemak" + Environment.NewLine + Environment.NewLine +
                           "Met vriendelijke groeten," + Environment.NewLine +
                           Environment.NewLine +
                           "Roeivereniging Roei-je-Rot";
            try
            {
                using (var smtpClient = new SmtpClient("smtp.gmail.com", 587))
                {
                    smtpClient.UseDefaultCredentials = false;
                    smtpClient.Credentials = new NetworkCredential()
                    {
                        UserName = userName,
                        Password = passWord,
                    };
                    smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtpClient.EnableSsl = true;

                    //smtpClient.Send("targetemail@targetdomain.xyz", "myemail@gmail.com", "Account verification", body);
                    smtpClient.Send(mail);
                }
            }
            catch (Exception e)
            {
                Console.Error.WriteLine("{0}: {1}", e.ToString(), e.Message);
            }
        }
    }
}
