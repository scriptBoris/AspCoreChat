using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication6.ModelsDb
{
    public class Message
    {
        public int Id { get; set; }

        /// <summary>
        /// Текст сообщения
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Дата создания сообщения
        /// </summary>
        public DateTime Date { get; set; }


        /// <summary>
        /// Автор сообщения
        /// </summary>
        public User User { get; set; }

        /// <summary>
        /// ID автора сообщения
        /// </summary>
        public int UserId { get; set; }

    }
}
