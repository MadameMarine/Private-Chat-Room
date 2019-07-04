using System;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.SignalR;
namespace SignalRChat
{
    public class ChatHub : Hub
    {

        public class ChatMessage
        {
            public string GroupChatId { get; set; }
            public string Name { get; set; }
            public string Message { get; set; }

        }

        public async Task Send(string groupId, string name, string message)
        {
            // Call the addNewMessageToPage method to update clients.
            await Clients.Group(groupId).addNewMessageToPage(new ChatMessage() { Name = name, Message = message });
        }

        //----------------WIP--------------
        public class TakingNotes
        {
            public string Notes { get; set; }
        }

        //envoi autorisation au groupe
        public async Task SendNotes(string GroupChatId, string notes)
        {
            await Clients.Group(GroupChatId).autorizeTakingNotes(new TakingNotes() { Notes = notes});
        }

        ////récupère autorisation au groupe
        //public async Task GetNotesToGroup(string GroupChatId, string notes)
        //{
        //    await Clients.Group(GroupChatId).sendAutorization("autorizeTakingNotes", notes);
        //}
        //----------------WIP--------------


        //ajout utilisateur dans un groupe
        public async Task JoinRoom(string GroupChatId) 
        {
            await Groups.Add(Context.ConnectionId, GroupChatId);
        }

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
    }
}