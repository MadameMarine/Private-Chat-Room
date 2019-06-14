using System;
using System.Web;
using Microsoft.AspNet.SignalR;
namespace SignalRChat
{
    public class ChatHub : Hub
    {
        //Rajout de string groupChatId qu'on récupère dans la View Chat
        public void Send(string groupChatId, string name, string message)
        {
            // Call the addNewMessageToPage method to update clients.
            Clients.All.addNewMessageToPage(groupChatId, name, message);
        }
    }
}