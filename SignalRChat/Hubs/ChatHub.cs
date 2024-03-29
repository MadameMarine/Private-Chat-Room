﻿using Microsoft.AspNet.SignalR;
using SignalRChat.url_friendly;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SignalRChat
{

    public class Session
    {
        public string AdminConnectionId { get; set; } 
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
            Session _returnValue = null;
            try
            {
                _returnValue = _stockSession[id];
            }
            catch (InvalidCastException e)
            {
                Console.WriteLine(e);
            }

            return _returnValue;
        }

        public string GetAdminConnexionId(string sessionId)
        {
            return GetSession(sessionId).AdminConnectionId;
        }

        public void UpdateCurrentActivity(string sessionId, string newActivity)
        {
            //Replace current activity by newActivity
            GetSession(sessionId).CurrentActivity = newActivity;           
        }

        public Session CreateSession(string suggestedFriendlyId, string adminConnectionId)
        {
            //set up id friendly
            var idStringHelper = StringHelper.URLFriendly(suggestedFriendlyId);
            var idFriend = Regex.Replace(idStringHelper, @"[^A-Za-z0-9'()\*\\+_~\:\/\?\-\.,;=#\[\]@!$&]", "");
            var idFriendly = Regex.Replace(idFriend, @"-", "");
            var res = new Session
            {
                AdminConnectionId = adminConnectionId,
                PublicUrl = "http://localhost:52527/Home/Chat/" + idFriendly,
                Id = idFriendly
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

            public string ParticipantId { get; set; }
        }

        public async Task SendNote(string sessionId, string name, string message, string participantId)
        {
            //Call the addNewMessageToPage method to send message/notes  to compositeur
            var adminConnectionId = SessionService.Instance.GetSession(sessionId).AdminConnectionId;
            var maestroClient = Clients.Client(adminConnectionId);
            await maestroClient.sendingNote(new ChatMessage() { Name = name, Message = message, ParticipantId = participantId });
           
        }


        public async Task StartActivity(string sessionId, string newActivity) 
        {
            //maj de Session dans sessionService: futurs join session s'initialisent avec activité courante     
            SessionService.Instance.UpdateCurrentActivity(sessionId, newActivity);            

            //appelle les clients pour démarrrer la nouvelle activité newActivity
            await Clients.Group(sessionId).startingActivity(newActivity);
           
        }

        public async Task StopActivity(string sessionId, string newActivity)
        {
            //maj de Session dans sessionService: futurs join session s'initialisent avec activité courante     
            SessionService.Instance.UpdateCurrentActivity(sessionId, newActivity);

            //appelle les clients pour stoppper l'activité en cours
            await Clients.Group(sessionId).stoppingActivity(newActivity);

        }

        public Session JoinSession(string sessionId) 
        {
             
            this.Groups.Add(this.Context.ConnectionId, sessionId);
            return SessionService.Instance.GetSession(sessionId);

            
        }
        

   
        public Session CreateSession(string suggestedId, string maestroConnexionId)
        {

            var adminId = Context.ConnectionId;

            var res =  SessionService.Instance.CreateSession(suggestedId, adminId);
            this.Groups.Add(this.Context.ConnectionId, res.Id);
            return res;
        }
    }
}