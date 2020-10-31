using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication6.Api.Models;
using WebApplication6.Core;
using WebApplication6.Core.Enums;
using WebApplication6.Core.Models;

namespace WebApplication6.Api
{
    [ApiController]
    [Route("Api/[controller]")]
    public class SendMessage : ControllerBase
    {
        public Chat Chat => Program.Chat;

        [HttpPost]
        public ActionResult<ResponseObject<Message>> Post(ReqSendMessage request)
        {
            if (request == null)
                return BadRequest();

            if (request.UserGuid == null)
                return BadRequest();

            var matchUser = Chat.Users.FirstOrDefault(x => x.Guid == request.UserGuid);
            if (matchUser == null)
                return new ObjectResult(new ResponseBase(401, "Не удалось найти пользователя по указанному токенту доступа"));

            // OK
            var msg = new Message
            {
                Id = Chat.Messages.Count,
                Text = request.Message,
                UserId = matchUser.Id,
                Author = matchUser,
                Date = DateTime.Now,
            };

            // Добавление сообщения
            Chat.Messages.Add(msg);

            // Сообщаем (почти) всем клиентам лонг пулинга что есть данные
            Chat.SetUpdate(msg, UpdateTypes.Add, matchUser);

            return new ObjectResult(new ResponseObject<Message>(msg));
            //return new ResponseBase();
        }


    }
}
