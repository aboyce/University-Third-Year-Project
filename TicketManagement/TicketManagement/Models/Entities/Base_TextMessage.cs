using System.ComponentModel.DataAnnotations;

namespace TicketManagement.Models.Entities
{
    public abstract class Base_TextMessage
    {
        [Key]
        [Editable(false)]
        public int Id { get; set; }

        [Required]
        public string ClockworkId { get; set; }

        [Required]
        public string To { get; set; }

        [Required]
        public string From { get; set; }

        [Required]
        public string Content { get; set; }
    }
}