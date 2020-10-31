using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication6.ModelsDb.Enums;

namespace WebApplication6.ModelsDb
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Guid { get; set; }
        public DateTime LastActivity { get; set; }
        public int LastDialogId { get; set; }
        public DialogTypes LastDialogType { get; set; }


        public List<Message> Messages { get; set; } 
        public List<Room> Rooms { get; set; }
    }
}
