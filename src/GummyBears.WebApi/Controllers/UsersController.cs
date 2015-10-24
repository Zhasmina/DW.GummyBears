using GummyBears.Contracts;
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
        public IHttpActionResult CreateUser(User user)
        {
            return null;
        }

        [HttpPut, Route("{userId:int}")]

        public IHttpActionResult UpdateUser(User user)
        {
            return null;
        }

        [HttpPost, Route("login")]
        public IHttpActionResult Login()
        {
            return null;
        }


    }
}