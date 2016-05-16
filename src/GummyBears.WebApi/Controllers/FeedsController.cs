using GummyBears.Contracts;
using GummyBears.Entities;
using GummyBears.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using System.Web;
using System.Web.Http;
using GummyBears.WebApi.Helpers;
using Dapperer;

namespace GummyBears.WebApi.Controllers
{
    [RoutePrefix("feeds")]
    public class FeedsController : BaseController
    {
        public FeedsController(IDbContext dbContext)
            : base(dbContext)
        {
        }

        [HttpPost]
        [AuthenticationTokenFilter]
        public async Task<Feed> CreateFeed([FromBody]Feed feed)
        {
            int authUserId = ((SimpleIdentity)((SimplePrincipal)ActionContext.RequestContext.Principal).Identity).Id;
            var createdFeed = await DbContext.FeedsRepo.CreateAsync(feed.ToEntity());
            feed.Id = createdFeed.Id;

            return feed;
        }

        [HttpGet]
        [AuthenticationTokenFilter]
        public async Task<FeedsPage> GetFeed(int pageNumber = 0, int pageSize = 10)
        {
            Page<FeedsEntity> page = await DbContext.FeedsRepo.PageAsync(pageNumber * pageSize, pageSize).ConfigureAwait(false);
            var authorsIds = page.Items.Select(i => i.AuthorId).Distinct();
            var authors = await DbContext.UsersRepo.GetByKeysAsync(authorsIds);
            var authorIdToNameMapping = authors.ToDictionary(u => u.Id, u => string.Format("{0} {1}", u.FirstName, u.LastName));

            var result = new FeedsPage
            {
                CurrentPage = page.CurrentPage,
                Items = page.Items.Select(i => new Feed
                {
                    AuthorId = i.AuthorId,
                    Id = i.Id,
                    Text = i.Text,
                    AurhorName = authorIdToNameMapping[i.AuthorId]
                }).ToList(),
                ItemsPerPage = page.ItemsPerPage,
                TotalItems = page.TotalItems,
                TotalPages = page.TotalPages
            };

            return result;
        }

        //[HttpDelete]
        //[AuthenticationTokenFilter]
        //public async Task DeleteFeed(int feedId)
        //{
        //    FeedsEntity entity = await DbContext.FeedsRepo.GetSingleOrDefaultAsync(feedId);
        //    if 
        //}
    }
}