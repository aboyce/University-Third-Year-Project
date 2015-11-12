using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketManagement.Models.Entities
{
    public class EntityBase
    {
        [Key]
        [Editable(false)]
        public int Id { get; set; }

        [Required]
        public DateTime Created { get; } = DateTime.Now;

        [Required]
        [DisplayName("Last Updated")]
        public DateTime LastUpdated { get; private set; } = DateTime.Now;

        protected void Updated()
        {
            LastUpdated = DateTime.Now;
        }
    }
}
