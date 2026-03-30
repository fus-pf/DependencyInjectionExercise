using DependencyInjectionExercise.Models;

namespace DependencyInjectionExercise.Services
{
    public class SmsNotificationSender : INotificationSender
    {
        private readonly NotificationHub _notificationHub;

        public SmsNotificationSender(NotificationHub notificationHub)
        {
            _notificationHub = notificationHub;
        }

        public string Channel => "sms";

        public void Send(Order order, string message)
        {
            Console.WriteLine($"[SMS] To: {order.CustomerPhone} | {message}");

            _notificationHub.Add(new NotificationLog
            {
                Timestamp = DateTime.UtcNow,
                Channel = Channel,
                Recipient = order.CustomerPhone,
                Message = message
            });
        }
    }
}
