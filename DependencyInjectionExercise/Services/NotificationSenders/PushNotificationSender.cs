using DependencyInjectionExercise.Models;

namespace DependencyInjectionExercise.Services.NotificationSenders
{
    public class PushNotificationSender : INotificationSender
    {
        private readonly NotificationHub _notificationHub;

        public PushNotificationSender(NotificationHub notificationHub)
        {
            _notificationHub = notificationHub;
        }

        public void Send(Order order, Book? book, string message)
        {
            Console.WriteLine($"[PUSH] To user: {order.CustomerName} | {message}");

            _notificationHub.Add(new NotificationLog
            {
                Timestamp = DateTime.UtcNow,
                Channel = "push",
                Recipient = order.CustomerName,
                Message = message
            });
        }
    }
}
