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
    public class UsersRepository : Repository<UserEntity, int>
    {
        public UsersRepository(IQueryBuilder queryBuilder, SqlDbFactory sqlDbFactory)
            : base(queryBuilder, sqlDbFactory)
        {
        }

        public async Task<UserEntity> GetByUserName(string userName)
        {
            string sql = @"
                    SELECT * FROM Users
                    WHERE UserName = @UserName";

            using (IDbConnection connection = CreateConnection())
            {
                return (await connection.QueryAsync<UserEntity>(sql, new
                {
                    UserName = userName
                }).ConfigureAwait(false)).SingleOrDefault();
            }
        }

        public async Task<UserEntity> GetByEmail(string email)
        {
            string sql = @"
                    SELECT * FROM Users
                    WHERE Email = @Email";

            using (IDbConnection connection = CreateConnection())
            {
                return (await connection.QueryAsync<UserEntity>(sql, new
                {
                    Email = email
                }).ConfigureAwait(false)).SingleOrDefault();
            }
        }
    }
}