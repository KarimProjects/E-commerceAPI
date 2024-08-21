namespace Store.Repository.Specification.Order
{
    public class OrderWithPaymentIntentSpecification : BaseSpecification<Data.Entities.OrderEntities.Order>
    {
        public OrderWithPaymentIntentSpecification(string? paymentIntentId)
            : base(order => order.PaymentIntentId == paymentIntentId)
        {
        }
    }
}
