using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroManagement.Core
{
    public record Project
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }

        public Project(Guid id, Guid userId, string name, string color)
        {
            Id = id;
            UserId = userId;
            Name = name;
            Color = color;
        }
    }
}
