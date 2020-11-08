using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebChat.Core;
using WebChat.Models;
using WebChat.Utils;

namespace WebChat.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [Authorize]
        public IActionResult Index()
        {
            int userId = Convert.ToInt32(User.Claims.ToArray()[1].Value);
            var user = Program.Chat.Users.FirstOrDefault(x => x.Id == userId);
            if (user == null)
                return RedirectToAction("Logout", "Auth");

            var vm = new HomeViewModel();
            // Передаем актуальное состояние сообщений
            vm.StateId = Program.Chat.StateMessageId;



            // Передаем список 20-ти последних сообщений
            var msgList = Program.Chat.Messages.TakeLast(20);
            vm.Messages = msgList.ToList();
            foreach (var msg in vm.Messages)
            {
                if (msg.Author == user)
                    msg.IsSelfMessage = true;
                else
                    msg.IsSelfMessage = false;
            }



            // Передаем список всех пользователей чата
            var memberList = Program.Chat.Users.ToList();
            if (memberList.Count > 1)
            {
                memberList.Remove(user);
                memberList.Insert(0, user);
            }
            vm.Members = memberList;


            return View(vm);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
