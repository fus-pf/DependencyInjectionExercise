using DependencyInjectionExercise.Models;

namespace DependencyInjectionExercise.Services.NotificationSenders
{
    public class EmailNotificationSender : INotificationSender
    {
        private readonly NotificationHub _notificationHub;

        public EmailNotificationSender(NotificationHub notificationHub)
        {
            _notificationHub = notificationHub;
        }

        public void Send(Order order, Book? book, string message)
        {
            Console.WriteLine($"[EMAIL] To: {order.CustomerEmail} | {message}");

            _notificationHub.Add(new NotificationLog
            {
                Timestamp = DateTime.UtcNow,
                Channel = "email",
                Recipient = order.CustomerEmail,
                Message = message
            });
        }
    }
}
