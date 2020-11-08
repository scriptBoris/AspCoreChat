using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebChat.Models
{
    public class LoginViewModel
    {
        public string Name { get; set; }
        public string Password { get; set; }
        public string PasswordAccept { get; set; }
    }
}
