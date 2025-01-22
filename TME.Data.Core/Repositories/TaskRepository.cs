using TME.Data.Core.Context;
using TME.Domain.Core.Entities;
using TME.Domain.Core.Repositories;


namespace TME.Data.Core.Repositories
{
    public class TaskRepository: RepositoryBase<TME_Task, Guid>, ITaskRepository 
    {
        public TaskRepository(TmeDbContext context) : base(context) { }
    }
}
