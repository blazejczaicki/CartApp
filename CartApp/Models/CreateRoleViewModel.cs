using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace CartApp.Models
{
    /// <summary>
    /// Class to store create user role data from view.
    /// </summary>
    public class CreateRoleViewModel
    {
        [Required]
        public string RoleName { get; set; }
    }
}
