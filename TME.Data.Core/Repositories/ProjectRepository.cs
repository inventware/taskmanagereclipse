using TME.Domain.Core.Entities;
using TME.Domain.Core.Repositories;


namespace TME.Data.Core.Repositories
{
    public class ProjectRepository: RepositoryBase<TME_Project, Guid>, IProjectRepository { }
}
