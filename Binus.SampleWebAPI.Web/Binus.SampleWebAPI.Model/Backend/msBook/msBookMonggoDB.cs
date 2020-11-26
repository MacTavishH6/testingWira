using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using Binus.SampleWebAPI.Model.Base;

namespace Binus.SampleWebAPI.Model.Backend.msBook
{

    [Serializable]
    [BsonIgnoreExtraElements]
    public class msBookMonggoDB
    {
        [JsonConverter(typeof(ObjectIdConverter))]
        public ObjectId _id { get; set; }
        public string IDEncrypt { get; set; }
        public string ISBN { get; set; }
        public string BookName { get; set; }
        public string Author { get; set; }
        public string Publisher { get; set; }
        public string Stsrc { get; set; }
        public string UserIn { get; set; }
        public string UserUp { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        [JsonConverter(typeof(DateConverter))]
        public DateTime? DateIn { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        [JsonConverter(typeof(DateConverter))]
        public DateTime? DateUp { get; set; }
    }

}
