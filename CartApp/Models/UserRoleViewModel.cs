using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CartApp.Models
{
    /// <summary>
    /// Class stores user role data from view.
    /// </summary>
    public class UserRoleViewModel
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public bool IsSelected { get; set; }
    }
}
