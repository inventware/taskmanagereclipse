using System.Linq.Expressions;
using TME.Domain.Core.Entities;
using TME.Domain.Core.Enums;
using TME.Domain.Core.Repositories;


namespace TME.Domain.Core.Services
{
    public class ProjectDomainService
    {
        private IProjectRepository _projectRepository;

        public ProjectDomainService(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }

        /// <summary>
        /// Listagem de Projetos - listar todos os projetos do usuário
        /// </summary>
        /// <returns></returns>
        public async Task<IList<TME_Project>> GetAllProjects()
        {
            return await _projectRepository.GetAllAsync();
        }


        /// <summary>
        /// Visualização de Tarefas - visualizar todas as tarefas de um projeto específico
        /// </summary>
        /// <param name="projDescription"></param>
        /// <returns></returns>
        public async Task<IList<TME_Task>> GetTasksOf(string projDescription)
        {
            Expression<Func<TME_Project, bool>> filter =
                (rec) => rec.Description == projDescription && rec.IsActive == true && rec.IsDeleted == false;

            var includeProperties = new List<Expression<Func<TME_Project, object>>>();
            Expression<Func<TME_Project, object>> includeProperty = rec =>
                rec.Tasks.Where(field => field.IsActive == true && field.IsDeleted == false);
            includeProperties.Add(includeProperty);

            var projectList = await _projectRepository.GetAsync(filter, null, includeProperties, null, null);
            return projectList.FirstOrDefault().Tasks.ToList();
        }


        /// <summary>
        /// Criação de Projetos - criar um novo projeto
        /// </summary>
        /// <param name="id"></param>
        /// <param name="description"></param>
        /// <param name="createdOn"></param>
        /// <param name="loggedUserId"></param>
        /// <returns></returns>
        public async Task CreateProject(Guid? id, string description, DateTime createdOn, Guid loggedUserId)
        {
            _projectRepository.Create(new TME_Project(id, description, createdOn, loggedUserId,
                null, null, false, false));
        }


        public async Task RemoveProject(string projDescription)
        {
            var tasksList = GetTasksOf(projDescription);
            if (tasksList.Result.Count == 0) 
            {
                Expression<Func<TME_Project, bool>> filter = (rec) => rec.Description == projDescription;
                _projectRepository.Delete(_projectRepository.GetAsync(filter, null, null, null, null).Result.FirstOrDefault());
            }
            throw new ArgumentException("Não é possível a exclusão de um projeto que possue tarefas vinuladas.");
        }
    }
}
