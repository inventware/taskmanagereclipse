
using TME.Crosscutting.Enums;

namespace TME.Domain.Core.Notifications
{
    public abstract class Notification
    {
        public Notification(string code, string description, EDomainNotificationType type)
        {
            Code = (code ?? (type == EDomainNotificationType.Notificação ? "Notificação" :
                (type == EDomainNotificationType.Success ? "Sucesso" : "Erro")));
            Description = description;
            Type = type;
        }


        public string Code { get; protected set; }

        public string Description { get; protected set; }

        public EDomainNotificationType Type { get; protected set; }
    }
}
