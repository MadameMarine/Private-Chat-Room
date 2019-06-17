using System;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.SignalR;
namespace SignalRChat
{
    public class ChatHub : Hub
    {
        public async Task Send(string name, string message) //ou void
        {
            // Call the addNewMessageToPage method to update clients.
            await Clients.All.addNewMessageToPage(new ChatMessage() { Name = name, Message = message });
        }

        public class ChatMessage
        {
            public string Name { get; set;}
            public string Message { get; set;}
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