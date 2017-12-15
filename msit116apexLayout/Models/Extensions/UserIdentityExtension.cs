using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;
using msit116apexLayout.Models;

namespace msit116apexLayout.Extensions
{
    public static class IdentityExtensions
    {
        public static bool GetIsAdmin(this IIdentity identity)
        {
            //string claimValue = ((ClaimsIdentity)identity).FindFirst("UserName").Value;
            Repository<IsAdmin> dbadmin = new Repository<IsAdmin>();
            IsAdmin ia = dbadmin.GetByID(identity.Name);
            // Test for null to avoid issues during local testing
            return (ia != null) ? true : false;
        }

        public static bool GetIsEmployee(this IIdentity identity)
        {
            //string claimValue = ((ClaimsIdentity)identity).FindFirst("UserName").Value;
            Repository<IsEmployee> dbEmployee = new Repository<IsEmployee>();
            IsEmployee iee = dbEmployee.GetByID(identity.Name);
            // Test for null to avoid issues during local testing
            return (iee != null) ? true : false;
        }

        public static string GetUserSName(this IIdentity identity)
        {
            //string claimValue = ((ClaimsIdentity)identity).FindFirst("UserName").Value;
            MSIT116APEXEntities db = new MSIT116APEXEntities();
            AspNetUsers anu = db.AspNetUsers.Where(n=>n.UserName== identity.Name).FirstOrDefault();
            // Test for null to avoid issues during local testing
            return (anu != null) ? anu.Name : "";
        }
    }
}