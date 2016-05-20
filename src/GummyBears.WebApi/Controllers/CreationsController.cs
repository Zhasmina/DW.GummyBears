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
            IEnumerable<CreationEntity> creations = await DbContext.CreationsRepo.GetUserCreations(userId);

            return creations.Select(c => c.ToContract()).ToList();
        }

        [HttpPost, Route("{userId:int}/creations")]
        [AuthenticationTokenFilter]
        public async Task<Contracts.Creation> AddCreation([FromUri]int userId, [FromBody]Contracts.Creation creation)
        {
            ValidateUserAsAuthenticated(userId);
            if (userId != creation.UserId)
            {
                ThrowHttpResponseException(HttpStatusCode.Unauthorized, "Cannot add creation for different user");
            }

            UserEntity author = await DbContext.UsersRepo.GetSingleOrDefaultAsync(userId);
            var authorString = string.Format("{0} {1}, a.k.a {2}", author.FirstName, author.LastName, author.UserName);
            var fileData = File.ReadAllBytes(creation.CreationPath);
            var rightCreation = new CreationRightsManager.Creation
            {
                Author = authorString,
                Owner = authorString,
                TimeOfCreation = DateTime.UtcNow,
                Data = new MemoryStream(fileData)
            };
            creation.Author = creation.Owner = authorString;

            CreationCertificateData cert = _creationRightsManager.Register(rightCreation);
            creation.Footprint = cert.CreationFootprint;
            creation.Signature = cert.Signature;

            var entity = creation.ToEntity();
            entity.TimeOfCreation = cert.TimeOfCreation;
            var savedCreation = await DbContext.CreationsRepo.CreateAsync(entity);
            creation.CreationId = savedCreation.Id;

            return creation;
        }

        [HttpDelete, Route("{userId:int}/creations/{creationId:int}")]
        [AuthenticationTokenFilter]
        public async Task<EmptyResponse> DeleteCreation(int userId, int creationId)
        {
            ValidateUserAsAuthenticated(userId);
            await ValidateCreation(creationId, userId);

            await DbContext.CreationsRepo.DeleteAsync(creationId).ConfigureAwait(false);

            return new EmptyResponse();
        }

        [HttpPut, Route("{userId:int}/creations/{creationId:int}/owner")]
        [AuthenticationTokenFilter]
        public async Task<Contracts.Creation> ChangeOwner([FromUri]int userId, [FromUri]int creationId, [FromBody]int newOwnerId)
        {
            ValidateUserAsAuthenticated(userId);
            CreationEntity entity = await ValidateCreation(creationId, userId);
            UserEntity newOwner = await DbContext.UsersRepo.GetSingleOrDefaultAsync(newOwnerId);
            var newOwnerString = string.Format("{0} {1}, a.k.a {2}", newOwner.FirstName, newOwner.LastName, newOwner.UserName);
            Contracts.Creation contractFromEntity = entity.ToContract();
            contractFromEntity.Owner = newOwnerString;

            var fileData = File.ReadAllBytes(entity.FilePath);
            var rightCreation = new CreationRightsManager.Creation
            {
                Author = contractFromEntity.Author,
                Owner = contractFromEntity.Owner,
                TimeOfCreation = entity.TimeOfCreation,
                Data = new MemoryStream(fileData)
            };
            CreationCertificateData cert = _creationRightsManager.Register(rightCreation);
            contractFromEntity.Footprint = cert.CreationFootprint;
            contractFromEntity.Signature = cert.Signature;

            await DbContext.CreationsRepo.UpdateAsync(contractFromEntity.ToEntity());

            return contractFromEntity;
        }

        private async Task<CreationEntity> ValidateCreation(int creationId, int userId)
        {
            var creation = await DbContext.CreationsRepo.GetSingleOrDefaultAsync(creationId).ConfigureAwait(false);

            if (creation == null)
                ThrowHttpResponseException(HttpStatusCode.NotFound, string.Format("Creation with id '{0}' not found.", creationId));

            if (creation.UserId != userId)
                ThrowHttpResponseException(HttpStatusCode.Unauthorized, string.Format("Creation with id {0} is not owned by user with id {1}", creationId, userId));

            return creation;
        }
    }
}