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
    public class GroupUsersRepository : Repository<GroupUserEntity, int>
    {
        public GroupUsersRepository(IQueryBuilder queryBuilder, SqlDbFactory sqlDbFactory)
            : base(queryBuilder, sqlDbFactory)
        { 
        }


        public async Task<GroupUserEntity> GetByUserIdAndGroupId(int userId, int groupId)
        {
            const string sql = @"
                SELECT * FROM UserGroups 
                WHERE UserId = @UserId
                AND GroupId =@GroupId";

            using (IDbConnection connection = CreateConnection())
            {
                return (await connection.QueryAsync<GroupUserEntity>(sql, new
                {
                    UserId = userId,
                    GroupId = groupId
                }).ConfigureAwait(false)).SingleOrDefault();
            }
        }

        public async Task<IEnumerable<GroupUserEntity>> GetByGroupId(int groupId)
        {
            const string sql = @"
                SELECT distinct * FROM UserGroups 
                WHERE GroupId =@GroupId";

            using (IDbConnection connection = CreateConnection())
            {
                return (await connection.QueryAsync<GroupUserEntity>(sql, new
                {
                    GroupId = groupId
                }).ConfigureAwait(false));
            }
        }

    }
}
