using TME.Domain.Core.Enums;


namespace TME.Domain.Core.Entities
{
    public class TME_Task : EntityBase<TME_Task, Guid>, IEntity<Guid>
    {
        /// <summary>
        /// IMPORTANTE: Usado SOMENTE pelo EF
        /// </summary>
        internal TME_Task()
            : base(Guid.Empty, null, DateTime.MinValue, Guid.Empty, DateTime.MinValue, Guid.Empty, false, false)
        {
        
        }

        public TME_Task(Guid? id, string title, string description, DateTime dueDate, TME_TaskStatus taskStatus,
            TME_TaskPriority taskPriority, DateTime createdOn, Guid createdByApplicationUserId, DateTime? lastUpdated, 
            Guid? lastUpdatedByApplicationUserId, bool isDeleted, bool isActive)
        : base(id.GetValueOrDefault(), null, createdOn, createdByApplicationUserId, lastUpdated,
              lastUpdatedByApplicationUserId, isDeleted, isActive)
        {
            Title = title;
            Description = description;
            DueDate = dueDate;
            TaskStatus = taskStatus;
            TaskPriority = taskPriority;
        }


        public string Title { get; protected set; }

        public string Description { get; protected set; }

        public DateTime DueDate { get; protected set; }

        public TME_TaskStatus TaskStatus { get; protected set; }

        public TME_TaskPriority TaskPriority { get; protected set; }
    }
}
