using Dapperer;
using Dapperer.DbFactories;
using GummyBears.Entities;

namespace GummyBears.Repository
{
   public class GroupMessagesRepository : Repository<GroupMessageEntity, int>
    {
       public GroupMessagesRepository(IQueryBuilder queryBuilder, SqlDbFactory sqlDbFactory)
            : base(queryBuilder, sqlDbFactory)
        {
        }
    }
}
