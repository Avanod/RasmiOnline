namespace RasmiOnline.Business.Observers
{
    using Protocol;
    using Domain.Dto;
    using Domain.Enum;
    using Domain.Entity;
    using Gnu.Framework.EntityFramework.DataAccess;

    public class AdminRoboTeleObserver : IObserver
    {
        public void Observe(IUnitOfWork uow, IMessageBusiness messageBusiness, ObserverMessage msg, User user)
        {
            messageBusiness.Insert(new Message
            {
                Content = msg.BotContent.Replace("NAME", user.FullName),
                Receiver = GlobalVariable.AdminInstanceId,
                State = StateType.Begin,
                Type = MessagingType.RoboTele,
                ExtraData = msg.Key,
                ReplyMessageId = msg.MessageId
            });
        }
    }
}
