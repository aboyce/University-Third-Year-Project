using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TicketManagement.Models.Entities
{
    public class Organisation
    {
        [Key]
        [Editable(false)]
        [Required]
        public int Id { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Name must be less that 50 characters but more than 5", MinimumLength = 5)]
        public string Name { get; set; }

        [Required]
        [DisplayName("Is Internal")]
        public bool IsInternal { get; set; }

        [ForeignKey("DefaultContact")]
        [DisplayName("Default Contact")]
        public int? ContactUserId { get; set; }

        virtual public User DefaultContact { get; set; }

        [Required]
        public DateTime Created { get; set; } = DateTime.Now;

        [Required]
        [DisplayName("Last Updated")]
        public DateTime LastUpdated { get; set; } = DateTime.Now;
    }
}
