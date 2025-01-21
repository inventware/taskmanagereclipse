using TME.Domain.Core.Enums;
using TME.Domain.Core.Factories;


namespace TME.Domain.Core.Entities
{
    public class TME_Project : EntityBase<TME_Project, Guid>, IEntity<Guid>
    {
        private TME_TaskFactory _taskFactory;
        private HashSet<TME_Task> _tasks;

        /// <summary>
        /// IMPORTANTE: Usado SOMENTE pelo EF
        /// </summary>
        public TME_Project()
            : base(Guid.Empty, null, DateTime.MinValue, Guid.Empty, DateTime.MinValue, Guid.Empty, false, false)
        {
            _tasks = new HashSet<TME_Task>();
        }

        public TME_Project(Guid? id, string description, DateTime createdOn, Guid createdByApplicationUserId, 
            DateTime? lastUpdated, Guid? lastUpdatedByApplicationUserId, bool isDeleted, bool isActive)
        : base(id.GetValueOrDefault(), null, createdOn, createdByApplicationUserId, lastUpdated,
            lastUpdatedByApplicationUserId, isDeleted, isActive)
        {
            Description = description;
            _tasks = new HashSet<TME_Task>();
        }


        public string Description { get; protected set; }

        public HashSet<TME_Task> Tasks { get => _tasks; }


        public TME_Task AddTask(Guid id, string title, string description, DateTime dueDate, TME_TaskStatus tME_TaskStatus,
            TME_TaskPriority taskPriority, DateTime createdOn, Guid createdByApplicationUserId, DateTime? lastUpdated,
            Guid? lastUpdatedByApplicationUserId, bool isDeleted, bool isActive)
        {
            _taskFactory = new TME_TaskFactory(id, title, description, tME_TaskStatus, taskPriority, dueDate, createdOn,
                createdByApplicationUserId, lastUpdated, lastUpdatedByApplicationUserId, isDeleted, isActive);

            var task = _taskFactory.Create();
            
            if (task.NotificationHandler.HasNotifications())
            {
                this.NotificationHandler = task.NotificationHandler;
                return null;
            }
            return task;
        }
    }
}
