using System;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.SignalR;
namespace SignalRChat
{
    public class ChatHub : Hub
    {
        public async Task Send(string groupId, string name, string message)
        {
            // Call the addNewMessageToPage method to update clients.
            await Clients.Group(groupId).addNewMessageToPage(new ChatMessage() {Name = name, Message = message });
        }

        public class ChatMessage
        {
           public string GroupChatId { get; set; }
            public string Name { get; set;}
            public string Message { get; set;}
        }

        //ajoute utilisateur dans un groupe
        public async Task JoinRoom(string GroupChatId)
        {
            await Groups.Add(Context.ConnectionId, GroupChatId);
            //Clients.Group(GroupChatId).addChatMessage(Context.User.Identity.Name + " joined.");
        }

        //provisoire
        public void JoinGroup(string GroupChatId)
        {
            this.Groups.Add(this.Context.ConnectionId, GroupChatId);
        }

        //supprime un utilisateur
        public Task LeaveRoom(string roomName)
        {
            return Groups.Remove(Context.ConnectionId, roomName);
        }

        //envoi au groupe
        public async Task SendToGroup(string GroupChatId,string name, string message)
        {
            await Clients.Group(GroupChatId).Send("addNewMessageToPage",name, message);
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