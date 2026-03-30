using DependencyInjectionExercise.Models;

namespace DependencyInjectionExercise.Services.NotificationSenders
{
    public interface INotificationSender
    {
        void Send(Order order, Book? book, string message);
    }
}
