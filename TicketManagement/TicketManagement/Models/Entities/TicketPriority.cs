using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TicketManagement.Models.Entities
{
    public class TicketPriority : EntityBase
    {
        private string _name;
        private string _colour = "#FFFFFF";

        [Required]
        [StringLength(50, ErrorMessage = "Ticket Priority Name must be less that 50 characters but more than 2", MinimumLength = 2)]
        [DisplayName("Name")]
        public string Name
        {
            get { return _name; }
            set { _name = value; Updated(); }
        }

        [StringLength(10, ErrorMessage = "Username must be less that 10 characters but more than 2", MinimumLength = 2)]
        public string Colour
        {
            get { return _colour; }
            set { _colour = value; Updated(); }
        }
    }
}
