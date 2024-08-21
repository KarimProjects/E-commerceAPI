using Store.Service.Services.BasketService.Dtos;
using Store.Service.Services.OrderService.Dtos;

namespace Store.Service.Services.PaymentService
{
    public interface IPaymentService
    {
        Task<CustomerBasketDto> CreateOrUpdatePaymentIntentForExistingOrder(CustomerBasketDto basket);
        Task<CustomerBasketDto> CreateOrUpdatePaymentIntentForNewOrder(string basketId);
        Task<OrderResultDto> UpdateOrderPaymentSucceed(string paymentIntentId);
        Task<OrderResultDto> UpdateOrderPaymentFailed(string paymentIntentId);
    }
}
