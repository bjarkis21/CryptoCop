using System.Collections.Generic;
using Cryptocop.Software.API.Models.Dtos;
using Cryptocop.Software.API.Models.InputModels;
using Cryptocop.Software.API.Repositories.Interfaces;
using Cryptocop.Software.API.Services.Interfaces;

namespace Cryptocop.Software.API.Services.Implementations
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IShoppingCartRepository _shoppingCartRepository;

        public OrderService(IOrderRepository orderRepository, IShoppingCartRepository shoppingCartRepository)
        {
            _orderRepository = orderRepository;
            _shoppingCartRepository = shoppingCartRepository;
        }

        public IEnumerable<OrderDto> GetOrders(string email)
        {
            throw new System.NotImplementedException();
        }

        public void CreateNewOrder(string email, OrderInputModel order)
        {
            var newOrder = _orderRepository.CreateNewOrder(email, order);
            _shoppingCartRepository.ClearCart(email);
            // TODO: Send RabbitMQ message
        }
    }
}