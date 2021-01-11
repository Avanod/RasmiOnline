using Gnu.Framework.Core;
using Gnu.Framework.Core.Email;
using Gnu.Framework.Core.Log;
using Gnu.Framework.EntityFramework.DataAccess;
using RasmiOnline.Business.Properties;
using RasmiOnline.Domain.Entity;
using System;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace RasmiOnline.Business.Implement
{
    public class EmailStrategy : IMessagingStrategy
    {
        #region Constructor
        readonly IUnitOfWork _uow;

        public EmailStrategy(IUnitOfWork uow)
        {

            _uow = uow;
        }
        #endregion

        public IActionResponse<bool> Send(Message message)
        {
            var result = new ActionResponse<bool>();

            try
            {
                MailMessage messageM = new MailMessage();
                messageM.To.Add(message.Receiver);
                messageM.From = new MailAddress("portal.amirshahi@gmail.com");
                messageM.Subject = message.ExtraData;
                messageM.Body = message.Content.Replace(Environment.NewLine,"<br>");
                messageM.IsBodyHtml = true;
                messageM.BodyEncoding = Encoding.UTF8;
                SmtpClient smtp = new SmtpClient();
                smtp.EnableSsl = true;
                smtp.Port = 587;
                smtp.UseDefaultCredentials = false;
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Host = "smtp.gmail.com";

                smtp.Credentials = new NetworkCredential("portal.amirshahi@gmail.com", "pOrtal@2020");

                smtp.Send(messageM);
                return result;
            }
            catch (Exception e)
            {
                FileLoger.Error(e, GlobalVariable.LogPath);
                result.Message = BusinessMessage.Error;
                return result;
            }

        }
    }
}
