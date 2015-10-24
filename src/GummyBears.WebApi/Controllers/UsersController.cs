using GummyBears.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;


namespace GummyBears.WebApi.Controllers
{
    [System.Web.Http.RoutePrefix("v1/users")]
    public class UsersController
    {
        private IDbContext _dbContext;
        public UsersController(IDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPost, Route("")]
        public IHttpActionResult CreateUser()
        {
            return null;
        }
    }
}