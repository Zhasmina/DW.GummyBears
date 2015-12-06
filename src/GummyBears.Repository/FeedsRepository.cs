using Dapperer;
using Dapperer.DbFactories;
using GummyBears.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GummyBears.Repository
{
    public class FeedsRepository : Repository<FeedsEntity, int>
    {
        public FeedsRepository(IQueryBuilder queryBuilder, SqlDbFactory sqlDbFactory)
            : base(queryBuilder, sqlDbFactory)
        {
        }
    }
}
