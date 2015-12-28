using GummyBears.Clients;
using GummyBears.Clients.Requests;
using GummyBears.Clients.Responses;
using GummyBears.Contracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using GummyBears.Web.Models;
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

                return RedirectToAction("GetUserProfile", new { token = response.Payload.Token, userId = response.Payload.UserId });
            }
            return View();
        }

        public ActionResult EditUserProfile(UserProfile userProfile)
        {
            return View(userProfile);
        }
        
        [HttpGet]
        public ActionResult Logout()
        {
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> GetUserProfile(string token, int userId)
        {
            Response<UserProfile> response = await _gummyBearClient.GetUserAsync(new UserProfileRequest()
             {
                 AuthenticationToken = token,
                 CorrelationToken = new Guid().ToString(),
                 UserId = userId
             }).ConfigureAwait(false);

            if (response.Status == Status.Failed)
            {
                return RedirectToAction("Login");
            }
            GummyBears.Web.Models.TokenResponse<UserProfile> tokenResponse = new TokenResponse<UserProfile>()
            {
                Payload = response.Payload,
                Token = token,
                UserId = response.Payload.Id
            };
            return View("EditProfile", tokenResponse);
        }

        [HttpGet]
        public async Task<ActionResult> GetUserCreations(string token, int userId)
        {
            Response<IEnumerable<Creation>> response = await _gummyBearClient.GetAllUserCreations(new UserProfileRequest()
            {
                AuthenticationToken = token,
                UserId = userId,
                CorrelationToken = new Guid().ToString()
            }).ConfigureAwait(false);

            if (response.Status == Status.Failed)
            {
                return RedirectToAction("Index");
            }

            TokenResponse<IEnumerable<Creation>> tokenResponse = new TokenResponse<IEnumerable<Creation>>()
            {
                Payload = response.Payload,
                Token = token,
                UserId = userId
            };

            return View("MyCreations", model: tokenResponse);
        }

        [HttpGet]
        public ActionResult AddUserCreations(string token, int userId)
        {
            return View(model: new AuthenticatedUserModel() { Token = token, UserId = userId });
        }

        [HttpPost]
        public async Task<ActionResult> AddUserCreations(string token, string creationName, int userId, HttpPostedFileBase file)
        {
            if (file.ContentLength > 0)
            {
                var fileName = Path.GetFileName(file.FileName);
                var path = Path.Combine(Server.MapPath("~/App_Data/Uploads"), fileName);
                file.SaveAs(path);

                var response = await _gummyBearClient.CreateUserCreations(new AuthenticatedCreationRequest()
                 {
                     AuthenticationToken = token,
                     CorrelationToken = new Guid().ToString(),
                     Payload = new Creation()
                     {
                         CreationPath = path,
                         CreationName = creationName,
                         UserId = userId
                     }
                 }).ConfigureAwait(false);

                if (response.Status == Status.Failed)
                {
                    return RedirectToAction("Index");
                }
            }

            return RedirectToAction("GetUserCreations", new { token = token, userId = userId });
        }
   
        public async Task<ActionResult> DeleteCreation(string token, int creationId, int userId)
        {
           var response = await _gummyBearClient.DeleteCreation(new AuthenticatedCreationRequest()
            {
                AuthenticationToken = token,
                CorrelationToken = new Guid().ToString(),
                Payload = new Creation
                {
                    CreationId = creationId,
                    UserId = userId
                }
            }).ConfigureAwait(false);

           return RedirectToAction("GetUserCreations", new { token = token, userId = userId });
        }

        [HttpGet]
        public async Task<ActionResult> GetFeeds(string token, int userId)
        {
            var response = await _gummyBearClient.GetFeeds(new PagedRequest() 
            { 
                CorrelationToken = new Guid().ToString(),
                AuthenticationToken = token
            }).ConfigureAwait(false);

            if (response.Status == Status.Failed)
            {
                RedirectToAction("Index");
            }

            return View(new AuthenticatedFeedsPageModel 
            {
                AuthenticationToken = token,
                UserId = userId,
                CurrentPage = response.Payload.CurrentPage,
                Items = response.Payload.Items,
                ItemsPerPage = response.Payload.ItemsPerPage,
                TotalItems = response.Payload.TotalItems,
                TotalPages = response.Payload.TotalPages
            });
        }

        [HttpGet]
        public ActionResult CreatePostInFeed(string token, int userId)
        {
            return View(new AuthenticatedFeedModel()
            {
                AuthenticationToken = token, 
                UserId = userId 
            });
        }
        [HttpPost]
        public async Task<ActionResult> CreatePostInFeed(AuthenticatedFeedModel authenticatedFeedModel)
        {
            await _gummyBearClient.PostToFeed(new AuthenticatedFeedRequest()
            {
                AuthenticationToken = authenticatedFeedModel.AuthenticationToken,
                CorrelationToken = new Guid().ToString(),
                Payload = new Feed()
                {
                    AuthorId = authenticatedFeedModel.UserId,
                    Text = authenticatedFeedModel.MessageText
                }
            }).ConfigureAwait(false);

            return RedirectToAction("Index");
        }
    }
}