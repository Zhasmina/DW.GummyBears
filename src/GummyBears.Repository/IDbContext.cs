using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GummyBears.Repository
{
    public interface IDbContext
    {
        UsersRepository UsersRepo { get; }

        CreationsRepository CreationsRepo { get; }

        GroupsRepository GroupsRepo { get; }

        GroupUsersRepository GroupsUsersRepo { get; }

        GroupCreationsRepository GroupCreationsRepo { get; }

        GroupMessagesRepository GroupMessagesRepo { get; }

    }
}
