using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebApplication6.Core.Enums;
using WebApplication6.Core.Models;

namespace WebApplication6.Core
{
    public class Chat
    {
        public int StateMessageId { get; private set; }
        public List<User> Users = new List<User>();
        public List<Message> Messages = new List<Message>();


        public void SetUpdate(object updateObject, UpdateTypes updateType, User ignoreUser = null)
        {
            StateMessageId++;

            foreach (var user in Users)
            {
                if (ignoreUser != null && ignoreUser == user)
                    continue;

                if (user.IsWaitingForUpdates)
                    user.SetTask(updateObject, updateType);
            }
        }
    }
}
