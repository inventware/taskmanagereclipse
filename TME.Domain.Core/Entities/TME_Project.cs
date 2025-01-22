using System.Threading.Tasks;
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
        internal TME_Project()
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
            _taskFactory = new TME_TaskFactory(id, title, description, tME_TaskStatus, taskPriority, this, dueDate, createdOn,
                createdByApplicationUserId, lastUpdated, lastUpdatedByApplicationUserId, isDeleted, isActive);

            var task = _taskFactory.Create();
            
            if (task.NotificationHandler.HasNotifications())
            {
                this.NotificationHandler = task.NotificationHandler;
                return null;
            }
            return IsThereTaskInProject(ref task);
        }

        private TME_Task IsThereTaskInProject(ref TME_Task task)
        {
            var auxTask = task as TME_Task;

            if (auxTask != null && _tasks.Where(rec => rec.Id == auxTask.Id || 
               (rec.Description == auxTask.Description && rec.IsDeleted == false && rec.IsActive == true)).Any())
            {
                this.NotificationHandler.Handle(new Notifications.DomainNotification(
                    (task.NotificationHandler.GetNotifications().Count + 1).ToString(),
                    $"A tarefa '{auxTask.Description}' já foi inserida no projeto '{this.Description}'."));
                
                return null;
            }
            return IsThereMoreThan20TasksInProject(ref task);
        }

        private TME_Task IsThereMoreThan20TasksInProject(ref TME_Task task)
        {
            if (_tasks.Count >= 20)
            {
                this.NotificationHandler.Handle(new Notifications.DomainNotification(
                    (task.NotificationHandler.GetNotifications().Count + 1).ToString(),
                    $"O projeto '{this.Description}' não pode receber mais tarefas, pois já possui 20 tarefas cadastradas."));

                return null;
            }

            _tasks.Add(task);
            
            return task;
        }

        public TME_Task UpdateTask(string taskTitle, string taskDescription, DateTime dueDate, TME_TaskStatus taskStatus, 
            TME_TaskPriority taskPriority, Guid loggedUserId)
        {
            var taskToUpdate = _tasks.Where(rec => rec.Description == taskDescription).FirstOrDefault();
            if (taskToUpdate == null){
                return null;
            }

            var newTask = new TME_Task(taskToUpdate.Id, taskTitle, taskDescription, dueDate, taskStatus, taskPriority, this,
                taskToUpdate.CreatedOn, taskToUpdate.CreatedByApplicationUserId, DateTime.Today, loggedUserId,
                taskToUpdate.IsDeleted, taskToUpdate.IsActive);
            if (newTask.NotificationHandler.HasNotifications()){
                return null;
            }

            _tasks.Remove(taskToUpdate);
            _tasks.Add(newTask);
            return newTask;
        }

        public bool RemoveTask(string taskDescription)
        {
            var taskToRemove = _tasks.Where(rec => rec.Description == taskDescription).FirstOrDefault();
            if (taskToRemove == null){
                return false;
            }

            _tasks.Remove(taskToRemove);
            return true;
        }
    }
}
