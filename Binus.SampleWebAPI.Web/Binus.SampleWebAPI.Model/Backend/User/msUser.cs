using System;
using System.ComponentModel.DataAnnotations;

namespace Binus.SampleWebAPI.Model.Backend.User
{

    [Serializable]
    public class msUser
    {
        [Key]
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Nama { get; set; }
        public string Stsrc { get; set; }
        public string UserIn { get; set; }
        public string UserUp { get; set; }
        public DateTime? DateIn { get; set; }
        public DateTime? DateUp { get; set; }
    }

}
