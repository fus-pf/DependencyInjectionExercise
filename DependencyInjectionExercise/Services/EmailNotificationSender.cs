using DependencyInjectionExercise.Models;

namespace DependencyInjectionExercise.Services
{
    public class EmailNotificationSender : INotificationSender
    {
        private readonly NotificationHub _notificationHub;

        public EmailNotificationSender(NotificationHub notificationHub)
        {
            _notificationHub = notificationHub;
        }

        public string Channel => "email";

        public void Send(Order order, string message)
        {
            Console.WriteLine($"[EMAIL] To: {order.CustomerEmail} | {message}");

            _notificationHub.Add(new NotificationLog
            {
                Timestamp = DateTime.UtcNow,
                Channel = Channel,
                Recipient = order.CustomerEmail,
                Message = message
            });
        }
    }
}
