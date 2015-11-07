using GummyBears.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GummyBears.WebApi.Controllers
{
    public class FeedsController : BaseController
    {
        //Endpoints to manage messages in feed
        public FeedsController(IDbContext dbContext)
            : base(dbContext)
        {
        }

    }
}