using System;
using System.Collections.Generic;

namespace DatingApp_Backend.Models
{
    public partial class Users
    {
        public int IdUser { get; set; }
        public string UserName { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
    }
}
