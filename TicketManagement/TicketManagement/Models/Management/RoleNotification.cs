using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity.EntityFramework;

namespace TicketManagement.Models.Management
{
    public class RoleNotification : NotificationBase
    {
        [Required]
        public string RoleId { get; set; }

        [Required]
        public IdentityRole Role { get; set; }

        [Required]
        public RoleNotificationType Type { get; set; }
    }
}
