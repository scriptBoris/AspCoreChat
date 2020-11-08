using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebChat.Api.Models
{
    public class ReqSendMessage
    {
        public string UserGuid { get; set; }
        public string Message { get; set; }
    }
}
