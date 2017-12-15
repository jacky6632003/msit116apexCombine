using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace msit116apexLayout.Models
{
    public class PowerViewModel
    {
        public IEnumerable<PagePowersViewModel> pagePowersViewModel { get; set; }
        public IEnumerable<RolesViewModel> rolesViewModel { get; set; }

        public IEnumerable<Department> department { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> roles { get; set; }
    }

    public class PagePowersViewModel
    {
        public uPages uPages { get; set; }
        public IEnumerable<uPowers> uPowers { get; set; }
    }    

    public class RoleFromPowersViewModel
    {
        public uPages uPages { get; set; }
        public IEnumerable<RoleFromPower> roleFromPower { get; set; }
    }

    public class RoleFromPower
    {
        public uPowers uPower { get; set; }
        public IEnumerable<uRolesDpmtUserIdUserName> uRolesDpmtUserIdUserNames { get; set; }
        public IEnumerable<UserIdUserNameRoles> UserIDNames { get; set; }
        public IEnumerable<string> Department { get; set; }
    }

    public class UserIdUserName
    {
        public string UserID { get; set; }
        public string UserName { get; set; }
    }

    public class UserIdUserNameRoles
    {
        public string UserID { get; set; }
        public string UserName { get; set; }
        public IEnumerable<uRolesDpmt> uRolesDpmts { get; set; }
    }
    
    public class uRolePowerConfirmWithRole
    {
        public uRolePowerConfirm uRolePowerConfirm { get; set; }
        public IEnumerable<uRoles> uRoles { get; set; }
    }

    public class uPowerWithBoolPage
    {
        public uPages uPages { get; set; }
        public IEnumerable<uPowerWithBool> uPowerWithBool { get; set; }
    }

    public class uPowerWithBool
    {
        public uPowers uPowers { get; set; }
        public bool hasPower { get; set; }
        public IEnumerable<uRolesDpmt> rolesConfirm { get; set; }
    }

    public class uRolesDpmt
    {
        public uRoles uRoles { get; set; }
        public string Department { get; set; }
    }

    public class uRolesWithDpmtSurName
    {
        public uRoles uRoles { get; set; }
        public string Department { get; set; }
        public string uRoleSurName { get; set; }
    }

    public class uRolesDpmtUserIdUserName
    {
        public uRoles uRoles { get; set; }
        public string Department { get; set; }
        public IEnumerable<UserIdUserName> userIdUserName { get; set; }
    }

    public class RoleEditUserViewModel
    {
        public int roleID { get; set; }
        public int limit { get; set; }
        public IEnumerable<UserIdUserNameRoles> UserIDNames { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> userNameAll { get; set; }
    }

    public class RoleEditPowersViewModel
    {
        public int roleID { get; set; }
        public IEnumerable<RoleEditPgPowers> roleEditPgPowers { get; set; }
    }

    public class RoleEditPgPowers
    {
        public uPages uPages { get; set; }
        public IEnumerable<RoleEditPower> roleEditPowers { get; set; }
    }

    public class RoleEditPower
    {
        public bool hasPower { get; set; }
        public int powerID { get; set; }
        public int pageID { get; set; }
        [DisplayName("權限名稱")]
        public string powerName { get; set; }
        [DisplayName("權限描述")]
        public string powerDescription { get; set; }
    }

    public class RolesViewModel
    {
        public uRoles uRoles { get; set; }
        public IEnumerable<uRoleUsers> uRoleUsers { get; set; }
        public IEnumerable<uRolePowers> uRolePowers { get; set; }        
    }

    public class RoleLevelViewModel
    {
        public uRoles uRoles { get; set; }
        public IEnumerable<RolesUserNames> rolesUserNames { get; set; }
        public IEnumerable<uRolePowers> uRolePowers { get; set; }
    }

    public class RoleLevelWithPowerViewModel
    {
        public IEnumerable<RoleLevelViewModel> roleLevelViewModel { get; set; }
        public IEnumerable<PagePowersViewModel> pagePowersViewModel { get; set; }
    }

    public class RolesUserNames
    {
        public uRoleUsers uRoleUser { get; set; }
        public string UserName { get; set; }
    }

    public class RoleEdit
    {
        public uRoles uRoles { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> departments { get; set; }
        [DisplayName("新增後自動清除")]
        public bool autoClearNewRole { get; set; }
        public int roleID { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> supervisors { get; set; }
    }

    public class UserIdUserNameRolesComparer : IEqualityComparer<UserIdUserNameRoles>
    {
        public bool Equals(UserIdUserNameRoles t1, UserIdUserNameRoles t2)
        {
            return (t1.UserID == t2.UserID);
        }

        public int GetHashCode(UserIdUserNameRoles t)
        {
            return t.ToString().GetHashCode();
        }
    }

    [MetadataType(typeof(uPagesMetadata))]
    public partial class uPages
    {
        public class uPagesMetadata
        {
            public int pageID { get; set; }
            [DisplayName("頁面名稱")]
            public string pageName { get; set; }
            [DisplayName("頁面描述")]
            public string pageDescription { get; set; }
        }
    }

    [MetadataType(typeof(uPowersMetadata))]
    public partial class uPowers
    {
        public class uPowersMetadata
        {
            public int powerID { get; set; }
            public int pageID { get; set; }
            [DisplayName("權限名稱")]
            public string powerName { get; set; }
            [DisplayName("權限描述")]
            public string powerDescription { get; set; }
        }
    }

    [MetadataType(typeof(uRolesMetadata))]
    public partial class uRoles
    {
        public class uRolesMetadata
        {
            public int uRoleID { get; set; }
            [DisplayName("角色名稱")]
            public string uRoleName { get; set; }
            [DisplayName("角色描述")]
            public string uRoleDescription { get; set; }
            [DisplayName("角色人數")]
            public int uRoleNums { get; set; }
            [DisplayName("部門")]
            public Nullable<int> departmentID { get; set; }
            [DisplayName("主管")]
            public Nullable<int> supervisorID { get; set; }
        }
    }

    [MetadataType(typeof(uRolePowerConfirmMetadata))]
    public partial class uRolePowerConfirm
    {
        public partial class uRolePowerConfirmMetadata
        {
            public int uRolePowerSn { get; set; }
            [DisplayName("每個角色最少確認人數")]
            public Nullable<int> uEachRoleMinNum { get; set; }
            [DisplayName("所有角色最少確認人數")]
            public Nullable<int> uTotlaRoleMinNum { get; set; }
            [DisplayName("此設定在多個角色時的優先度")]
            public Nullable<int> rolePriority { get; set; }
            [DisplayName("覆核有效天數")]
            public Nullable<int> maxDay { get; set; }
            public int ucrSn { get; set; }
        }
    }
}