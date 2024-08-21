using AutoMapper;
using Store.Data.Entities;
using Store.Data.Entities.OrderEntities;
using Store.Repository.Interfaces;
using Store.Repository.Specification.Order;
using Store.Service.Services.BasketService;
using Store.Service.Services.OrderService.Dtos;
using Store.Service.Services.PaymentService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Service.Services.OrderService
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBasketService _basketService;
        private readonly IMapper _mapper;
        private readonly IPaymentService _paymentService;

        public OrderService(
            IUnitOfWork unitOfWork,
            IBasketService basketService,
            IMapper mapper,
            IPaymentService paymentService)
        {
            _unitOfWork = unitOfWork;
            _basketService = basketService;
            _mapper = mapper;
            _paymentService = paymentService;
        }

        public async Task<OrderResultDto> CreateOrderAsync(OrderDto input)
        {
            var basket = await _basketService.GetBasketAsync(input.BasketId);
            if (basket is null)
                throw new Exception("Basket Not Exist");

            var orderitems = new List<OrderItemDto>();
            foreach(var basketItem in basket.BasketItems)
            {
                var productItem = await _unitOfWork.Repository<Product, int>().GetByIdAsync(basketItem.ProductId);
                if (productItem is null)
                    throw new Exception($"Product With Id : {basketItem.ProductId} Not Exist");

                var itemOrdered = new ProductItemOrdered
                {
                    ProductItemId = productItem.Id,
                    ProductName = productItem.Name,
                    PictureUrl = productItem.PictureUrl
                };

                var orderItem = new OrderItem
                {
                    Price = productItem.Price,
                    Quantity = basketItem.Quantity,
                    ItemOrdered = itemOrdered
                };

                var mappedOrderItem = _mapper.Map<OrderItemDto>(orderItem);
                orderitems.Add(mappedOrderItem);
            }

            var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod, int>().GetByIdAsync(input.DeliveryMethodId);
            if (deliveryMethod is null)
                throw new Exception("Delivery Method Not Provided");

            var subtotal = orderitems.Sum(item => item.Quantity * item.Price);

            var specs = new OrderWithPaymentIntentSpecification(basket.PaymentIntentId);
            var existingOrder = await _unitOfWork.Repository<Order, Guid>().GetWithSpecificationByIdAsync(specs);
            if (existingOrder != null)
            {
                _unitOfWork.Repository<Order, Guid>().Delete(existingOrder);
                await _paymentService.CreateOrUpdatePaymentIntentForExistingOrder(basket);
            }
            else
            {
                await _paymentService.CreateOrUpdatePaymentIntentForNewOrder(basket.Id);
            }

            var mappedShippingAddress = _mapper.Map<ShippingAddress>(input.ShippingAddress);
            var mappedOrderItems = _mapper.Map<List<OrderItem>>(orderitems);
            var order = new Order
            {
                DeliveryMethodId = deliveryMethod.Id,
                ShippingAddress = mappedShippingAddress,
                BuyerEmail = input.BuyerEmail,
                OrderItems = mappedOrderItems,
                SubTotal = subtotal,
                BasketId = basket.Id,
                PaymentIntentId = basket.PaymentIntentId
            };

            await _unitOfWork.Repository<Order, Guid>().AddAsync(order);
            await _unitOfWork.CompleteAsync();
            var mappedOrder = _mapper.Map<OrderResultDto>(order);
            return mappedOrder;
        }

        public async Task<IReadOnlyList<DeliveryMethod>> GetAllDeliveryMethodsAsync()
            => await _unitOfWork.Repository<DeliveryMethod, int>().GetAllAsync();

        public async Task<IReadOnlyList<OrderResultDto>> GetAllOrdersForUserAsync(string buyerEmail)
        {
            var specs = new OrderWithItemsSpecifications(buyerEmail);
            var orders = await _unitOfWork.Repository<Order, Guid>().GetAllWithSpecificationAsync(specs);
            if (orders is { Count: <= 0 })
                throw new Exception("You Don't Have Any Orders Yet");
            var mappedOrders = _mapper.Map<List<OrderResultDto>>(orders);
            return mappedOrders;
        }

        public async Task<OrderResultDto> GetOrderByIdAsync(Guid id, string buyerEmail)
        {
            var specs = new OrderWithItemsSpecifications(id, buyerEmail);
            var order = await _unitOfWork.Repository<Order, Guid>().GetWithSpecificationByIdAsync(specs);
            if (order is null)
                throw new Exception($"There Is No Order With Id : {id}");
            var mappedOrder = _mapper.Map<OrderResultDto>(order);
            return mappedOrder;
        }
    }
}
