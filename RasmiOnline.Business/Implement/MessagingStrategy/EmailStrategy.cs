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
                const string fromEmail = "portal@amirshahigroup.com";
                var emailMessage = new MailMessage
                {
                    From = new MailAddress(fromEmail),
                    To = { message.Receiver },
                    Subject = message.ExtraData,
                    Body = message.Content,//.Replace(Environment.NewLine, "<br>"),
                    DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure
                };
                using (SmtpClient smtpClient = new SmtpClient("207.176.218.157"))
                {
                    smtpClient.Credentials = new NetworkCredential("portal@amirshahigroup.com", "969k?kTz");

                    smtpClient.Send(emailMessage);
                }
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
