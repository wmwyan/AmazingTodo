using AmazingTodo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AmazingTodo.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            AmazingTodoContext db = new AmazingTodoContext();
            db.TodoModels.Count();
            return View();
        }
    }
}
