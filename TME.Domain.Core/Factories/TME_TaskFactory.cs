﻿using TME.Domain.Core.Entities;
using TME.Domain.Core.Enums;


namespace TME.Domain.Core.Factories
{
    public class TME_TaskFactory
    {
        public TME_TaskFactory(Guid? id, string title, string description, TME_TaskStatus taskStatus, TME_TaskPriority
            taskPriority, TME_Project project, DateTime dueDate, DateTime createdOn, Guid createdByApplicationUserId,
            DateTime? lastUpdated, Guid? lastUpdatedByApplicationUserId, bool isDeleted, bool isActive)
        {
            Id = id;
            Title = title;
            Description = description;
            DueDate = dueDate;
            TaskStatus = taskStatus;
            TaskPriority = taskPriority;
            Project = project;

            // Base
            CreatedOn = createdOn;
            CreatedByApplicationUserId = createdByApplicationUserId;
            IsActive = isActive;
            IsDeleted = isDeleted;
            LastUpdated = lastUpdated;
            LastUpdatedByApplicationUserId = lastUpdatedByApplicationUserId;
        }


        public Guid? Id { get; set; }

        public string Title { get; private set; }

        public string Description { get; private set; }

        public DateTime DueDate { get; private set; }

        public TME_TaskStatus TaskStatus { get; private set; }

        public TME_TaskPriority TaskPriority { get; private set; }

        public TME_Project Project { get; private set; }

        public DateTime CreatedOn { get; private set; }

        public Guid CreatedByApplicationUserId { get; private set; }

        public DateTime? LastUpdated { get; private set; }

        public Guid? LastUpdatedByApplicationUserId { get; private set; }

        public bool IsDeleted { get; private set; }

        public bool IsActive { get; private set; }


        public TME_Task Create()
        {
            var task = new TME_Task(Id, Title, Description, DueDate, TaskStatus, TME_TaskPriority.Alta, Project, 
                CreatedOn, CreatedByApplicationUserId, LastUpdated, LastUpdatedByApplicationUserId, IsDeleted, IsActive);

            if (Id == Guid.Empty){
                task.NotificationHandler.Handle(new Notifications.DomainNotification(
                    (task.NotificationHandler.GetNotifications().Count + 1).ToString(),
                    "O campo ID não pode ser nulo ou vazio."));
            }
            return CheckTitle(ref task);
        }

        private TME_Task CheckTitle(ref TME_Task task)
        {
            if (string.IsNullOrEmpty(Title))
            {
                task.NotificationHandler.Handle(new Notifications.DomainNotification(
                    (task.NotificationHandler.GetNotifications().Count + 1).ToString(),
                    "O campo Título não pode ser nulo ou vazio."));
            }
            else
            {
                if (Title.Length <= 2 || Title.Length > 120) {
                    task.NotificationHandler.Handle(new Notifications.DomainNotification(
                        (task.NotificationHandler.GetNotifications().Count + 1).ToString(),
                        "O campo Título deve ter mais que 2 caracteres e menos que 120."));
                }
            }

            return CheckDescription(ref task);
        }

        private TME_Task CheckDescription(ref TME_Task task)
        {
            if (string.IsNullOrEmpty(Description))
            {
                task.NotificationHandler.Handle(new Notifications.DomainNotification(
                    (task.NotificationHandler.GetNotifications().Count + 1).ToString(),
                    "O campo Descrição não pode ser nulo ou vazio."));
            }
            else
            {
                if (Description.Length <= 2 || Description.Length > 1024)
                {
                    task.NotificationHandler.Handle(new Notifications.DomainNotification(
                        (task.NotificationHandler.GetNotifications().Count + 1).ToString(),
                        "O campo Descrição deve ter mais que 2 caracteres e menos que 120."));
                }
            }
            return CheckDueDate(ref task);
        }

        private TME_Task CheckDueDate(ref TME_Task task)
        {
            if (DueDate < DateTime.Today)
            {
                task.NotificationHandler.Handle(new Notifications.DomainNotification(
                    (task.NotificationHandler.GetNotifications().Count + 1).ToString(),
                        "A data de vencimento deve ser maior ou igual a data corrente."));
            }
            return task;
        }
    }
}
