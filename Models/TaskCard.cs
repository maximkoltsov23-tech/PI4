using System;
using System.Runtime.Serialization;

namespace PI4.Models
{
    [DataContract]
    public class TaskCard : NotifyObject
    {
        private string title;
        private string description;
        private string assignee;
        private string priority;
        private string dueDate;

        public TaskCard()
        {
            Id = Guid.NewGuid().ToString();
            Priority = "Средний";
            DueDate = DateTime.Today.AddDays(3).ToString("dd.MM.yyyy");
        }

        [DataMember]
        public string Id { get; set; }

        [DataMember]
        public string Title
        {
            get => title;
            set => SetProperty(ref title, value);
        }

        [DataMember]
        public string Description
        {
            get => description;
            set => SetProperty(ref description, value);
        }

        [DataMember]
        public string Assignee
        {
            get => assignee;
            set => SetProperty(ref assignee, value);
        }

        [DataMember]
        public string Priority
        {
            get => priority;
            set => SetProperty(ref priority, value);
        }

        [DataMember]
        public string DueDate
        {
            get => dueDate;
            set => SetProperty(ref dueDate, value);
        }
    }
}
