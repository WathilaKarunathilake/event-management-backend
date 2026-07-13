// <copyright file="IUnitOfWork.cs" company="Ascentic">
// Copyright (c) Ascentic. All rights reserved.
// </copyright>
namespace EventManagementAPI.Core.Application.Contracts.Persistence
{
    public interface IUnitOfWork : IDisposable
    {
        Task BeginTransactionAsync();

        Task CommitAsync();

        Task RollbackAsync();

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
