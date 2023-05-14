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
        public string Name { get; set; }
        public string Color { get; set; }

        public Project(Guid id, string name, string color)
        {
            Id = id;
            Name = name;
            Color = color;
        }
    }
}
