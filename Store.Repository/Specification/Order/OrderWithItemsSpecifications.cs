using System.Linq.Expressions;

namespace Store.Repository.Specification.Order
{
    public class OrderWithItemsSpecifications : BaseSpecification<Data.Entities.OrderEntities.Order>
    {
        public OrderWithItemsSpecifications(string buyerEmail) 
            : base(order => order.BuyerEmail == buyerEmail)
        {
            AddInclude(order => order.OrderItems);
            AddInclude(order => order.DeliveryMethod);
            AddOrderByDescending(order => order.OrderDate);
        }

        public OrderWithItemsSpecifications(Guid id, string buyerEmail)
             : base(order => order.BuyerEmail == buyerEmail && order.Id == id)
        {
            AddInclude(order => order.OrderItems);
            AddInclude(order => order.DeliveryMethod);
        }
    }
}
