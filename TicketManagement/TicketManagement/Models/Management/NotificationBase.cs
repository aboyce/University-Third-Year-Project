using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketManagement.Models.Management
{
    public abstract class NotificationBase
    {
        [Key]
        [Editable(false)]
        public int Id { get; set; }

        [Required]
        public string UserIdNotificationOn { get; set; }

        [Required]
        public NotificationType NotificationType { get; set; }

        public string NotificationMessage { get; set; }
    }
}
