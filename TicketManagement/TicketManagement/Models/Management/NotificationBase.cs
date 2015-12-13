using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketManagement.Models.Entities;

namespace TicketManagement.Models.Management
{
    public abstract class NotificationBase
    {
        [Key]
        [Editable(false)]
        public int Id { get; set; }

        [Required]
        public string NotificationAboutId { get; set; }

        [Required]
        public User NotificationAbout { get; set; }

        public string Message { get; set; }
    }
}
