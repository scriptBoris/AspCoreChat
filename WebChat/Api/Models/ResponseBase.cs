using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;

namespace WebChat.Api.Models
{
    public class ResponseBase
    {
        public int Code { get; set; }
        public string Description { get; set; }

        public ResponseBase()
        {
        }

        public ResponseBase(int code, string desc)
        {
            Code = code;
            Description = desc;
        }
    }
}
