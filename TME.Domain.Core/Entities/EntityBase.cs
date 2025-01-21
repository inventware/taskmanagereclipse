using System.ComponentModel.DataAnnotations.Schema;
using TME.Domain.Core.Notifications;


namespace TME.Domain.Core.Entities
{
    // http://www.channelsean.com/2012/07/baseentity-c.html
    /// <typeparam name="TEntity">The type of the entity.</typeparam>  12:      
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// 

    public abstract class EntityBase<TEntity, TKey>
        where TEntity : class, IEntity<TKey>
        where TKey : struct
    {
        public EntityBase(TKey id, long? idOrigin, DateTime createdOn, Guid createdByApplicationUserId,
            DateTime? lastUpdated, Guid? lastUpdatedByApplicationUserId, bool isDeleted, bool isActive)
        {
            Id = id;
            IdOrigin = idOrigin;
            CreatedOn = createdOn;
            CreatedByApplicationUserId = createdByApplicationUserId;
            IsActive = isActive;
            IsDeleted = isDeleted;
            LastUpdated = lastUpdated;
            LastUpdatedByApplicationUserId = lastUpdatedByApplicationUserId;
            NotificationHandler = new DomainNotificationHandler();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public TKey Id { get; set; }

        /// <summary>
        /// Bind a legacy entity Id to this entity.
        /// </summary>
        public long? IdOrigin { get; set; }

        public DateTime CreatedOn { get; set; }

        public Guid CreatedByApplicationUserId { get; set; }

        public DateTime? LastUpdated { get; set; }

        public Guid? LastUpdatedByApplicationUserId { get; set; }

        public bool IsDeleted { get; set; }

        public bool IsActive { get; set; }

        /// <summary>
        /// Utiliza-se este handler de manipulação de notificações, no ato da criação de algumas entidades, por si 
        /// mesma, ou por alguma entidade envolvida.
        /// </summary>
        public DomainNotificationHandler NotificationHandler { get; protected set; }


        public override bool Equals(object obj)
        {
            var compareTo = obj as EntityBase<TEntity, TKey>;

            if (ReferenceEquals(this, compareTo)) return true;
            if (ReferenceEquals(null, compareTo)) return false;

            return Id.Equals(compareTo.Id);
        }


        public static bool operator ==(EntityBase<TEntity, TKey> entityA, EntityBase<TEntity, TKey> entityB)
        {
            if (ReferenceEquals(entityA, null) && ReferenceEquals(entityB, null))
            {
                return true;
            }

            if (ReferenceEquals(entityA, null) || ReferenceEquals(entityB, null))
            {
                return false;
            }
            return entityA.Equals(entityB);
        }


        public static bool operator !=(EntityBase<TEntity, TKey> entityA, EntityBase<TEntity, TKey> entityB)
        {
            return !(entityA == entityB);
        }


        public override int GetHashCode()
        {
            return (GetType().GetHashCode() * 907) + Id.GetHashCode();
        }


        public override string ToString()
        {
            return GetType().Name + " [Id=" + Id + "]";
        }
    }
}
