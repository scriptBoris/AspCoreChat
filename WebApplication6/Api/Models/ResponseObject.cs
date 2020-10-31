using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;

namespace WebApplication6.Api.Models
{
    public class ResponseObject<T> : ResponseBase
    {
        /// <summary>
        /// Результат ответа
        /// </summary>
        public T Result { get; set; }

        public ResponseObject()
        {
        }

        public ResponseObject(T result)
        {
            Result = result;
        }

        public ResponseObject(int code, string desc)
        {
            Code = code;
            Description = desc;
        }
    }
}
