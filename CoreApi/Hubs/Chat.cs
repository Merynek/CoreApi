using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace CoreApi.Hubs
{
    [Authorize]
    public class Chat : Hub
    {
        public static ConcurrentDictionary<string, MyUserType> MyUsers = new ConcurrentDictionary<string, MyUserType>();

        public override Task OnConnectedAsync()
        {
            MyUsers.TryAdd(Context.ConnectionId, new MyUserType() { UserId = Context.User.FindFirst("ID").Value });
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            MyUserType garbage;

            MyUsers.TryRemove(Context.ConnectionId, out garbage);
            return base.OnDisconnectedAsync(exception);
        }

        public async Task Send(string message)
        {
            // await Clients.All.SendAsync("Send", message);
            foreach (var user in MyUsers)
            {
                if (user.Value.UserId == "2")
                {
                    await Clients.Client(user.Key).SendAsync("Send", message);
                }
            }
        }
    }

    public class MyUserType
    {
        public string UserId { get; set; }
    }
}
