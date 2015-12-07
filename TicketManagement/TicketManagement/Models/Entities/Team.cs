using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TicketManagement.Models.Entities
{
    public class Team : EntityBase
    {
        private string _name;
        private int? _organisationId = null;
        private Organisation _organisation = null;

        [Required]
        [StringLength(50, ErrorMessage = "Team Name must be less that 50 characters but more than 2", MinimumLength = 2)]
        [DisplayName("Name")]
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
    }
}
