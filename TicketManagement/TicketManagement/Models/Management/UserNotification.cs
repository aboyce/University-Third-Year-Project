using System.ComponentModel.DataAnnotations;

namespace TicketManagement.Models.Management
{
    public class UserNotification : NotificationBase
    {
        [Required]
        public UserNotificationType Type { get; set; }
    }
}
