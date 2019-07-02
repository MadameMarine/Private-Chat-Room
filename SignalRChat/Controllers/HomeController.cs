using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Client;
using SignalRChat.url_friendly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace SignalRChat.Controllers
{
    public class HomeController : Controller
    {
       
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }


        public class ChatEditViewModel
        {
            public string Id { get; set; }
        }

    
        public ActionResult Chat(string id)
        {
            return View(new ChatEditViewModel() { Id = id});
        }



        [HttpGet]
        public JsonResult CreateSession(string id)
        {
            var idStringHelper = StringHelper.URLFriendly(id);
            var idFriendly = Regex.Replace(idStringHelper, @"[^A-Za-z0-9'()\*\\+_~\:\/\?\-\.,;=#\[\]@!$&]", "");
            var generatedGroupId = idFriendly;
            
           
            var res = new
            {
                publicUrl = "http://localhost:52527/Home/Chat/"+ generatedGroupId,
                groupId = generatedGroupId
            };

            return Json(res, JsonRequestBehavior.AllowGet);
        }

    }
}