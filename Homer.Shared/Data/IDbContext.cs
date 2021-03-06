﻿using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Homer.Shared.Data
{
    public interface IDbContext
    {
        DbSet<TEntity> Set<TEntity>() where TEntity : class, new(); //BaseEntity;

        int SaveChanges();

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));
    }
}
