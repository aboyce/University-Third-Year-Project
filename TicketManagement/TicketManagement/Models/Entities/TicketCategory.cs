using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TicketManagement.Models.Entities
{
    public class TicketCategory : EntityBase
    {
        private string _name;
        private int? _projectId = null;
        private Project _project = null;

        [Required]
        [StringLength(50, ErrorMessage = "Ticket Category Name must be less that 50 characters but more than 2", MinimumLength = 2)]
        [DisplayName("Ticket Category Name")]
        public string Name
        {
            get { return _name; }
            set { _name = value; Updated(); }
        }

        [ForeignKey("Project")]
        [DisplayName("Project")]
        public int? ProjectId
        {
            get { return _projectId; }
            set { _projectId = value; Updated(); }
        }

        public virtual Project Project
        {
            get { return _project; }
            set { _project = value; Updated(); }
        }
    }
}
