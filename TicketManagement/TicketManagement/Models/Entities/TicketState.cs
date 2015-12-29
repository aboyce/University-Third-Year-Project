using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TicketManagement.Models.Entities
{
    public class TicketState : EntityBase
    {
        private string _name;
        private string _colour = "#FFFFFF";

        [Required]
        [StringLength(50, ErrorMessage = "Ticket State Name must be less that 50 characters but more than 2", MinimumLength = 2)]
        [DisplayName("Name")]
        public string Name
        {
            get { return _name; }
            set { _name = value; Updated(); }
        }

        [StringLength(10, ErrorMessage = "Colour must be less that 10 characters but more than 3", MinimumLength = 3)]
        public string Colour
        {
            get { return _colour; }
            set { _colour = value; Updated(); }
        }
    }
}
