using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebApplication6.Core.Enums;
using WebApplication6.Core.Models;
using WebApplication6.Models;
using WebApplication6.Utils;

namespace WebApplication6.Controllers
{
    public class AuthController : Controller
    {
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel vm)
        {
            if (string.IsNullOrWhiteSpace(vm.Name))
                return this.ViewMessage("Укажите имя пользователя", ViewMessages.MsgTypes.Error);

            if (string.IsNullOrWhiteSpace(vm.Password) || string.IsNullOrWhiteSpace(vm.PasswordAccept))
                return this.ViewMessage("Укажите пароль", ViewMessages.MsgTypes.Error);

            var user = Program.Chat.Users.FirstOrDefault(x => x.Name == vm.Name);
            if (user != null && user.Password != vm.Password)
                return this.ViewMessage("Данное имя пользователя уже занято", ViewMessages.MsgTypes.Error);

            if (vm.Password.Length < 5)
                return this.ViewMessage("Длина пароля не может быть меньше 5 символов", ViewMessages.MsgTypes.Error);

            if (vm.Password != vm.PasswordAccept)
                return this.ViewMessage("Пароли должны совпадать", ViewMessages.MsgTypes.Error);

            if (user != null && user.Password != vm.Password)
                return this.ViewMessage("Неверный пароль", ViewMessages.MsgTypes.Error);

            await Authenticate(vm.Name, vm.Password, user);
            return RedirectToAction("Index", "Home");
        }


        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            int userId = Convert.ToInt32(User.Claims.ToArray()[1].Value);
            var user = Program.Chat.Users.FirstOrDefault(x => x.Id == userId);
            if (user != null)
            {
                user.Status = UserStatus.Offline;
                Program.Chat.SetUpdate(user, UpdateTypes.Edit);
            }

            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("~/");
        }

        private async Task Authenticate(string name, string password, User matchUser)
        {
            int userId;
            string userGuid;

            if (matchUser == null)
            {
                userId = Program.Chat.Users.Count;
                userGuid = Guid.NewGuid().ToString();
            }
            else
            {
                userId = matchUser.Id;
                userGuid = matchUser.Guid;
            }

            if (matchUser == null)
            {
                // добавляем пользователя в коллекцию
                matchUser = new User
                {
                    Id = userId,
                    Name = name,
                    Guid = userGuid,
                    Password = password,
                    Status = UserStatus.Online,
                };
                Program.Chat.Users.Add(matchUser);

                // говорим всем участникам лонг пулинга 
                // что появился еще один пользователь 
                Program.Chat.SetUpdate(matchUser, UpdateTypes.Add, matchUser);
            }
            else
            {
                // говорим всем участникам лонг пулинга 
                // что пользователь стал онлайн
                matchUser.Status = UserStatus.Online;
                Program.Chat.SetUpdate(matchUser, UpdateTypes.Edit, matchUser);
            }


            // создаем один claim
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, name),
                new Claim("USER_ID", userId.ToString()),
                new Claim("USER_GUID", userGuid),
            };

            // создаем объект ClaimsIdentity
            var identity = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);

            // создаем Principal
            var principal = new ClaimsPrincipal(identity);

            // создаем AuthenticationProperties
            var props = new AuthenticationProperties();

            // установка аутентификационных куки
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, props);
        }
    }
}
