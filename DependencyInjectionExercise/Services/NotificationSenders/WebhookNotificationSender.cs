using DependencyInjectionExercise.Models;

namespace DependencyInjectionExercise.Services.NotificationSenders
{
    public class WebhookNotificationSender : INotificationSender
    {
        private readonly NotificationHub _notificationHub;

        public WebhookNotificationSender(NotificationHub notificationHub)
        {
            _notificationHub = notificationHub;
        }

        public void Send(Order order, Book? book, string message)
        {
            Console.WriteLine($"[WEBHOOK] To: {order.CustomerEmail} | {message}");

            _notificationHub.Add(new NotificationLog
            {
                Timestamp = DateTime.UtcNow,
                Channel = "webhook",
                Recipient = order.CustomerEmail,
                Message = message
            });
        }
    }
}
