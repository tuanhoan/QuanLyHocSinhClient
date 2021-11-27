using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuanLyHocSinhClient.Models
{
    public class Account
    {
        public Guid Id { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }
        public string Role { get; set; }
    }
}
