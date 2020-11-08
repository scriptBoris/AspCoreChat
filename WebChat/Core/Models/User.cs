using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebChat.Core.Enums;

namespace WebChat.Core.Models
{
    public class User
    {
        private TaskCompletionSource<UpdateObject> taskUpdate;
        private System.Timers.Timer lastTimer;

        public int Id { get; set; }
        public string Name { get; set; }
        public string Guid { get; set; }
        [System.Text.Json.Serialization.JsonIgnore]
        public string Password { get; set; }
        public UserStatus Status { get; set; }
        public bool IsWaitingForUpdates => taskUpdate != null;


        public void RecreateLastTask()
        {
            if (taskUpdate != null)
                taskUpdate.TrySetException(new OperationCanceledException());

            if (lastTimer != null)
            {
                lastTimer.Stop();
                lastTimer.Close();
                lastTimer = null;
            }

            taskUpdate = new TaskCompletionSource<UpdateObject>();
        }

        public async Task<UpdateObject> WaitingUpdates(int timeoutSeconds)
        {
            lastTimer = new System.Timers.Timer(timeoutSeconds * 1000);
            lastTimer.AutoReset = false;
            lastTimer.Elapsed += Timer_Elapsed;
            lastTimer.Start();

            UpdateObject res = null;
            try
            {
                res = await taskUpdate.Task;
            }
            catch (Exception)
            {
            }

            return res;
        }

        public void SetTask(object updateObject, UpdateTypes type)
        {
            if (lastTimer != null)
            {
                lastTimer.Stop();
                lastTimer.Close();
                lastTimer = null;
            }

            taskUpdate?.TrySetResult(new UpdateObject(updateObject, type));
            taskUpdate = null;
        }

        private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (lastTimer != null)
            {
                lastTimer.Stop();
                lastTimer.Close();
                lastTimer = null;
            }
            taskUpdate?.TrySetCanceled();
            taskUpdate = null;
        }
    }
}
