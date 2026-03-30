using DependencyInjectionExercise.Models;
using Microsoft.AspNetCore.Mvc;

namespace DependencyInjectionExercise.Services.OrderServices
{
    public interface IOrderService
    {
        Task<Order> PlaceOrderAsync(Order order);
        Task<Order> GetOrderAsync(int id);
        Task<List<Order>> GetOrdersAsync();
        Task CancelOrderAsync(int id);
    }
}
