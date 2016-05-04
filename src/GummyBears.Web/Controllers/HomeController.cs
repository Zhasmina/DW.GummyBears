﻿using GummyBears.Clients;
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

        #region Register
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
                    CorrelationToken = Guid.NewGuid().ToString(),
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
        #endregion

        #region Login/out

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
                    CorrelationToken = Guid.NewGuid().ToString(),
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
        [HttpGet]
        public ActionResult Logout()
        {
            return View();
        }
        #endregion

        #region profile
        public ActionResult EditUserProfile(UserProfile userProfile)
        {
            return View(userProfile);
        }

        [HttpGet]
        public async Task<ActionResult> GetUserProfile(string token, int userId)
        {
            Response<UserProfile> response = await _gummyBearClient.GetUserAsync(new UserProfileRequest()
             {
                 AuthenticationToken = token,
                 CorrelationToken = Guid.NewGuid().ToString(),
                 UserId = userId
             }).ConfigureAwait(false);

            if (response.Status == Status.Failed)
            {
                return RedirectToAction("Login");
            }
            TokenResponse<UserProfile> tokenResponse = new TokenResponse<UserProfile>()
            {
                Payload = response.Payload,
                Token = token,
                UserId = response.Payload.Id
            };
            return View("EditProfile", tokenResponse);
        }

        #endregion

        #region creations
        [HttpGet]
        public async Task<ActionResult> GetUserCreations(string token, int userId)
        {
            Response<IEnumerable<Creation>> response = await _gummyBearClient.GetUserCreations(new UserProfileRequest()
            {
                AuthenticationToken = token,
                UserId = userId,
                CorrelationToken = Guid.NewGuid().ToString()
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
                     CorrelationToken = Guid.NewGuid().ToString(),
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
                 CorrelationToken = Guid.NewGuid().ToString(),
                 Payload = new Creation
                 {
                     CreationId = creationId,
                     UserId = userId
                 }
             }).ConfigureAwait(false);

            return RedirectToAction("GetUserCreations", new { token = token, userId = userId });
        }

        #endregion

        #region feed
        [HttpGet]
        public async Task<ActionResult> GetFeeds(string token, int userId)
        {
            var response = await _gummyBearClient.GetFeeds(new PagedRequest()
            {
                CorrelationToken = Guid.NewGuid().ToString(),
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
                CorrelationToken = Guid.NewGuid().ToString(),
                Payload = new Feed()
                {
                    AuthorId = authenticatedFeedModel.UserId,
                    Text = authenticatedFeedModel.MessageText
                }
            }).ConfigureAwait(false);

            return RedirectToAction("Index");
        }
        #endregion

        [HttpGet]
        public async Task<ActionResult> GetGroups(string token, int userId)
        {
            var response = await _gummyBearClient.GetAllUserGroups(new UserProfileRequest
            {
                AuthenticationToken = token,
                CorrelationToken = Guid.NewGuid().ToString(),
                UserId = userId
            }).ConfigureAwait(false);

            if (response.Status == Status.Failed)
            {
                RedirectToAction("Index");
            }

            return View(new AuthenticatedUserGroupsModel
            {
                AuthenticationToken = token,
                UserId = userId,
                Groups = response.Payload
            });
        }

        [HttpGet]
        public ActionResult CreateGroup(string token, int userId)
        {
            return View(model: new AuthenticatedGroupModel()
            {
                AuthenticationToken = token,
                UserId = userId
            });
        }

        [HttpPost]
        public async Task<ActionResult> CreateGroup(AuthenticatedGroupModel model)
        {
            Response<Group> response = await _gummyBearClient.CreateGroup(new AuthenticatedGroupRequest()
             {
                 AuthenticationToken = model.AuthenticationToken,
                 CorrelationToken = Guid.NewGuid().ToString(),
                 Payload = new Group()
                 {
                     AuthorId = model.UserId,
                     GroupName = model.GroupName
                 }
             }).ConfigureAwait(false);

            if (response.Status == Status.Failed)
            {
                return RedirectToAction("Index");
            }

            return RedirectToAction("GetGroups", new { token = model.AuthenticationToken, userId = model.UserId });
        }

        [HttpGet]
        public async Task<ActionResult> GetMessagesInGroup(string token, int userId, int groupId)
        {
            Response<IEnumerable<GroupMessage>> response = await _gummyBearClient.GetMessagesInGroup(new GroupMessagesRequest()
             {
                 AuthenticationToken = token,
                 CorrelationToken = Guid.NewGuid().ToString(),
                 GroupId = groupId
             }).ConfigureAwait(false);

            if (response.Status == Status.Failed)
            {
                return RedirectToAction("Index");
            }

            return View(new AuthenticatedGroupMessagesModel()
            {
                AuthenticationToken = token,
                UserId = userId,
                GroupMessages = response.Payload
            });
        }

        [HttpGet]
        public async Task<ActionResult> GetGroupParticipants(string token, int userId, int groupId)
        {
            Response<IEnumerable<GroupParticipants>> groupParticipatsResponse = await _gummyBearClient.GetParticipantsInGroup(new AuthenticatedGroupRequest
              {
                  AuthenticationToken = token,
                  CorrelationToken = Guid.NewGuid().ToString(),
                  Payload = new Group
                  {
                      GroupId = groupId
                  }
              });

            if (groupParticipatsResponse.Status == Status.Failed)
            {
                return RedirectToAction("Index");
            }

            return View(new AuthenticatedGroupParticipantsModel
              {
                  AuthenticationToken = token,
                  GroupId = groupId,
                  UserId = userId,
                  ParticipantIds = groupParticipatsResponse.Payload.Select(gp => gp.ParticipantId).ToList()
              });
        }

        [HttpGet]
        public ActionResult AddParticipant(string token, int userId, int groupId)
        {
            return View(new AuthenticatedGroupParticipantsModel
            {
                AuthenticationToken = token,
                GroupId = groupId,
                UserId = userId
            });
        }

        [HttpPost]
        public async Task<ActionResult> AddParticipant(AuthenticatedGroupParticipantsModel model)
        {
            await _gummyBearClient.AddParticipantsInGroup(new AuthenticatedGroupParticipantsRequest
            {
                AuthenticationToken = model.AuthenticationToken,
                CorrelationToken = Guid.NewGuid().ToString(),
                Payload = model.ParticipantIds.ToList(),
                GroupId = model.GroupId
            });
            return RedirectToAction("Index");
        }
    }
}