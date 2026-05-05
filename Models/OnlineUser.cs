using System.Runtime.Serialization;

namespace PI4.Models
{
    [DataContract]
    public class OnlineUser : NotifyObject
    {
        private string name;
        private string role;

        [DataMember]
        public string Name
        {
            get => name;
            set => SetProperty(ref name, value);
        }

        [DataMember]
        public string Role
        {
            get => role;
            set => SetProperty(ref role, value);
        }
    }
}
