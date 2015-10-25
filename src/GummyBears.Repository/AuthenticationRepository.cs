using Dapperer;
using GummyBears.Entities;
using Dapperer.DbFactories;

namespace GummyBears.Repository
{
    public class AuthenticationRepository : Repository<AuthenticationEntity, string>
    {
        public AuthenticationRepository(IQueryBuilder queryBuilder, SqlDbFactory sqlDbFactory)
            : base(queryBuilder, sqlDbFactory)
        {
        }
    }
}
