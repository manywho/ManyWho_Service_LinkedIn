using System;
using System.Runtime.Serialization;

namespace ManyWho.Service.LinkedIn.Models
{
    [Serializable]
    [DataContract]
    public class LinkedInAuthenticationResultAPI
    {
        /// <summary>
        /// This comes back as the number of milliseconds to expiry.
        /// </summary>
        [DataMember]
        public Int32 expires_in
        {
            get;
            set;
        }

        /// <summary>
        /// The actual token we'll need for subsequent calls against the API.
        /// </summary>
        [DataMember]
        public String access_token
        {
            get;
            set;
        }
    }
}