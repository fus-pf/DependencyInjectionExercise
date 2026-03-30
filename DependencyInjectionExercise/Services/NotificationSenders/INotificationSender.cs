using DependencyInjectionExercise.Models;

namespace DependencyInjectionExercise.Services.NotificationSenders
{
    public interface INotificationSender
    {
        string Channel { get; }
        void Send(Order order, string message);
    }
}
