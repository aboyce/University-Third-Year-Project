using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TicketManagement.Models.Entities
{
    public class TicketLogType : EntityBase
    {
        private string _name;

        [Required]
        [StringLength(50, ErrorMessage = "Ticket Log Type Name must be less that 50 characters but more than 2", MinimumLength = 2)]
        [DisplayName("Name")]
        public string Name
        {
            get { return _name; }
            set { _name = value; Updated(); }
        }
    }
}
