using System;

namespace MicroManagement.Core;

public record Context
{
    public Guid Id { get; init; }
    public string Name { get; init; }
    public string Icon { get; init; }
}
