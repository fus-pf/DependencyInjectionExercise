using DependencyInjectionExercise.Models;

namespace DependencyInjectionExercise.Services.NotificationSenders
{
    public class SmsNotificationSender : INotificationSender
    {
        private readonly NotificationHub _notificationHub;

        public SmsNotificationSender(NotificationHub notificationHub)
        {
            _notificationHub = notificationHub;
        }

        public void Send(Order order, Book? book, string message)
        {
            Console.WriteLine($"[SMS] To: {order.CustomerPhone} | {message}");

            _notificationHub.Add(new NotificationLog
            {
                Timestamp = DateTime.UtcNow,
                Channel = "sms",
                Recipient = order.CustomerPhone,
                Message = message
            });
        }
    }
}
