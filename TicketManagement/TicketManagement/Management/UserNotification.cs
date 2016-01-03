using System.ComponentModel.DataAnnotations;

namespace TicketManagement.Management
{
    public class UserNotification : NotificationBase
    {
        [Required]
        public UserNotificationType Type { get; set; }
    }
}
