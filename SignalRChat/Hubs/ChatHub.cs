using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.SignalR;
using SignalRChat.url_friendly;

namespace SignalRChat
{

    public class Session
    {
        public string PublicUrl { get; set; }
        public string Id { get; set; }
        public string CurrentActivity { get; set; }
    }

    public class SessionService
    {
        private readonly Dictionary<string, Session> _stockSession = new Dictionary<string, Session>();
        public static SessionService Instance { get; } = new SessionService();
             

        private SessionService()
        {
        }

        public Session GetSession(string id) 
        {            
            return _stockSession[id];             
        }

        public Session GetCurrentActivity(string currentActivity)
        {
            return _stockSession[currentActivity];
        }


        public Session CreateSession(string suggestedId, string suggestedCurrentActivity)
        {
            var idStringHelper = StringHelper.URLFriendly(suggestedId);
            var idFriend = Regex.Replace(idStringHelper, @"[^A-Za-z0-9'()\*\\+_~\:\/\?\-\.,;=#\[\]@!$&]", "");
            var idFriendly = Regex.Replace(idFriend, @"-", "");
            var res = new Session
            {
                PublicUrl = "http://localhost:52527/Home/Chat/" + idFriendly,
                Id = idFriendly,
                CurrentActivity = suggestedCurrentActivity
            };

            _stockSession[res.Id] = res;
            return res;

        }
    }
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

        public class TakingNotes
        {
            public string Notes { get; set; }
        }

        //envoi autorisation au groupe
        public async Task SendNotes(string sessionId, string notes)
        {
            await Clients.Group(sessionId).autorizeTakingNotes(new TakingNotes { Notes = notes});
        }
     

        public Session JoinSession(string sessionId)
        {
            this.Groups.Add(this.Context.ConnectionId, sessionId);
            return SessionService.Instance.GetSession(sessionId);
        }

        //supprime un utilisateur
        //public Task LeaveRoom(string roomName)
        //{
        //    return Groups.Remove(Context.ConnectionId, roomName);
        //}

    }
}