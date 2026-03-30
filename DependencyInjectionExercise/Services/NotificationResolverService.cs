using DependencyInjectionExercise.Models;
using DependencyInjectionExercise.Services.NotificationSenders;

namespace DependencyInjectionExercise.Services
{
    public class NotificationResolverService
    {
        private readonly Dictionary<string, INotificationSender> _senders;

        public NotificationResolverService(IEnumerable<INotificationSender> senders)
        {
            _senders = senders.ToDictionary(s => s.Channel, StringComparer.OrdinalIgnoreCase);
        }

        public void Send(Order order, Book? book, string message)
        {
            if (_senders.TryGetValue(order.NotificationMethod, out var sender))
            {
                sender.Send(order, message);
            }
            else
            {
                Console.WriteLine($"[UNKNOWN CHANNEL: {order.NotificationMethod}] {message}");
            }
        }
    }
}
