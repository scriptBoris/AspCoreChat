using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication6.Utils
{
    public static class ViewMessages
    {
        public static ViewResult ViewMessage(this Controller controller, string msg, MsgTypes type)
        {
            controller.TempData[type.ToString()] = msg;
            return controller.View();
        }

        public enum MsgTypes
        {
            Message,
            Success,
            Warning,
            Error,
        }
    }
}
