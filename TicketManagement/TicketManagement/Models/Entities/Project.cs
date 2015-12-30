using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TicketManagement.Models.Entities
{
    public class Project : Base_Entity
    {
        private string _name;
        private int? _organisationId = null;
        private Organisation _organisation = null;
        private int? _teamAssignedToId = null;
        private Team _teamAssignedTo = null;

        [Required]
        [StringLength(50, ErrorMessage = "Name must be less that 50 characters but more than 2", MinimumLength = 2)]
        public string Name
        {
            get { return _name; }
            set { _name = value; Updated(); }
        }

        [ForeignKey("Organisation")]
        [DisplayName("Organisation")]
        public int? OrganisationId
        {
            get { return _organisationId; }
            set { _organisationId = value; Updated(); }
        }

        virtual public Organisation Organisation
        {
            get { return _organisation; }
            set { _organisation = value; Updated(); }
        }

        [ForeignKey("TeamAssignedTo")]
        [DisplayName("Team Assigned To")]
        public int? TeamAssignedToId
        {
            get { return _teamAssignedToId; }
            set { _teamAssignedToId = value; Updated(); }
        }

        virtual public Team TeamAssignedTo
        {
            get { return _teamAssignedTo; }
            set { _teamAssignedTo = value; Updated(); }
        }
    }
}
