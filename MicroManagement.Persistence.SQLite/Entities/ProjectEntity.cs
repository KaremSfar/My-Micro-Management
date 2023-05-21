using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroManagement.Persistence.SQLite.Entities
{
    [Table("ProjectsTable")]
    public record ProjectEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }
        [Required]
        public string Color { get; set; }

        public virtual ICollection<TimeSessionEntity> TimeSessions { get; set; } = new List<TimeSessionEntity>();
    }
}
