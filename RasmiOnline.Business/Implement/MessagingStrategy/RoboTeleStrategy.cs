namespace RasmiOnline.Business.Implement
{
    using System;
    using Domain.Enum;
    using Telegram.Bot;
    using Domain.Entity;
    using Gnu.Framework.Core;
    using System.Data.Entity;
    using System.Configuration;
    using Gnu.Framework.Core.Log;
    using RasmiOnline.Business.Properties;
    using Gnu.Framework.EntityFramework.DataAccess;
    using System.Net;

    public class RoboTeleStrategy : IMessagingStrategy
    {
        #region Constructor
        readonly IUnitOfWork _uow;
        private readonly TelegramBotClient Bot = new TelegramBotClient(ConfigurationManager.AppSettings["TelegramToken"]);

        public RoboTeleStrategy(IUnitOfWork uow)
        {
            _uow = uow;
        }
        #endregion

        public IActionResponse<bool> Send(Message message)
        {
            var result = new ActionResponse<bool>();
            try
            {

                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
                       | SecurityProtocolType.Tls11
                       | SecurityProtocolType.Tls12
                       | SecurityProtocolType.Ssl3;

                //ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
                //Something went wrong, please try again.
                if (message.Content != "Something went wrong, please try again.")
                {
                    var roboResponse = Bot.SendTextMessageAsync(message.Receiver, message.Content).Result;
                    message.SendStatus = roboResponse.MessageId.ToString();
                    message.State = StateType.Accepted;
                    _uow.Entry(message).State = EntityState.Modified;
                    _uow.SaveChanges();
                }
                else
                {
                    FileLoger.Info(message.SerializeToJson(), GlobalVariable.LogPath);
                }

                result.Result = true;
                result.IsSuccessful = true;
                result.Message = BusinessMessage.Success;
                return result;
                //result.IsSuccessful = true;
                //return result;
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