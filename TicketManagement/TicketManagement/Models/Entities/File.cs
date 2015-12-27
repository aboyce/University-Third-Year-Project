using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketManagement.Models.Management;

namespace TicketManagement.Models.Entities
{
    public class File
    {
        [Key]
        [Editable(false)]
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string FileName { get; set; }

        [StringLength(100)]
        public string ContentType { get; set; }

        public byte[] Content { get; set; }

        [Required]
        public FileType FileType { get; set; }
    }
}
