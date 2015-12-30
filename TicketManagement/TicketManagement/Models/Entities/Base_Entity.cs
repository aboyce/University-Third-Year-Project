using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TicketManagement.Models.Entities
{
    public abstract class Base_Entity
    {
        [Key]
        [Editable(false)]
        public int Id { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = false)]
        public DateTime Created { get; set; } = DateTime.Now;

        [Required]
        [DisplayName("Last Updated")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = false)]
        public DateTime LastUpdated { get; set; } = DateTime.Now;

        protected void Updated()
        {
            //LastUpdated = DateTime.Now;
        }
    }
}
