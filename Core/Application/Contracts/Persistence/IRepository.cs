// <copyright file="IRepository.cs" company="Ascentic">
// Copyright (c) Ascentic. All rights reserved.
// </copyright>
using System.Linq.Expressions;

namespace EventManagementAPI.Core.Application.Contracts.Persistence
{
    public interface IRepository<T>
        where T : class
    {
        Task<T?> GetByIdAsync(Guid id);

        Task<List<T>> GetAllAsync();

        Task<T> AddAsync(T entity);

        Task UpdateAsync(T entity);

        Task DeleteAsync(Guid id);

        Task SaveAsync();

        Task<List<T>> FindAllAsync(Expression<Func<T, bool>> predicate);

        Task<T?> FindFirstOrDefaultAsync(Expression<Func<T, bool>> predicate);
    }
}
