using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication6.Core.Models
{
    public class Updates
    {
        public int NewStateId { get; set; }
        public List<Message> NewMessages { get; set; }
        public List<Message> EditMessages { get; set; }
        public List<User> NewUsers { get; set; }
        public List<User> EditUsers { get; set; }
        public List<User> RemoveUsers { get; set; }
        public List<int> RemoveMessages { get; set; }
    }
}
