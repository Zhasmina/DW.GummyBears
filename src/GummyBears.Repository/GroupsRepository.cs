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
    public class GroupsRepository : Repository<GroupEntity, int>
    {
        public GroupsRepository(IQueryBuilder queryBuilder, SqlDbFactory sqlDbFactory)
            : base(queryBuilder, sqlDbFactory)
        {
        }

        public async Task<IEnumerable<GroupEntity>> GetUserGroups(int userId)
        {
            const string sql = @"
                SELECT g.* FROM Groups g 
                JOIN UserGroups ug ON ug.GroupId = g.Id
                WHERE ug.UserId = @UserId"; 

              using (IDbConnection connection = CreateConnection())
            {
                return await connection.QueryAsync<GroupEntity>(sql, new
                {
                    UserId = userId
                }).ConfigureAwait(false);
            }
        }
    }
}
