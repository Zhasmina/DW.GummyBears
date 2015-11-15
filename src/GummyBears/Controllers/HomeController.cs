using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GummyBears.Clients;
using GummyBears.Contracts;
using GummyBears.Clients.Responses;
using System.Threading.Tasks;

namespace GummyBears.Controllers
{
    public class HomeController : Controller
    {
        public IGummyBearClient _gummyBearClient;

        public HomeController(IGummyBearClient gummyBearClient)
        {
            _gummyBearClient = gummyBearClient;
        }

        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> Register(User userProfile)
        {
            if (ModelState.IsValid)
            {
                Response<User> response = await _gummyBearClient.CreateUserAsync(new Clients.Requests.UserRequest()
                {
                    CorrelationToken = new Guid().ToString(),
                    Payload = userProfile
                }).ConfigureAwait(false);

                if (response.Status == Status.Failed)
                {
                    //Notify for error while creating user profile
                    return View(userProfile);
                }
                // Notify for success
                return View("Index");

            }

            return View(userProfile);
        }

        public ActionResult Login()
        {
            return View();
        }

        public ActionResult Logout()
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
    }
}