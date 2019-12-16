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
    }
    public class MailService : IMailService
    {
        private MailAddress fromAddress = new MailAddress("roeijerot@gmail.com", "Roeivereniging Roei-je-Rot");

        public void SendConfirmation(string email, string firstName, DateTime datum, TimeSpan tijd)
        {
            try
            {
                MailMessage mailtje = new MailMessage();
                mailtje.From = fromAddress;
                mailtje.To.Add(new MailAddress(email, firstName));
                mailtje.Subject = "Bevestiging boot reservatie";
                mailtje.Body = $"Beste {firstName}" + Environment.NewLine + Environment.NewLine +
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
                str.AppendLine(string.Format("DTSTART:{0:yyyyMMddTHHmmssZ}", datum));
                str.AppendLine(string.Format("DTSTAMP:{0:yyyyMMddTHHmmssZ}", DateTime.UtcNow));
                str.AppendLine(string.Format("DTEND:{0:yyyyMMddTHHmmssZ}", datum + tijd));
                str.AppendLine("LOCATION: Vereneging Roei-je-Rot");
                str.AppendLine(string.Format("UID:{0}", Guid.NewGuid()));
                str.AppendLine(string.Format("DESCRIPTION: Reservering Roei-je-Rot"));
                str.AppendLine(string.Format("X-ALT-DESC;FMTTYPE=text/html:{0}", mailtje.Body));
                str.AppendLine(string.Format("SUMMARY: Reservering Roei-je-Rot"));
                str.AppendLine(string.Format("ORGANIZER:MAILTO:{0}", mailtje.From.Address));

                str.AppendLine(string.Format("ATTENDEE;CN=\"{0}\";RSVP=TRUE:mailto:{1}", mailtje.To[0].DisplayName,
                    mailtje.To[0].Address));

                str.AppendLine("BEGIN:VALARM");
                str.AppendLine("TRIGGER:-PT15M");
                str.AppendLine("ACTION:DISPLAY");
                str.AppendLine("DESCRIPTION:Reminder");
                str.AppendLine("END:VALARM");
                str.AppendLine("END:VEVENT");
                str.AppendLine("END:VCALENDAR");

                var calendarBytes = Encoding.UTF8.GetBytes(str.ToString());
                MemoryStream ms = new MemoryStream(calendarBytes);
                mailtje.Attachments.Add(new Attachment(ms, "Reservering.ics", "text/calendar "));

                try
                {
                    using (var smtpClient = new SmtpClient("smtp.gmail.com", 587))
                    {
                        smtpClient.UseDefaultCredentials = false;
                        smtpClient.Credentials = new NetworkCredential()
                        {
                            UserName = "roeijerot@gmail.com",
                            Password = "Gruppe4KBS",
                        };
                        smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                        smtpClient.EnableSsl = true;


                        //smtpClient.Send("targetemail@targetdomain.xyz", "myemail@gmail.com", "Account verification", body);
                        smtpClient.Send(mailtje);
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
    }
}
