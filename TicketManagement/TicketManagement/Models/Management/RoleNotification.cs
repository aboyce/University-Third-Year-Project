using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Management;
using Microsoft.AspNet.Identity.EntityFramework;

namespace TicketManagement.Models.Management
{
    public class RoleNotification : NotificationBase
    {
        [Required]
        public string RoleId { get; set; }

        [Required]
        public IdentityRole Role { get; set; }
    }
}
