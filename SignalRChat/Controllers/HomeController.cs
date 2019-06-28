using System;
using System.Collections.Generic;
using System.Linq;
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
        public JsonResult CreateSession()
        {
            var res = new
            {
                publicUrl = "http://localhost:52527/Home/Chat/Compositeur"
            };

            return Json(res, JsonRequestBehavior.AllowGet);
        }

    }
}