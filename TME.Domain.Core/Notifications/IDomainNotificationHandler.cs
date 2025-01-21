
namespace TME.Domain.Core.Notifications
{
    public interface IDomainNotificationHandler<T>
    where T : Notification
    {
        bool HasNotifications();

        List<T> GetNotifications();
    }
}
