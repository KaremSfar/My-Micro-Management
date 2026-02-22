using System;

namespace MicroManagement.Persistence.EF.Entities;

public class ContextEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Icon { get; set; }

    public virtual ICollection<ProjectEntity> Projects { get; set; }
}
