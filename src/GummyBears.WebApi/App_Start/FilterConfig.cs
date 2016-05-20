using System.Web;
using System.Web.Mvc;

namespace GummyBears.WebApi
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new WebApi.Filters.ApiErrorFilter());
           // filters.Add(new HandleErrorAttribute());
        }
    }
}
