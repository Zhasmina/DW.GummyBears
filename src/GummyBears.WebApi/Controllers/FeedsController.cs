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
            var createdFeed = await DbContext.FeedsRepo.CreateAsync(feed.ToEntity());
            feed.Id = createdFeed.Id;

            return feed;
        }

        [HttpGet]
        [AuthenticationTokenFilter]
        public async Task<FeedsPage> GetFeed(int pageNumber = 0, int pageSize = 10)
        {
            Page<FeedsEntity> page = await DbContext.FeedsRepo.PageAsync(pageNumber * pageSize, pageSize).ConfigureAwait(false);
           
            var result = new FeedsPage
            {
                CurrentPage = page.CurrentPage,
                Items = page.Items.Select(i => new Feed
                {
                    AuthorId = i.AuthorId,
                    Id = i.Id,
                    Text = i.Text,
                    
                }).ToList(),
                ItemsPerPage = page.ItemsPerPage,
                TotalItems = page.TotalItems,
                TotalPages = page.TotalPages
            };

            return result;
        }
    }
}