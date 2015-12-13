using GummyBears.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GummyBears.WebApi.Controllers
{
    public class CreationsController : BaseController
    {
        //create record for creation
        //get user's records ... paths :)
        public CreationsController(IDbContext dbContext)
            :base(dbContext)
        {

        }
    }
}