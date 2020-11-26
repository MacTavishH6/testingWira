using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Binus.SampleWebAPI.Model.Backend.msBook
{

    [Serializable]
    public class msBookMSSQLOracle
    {
        [Key]
        public int ID { get; set; }
        public string IDEncrypt { get; set; }
        [NotMapped]
        public string Key { get; set; }
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
