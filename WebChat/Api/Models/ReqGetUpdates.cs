﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebChat.Api.Models
{
    public class ReqGetUpdates
    {
        public string UserGuid { get; set; }
        public int StateId { get; set; }
    }
}
