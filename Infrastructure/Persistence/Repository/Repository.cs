// <copyright file="Repository.cs" company="Ascentic">
// Copyright (c) Ascentic. All rights reserved.
// </copyright>
namespace EventManagementAPI.Infrastructure.Persistence.Repository
{
    using System.Linq.Expressions;
    using EventManagementAPI.Core.Application.Contracts.Persistence;
    using EventManagementAPI.Infrastructure.Persistence.Context;
    using Microsoft.EntityFrameworkCore;

    public class Repository<T> : IRepository<T>
        where T : class
    {
        private readonly AppDbContext context;
        private readonly DbSet<T> dbSet;

        public Repository(AppDbContext context)
        {
            this.context = context;
            this.dbSet = this.context.Set<T>();
        }

        public async Task<T?> GetByIdAsync(Guid id) => await this.dbSet.FindAsync(id);

        public async Task<List<T>> GetAllAsync() => await this.dbSet.ToListAsync();

        public async Task<T> AddAsync(T entity)
        {
            var entry = await this.dbSet.AddAsync(entity);
            return entry.Entity;
        }

        public Task UpdateAsync(T entity)
        {
            this.dbSet.Update(entity);
            return Task.CompletedTask;
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await this.dbSet.FindAsync(id);
            if (entity != null)
            {
                this.dbSet.Remove(entity);
            }
        }

        public async Task SaveAsync() => await this.context.SaveChangesAsync();

        public async Task<List<T>> FindAllAsync(Expression<Func<T, bool>> predicate)
        {
            return await this.dbSet.Where(predicate).ToListAsync();
        }

        public async Task<T?> FindFirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
        {
            return await this.dbSet.FirstOrDefaultAsync(predicate);
        }
    }
}
