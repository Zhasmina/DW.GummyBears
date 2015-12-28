using GummyBears.Contracts;
using GummyBears.Entities;
using GummyBears.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using GummyBears.WebApi.Helpers;
using GummyBears.CreationRightsManager;
using System.IO;

namespace GummyBears.WebApi.Controllers
{
    [System.Web.Http.RoutePrefix("users")]
    public class CreationsController : BaseController
    {
        private readonly Manager _creationRightsManager;
        //create record for creation
        //get user's records ... paths :)
        public CreationsController(IDbContext dbContext, Manager creationRightsManager)
            : base(dbContext)
        {
            _creationRightsManager = creationRightsManager;
        }

        [HttpGet, Route("{userId:int}/creations")]
        [AuthenticationTokenFilter]
        public async Task<List<Contracts.Creation>> GetAllUserCreations([FromUri]int userId)
        {
            UserEntity user = await DbContext.UsersRepo.GetSingleOrDefaultAsync(userId);
            if (user == null)
            {
                ThrowHttpResponseException(HttpStatusCode.NotFound, string.Format("User with id '{0}' not found.", userId));
            }

            IEnumerable<CreationEntity> creations = await DbContext.CreationsRepo.GetUserCreations(userId);

            return creations.Select(c => c.ToContract()).ToList();
        }

        [HttpPost, Route("{userId:int}/creations")]
        [AuthenticationTokenFilter]
        public async Task<Contracts.Creation> AddCreation(Contracts.Creation creation)
        {
            UserEntity author = await DbContext.UsersRepo.GetSingleOrDefaultAsync(creation.UserId);
            var authorString = string.Format("{0} {1}, a.k.a {2}", author.FirstName, author.LastName, author.UserName);
            var fileData = File.ReadAllBytes(creation.CreationPath);
            var rightCreation = new CreationRightsManager.Creation()
            {
                Author = authorString,
                Owner = authorString,
                TimeOfCreation = DateTime.UtcNow,
                Data = new MemoryStream(fileData)
            };

            CreationCertificateData cert = _creationRightsManager.Register(rightCreation);
            creation.Footprint = cert.CreationFootprint;
            creation.Signature = cert.Signature;

            var savedCreation = await DbContext.CreationsRepo.CreateAsync(creation.ToEntity());
            creation.CreationId = savedCreation.Id;

            return creation;
        }

        [HttpDelete, Route("{userId:int}/creations/{creationId:int}")]
        [AuthenticationTokenFilter]
        public async Task<EmptyResponse> DeleteCreation(int userId, int creationId)
        {
            UserEntity user = await DbContext.UsersRepo.GetSingleOrDefaultAsync(userId);

            if (user == null)
            {
                ThrowHttpResponseException(HttpStatusCode.NotFound, string.Format("User with id '{0}' not found.", userId));
            }

            var creation = await DbContext.CreationsRepo.GetSingleOrDefaultAsync(creationId).ConfigureAwait(false);

            if (creation == null)
            {
                ThrowHttpResponseException(HttpStatusCode.NotFound, string.Format("Creation with id '{0}' not found.", creationId));
            }

            await DbContext.CreationsRepo.DeleteAsync(creationId).ConfigureAwait(false);

            return new EmptyResponse();
        }
    }
}