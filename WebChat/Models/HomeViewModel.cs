using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebChat.Core.Models;

namespace WebChat.Models
{
    public class HomeViewModel
    {
        public int StateId { get; set; }
        public List<User> Members { get; set; } = new List<User>();
        public List<Message> Messages { get; set; } = new List<Message>();
    }
}
