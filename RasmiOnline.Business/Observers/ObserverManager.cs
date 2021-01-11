namespace RasmiOnline.Business.Observers
{
    using Protocol;
    using System.IO;
    using Domain.Dto;
    using Domain.Enum;
    using Domain.Entity;
    using Newtonsoft.Json;
    using System.Reflection;
    using System.Collections.Generic;
    using Gnu.Framework.EntityFramework.DataAccess;
    using System;
    using Gnu.Framework.Core;

    public class ObserverManager : IObserverManager
    {
        private readonly IUnitOfWork _uow;
        private readonly IMessageBusiness _messageBusiness;

        public ObserverManager(IUnitOfWork uow, IMessageBusiness messageBusiness)
        {
            _uow = uow;
            _messageBusiness = messageBusiness;
        }

        private List<IObserver> _observers = new List<IObserver>();

        public void Attach(IObserver observer) => _observers.Add(observer);

        public void Detach(IObserver observer) => _observers.Remove(observer);

        public void Notify(ConcreteKey concrete, ObserverMessage msg)
        {
            var user = new User();
            var officeUser = new User();
            if (msg.UserId.IsNotNullGuid()) user = _uow.Set<User>().Find(msg.UserId);
            if (msg.OfficeUserId.IsNotNullGuid()) officeUser = _uow.Set<User>().Find(msg.OfficeUserId);

            var observers = JsonConvert.DeserializeObject<IEnumerable<Concrete>>(File.ReadAllText(GlobalVariable.ObserverConfig));
            var officeUsername = false;
            foreach (var item in observers)
            {
                if (item.Key == concrete.ToString())
                {
                    if (item.Key == "Order_Status_Changed" ||
                    item.Key == "Offline_Payment") officeUsername = true;

                    foreach (var obs in item.Observers)
                    {
                        Assembly assembly = Assembly.GetExecutingAssembly();
                        _observers.Add(assembly.CreateInstance(obs) as IObserver);
                    }
                    msg.Subject = item.Key.Replace("_",string.Empty);
                    foreach (IObserver o in _observers)
                    {
                        if (officeUsername)
                            o.Observe(_uow, _messageBusiness, msg, officeUser.UserId.IsNotNullGuid() ? officeUser : user);
                        else
                            o.Observe(_uow, _messageBusiness, msg, user);
                    }
                    break;
                }
            }
            _uow.SaveChanges();
        }
    }
}