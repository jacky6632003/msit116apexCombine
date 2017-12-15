using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace msit116apexLayout.Models
{
    public class RoleMethod
    {
        MSIT116APEXEntities db = new MSIT116APEXEntities();
        public List<uRoles> allSupervisorID(int SupervisorID, List<uRoles> listResult)
        {
            uRoles newSupervisorRole = db.uRoles.Where(n => n.uRoleID == SupervisorID).FirstOrDefault();
            listResult.Add(newSupervisorRole);
            if (newSupervisorRole.supervisorID.HasValue)                
                allSupervisorID(newSupervisorRole.supervisorID.Value, listResult);

            return listResult;
        }

    }
}