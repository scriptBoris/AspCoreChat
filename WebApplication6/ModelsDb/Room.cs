using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication6.ModelsDb
{
    public class Room
    {
        public int Id { get; set; }
        public string Name { get; set; }


        /// <summary>
        /// Создатель комнаты
        /// </summary>
        public User User { get; set; }

        /// <summary>
        /// Создатель комнаты
        /// </summary>
        public int UserId { get; set; }
    }
}
