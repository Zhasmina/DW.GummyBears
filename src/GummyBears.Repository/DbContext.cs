using Dapperer;
using Dapperer.DbFactories;
using GummyBears.Common;
using System.Data;
using System.Data.SqlClient;

namespace GummyBears.Repository
{
    public class DbContext : IDbContext
    {
        private readonly string _databaseConnection;

        public DbContext(IQueryBuilder queryBuilder)
        {
            _databaseConnection = AppSettingsProvider.DatabaseConnectionString;

            var sqlDbFactory = new SqlDbFactory(new DappererSettings());

            UsersRepo = new UsersRepository(queryBuilder, sqlDbFactory);
            CreationsRepo = new CreationsRepository(queryBuilder, sqlDbFactory);
            GroupsRepo = new GroupsRepository(queryBuilder, sqlDbFactory);
            GroupsUsersRepo = new GroupUsersRepository(queryBuilder, sqlDbFactory);
            GroupCreationsRepo = new GroupCreationsRepository(queryBuilder, sqlDbFactory);
            GroupMessagesRepo = new GroupMessagesRepository(queryBuilder, sqlDbFactory);
        }

        public UsersRepository UsersRepo
        {
            get;
            private set;
        }

        public CreationsRepository CreationsRepo
        {
            get;
            private set;
        }

        public GroupsRepository GroupsRepo
        {
            get;
            private set;
        }

        public GroupUsersRepository GroupsUsersRepo
        {
            get;
            private set;
        }

        public GroupCreationsRepository GroupCreationsRepo
        {
            get;
            private set;
        }

        public GroupMessagesRepository GroupMessagesRepo
        {
            get;
            private set;
        }

        public IDbConnection CreateConnection()
        {
            return new SqlConnection(_databaseConnection);
        }
    }
}
