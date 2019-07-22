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

        #region ActionControllers

        [HttpGet]
        public JsonResult CreateSession(string id)
        {

            var session = SessionService.Instance.CreateSession(id);
           return Json(session, JsonRequestBehavior.AllowGet);
        }

        #endregion

    }
}