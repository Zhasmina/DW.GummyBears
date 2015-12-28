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
    public class CreationsRepository : Repository<CreationEntity, int>
    {
        public CreationsRepository(IQueryBuilder queryBuilder, SqlDbFactory sqlDbFactory)
            : base(queryBuilder, sqlDbFactory)
        {
        }

        public async Task<IEnumerable<CreationEntity>> GetUserCreations(int userId)
        {
            const string sql = @"
                SELECT c.* FROM Creations c
                WHERE c.UserId = @UserId";

            using (System.Data.IDbConnection connection = CreateConnection())
            {
                return await connection.QueryAsync<CreationEntity>(sql, new
                {
                    UserId = userId
                }).ConfigureAwait(false);
            }
        }
    }
}
