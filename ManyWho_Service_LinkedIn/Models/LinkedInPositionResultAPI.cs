using System;
using System.Runtime.Serialization;

namespace ManyWho.Service.LinkedIn.Models
{
    [Serializable]
    [DataContract]
    public class LinkedInPositionResultAPI
    {
        [DataMember(Name = "id")]
        public String id
        {
            get;
            set;
        }

        [DataMember(Name = "title")]
        public String title
        {
            get;
            set;
        }

        [DataMember(Name = "summary")]
        public String summary
        {
            get;
            set;
        }

        [DataMember(Name = "start-date")]
        public String startDate
        {
            get;
            set;
        }

        [DataMember(Name = "end-date")]
        public String endDate
        {
            get;
            set;
        }

        [DataMember(Name = "is-current")]
        public Boolean isCurrent
        {
            get;
            set;
        }

        [DataMember(Name = "company")]
        public String company
        {
            get;
            set;
        }
    }
}
