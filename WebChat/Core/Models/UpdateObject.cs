using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebChat.Core.Enums;

namespace WebChat.Core.Models
{
    public class UpdateObject
    {
        public object Object;
        public UpdateTypes Type;

        public UpdateObject(object obj, UpdateTypes type)
        {
            Object = obj;
            Type = type;
        }

        //public string UserGuid;
        //private CancellationToken Cancel = new CancellationToken();
        //public TaskCompletionSource<T> Source = new TaskCompletionSource<T>();

        //public UpdateTask()
        //{

        //}

        //public async Task<T> Get()
        //{
        //    using (Cancel.Register(() => 
        //    {
        //        Source.TrySetCanceled
        //    }))
        //    {

        //    }
        //}
    }
}
