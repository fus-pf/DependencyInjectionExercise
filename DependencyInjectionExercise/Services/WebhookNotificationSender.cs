using DependencyInjectionExercise.Models;

namespace DependencyInjectionExercise.Services
{
    public class WebhookNotificationSender : INotificationSender
    {
        private readonly NotificationHub _notificationHub;

        public WebhookNotificationSender(NotificationHub notificationHub)
        {
            _notificationHub = notificationHub;
        }

        public string Channel => "webhook";

        public void Send(Order order, string message)
        {
            Console.WriteLine($"[WEBHOOK] To: {order.CustomerEmail} | {message}");

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
