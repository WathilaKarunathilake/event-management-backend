// <copyright file="UnitOfWork.cs" company="Ascentic">
// Copyright (c) Ascentic. All rights reserved.
// </copyright>
namespace EventManagementAPI.Infrastructure.Persistence.UnitOfWork
{
    using EventManagementAPI.Core.Application.Contracts.Persistence;
    using EventManagementAPI.Infrastructure.Persistence.Context;
    using Microsoft.EntityFrameworkCore.Storage;

    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext context;
        private IDbContextTransaction? transaction;

        public UnitOfWork(AppDbContext context)
        {
            this.context = context;
        }

        public async Task BeginTransactionAsync()
        {
            this.transaction = await this.context.Database.BeginTransactionAsync();
        }

        public async Task CommitAsync()
        {
            if (this.transaction == null)
            {
                throw new InvalidOperationException("Transaction has not been started.");
            }

            await this.context.SaveChangesAsync();
            await this.transaction.CommitAsync();
        }

        public async Task RollbackAsync()
        {
            if (this.transaction != null)
            {
                await this.transaction.RollbackAsync();
                await this.transaction.DisposeAsync();
            }
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await this.context.SaveChangesAsync(cancellationToken);
        }

        public void Dispose()
        {
            this.transaction?.Dispose();
            this.context.Dispose();
        }
    }
}
