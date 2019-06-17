using System;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.SignalR;
namespace SignalRChat
{
    public class ChatHub : Hub
    {
        public async Task Send(string groupChatId, string name, string message) //ou void
        {
            // Call the addNewMessageToPage method to update clients.
            await Clients.All.addNewMessageToPage(groupChatId, name, message);
        }


        //public Task JoinGroup(string group)
        //{
        //    return Groups.AddToGroupAsync(Context.ConnectionId, group);
        //}

        //public Task SendMessageToGroup(string group, string message)
        //{
        //    return Clients.Group(group).SendAsync("ReceiveMessage", message);
        //}
        


    }
}