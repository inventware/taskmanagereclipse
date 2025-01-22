using System.Linq.Expressions;
using TME.Domain.Core.Entities;
using TME.Domain.Core.Enums;
using TME.Domain.Core.Repositories;


namespace TME.Domain.Core.Services
{
    public class TaskDomainService
    {
        private IProjectRepository _projectRepository;
        private ITaskRepository _taskRepository;

        public TaskDomainService(IProjectRepository projectRepository, ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
            _projectRepository = projectRepository;
        }


        private async Task<TME_Project> GetProjectByDesc(string description)
        {
            Expression<Func<TME_Project, bool>> filter =
                (rec) => rec.Description == description && rec.IsActive == true && rec.IsDeleted == false;

            var includeProperties = new List<Expression<Func<TME_Project, object>>>();
            Expression<Func<TME_Project, object>> includeProperty = rec =>
                rec.Tasks.Where(field => field.IsActive == true && field.IsDeleted == false);
            includeProperties.Add(includeProperty);

            var projectList = await _projectRepository.GetAsync(filter, null, includeProperties, null, null);
            return projectList.FirstOrDefault();
        }


        /// <summary>
        /// Criação de Tarefas - adicionar uma nova tarefa a um projeto
        /// </summary>
        /// <param name="projDescription"></param>
        /// <returns></returns>
        public async Task<TME_Task> CreateTask(string projDescription, string taskTitle, string taskDescription,
            DateTime dueDate, TME_TaskStatus tME_TaskStatus, TME_TaskPriority taskPriority, Guid loggedUserId)
        {
            var projectToAdd = await GetProjectByDesc(projDescription);

            var task = projectToAdd.AddTask(Guid.NewGuid(), taskTitle, taskDescription, dueDate, tME_TaskStatus,
                taskPriority, DateTime.Today, loggedUserId, null, null, false, true);

            _projectRepository.Create(projectToAdd);
            _projectRepository.AppContext.SaveChangesAsync();

            return task;
        }


        /// <summary>
        /// Atualização de Tarefas - atualizar o status ou detalhes de uma tarefa
        /// </summary>
        /// <param name="projDescription"></param>
        /// <returns></returns>
        public async Task<TME_Task> UpdateTask(string projDescription, string taskTitle, string taskDescription,
            DateTime dueDate, TME_TaskStatus tME_TaskStatus, TME_TaskPriority taskPriority, Guid loggedUserId)
        {
            var project = await GetProjectByDesc(projDescription);
            project.RemoveTask(taskDescription);

            return project.AddTask(Guid.NewGuid(), taskTitle, taskDescription, dueDate, tME_TaskStatus,
                taskPriority, DateTime.Today, loggedUserId, null, null, false, true);
        }



        /// <summary>
        /// Criação de Tarefas - adicionar uma nova tarefa a um projeto
        /// </summary>
        /// <param name="projDescription"></param>
        /// <returns></returns>
        public async Task<TME_Task> RemoveTask(string projDescription, string taskTitle, string taskDescription,
            DateTime dueDate, TME_TaskStatus tME_TaskStatus, TME_TaskPriority taskPriority, Guid loggedUserId)
        {
            var project = await GetProjectByDesc(projDescription);
            bool isRemoved = project.RemoveTask(taskDescription);

            return project.AddTask(Guid.NewGuid(), taskTitle, taskDescription, dueDate, tME_TaskStatus,
                taskPriority, DateTime.Today, loggedUserId, null, null, false, true);
        }
    }
}
