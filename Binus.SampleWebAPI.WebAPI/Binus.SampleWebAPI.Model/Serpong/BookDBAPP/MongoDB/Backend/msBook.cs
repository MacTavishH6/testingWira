using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using Binus.SampleWebAPI.Model.Base;

namespace Binus.SampleWebAPI.Model.Serpong.BookDBAPP.MongoDB.Backend
{

    [Serializable]
    [BsonIgnoreExtraElements]
    public class msBook
    {
        [JsonConverter(typeof(ObjectIdConverter))]
        public ObjectId _id { get; set; }
        [BsonIgnore]
        public string IDEncrypt { get; set; }
        public string ISBN { get; set; }
        public string BookName { get; set; }
        public string Author { get; set; }
        public string Publisher { get; set; }
        public string Stsrc { get; set; }
        public string UserIn { get; set; }
        public string UserUp { get; set; }
        public DateTime? DateIn { get; set; }
        public DateTime? DateUp { get; set; }
    }

}
