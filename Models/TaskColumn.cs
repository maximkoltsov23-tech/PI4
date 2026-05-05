using System;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace PI4.Models
{
    [DataContract]
    public class TaskColumn : NotifyObject
    {
        private string title;
        private string newCardTitle;

        public TaskColumn()
        {
            Id = Guid.NewGuid().ToString();
            Cards = new ObservableCollection<TaskCard>();
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
        public ObservableCollection<TaskCard> Cards { get; set; }

        [IgnoreDataMember]
        public string NewCardTitle
        {
            get => newCardTitle;
            set => SetProperty(ref newCardTitle, value);
        }
    }
}
