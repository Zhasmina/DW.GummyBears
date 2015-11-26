using GummyBears.Clients;
using GummyBears.Clients.Requests;
using GummyBears.Clients.Responses;
using GummyBears.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace GummyBears.Web.Controllers
{
    public class HomeController : Controller
    {
        private IGummyBearClient _gummyBearClient;

        public HomeController(IGummyBearClient gummyBearClient)
        {
            _gummyBearClient = gummyBearClient;
        }
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View(new User());
        }

        [HttpPost]
        public async Task<ActionResult> Register(User userProfile)
        {
            if (ModelState.IsValid)
            {
                Response<User> response = await _gummyBearClient.CreateUserAsync(new UserRequest()
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
                return RedirectToAction("Index");
            }

            return View(userProfile);
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View(new Credentials());
        }

        [HttpPost]
        public async Task<ActionResult> Login(Credentials credentials)
        {
            if (ModelState.IsValid)
            {
                var response = await _gummyBearClient.Login(new AuthenticationRequest()
                {
                    CorrelationToken = new Guid().ToString(),
                    Payload = credentials
                }).ConfigureAwait(false);

                if (response.Status == Status.Failed) 
                {
                    return View(credentials);
                }

                return RedirectToAction("EditProfile", new { token = response.Payload.Token, userId = response.Payload.UserId});
            }
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> EditProfile(string token, int userId) 
        {
           var response = await _gummyBearClient.GetUserAsync(new UserProfileRequest()
            {
                AuthenticationToken = token,
                CorrelationToken = new Guid().ToString(),
                UserId = userId
            }).ConfigureAwait(false);

           if (response.Status == Status.Failed)
           {
               return RedirectToAction("Login");
           }

           return View(response.Payload);
        }

        public ActionResult Logout()
        {
            return View();
        }

    }
}