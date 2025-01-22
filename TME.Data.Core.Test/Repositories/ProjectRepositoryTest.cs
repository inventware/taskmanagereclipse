using System.Linq.Expressions;
using TME.Data.Core.Context;
using TME.Data.Core.Repositories;
using TME.Domain.Core.Entities;
using TME.Domain.Core.Enums;


namespace TME.Data.Core.Test.Repositories
{
    [TestClass]
    public class ProjectRepositoryTest
    {
        private SQLiteDataBaseForTests _sQLiteDataBaseForTests;

        [TestMethod]
        public async Task CheckListOfProjectsIsComplete()
        {
            var options = SQLiteDataBaseForTests.CreateOptions<TmeDbContext>();

            // ARRANGE
            using (var context = new TmeDbContext(options))
            {
                context.Database.EnsureCreated();
                _sQLiteDataBaseForTests = new SQLiteDataBaseForTests();
                _sQLiteDataBaseForTests.Seed(context);
            }

            // Second instance (separate) of the context to verify correct data was saved to database.
            using (var context = new TmeDbContext(options))
            {
                // ARRANGE
                var projectRepository = new ProjectRepository(context);

                Expression<Func<TME_Project, bool>> filter =
                    (rec) => rec.Description == "PROJ-01" && rec.IsActive == true && rec.IsDeleted == false;

                var includeProperties = new List<Expression<Func<TME_Project, object>>>();
                Expression<Func<TME_Project, object>> includeProperty = rec => 
                    rec.Tasks.Where(field => field.IsActive == true && field.IsDeleted == false);
                includeProperties.Add(includeProperty);

                // ACT
                // Embora haja na tabela 5 registros deste projeto, sendo 1 inativo e outro excluído logicamente,
                // somente os registros válidos são pegos usando o método com filtro abaixo:
                var projectsList = projectRepository.GetAsync(filter, null, includeProperties, null, null).Result.ToList();

                // ASSERT
                Assert.IsNotNull(projectsList);
                Assert.AreEqual(projectsList.First().Tasks.Count, 3);
            }
        }


        [TestMethod]
        public async Task CheckAddTaskWhenListOfProjectsHaveMoreThan20Tasks()
        {
            var options = SQLiteDataBaseForTests.CreateOptions<TmeDbContext>();

            // ARRANGE
            using (var context = new TmeDbContext(options))
            {
                context.Database.EnsureCreated();
                _sQLiteDataBaseForTests = new SQLiteDataBaseForTests();
                _sQLiteDataBaseForTests.Seed(context);
            }

            // Second instance (separate) of the context to verify correct data was saved to database.
            using (var context = new TmeDbContext(options))
            {
                // ARRANGE
                var projectRepository = new ProjectRepository(context);
                
                Expression<Func<TME_Project, bool>> filter =
                    (rec) => rec.Description == "PROJ-05" && rec.IsActive == true && rec.IsDeleted == false;

                var includeProperties = new List<Expression<Func<TME_Project, object>>>();
                Expression<Func<TME_Project, object>> includeProperty = rec =>
                    rec.Tasks.Where(field => field.IsActive == true && field.IsDeleted == false);
                includeProperties.Add(includeProperty);

                // ACT
                var project = projectRepository.GetAsync(filter, null, includeProperties, null, null).Result.FirstOrDefault();
                project.AddTask(Guid.NewGuid(), "new title", "new description", DateTime.Today, TME_TaskStatus.EmAndamento,
                    TME_TaskPriority.Média, DateTime.Today, Guid.NewGuid(), null, null, false, true);

                // ASSERT
                Assert.IsTrue(project.NotificationHandler.HasNotifications());
                Assert.IsTrue(project.NotificationHandler.GetNotifications().Where(rec => rec.Description ==
                    "O projeto 'PROJ-05' não pode receber mais tarefas, pois já possui 20 tarefas cadastradas.").Any());
            }
        }
    }
}
