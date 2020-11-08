using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebChat.Core;
using WebChat.Core.Enums;

namespace WebChat.Api
{
    [ApiController]
    [Route("Api/[controller]")]
    public class Logout : ControllerBase
    {
        public Chat Chat => Program.Chat;

        [HttpPost]
        public async Task<ActionResult> Post(string userGuid)
        {
            if (userGuid == null)
                return BadRequest();

            var matchUser = Chat.Users.FirstOrDefault(x => x.Guid == userGuid);
            if (matchUser == null)
                return NotFound();

            Program.Chat.Users.Remove(matchUser);
            Program.Chat.SetUpdate(matchUser, UpdateTypes.Remove);

            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return Ok();
        }
    }
}
