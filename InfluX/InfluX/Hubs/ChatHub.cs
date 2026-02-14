using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace InfluX.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
        // إجرائية تحدد ماذا سترسل و من سيرسل و إلى من سترسل 
        public async Task SendToAll(string Message)
        {
            var Sender = Context.User?.Identity?.Name ?? "UnKown";
            await Clients.All.SendAsync("RecivePublic",Sender, Message);
        }




        //=======================================================================


        public async Task SendPrivet (string ToUserID , string Message)
        {
            var Sender = Context.User?.Identity?.Name ?? "UnKown";
            await Clients.User(ToUserID).SendAsync("RecivePrivet", Sender, Message);
        }
    }
}
