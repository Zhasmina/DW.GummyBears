using Dapperer;
using Dapperer.DbFactories;
using GummyBears.Entities;

namespace GummyBears.Repository
{
    public class GroupsRepository : Repository<GroupEntity, int>
    {
        public GroupsRepository(IQueryBuilder queryBuilder, SqlDbFactory sqlDbFactory)
            : base(queryBuilder, sqlDbFactory)
        {
        }
    }
}
