using TME.Domain.Core.Entities;
using TME.Domain.Core.Enums;


namespace TME.Domain.Core.Test.Entities
{
    [TestClass]
    public class TME_ProjectTest
    {
        [TestMethod]
        public void CheckAddTaskWhenIdIsEmpty()
        {
            // ARRANGE
            var project = new TME_Project(Guid.NewGuid(), "description", DateTime.Today, Guid.NewGuid(),
                null, null, false, true);

            Guid testTaskId = Guid.Empty;   //<-- O Id não pode ser vazio!
            Guid createdByApplicationUserId = Guid.NewGuid();
            Guid lastUpdatedByApplicationUserId = Guid.NewGuid();

            // ACT
            project.AddTask(testTaskId, "title", "description", DateTime.Now, TME_TaskStatus.Pendente, 
                TME_TaskPriority.Alta, DateTime.Now, createdByApplicationUserId, null, Guid.Empty, false, true);

            // ASSERTS
            Assert.IsTrue(project.NotificationHandler.HasNotifications());
            Assert.IsTrue(project.NotificationHandler.GetNotifications().Where(rec => rec.Description ==
                "O campo ID não pode ser nulo ou vazio.").Any());
        }


        [TestMethod]
        public void CheckAddTaskWhenTitleIsEmpty()
        {
            // ARRANGE
            var project = new TME_Project(Guid.NewGuid(), "description", DateTime.Today, Guid.NewGuid(),
                null, null, false, true);

            Guid testTaskId = Guid.NewGuid();
            Guid createdByApplicationUserId = Guid.NewGuid();
            Guid lastUpdatedByApplicationUserId = Guid.NewGuid();
            string title = string.Empty;    //<-- title vazio!

            // ACT
            project.AddTask(testTaskId, title, "description", DateTime.Now, TME_TaskStatus.Pendente, 
                TME_TaskPriority.Alta, DateTime.Now, createdByApplicationUserId, null, Guid.Empty, false, true);

            // ASSERTS
            Assert.IsTrue(project.NotificationHandler.HasNotifications());
            Assert.IsTrue(project.NotificationHandler.GetNotifications().Where(rec => rec.Description ==
                "O campo Título não pode ser nulo ou vazio.").Any());
        }


        [TestMethod]
        public void CheckAddTaskWhenTitleIsLessThan2Characters()
        {
            // ARRANGE
            var project = new TME_Project(Guid.NewGuid(), "description", DateTime.Today, Guid.NewGuid(),
                null, null, false, true);

            Guid testTaskId = Guid.NewGuid();   //<-- O Id não pode ser vazio!
            Guid createdByApplicationUserId = Guid.NewGuid();
            Guid lastUpdatedByApplicationUserId = Guid.NewGuid();
            string title = "A1";    //<-- title menor igual a 2 caracteres!

            // ACT
            project.AddTask(testTaskId, title, "description", DateTime.Now, TME_TaskStatus.Pendente, 
                TME_TaskPriority.Alta, DateTime.Now, createdByApplicationUserId, null, Guid.Empty, false, true);

            // ASSERTS
            Assert.IsTrue(project.NotificationHandler.HasNotifications());
            Assert.IsTrue(project.NotificationHandler.GetNotifications().Where(rec => rec.Description ==
                "O campo Título deve ter mais que 2 caracteres e menos que 120.").Any());
        }


        [TestMethod]
        public void CheckAddTaskWhenDescriptionIsEmpty()
        {
            // ARRANGE
            var project = new TME_Project(Guid.NewGuid(), "description", DateTime.Today, Guid.NewGuid(),
                null, null, false, true);

            Guid testTaskId = Guid.NewGuid();
            Guid createdByApplicationUserId = Guid.NewGuid();
            Guid lastUpdatedByApplicationUserId = Guid.NewGuid();
            string description = string.Empty;    //<-- description vazio!

            // ACT
            project.AddTask(testTaskId, "title", description, DateTime.Now, TME_TaskStatus.Pendente, 
                TME_TaskPriority.Alta, DateTime.Now, createdByApplicationUserId, null, Guid.Empty, false, true);

            // ASSERTS
            Assert.IsTrue(project.NotificationHandler.HasNotifications());
            Assert.IsTrue(project.NotificationHandler.GetNotifications().Where(rec => rec.Description ==
                "O campo Descrição não pode ser nulo ou vazio.").Any());
        }


        [TestMethod]
        public void CheckAddTaskWhenDescriptionIsLessThan2Characters()
        {
            // ARRANGE
            var project = new TME_Project(Guid.NewGuid(), "description", DateTime.Today, Guid.NewGuid(),
                null, null, false, true);

            Guid testTaskId = Guid.NewGuid();   //<-- O Id não pode ser vazio!
            Guid createdByApplicationUserId = Guid.NewGuid();
            Guid lastUpdatedByApplicationUserId = Guid.NewGuid();
            string description = "Oi";    //<-- description menor igual a 2 caracteres!

            // ACT
            project.AddTask(testTaskId, "title", description, DateTime.Now, TME_TaskStatus.Pendente, 
                TME_TaskPriority.Alta, DateTime.Now, createdByApplicationUserId, null, Guid.Empty, false, true);

            // ASSERTS
            Assert.IsTrue(project.NotificationHandler.HasNotifications());
            Assert.IsTrue(project.NotificationHandler.GetNotifications().Where(rec => rec.Description ==
                "O campo Descrição deve ter mais que 2 caracteres e menos que 120.").Any());
        }


        [TestMethod]
        public void CheckAddTaskWhenDueDateIsInvalid()
        {
            // ARRANGE
            var project = new TME_Project(Guid.NewGuid(), "description", DateTime.Today, Guid.NewGuid(),
                null, null, false, true);

            Guid testTaskId = Guid.NewGuid();   //<-- O Id não pode ser vazio!
            Guid createdByApplicationUserId = Guid.NewGuid();
            Guid lastUpdatedByApplicationUserId = Guid.NewGuid();

            var yesterdayDate = DateTime.Now.AddDays(-1);

            // ACT
            project.AddTask(testTaskId, "title", "description", yesterdayDate, TME_TaskStatus.Pendente,
                TME_TaskPriority.Alta, DateTime.MaxValue, createdByApplicationUserId, null, Guid.Empty, false, true);

            // ASSERTS
            Assert.IsTrue(project.NotificationHandler.HasNotifications());
            Assert.IsTrue(project.NotificationHandler.GetNotifications().Where(rec => rec.Description ==
                "A data de vencimento deve ser maior ou igual a data corrente.").Any());
        }


    }
}
