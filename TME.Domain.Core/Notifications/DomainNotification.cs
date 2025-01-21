using TME.Crosscutting.Enums;


namespace TME.Domain.Core.Notifications
{
    public class DomainNotification : Notification
    {
        public DomainNotification(string code, string description)
            : base(code, description, EDomainNotificationType.Notificação)
        {
            DomainNotificationId = Guid.NewGuid();
            Version = 1;
        }


        public Guid DomainNotificationId { get; private set; }

        public int Version { get; private set; }
    }
}
