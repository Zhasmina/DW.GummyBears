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
                    ModelState.AddModelError("ServerError", "Login failed. Wrong username or password.");
                    return View(credentials);
                }

                return RedirectToAction("GetFeeds",
                    new
                    {
                        token = response.Payload.Token,
                        userId = response.Payload.UserId,
                        username = response.Payload.Username
                    });
            }
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Logout(string token)
        {
            await _gummyBearClient.Logout(new AuthenticationTokenRequest
            {
                AuthenticationToken = token,
                CorrelationToken = Guid.NewGuid().ToString()
            });

            return RedirectToAction("Index");
        }
        #endregion

        #region profile
        public ActionResult EditUserProfile(UserProfile userProfile)
        {
            return View(userProfile);
        }

        [HttpGet]
        public async Task<ActionResult> GetUserProfile(string token, int userId, string username)
        {
            Response<UserProfile> response = await _gummyBearClient.GetUserByIdAsync(new UserProfileRequest()
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
                UserId = response.Payload.Id,
                Username = username
            };
            return View("EditProfile", tokenResponse);
        }

        [HttpGet]
        public async Task<ActionResult> GetOtherUserProfile(string token, int userId, string username, int targetUserId)
        {
           Response<UserProfile> response = await _gummyBearClient.GetUserByIdAsync(new UserProfileRequest
            {
                AuthenticationToken = token,
                CorrelationToken = Guid.NewGuid().ToString(),
                UserId = targetUserId
            });

            if (response.Status == Status.Failed)
            {
                ModelState.AddModelError("ServerError", "Getting profile failed.");
                return RedirectToAction("GetFeeds", new { token = token, userId = userId, username = username });
            }

            return View(new AuthenticatedOtherProfileModel
            {
                AuthenticationToken = token,
                MyUserId = userId,
                MyUsername = username,
                TargerFirstName = response.Payload.FirstName,
                TargetLastName = response.Payload.LastName,
                TargetUserId = response.Payload.Id,
                TargetProfilePath = response.Payload.ProfilePicturePath,
                TargetDateOfBirth = response.Payload.DateOfBirth,
                TargetDescription = response.Payload.Description,
                TargetCountry = response.Payload.Country
            });
        }
        #endregion

        #region creations
        [HttpGet]
        public async Task<ActionResult> GetUserCreations(string token, int userId, string username)
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
                UserId = userId,
                Username = username
            };

            return View("MyCreations", model: tokenResponse);
        }

        [HttpGet]
        public ActionResult AddUserCreations(string token, int userId, string username)
        {
            return View(model: new AuthenticatedUserModel() { Token = token, UserId = userId, Username = username });
        }

        [HttpPost]
        public async Task<ActionResult> AddUserCreations(string token, string creationName, int userId, string username, HttpPostedFileBase file)
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
                    ModelState.AddModelError("ServerError", "Saving creation failed. Please, try again later");
                    return View("AddUserCreations", model: new AuthenticatedUserModel() { Token = token, UserId = userId, Username = username });
                }
            }

            return RedirectToAction("GetUserCreations", new { token = token, userId = userId, username = username });
        }

        public async Task<ActionResult> DeleteCreation(string token, int creationId, int userId, string username)
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

            return RedirectToAction("GetUserCreations", new { token = token, userId = userId, username = username });
        }

        public FileResult DownloadCreation(string token, int userId, string username, string filePath)
        {
            byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet);
        }

        #endregion

        #region feed
        public async Task<ActionResult> GetFeeds(string token, int userId, string username)
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
                Username = username,
                CurrentPage = response.Payload.CurrentPage,
                Items = response.Payload.Items,
                ItemsPerPage = response.Payload.ItemsPerPage,
                TotalItems = response.Payload.TotalItems,
                TotalPages = response.Payload.TotalPages
            });
        }

        [HttpGet]
        public ActionResult CreatePostInFeed(string token, int userId, string username)
        {
            return View(new AuthenticatedFeedModel
            {
                AuthenticationToken = token,
                UserId = userId,
                Username = username
            });
        }
        [HttpPost]
        public async Task<ActionResult> CreatePostInFeed(AuthenticatedFeedModel authenticatedFeedModel)
        {
            await _gummyBearClient.PostToFeed(new AuthenticatedFeedRequest()
            {
                AuthenticationToken = authenticatedFeedModel.AuthenticationToken,
                CorrelationToken = Guid.NewGuid().ToString(),
                Payload = new Feed
                {
                    AuthorId = authenticatedFeedModel.UserId,
                    Text = authenticatedFeedModel.MessageText
                }
            }).ConfigureAwait(false);

            return RedirectToAction("GetFeeds",
                    new
                    {
                        token = authenticatedFeedModel.AuthenticationToken,
                        userId = authenticatedFeedModel.UserId,
                        username = authenticatedFeedModel.Username
                    });
        }
        #endregion

        #region groups
        [HttpGet]
        public async Task<ActionResult> GetGroups(string token, int userId, string username)
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
                Groups = response.Payload,
                Username = username
            });
        }

        [HttpGet]
        public ActionResult CreateGroup(string token, int userId, string username)
        {
            return View(new AuthenticatedGroupModel()
            {
                AuthenticationToken = token,
                UserId = userId,
                Username = username
            });
        }

        [HttpPost]
        public async Task<ActionResult> CreateGroup(AuthenticatedGroupModel model)
        {
            if (ModelState.IsValid)
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
                    ModelState.AddModelError("ServerError", string.Format("Creation of group failed. {0}", string.Join(" ,", response.Errors)));

                    return RedirectToAction("CreateGroup", new { token = model.AuthenticationToken, userId = model.UserId, username = model.Username });
                }

                return RedirectToAction("GetGroups", new { token = model.AuthenticationToken, userId = model.UserId, username = model.Username });
            }

            ModelState.AddModelError("ServerError", "Creation of group failed.");

            return RedirectToAction("CreateGroup", new { token = model.AuthenticationToken, userId = model.UserId, username = model.Username });

        }

        [HttpGet]
        public async Task<ActionResult> GetMessagesInGroup(string token, int userId, int groupId, string username, string groupName)
        {
            Response<IEnumerable<GroupMessage>> response = await _gummyBearClient.GetMessagesInGroup(new GroupMessagesRequest()
            {
                AuthenticationToken = token,
                CorrelationToken = Guid.NewGuid().ToString(),
                GroupId = groupId,
                UserId = userId
            }).ConfigureAwait(false);

            if (response.Status == Status.Failed)
            {
                ModelState.AddModelError("ServerError", string.Format("Create message failed. {0}", string.Join(" ,", response.Errors)));
                return RedirectToAction("GetGroups", new { token = token, userId = userId, username = username });
            }

            return View(new AuthenticatedGroupMessagesModel()
            {
                AuthenticationToken = token,
                UserId = userId,
                GroupMessages = response.Payload,
                Username = username,
                GroupId = groupId,
                GroupName = groupName
            });
        }

        [HttpGet]
        public async Task<ActionResult> GetGroupParticipants(string token, int userId, int groupId, string username, string groupName)
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
                ModelState.AddModelError("ServerError", string.Format("Creation of group failed. {0}", string.Join(" ,", groupParticipatsResponse.Errors)));

                return RedirectToAction("GetGroups", new { token = token, userId = userId, username = username });
            }

            return View(new AuthenticatedGroupParticipantsModel
            {
                AuthenticationToken = token,
                GroupId = groupId,
                UserId = userId,
                Username = username,
                GroupName = groupName,
                Participants = groupParticipatsResponse.Payload
                .Select(gp =>
                    new Participant
                    {
                        Id = gp.ParticipantId,
                        Name = gp.ParticipantName
                    }).ToList()
            });
        }

        [HttpGet]
        public ActionResult PostMessageInGroup(string token, int userId, int groupId, string username, string groupName)
        {
            return View(new AuthenticatedGroupMessageModel
            {
                AuthenticationToken = token,
                GroupId = groupId,
                MessageText = string.Empty,
                UserId = userId,
                Username = username,
                GroupName = groupName
            });
        }

        [HttpPost]
        public async Task<ActionResult> PostMessageInGroup(AuthenticatedGroupMessageModel model)
        {
            if (ModelState.IsValid)
            {
                Response<GroupMessage> response = await _gummyBearClient.CreateMessagesInGroup(new CreateGroupMessageRequest
                {
                    UserId = model.UserId,
                    AuthenticationToken = model.AuthenticationToken,
                    CorrelationToken = Guid.NewGuid().ToString(),
                    GroupId = model.GroupId,
                    Text = model.MessageText,
                    Payload = new GroupMessage
                    {
                        AuthorName = model.Username,
                        Username = model.Username,
                        GroupId = model.GroupId,
                        SendDate = DateTime.Now,
                        Message = model.MessageText,
                        UserId = model.UserId
                    }
                });

                if (response.Status == Status.Failed)
                {
                    ModelState.AddModelError("ServerError", string.Format("Create message failed. {0}", string.Join(" ,", response.Errors)));
                    return View(model);
                }

                return RedirectToAction("GetMessagesInGroup", new { token = model.AuthenticationToken, userId = model.UserId, groupId = model.GroupId, username = model.Username, groupName = model.GroupName });
            }

            ModelState.AddModelError("ServerError", "Create message failed.");
            return View(model);
        }

        [HttpGet]
        public ActionResult AddParticipant(string token, int userId, int groupId, string username, string groupName)
        {
            return View(new AuthenticatedGroupParticipantsModel
            {
                AuthenticationToken = token,
                GroupId = groupId,
                UserId = userId,
                Username = username,
                GroupName = groupName
            });
        }

        [HttpPost]
        public async Task<ActionResult> AddParticipant(AuthenticatedGroupParticipantsModel model)
        {
            await _gummyBearClient.AddParticipantsInGroup(new AuthenticatedGroupParticipantsRequest
            {
                AuthenticationToken = model.AuthenticationToken,
                CorrelationToken = Guid.NewGuid().ToString(),
                Payload = model.Participants.Select(p => p.Id).ToList(),
                GroupId = model.GroupId
            });

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<ActionResult> GetCreationsInGroup(string token, int userId, int groupId, string username, string groupName)
        {
            Response<IEnumerable<Creation>> response =
                await _gummyBearClient.GetAttachedFilesInGroup(new AuthenticatedGroupCreationsRequest
                {
                    AuthenticationToken = token,
                    CorrelationToken = Guid.NewGuid().ToString(),
                    UserId = userId,
                    Payload = new GroupCreation
                    {
                        GroupId = groupId
                    }
                });

            if (response.Status == Status.Failed)
            {
                ModelState.AddModelError("ServerError", string.Format("Getting creation in group failed. {0}", string.Join(" ,", response.Errors)));
                return RedirectToAction("GetGroups", new { token = token, userId = userId, username = username });
            }

            return View(new AuthenticatedGroupCreationsModel
            {
                AuthenticationToken = token,
                GroupId = groupId,
                GroupName = groupName,
                UserId = userId, 
                Username = username,
                Creations = response.Payload.ToList()
            });
        }

        [HttpGet]
        public async Task<ActionResult> AddCreationToGroup(string token, int userId, int groupId, string username, string groupName)
        {
           var response = await _gummyBearClient.GetUserCreations(new UserProfileRequest
            {
               AuthenticationToken = token,
               CorrelationToken = Guid.NewGuid().ToString(),
               UserId = userId,
               Username = username
            });

            if(response.Status == Status.Failed)
            {
                ModelState.AddModelError("ServerError", string.Format("Getting user creations failed. {0}", string.Join(" ,", response.Errors)));
                return RedirectToAction("GetGroups", new { token = token, userId = userId, username = username });
            }

            return View(new AuthenticatedGroupCreationsModel
            {
                AuthenticationToken = token,
                Creations = response.Payload.ToList(),
                GroupId = groupId,
                GroupName = groupName,
                UserId = userId,
                Username = username
            });
        }

        [HttpPost]
        public async Task<ActionResult> AddCreationToGroup(AuthenticatedGroupCreationsModel model)
        {
            Response<GroupCreation> response = await _gummyBearClient.AttatchFileToGroup(
                new AuthenticatedGroupCreationsRequest
                {
                AuthenticationToken = model.AuthenticationToken,
                CorrelationToken = Guid.NewGuid().ToString(),
                UserId = model.UserId,
                Payload = new GroupCreation
                {
                    CreationId = model.CreationId,
                    GroupId = model.GroupId,
                }
            });

            if(response.Status == Status.Failed)
            {
                ModelState.AddModelError("ServerError", string.Format("Getting user creations failed. {0}", string.Join(" ,", response.Errors)));
                return View("AddCreationToGroup", model);
            }

            return RedirectToAction("GetCreationsInGroup",
                new
                {
                    token = model.AuthenticationToken,
                    userId = model.UserId,
                    groupId = model.GroupId,
                    username = model.Username,
                    groupName = model.GroupName
                });
        } 
        #endregion
    }
}