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
    public class GroupMessagesRepository : Repository<GroupMessageEntity, int>
    {
        public GroupMessagesRepository(IQueryBuilder queryBuilder, SqlDbFactory sqlDbFactory)
            : base(queryBuilder, sqlDbFactory)
        {
        }

        public async Task<IEnumerable<GroupMessageEntity>> GetGroupMessages(int groupId)
        {
            const string sql = @"
                SELECT * FROM GroupMessages  
                WHERE GroupId = @GroupId";

            using (IDbConnection connection = CreateConnection())
            {
                return await connection.QueryAsync<GroupMessageEntity>(sql, new
                {
                    GroupId = groupId
                }).ConfigureAwait(false);
            }
        }
    }
}
