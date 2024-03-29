﻿using System.Web;
using System.Web.Mvc;

namespace BugTracker
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            if (!HttpContext.Current.IsDebuggingEnabled)
            {
                filters.Add(new RequireHttpsAttribute());
            }
            filters.Add(new HandleErrorAttribute());
        }
    }
}
