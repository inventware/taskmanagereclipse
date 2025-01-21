
namespace TME.Domain.Core.Entities
{
    public interface IEntity<TKey>
        where TKey : struct
    {
        TKey Id { get; }

        // Bind a legacy entity Id to this entity.
        long? IdOrigin { get; }

        DateTime CreatedOn { get; }

        Guid CreatedByApplicationUserId { get; }

        DateTime? LastUpdated { get; }

        Guid? LastUpdatedByApplicationUserId { get; }

        bool IsDeleted { get; }

        bool IsActive { get; }
    }
}
