using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TicketManagement.Models.Entities
{
    public class Organisation : EntityBase
    {
        private string _name;
        private bool _isInternal = false;
        private string _defaultContactId = null;
        private User _defaultContact = null;

        [Required]
        [StringLength(50, ErrorMessage = "Name must be less that 50 characters but more than 2", MinimumLength = 2)]
        public string Name
        {
            get { return _name; }
            set { _name = value; Updated(); }
        }

        [Required]
        [DisplayName("Is Internal")]
        public bool IsInternal
        {
            get { return _isInternal; }
            set { _isInternal = value; Updated(); }
        }

        [ForeignKey("DefaultContact")]
        [DisplayName("Default Contact")]
        public string DefaultContactId
        {
            get { return _defaultContactId; }
            set { _defaultContactId = value; Updated(); }
        }

        public virtual User DefaultContact
        {
            get { return _defaultContact; }
            set { _defaultContact = value; Updated(); }
        }
    }
}
