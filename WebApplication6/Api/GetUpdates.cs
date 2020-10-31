using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebApplication6.Api.Models;
using WebApplication6.Core;
using WebApplication6.Core.Enums;
using WebApplication6.Core.Models;

namespace WebApplication6.Api
{
    [ApiController]
    [Route("Api/[controller]")]
    public class GetUpdates : ControllerBase
    {
        public Chat Chat => Program.Chat;

        [HttpPost]
        public async Task<ActionResult<ResponseObject<Updates>>> Post(ReqGetUpdates request)
        {
            if (request == null)
                return BadRequest();

            if (request.UserGuid == null)
                return BadRequest();

            var matchUser = Chat.Users.FirstOrDefault(x => x.Guid == request.UserGuid);
            if (matchUser == null)
                return new ActionResult<ResponseObject<Updates>>(new ResponseObject<Updates>(401, "Не удалось найти пользователя по GUID"));


            matchUser.RecreateLastTask();
            var updates = new Updates();
            updates.NewStateId = Chat.StateMessageId;

            if (request.StateId == Chat.StateMessageId)
            {
                var res = await matchUser.WaitingUpdates(50);

                // Обновились сообщения
                if (res?.Object is Message message)
                {
                    if (res.Type == UpdateTypes.Add)
                        updates.NewMessages = new List<Message> { message };
                    else if (res.Type == UpdateTypes.Edit)
                        updates.EditMessages = new List<Message> { message };
                    else if (res.Type == UpdateTypes.Remove)
                        updates.RemoveMessages = new List<int> { message.Id };
                }

                // Обновились пользователи
                if (res?.Object is User user)
                {
                    if (res.Type == UpdateTypes.Add)
                        updates.NewUsers = new List<User> { user };
                    else if (res.Type == UpdateTypes.Remove)
                        updates.RemoveUsers = new List<User> { user };
                    else if (res.Type == UpdateTypes.Edit)
                        updates.EditUsers = new List<User> { user };

                }
            }
            else
            {
                // TODO Сделать получение старых данных
            }

            return new ActionResult<ResponseObject<Updates>>(new ResponseObject<Updates>(updates));
        }

    }
}
