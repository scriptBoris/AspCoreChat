using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication6.Core.Models;

namespace WebApplication6.Models
{
    public class HomeViewModel
    {
        public int StateId { get; set; }
        public List<User> Members { get; set; } = new List<User>();
        public List<Message> Messages { get; set; } = new List<Message>();
    }
}
