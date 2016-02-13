using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TicketManagement.Models.Entities
{
    public class Ticket : Base_Entity
    {
        private string _title;
        private string _description;
        private string _openedById;
        private User _openedBy;
        private int _ticketPriorityId;
        private TicketPriority _ticketPriority;
        private string _userAssignedToId = null;
        private User _userAssignedTo = null;
        private int? _teamAssignedToId = null;
        private Team _teamAssignedTo = null;
        private int? _organisationAssignedToId = null;
        private Organisation _organisationAssignedTo = null;
        private int _ticketStateId;
        private TicketState _ticketState;
        private int? _projectId = null;
        private Project _project = null;
        private int _ticketCategoryId;
        private TicketCategory _ticketCategory;
        private DateTime? _deadline = null;
        private DateTime? _lastMessage = null;
        private DateTime? _lastResponse = null;

        [Required]
        [StringLength(100, ErrorMessage = "Title must be less that 100 characters but more than 2", MinimumLength = 2)]
        public string Title
        {
            get { return _title; }
            set { _title = value; Updated(); }
        }

        [StringLength(250, ErrorMessage = "Description must be less that 250 characters but more than 2", MinimumLength = 2)]
        public string Description
        {
            get { return _description; }
            set { _description = value; Updated(); }
        }

        [Required]
        [ForeignKey("OpenedBy")]
        [DisplayName("Opened By")]
        public string OpenedById
        {
            get { return _openedById; }
            set { _openedById = value; Updated(); }
        }

        public virtual User OpenedBy
        {
            get { return _openedBy; }
            set { _openedBy = value; Updated(); }
        }

        [Required]
        [ForeignKey("TicketPriority")]
        [DisplayName("Ticket Priority")]
        public int TicketPriorityId
        {
            get { return _ticketPriorityId; }
            set { _ticketPriorityId = value; Updated(); }
        }

        virtual public TicketPriority TicketPriority
        {
            get { return _ticketPriority; }
            set { _ticketPriority = value; Updated(); }
        }

        //[Required]
        //[ForeignKey("TicketPriorityInternal")]
        //[DisplayName("Ticket Priority Internally")]
        //public int TicketPriorityInternalId { get; set; }

        //virtual public TicketPriority TicketPriorityInternal { get; set; }

        [ForeignKey("UserAssignedTo")]
        [DisplayName("User Assigned To")]
        public string UserAssignedToId
        {
            get { return _userAssignedToId; }
            set { _userAssignedToId = value; Updated(); }
        }

        public virtual User UserAssignedTo
        {
            get { return _userAssignedTo; }
            set { _userAssignedTo = value; Updated(); }
        }

        [ForeignKey("TeamAssignedTo")]
        [DisplayName("Team Assigned To")]
        public int? TeamAssignedToId
        {
            get { return _teamAssignedToId; }
            set { _teamAssignedToId = value; Updated(); }
        }

        public virtual Team TeamAssignedTo
        {
            get { return _teamAssignedTo; }
            set { _teamAssignedTo = value; Updated(); }
        }

        [ForeignKey("OrganisationAssignedTo")]
        [DisplayName("Organisation Assigned To")]
        public int? OrganisationAssignedToId
        {
            get { return _organisationAssignedToId; }
            set { _organisationAssignedToId = value; Updated(); }
        }

        public virtual Organisation OrganisationAssignedTo
        {
            get { return _organisationAssignedTo; }
            set { _organisationAssignedTo = value; Updated(); }
        }

        [Required]
        [ForeignKey("TicketState")]
        [DisplayName("Ticket State")]
        public int TicketStateId
        {
            get { return _ticketStateId; }
            set { _ticketStateId = value; Updated(); }
        }

        public virtual TicketState TicketState
        {
            get { return _ticketState; }
            set { _ticketState = value; Updated(); }
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

        [Required]
        [ForeignKey("TicketCategory")]
        [DisplayName("Ticket Category")]
        public int TicketCategoryId
        {
            get { return _ticketCategoryId; }
            set { _ticketCategoryId = value; Updated(); }
        }

        public virtual TicketCategory TicketCategory
        {
            get { return _ticketCategory; }
            set { _ticketCategory = value; Updated(); }
        }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = false)]
        public DateTime? Deadline
        {
            get { return _deadline; }
            set { _deadline = value; Updated(); }
        }

        [DisplayName("Last Message")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = false)]
        public DateTime? LastMessage
        {
            get { return _lastMessage; }
            set { _lastMessage = value; Updated(); }
        }

        [DisplayName("Last Response")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = false)]
        public DateTime? LastResponse
        {
            get { return _lastResponse; }
            set { _lastResponse = value; Updated(); }
        }
    }
}
