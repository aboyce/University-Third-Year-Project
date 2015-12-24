using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketManagement.Models.Entities
{
    public abstract class EntityBase
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
