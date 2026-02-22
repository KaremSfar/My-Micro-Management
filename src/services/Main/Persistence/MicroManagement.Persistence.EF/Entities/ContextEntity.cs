using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace MicroManagement.Persistence.EF.Entities;

[Table("ContextsTable")]
public class ContextEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Icon { get; set; }
    public Guid UserId { get; set; }

    public virtual ICollection<ProjectEntity> Projects { get; set; }
}
