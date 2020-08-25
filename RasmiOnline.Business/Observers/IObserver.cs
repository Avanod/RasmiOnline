namespace RasmiOnline.Business.Observers
{
    using Protocol;
    using Domain.Dto;
    using Domain.Entity;
    using Gnu.Framework.EntityFramework.DataAccess;

    public interface IObserver
    {
        void Observe(IUnitOfWork uow, IMessageBusiness messageBusiness, ObserverMessage msg, User user);
    }
}