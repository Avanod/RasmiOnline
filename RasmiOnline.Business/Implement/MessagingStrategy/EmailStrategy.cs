using Gnu.Framework.Core;
using Gnu.Framework.Core.Email;
using Gnu.Framework.Core.Log;
using Gnu.Framework.EntityFramework.DataAccess;
using RasmiOnline.Business.Properties;
using RasmiOnline.Domain.Entity;
using System;
using System.Net;
using System.Net.Mail;

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
                //var fromAddress = new MailAddress("Portal.amirshahi@gmail.com", "سامانه ثبت سفارشات");
                //var toAddress = new MailAddress(message.Receiver, "دارالترجمه امیرشاهی");
                //const string fromPassword = "pOrtal@2020";
                //string subject = message.ExtraData;
                //string body = message.Content;

                //var smtp = new SmtpClient
                //{
                //    Host = "smtp.gmail.com",
                //    Port = 587,
                //    EnableSsl = true,
                //    DeliveryMethod = SmtpDeliveryMethod.Network,
                //    UseDefaultCredentials = false,
                //    Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
                //};
                //using (var emailMessage = new MailMessage(fromAddress, toAddress)
                //{
                //    Subject = subject,
                //    Body = body
                //})
                //{
                //    smtp.Send(emailMessage);
                //}

                // Configure mail client (may need additional
                // code for authenticated SMTP servers)
                SmtpClient mailClient = new SmtpClient("smtp.gmail.com", 587);

                // set the network credentials
                mailClient.Credentials = new NetworkCredential("Portal.amirshahi@gmail.com", "pOrtal@2020");

                //enable ssl
               // mailClient.EnableSsl = true;

                // Create the mail message (from, to, subject, body)
                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress("Portal.amirshahi@gmail.com");
                mailMessage.To.Add(message.Receiver);
                mailClient.UseDefaultCredentials = true;
                mailMessage.Subject = message.ExtraData;
                mailMessage.Body = message.Content;
                mailMessage.IsBodyHtml = false;
                mailMessage.Priority = MailPriority.High;

                // send the mail
                mailClient.Send(mailMessage);

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
