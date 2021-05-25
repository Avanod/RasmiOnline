namespace RasmiOnline.Business.Observers
{
    using Protocol;
    using Domain.Dto;
    using Domain.Enum;
    using Domain.Entity;
    using Gnu.Framework.EntityFramework.DataAccess;

    public class AdminEmailObserver : IObserver
    {
        public void Observe(IUnitOfWork uow, IMessageBusiness messageBusiness, ObserverMessage msg, User user)
        {
            messageBusiness.Insert(new Message
            {
                Content = msg.BotContent.Replace("NAME", user.FullName),
                State = StateType.Begin,
                Type = MessagingType.Email,
                ExtraData = msg.Subject,
                ReplyMessageId = msg.MessageId,
                Receiver = "translation@amirshahigroup.com",
            });
        }
    }
}