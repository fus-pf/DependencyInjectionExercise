using DependencyInjectionExercise.Models;

namespace DependencyInjectionExercise.Services
{
    public class PushNotificationSender : INotificationSender
    {
        private readonly NotificationHub _notificationHub;

        public PushNotificationSender(NotificationHub notificationHub)
        {
            _notificationHub = notificationHub;
        }

        public string Channel => "push";

        public void Send(Order order, string message)
        {
            Console.WriteLine($"[PUSH] To user: {order.CustomerName} | {message}");

            _notificationHub.Add(new NotificationLog
            {
                Timestamp = DateTime.UtcNow,
                Channel = Channel,
                Recipient = order.CustomerName,
                Message = message
            });
        }
    }
}
