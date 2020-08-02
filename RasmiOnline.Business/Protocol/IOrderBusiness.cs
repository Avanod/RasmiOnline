namespace RasmiOnline.Business.Protocol
{
    using System;
    using Domain.Entity;
    using Gnu.Framework.Core;
    using RasmiOnline.Domain.Dto;
    using RasmiOnline.Domain.Enum;
    using System.Collections.Generic;

    public interface IOrderBusiness
    {
        IActionResponse<Order> Update(Order model);
        IActionResponse<Order> BriefUpdate(Order model, string baseDomain = "");
        IActionResponse<Order> Update(int orderId, LangType newLangType, IEnumerable<OrderItem> items);
        IActionResponse<Order> UpdateStatus(int orderId);
        IActionResponse<Order> UpdateStatus(int orderId, OrderStatus status);
        IActionResponse<Order> UpdateOrderDeliverFiles(int orderId);
        void StatusNotifier(Order order, string baseDomain = "");
        IActionResponse<IEnumerable<Order>> GetAllOrder(Guid? userId = null, Guid? officeUserId = null, OrderStatus? orderStatus = null, int count = 100);
        IActionResponse<Order> Insert(Order model);
        IActionResponse<int> InsertBehalfOfUser(Order model, int roleId);
        IActionResponse<Order> QuickInsert(AddOrderModel model);
        Order Find(int orderId, string relatedEntities = "none");
        Order Find(int orderId, Guid officeUserId, string relatedEntities = "none");
        IActionResponse<List<OrderDetailsModel>> Search(FilterOrderModel filterModel);
        IActionResponse<List<ItemTextValueModel<OrderStatus, int>>> GetOrderDetails();
        IActionResponse<List<ItemTextValueModel<OrderStatus, int>>> GetOrderDetails(Guid UserId);
        IActionResponse<Order> CompleteOrder(CompleteOrderModel model);
        bool HasOrder(Guid userId);
        int TodayOrderCount(Guid? officeUserId = null);
        IEnumerable<Order> AllTodayOrder(Guid? officeUserId = null);
        Dictionary<string, string> GetOrderPaymentInfo(int orderId);
        IEnumerable<Order> AllOlderOrder(Guid? officeUserId = null);
        int OlderOrderCount(Guid? officeUserId = null);

        IEnumerable<Order> GetReport(Guid? officeUserId = null, string fromDate = null, string toDate = null);

        IActionResponse<Order> Add(AddOrderModel model);
        IActionResponse<Tuple<Order, int>> UpdateBeforePayment(CompleteOrderModel model);
    }
}
