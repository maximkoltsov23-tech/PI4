using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace PI4.Models
{
    [DataContract]
    public class BoardState
    {
        public BoardState()
        {
            Columns = new ObservableCollection<TaskColumn>();
        }

        [DataMember]
        public ObservableCollection<TaskColumn> Columns { get; set; }
    }
}
