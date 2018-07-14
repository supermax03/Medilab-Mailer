using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Net.Mail;

namespace MedilabMailer.Mailer
{
    class Mailer
    {
        private const int SUCCESS = 0;
        private const int ERROR = -1;
        public static int sendEmail(String receiver,String subject, String msg)
        {
            try
            {
                
                SmtpClient client = new SmtpClient();
                client.Port = 587;
                client.Host = ConfigurationManager.AppSettings["clientHost"];
                client.EnableSsl = true;
                client.Timeout = 10000;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["from"], ConfigurationManager.AppSettings["password"]);
                MailMessage mm = new MailMessage(ConfigurationManager.AppSettings["from"], receiver,subject, msg);
                mm.IsBodyHtml = true;               
                mm.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
                client.Send(mm);
                return SUCCESS;

            }
            catch (Exception exc)
            {
                Library.WriteErrorLog("Error" + exc.InnerException.Message);
                Library.WriteErrorLog("Error" + exc.Message);
                return ERROR;

            }

        }


    }
}
