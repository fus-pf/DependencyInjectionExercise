using DependencyInjectionExercise.Models;
using DependencyInjectionExercise.Services.NotificationSenders;

namespace DependencyInjectionExercise.Services
{
    public class NotificationResolverService
    {
        private readonly IServiceProvider _serviceProvider;

        public NotificationResolverService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public void Send(Order order, Book? book, string message)
        {
            var sender = _serviceProvider.GetRequiredKeyedService<INotificationSender>(order.NotificationMethod);
            if (sender != null)
            {
                sender.Send(order, book, message);
            }
            else
            {
                Console.WriteLine($"[UNKNOWN CHANNEL: {order.NotificationMethod}] {message}");
            }
        }
    }
}
