using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebChat.ModelsDb;

namespace WebChat.Utils
{
    public static class DbWorker
    {
        public static void InitDb()
        {
            var db = new WebContext();

            // Init DB sqlite
            db.Database.EnsureCreated();
            db.Database.Migrate();
        }

        internal static async Task<User> GetUserByGuid(string userGuid)
        {
            using var db = new WebContext();
            return await db.Users.FirstOrDefaultAsync(x => x.Guid == userGuid);
        }
    }
}
