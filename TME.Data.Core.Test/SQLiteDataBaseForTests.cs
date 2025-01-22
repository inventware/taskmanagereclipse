using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using TME.Data.Core.Context;
using TME.Domain.Core.Entities;
using TME.Domain.Core.Enums;


namespace TME.Data.Core.Test
{
    internal class SQLiteDataBaseForTests
    {
        public static DbContextOptions<T> CreateOptions<T>()
            where T : DbContext
        {
            var connectionStringBuilder = new SqliteConnectionStringBuilder {
                DataSource = ":memory:"
            };
            var connectionString = connectionStringBuilder.ToString();
            var connection = new SqliteConnection(connectionString);

            try
            {
                connection.Open();

                // create in-memory context
                var builder = new DbContextOptionsBuilder<T>();
                builder.UseSqlite(connection);

                return builder.Options;
            }
            catch (Exception err)
            {
                throw new SqliteException(err.Message, 1);
            }
        }


        public void Seed(TmeDbContext context)
        {
            var loggedUser = Guid.NewGuid();

            var project01 = new TME_Project(Guid.NewGuid(), "PROJ-01", DateTime.Today, loggedUser, null, null, false, true);

                project01.AddTask(Guid.NewGuid(), "TASK-LR-01", "Levantamento de Requisitos", DateTime.Today.AddDays(30),
                    TME_TaskStatus.EmAndamento, TME_TaskPriority.Baixa, DateTime.Today, loggedUser, null, null, false, true);

                project01.AddTask(Guid.NewGuid(), "TASK-DEV-01", "Construção e Testes", DateTime.Today.AddDays(60),
                    TME_TaskStatus.EmAndamento, TME_TaskPriority.Baixa, DateTime.Today, loggedUser, null, null, false, true);

                // Foi excluído logicamente:
                project01.AddTask(Guid.NewGuid(), "TASK-IMPL-01", "Implantação e suporte", DateTime.Today.AddDays(60),
                    TME_TaskStatus.EmAndamento, TME_TaskPriority.Baixa, DateTime.Today, loggedUser, null, null, true, true);

                // Foi inativado:
                project01.AddTask(Guid.NewGuid(), "TASK-IMPL-01", "Implantação e suporte", DateTime.Today.AddDays(60),
                    TME_TaskStatus.EmAndamento, TME_TaskPriority.Baixa, DateTime.Today, loggedUser, null, null, false, false);

                // Ativo:
                project01.AddTask(Guid.NewGuid(), "TASK-IMPL-01", "Nova Implantação e suporte", DateTime.Today.AddDays(90),
                    TME_TaskStatus.EmAndamento, TME_TaskPriority.Baixa, DateTime.Today, loggedUser, null, null, false, true);

            var project02 = new TME_Project(Guid.NewGuid(), "PROJ-02", DateTime.Today, loggedUser, null, null, false, true);

                project02.AddTask(Guid.NewGuid(), "TASK-SUP-01", "Sustentação e suporte", DateTime.Today.AddDays(30),
                    TME_TaskStatus.EmAndamento, TME_TaskPriority.Baixa, DateTime.Today, loggedUser, null, null, false, true);

                //isActive is false:
                project02.AddTask(Guid.NewGuid(), "TASK-SUP-02", "Construção e Testes", DateTime.Today.AddDays(60),
                    TME_TaskStatus.EmAndamento, TME_TaskPriority.Baixa, DateTime.Today, loggedUser, null, null, false, false);

            //isActive is false:
            var project03 = new TME_Project(Guid.NewGuid(), "PROJ-03", DateTime.Today, loggedUser, null, null, false, false);

            //isDeleted is TRUE, logical exclusion:
            var project04 = new TME_Project(Guid.NewGuid(), "PROJ-04", DateTime.Today, loggedUser, null, null, true, true);

            // Projeto contendo 20 tarefas adicionadas.
            var project05 = new TME_Project(Guid.NewGuid(), "PROJ-05", DateTime.Today, loggedUser, null, null, false, true);

                project05.AddTask(Guid.NewGuid(), "TASK-01", "Tarefa 1 à ser executada!", DateTime.Today,
                    TME_TaskStatus.EmAndamento, TME_TaskPriority.Baixa, DateTime.Today, loggedUser, null, null, false, true);

                project05.AddTask(Guid.NewGuid(), "TASK-02", "Tarefa 2 à ser executada!", DateTime.Today,
                    TME_TaskStatus.EmAndamento, TME_TaskPriority.Baixa, DateTime.Today, loggedUser, null, null, false, true);

                project05.AddTask(Guid.NewGuid(), "TASK-03", "Tarefa 3 à ser executada!", DateTime.Today,
                    TME_TaskStatus.EmAndamento, TME_TaskPriority.Baixa, DateTime.Today, loggedUser, null, null, false, true);

                project05.AddTask(Guid.NewGuid(), "TASK-04", "Tarefa 4 à ser executada!", DateTime.Today,
                    TME_TaskStatus.EmAndamento, TME_TaskPriority.Baixa, DateTime.Today, loggedUser, null, null, false, true);

                project05.AddTask(Guid.NewGuid(), "TASK-05", "Tarefa 5 à ser executada!", DateTime.Today,
                    TME_TaskStatus.EmAndamento, TME_TaskPriority.Baixa, DateTime.Today, loggedUser, null, null, false, true);

                project05.AddTask(Guid.NewGuid(), "TASK-06", "Tarefa 6 à ser executada!", DateTime.Today,
                    TME_TaskStatus.EmAndamento, TME_TaskPriority.Baixa, DateTime.Today, loggedUser, null, null, false, true);

                project05.AddTask(Guid.NewGuid(), "TASK-07", "Tarefa 7 à ser executada!", DateTime.Today,
                    TME_TaskStatus.EmAndamento, TME_TaskPriority.Baixa, DateTime.Today, loggedUser, null, null, false, true);

                project05.AddTask(Guid.NewGuid(), "TASK-08", "Tarefa 8 à ser executada!", DateTime.Today,
                    TME_TaskStatus.EmAndamento, TME_TaskPriority.Baixa, DateTime.Today, loggedUser, null, null, false, true);

                project05.AddTask(Guid.NewGuid(), "TASK-09", "Tarefa 9 à ser executada!", DateTime.Today,
                    TME_TaskStatus.EmAndamento, TME_TaskPriority.Baixa, DateTime.Today, loggedUser, null, null, false, true);

                project05.AddTask(Guid.NewGuid(), "TASK-10", "Tarefa 10 à ser executada!", DateTime.Today,
                    TME_TaskStatus.EmAndamento, TME_TaskPriority.Baixa, DateTime.Today, loggedUser, null, null, false, true);

                project05.AddTask(Guid.NewGuid(), "TASK-11", "Tarefa 11 à ser executada!", DateTime.Today,
                    TME_TaskStatus.EmAndamento, TME_TaskPriority.Baixa, DateTime.Today, loggedUser, null, null, false, true);

                project05.AddTask(Guid.NewGuid(), "TASK-12", "Tarefa 12 à ser executada!", DateTime.Today,
                    TME_TaskStatus.EmAndamento, TME_TaskPriority.Baixa, DateTime.Today, loggedUser, null, null, false, true);

                project05.AddTask(Guid.NewGuid(), "TASK-13", "Tarefa 13 à ser executada!", DateTime.Today,
                    TME_TaskStatus.EmAndamento, TME_TaskPriority.Baixa, DateTime.Today, loggedUser, null, null, false, true);

                project05.AddTask(Guid.NewGuid(), "TASK-14", "Tarefa 14 à ser executada!", DateTime.Today,
                    TME_TaskStatus.EmAndamento, TME_TaskPriority.Baixa, DateTime.Today, loggedUser, null, null, false, true);

                project05.AddTask(Guid.NewGuid(), "TASK-15", "Tarefa 15 à ser executada!", DateTime.Today,
                    TME_TaskStatus.EmAndamento, TME_TaskPriority.Baixa, DateTime.Today, loggedUser, null, null, false, true);

                project05.AddTask(Guid.NewGuid(), "TASK-16", "Tarefa 16 à ser executada!", DateTime.Today,
                    TME_TaskStatus.EmAndamento, TME_TaskPriority.Baixa, DateTime.Today, loggedUser, null, null, false, true);

                project05.AddTask(Guid.NewGuid(), "TASK-17", "Tarefa 17 à ser executada!", DateTime.Today,
                    TME_TaskStatus.EmAndamento, TME_TaskPriority.Baixa, DateTime.Today, loggedUser, null, null, false, true);

                project05.AddTask(Guid.NewGuid(), "TASK-18", "Tarefa 18 à ser executada!", DateTime.Today,
                    TME_TaskStatus.EmAndamento, TME_TaskPriority.Baixa, DateTime.Today, loggedUser, null, null, false, true);

                project05.AddTask(Guid.NewGuid(), "TASK-19", "Tarefa 19 à ser executada!", DateTime.Today,
                    TME_TaskStatus.EmAndamento, TME_TaskPriority.Baixa, DateTime.Today, loggedUser, null, null, false, true);

                project05.AddTask(Guid.NewGuid(), "TASK-20", "Tarefa 20 à ser executada!", DateTime.Today,
                    TME_TaskStatus.EmAndamento, TME_TaskPriority.Baixa, DateTime.Today, loggedUser, null, null, false, true);

            context.TME_PROJECT.Add(project01);
            context.TME_PROJECT.Add(project02);
            context.TME_PROJECT.Add(project03);
            context.TME_PROJECT.Add(project04);
            context.TME_PROJECT.Add(project05);

            context.SaveChanges();
        }
    }
}
