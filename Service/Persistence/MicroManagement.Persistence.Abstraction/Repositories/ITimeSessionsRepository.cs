﻿using MicroManagement.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroManagement.Persistence.Abstraction.Repositories
{
    public interface ITimeSessionsRepository
    {
        Task<IEnumerable<TimeSession>> GetAllAsync(Guid userId);
        Task AddAsync(TimeSession timeSession);
        Task UpdateAsync(TimeSession timeSession);
    }
}
