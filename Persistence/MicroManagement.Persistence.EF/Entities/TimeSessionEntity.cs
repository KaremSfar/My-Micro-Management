using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroManagement.Persistence.SQLite.Entities
{
    [Table("TimeSessionsTable")]
    public record TimeSessionEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndDate { get; set; }

        public virtual ICollection<ProjectEntity> Projects { get; set; } = new List<ProjectEntity>();
    }
}
