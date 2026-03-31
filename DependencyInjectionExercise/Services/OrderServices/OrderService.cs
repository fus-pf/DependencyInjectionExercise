using DependencyInjectionExercise.Models;
using DependencyInjectionExercise.Respositories.BookRepositories;
using DependencyInjectionExercise.Respositories.OrderRepositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DependencyInjectionExercise.Services.OrderServices
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IBookRepository _bookRepository;
        private readonly DiscountService _discountService;
        private readonly OrderTrackingService _orderTracking;
        private readonly NotificationResolverService _notificationResolverService;
        private readonly IServiceProvider _services;

        public OrderService(IOrderRepository orderRepository, 
            IBookRepository bookRepository,
            DiscountService discountService, 
            OrderTrackingService orderTracking,
            NotificationResolverService notificationResolverService,
            IServiceProvider services)
        {
            _orderRepository = orderRepository;
            _bookRepository = bookRepository;
            _discountService = discountService;
            _orderTracking = orderTracking;
            _notificationResolverService = notificationResolverService;
            _services = services;
        }

        private void SendNotification(Order order, Book? book, string message)
        {
            _notificationResolverService.Send(order, book, message);
        }

        public async Task<Order> PlaceOrderAsync(Order order)
        {
            var book = await _bookRepository.GetOneAsync(b => b.Id == order.BookId);
            if (book == null)
                throw new KeyNotFoundException("Book not found");

            if (order.Quantity <= 0)
                throw new InvalidOperationException("Quantity must be greater than zero");

            if (book.Stock < order.Quantity)
                throw new InvalidOperationException($"Not enough stock. Available: {book.Stock}");

            order.TotalPrice = book.Price * order.Quantity;

            var discountPercent = _discountService.CalculateDiscount(
                book.Category, order.Quantity, order.CustomerName);
            order.DiscountApplied = discountPercent;
            order.TotalPrice *= (1 - discountPercent);

            book.Stock -= order.Quantity;

            order.OrderDate = DateTime.UtcNow;
            order.Status = "confirmed";

            _orderTracking.SetTrackingNote($"Order placed for {order.Quantity}x '{book.Title}'");

            await _orderRepository.AddAsync(order);
            await _orderRepository.SaveChangesAsync();

            var secondTracking = _services.GetRequiredService<OrderTrackingService>();
            var trackingNote = secondTracking.GetTrackingNote();
            order.TrackingNote = trackingNote;

            await _orderRepository.SaveChangesAsync();

            SendNotification(order, book,
                $"Your order for {order.Quantity}x '{book.Title}' has been confirmed. Total: ${order.TotalPrice:F2}");

            return order;
        }

        public async Task<Order> GetOrderAsync(int id)
        {
            var order = await _orderRepository.GetOneAsync(o => o.Id == id);
            if (order == null) throw new KeyNotFoundException();
            return order;
        }

        public async Task<List<Order>> GetOrdersAsync()
        {
            var result = await _orderRepository.GetAsync();
            return result.ToList();
        }

        public async Task CancelOrderAsync(int id)
        {
            var order = await _orderRepository.GetOneAsync(o => o.Id == id);
            if (order == null) throw new KeyNotFoundException();

            if (order.Status == "cancelled")
                throw new InvalidOperationException("Order is already cancelled");

            var book = await _bookRepository.GetOneAsync(b => b.Id == order.BookId);
            if (book != null)
                book.Stock += order.Quantity;

            order.Status = "cancelled";
            await _orderRepository.SaveChangesAsync();

            SendNotification(order, book,
                $"Your order #{order.Id} has been cancelled. Refund: ${order.TotalPrice:F2}");
        }
    }
}
