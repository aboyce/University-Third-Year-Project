using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TicketManagement.Models.Entities
{
    public class User : EntityBase
    {
        private string _applicationUserId;
        private string _firstName;
        private string _lastName;
        private bool _isInternal = false;
        private bool _isAdmin = false;
        private bool _isArchived = false;
        private int? _teamId = null;
        private Team _team = null;
        private bool _isTeamLeader = false;

        //[Required]
        [DisplayName("Application User")]
        public string ApplicationUserId
        {
            get { return _applicationUserId;}
            set { _applicationUserId = value; Updated(); }
        }

        [Required]
        [StringLength(50, ErrorMessage = "First Name must be less that 50 characters but more than 2", MinimumLength = 2)]
        [DisplayName("First Name")]
        public string FirstName
        {
            get { return _firstName; }
            set { _firstName = value; Updated(); }
        }

        [Required]
        [StringLength(50, ErrorMessage = "Last Name must be less that 50 characters but more than 2", MinimumLength = 2)]
        [DisplayName("Last Name")]
        public string LastName
        {
            get { return _lastName; }
            set { _lastName = value; Updated(); }
        }

        [Required]
        [DisplayName("Internal User")]
        public bool IsInternal
        {
            get { return _isInternal; }
            set { _isInternal = value; Updated(); }
        }

        [Required]
        [DisplayName("Admin User")]
        public bool IsAdmin
        {
            get { return _isAdmin; }
            set { _isAdmin = value; Updated(); }
        }

        [Required]
        [DisplayName("Archived")]
        public bool IsArchived
        {
            get { return _isArchived; }
            set { _isArchived = value; Updated(); }
        }

        [ForeignKey("Team")]
        [DisplayName("Team")]
        public int? TeamId
        {
            get { return _teamId; }
            set { _teamId = value; Updated(); }
        }

        virtual public Team Team
        {
            get { return _team; }
            set { _team = value; Updated(); }
        }

        [DisplayName("Is Team Leader")]
        public bool IsTeamLeader
        {
            get { return _isTeamLeader; }
            set { _isTeamLeader = value; Updated(); }
        }
    }
}
