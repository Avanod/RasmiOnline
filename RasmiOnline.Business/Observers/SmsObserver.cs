namespace RasmiOnline.Business.Observers
{
    using Protocol;
    using Domain.Dto;
    using Domain.Enum;
    using Domain.Entity;
    using Gnu.Framework.EntityFramework.DataAccess;

    public class SmsObserver : IObserver
    {
        public void Observe(IUnitOfWork uow, IMessageBusiness messageBusiness, ObserverMessage msg, User user)
        {
            if (user.MobileNumber == 0) return;

            messageBusiness.Insert(new Message
            {
                Content = msg.SmsContent.Replace("NAME", user.FullName),
                Receiver = user.MobileNumber.ToString(),
                State = StateType.Begin,
                Type = MessagingType.Sms,
            });
        }
    }
}
