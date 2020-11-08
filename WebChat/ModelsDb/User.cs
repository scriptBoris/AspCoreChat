using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebChat.ModelsDb.Enums;

namespace WebChat.ModelsDb
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
