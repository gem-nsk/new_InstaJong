using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization;

namespace Assets.Accounts.Convert
{
    [DataContract]
    public class Counts
    {
        public int media { get; set; }
        public int follows { get; set; }
        public int followed_by { get; set; }
    }
    [DataContract]
    public class Data
    {
        [DataMember(Name = "id")]

        public string id { get; set; }
        [DataMember(Name = "username")]

        public string username { get; set; }
        public string profile_picture { get; set; }
        public string full_name { get; set; }
        public string bio { get; set; }
        public string website { get; set; }
        public bool is_business { get; set; }
        public Counts counts { get; set; }
    }

    [DataContract]
    public class Meta
    {
        public int code { get; set; }
    }

    [DataContract]
    public class RootObject
    {
        [DataMember(Name = "data")]
        public Data data { get; set; }
        public Meta meta { get; set; }

    }
}