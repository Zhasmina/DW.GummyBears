using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Dapperer;
using Dapperer.DbFactories;
using Dapperer.QueryBuilders.MsSql;
using GummyBears.Common;
using GummyBears.Entities;
using System.Data;

namespace GummyBears.Repository
{
    public class GroupCreationsRepository : Repository<GroupCreationEntity, int>
    {
        public GroupCreationsRepository(IQueryBuilder queryBuilder, SqlDbFactory sqlDbFactory)
            : base(queryBuilder, sqlDbFactory)
        {
        }

        public async Task<IEnumerable<CreationEntity>> GetGroupCreations(int groupId)
        {
            const string sql = @"
                SELECT c.* FROM GroupCreations gc
                JOIN Creations c ON c.Id = gc.CreationId
                WHERE gc.GroupId = @GroupId";

            using (IDbConnection connection = CreateConnection())
            {
                return await connection.QueryAsync<CreationEntity>(sql, new
                {
                    GroupId = groupId
                }).ConfigureAwait(false);
            }
        }
    }
}
