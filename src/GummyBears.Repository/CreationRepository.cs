using Dapperer;
using Dapperer.DbFactories;
using GummyBears.Entities;

namespace GummyBears.Repository
{
    public class CreationsRepository : Repository<CreationEntity, int>
    {
        public CreationsRepository(IQueryBuilder queryBuilder, SqlDbFactory sqlDbFactory)
            : base(queryBuilder, sqlDbFactory)
        {
        }
    }
}
