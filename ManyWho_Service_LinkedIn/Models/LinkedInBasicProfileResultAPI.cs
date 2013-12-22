using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ManyWho.Service.LinkedIn.Models
{
    [Serializable]
    [DataContract]
    public class LinkedInBasicProfileResultAPI
    {
        [DataMember(Name = "id")]
        public String id
        {
            get;
            set;
        }

        [DataMember(Name = "first-name")]
        public String firstName
        {
            get;
            set;
        }

        [DataMember(Name = "last-name")]
        public String lastName
        {
            get;
            set;
        }

        [DataMember(Name = "email-address")]
        public String emailAddress
        {
            get;
            set;
        }

        [DataMember(Name = "maiden-name")]
        public String maidenName
        {
            get;
            set;
        }

        [DataMember(Name = "formatted-name")]
        public String formattedName
        {
            get;
            set;
        }

        [DataMember(Name = "phonetic-first-name")]
        public String phoneticFirstName
        {
            get;
            set;
        }

        [DataMember(Name = "phonetic-last-name")]
        public String phoneticLastName
        {
            get;
            set;
        }

        [DataMember(Name = "formatted-phonetic-name")]
        public String formattedPhoneticName
        {
            get;
            set;
        }

        [DataMember(Name = "headline")]
        public String headline
        {
            get;
            set;
        }

        [DataMember(Name = "location:(name)")]
        public String locationName
        {
            get;
            set;
        }

        [DataMember(Name = "location:(country:(code))")]
        public String locationCountryCode
        {
            get;
            set;
        }

        [DataMember(Name = "industry")]
        public String industry
        {
            get;
            set;
        }

        [DataMember(Name = "distance")]
        public Int32 distance
        {
            get;
            set;
        }

        [DataMember(Name = "relation-to-viewer:(distance)")]
        public String relationToViewerDistance
        {
            get;
            set;
        }

        [DataMember(Name = "current-share")]
        public String currentShare
        {
            get;
            set;
        }

        [DataMember(Name = "num-connections")]
        public Int32 numConnections
        {
            get;
            set;
        }

        [DataMember(Name = "num-connections-capped")]
        public Boolean numConnectionsCapped
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

        [DataMember(Name = "specialties")]
        public String specialties
        {
            get;
            set;
        }

        [DataMember(Name = "positions")]
        public List<LinkedInPositionResultAPI> positions
        {
            get;
            set;
        }

        [DataMember(Name = "picture-url")]
        public String pictureUrl
        {
            get;
            set;
        }

        [DataMember(Name = "site-standard-profile-request")]
        public String siteStandardProfileRequest
        {
            get;
            set;
        }

        [DataMember(Name = "api-standard-profile-request:(url)")]
        public String apiStandardProfileRequestUrl
        {
            get;
            set;
        }

        [DataMember(Name = "api-standard-profile-request:(headers)")]
        public String apiStandardProfileRequestHeaders
        {
            get;
            set;
        }

        [DataMember(Name = "public-profile-url")]
        public String publicProfileUrl
        {
            get;
            set;
        }
    }
}