using DependencyInjectionExercise.Models;

namespace DependencyInjectionExercise.Services
{
    public interface INotificationSender
    {
        string Channel { get; }
        void Send(Order order, string message);
    }
}
