using FinanceTracker.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using MongoDB.Driver;

namespace FinanceTracker.Infra.Repositories
{
    public class MongoRoleStore : IRoleStore<RoleEntity>
    {
        private readonly IMongoCollection<RoleEntity> _roleCollection;

        public MongoRoleStore(IMongoDatabase database)
        {
            _roleCollection = database.GetCollection<RoleEntity>("Roles");
        }

        public Task<IdentityResult> CreateAsync(RoleEntity role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            _roleCollection.InsertOne(role);
            return Task.FromResult(IdentityResult.Success);
        }

        public Task<IdentityResult> UpdateAsync(RoleEntity role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var result = _roleCollection.ReplaceOne(r => r.Id == role.Id, role);
            return Task.FromResult(result.IsAcknowledged ? IdentityResult.Success : IdentityResult.Failed());
        }

        public Task<IdentityResult> DeleteAsync(RoleEntity role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var result = _roleCollection.DeleteOne(r => r.Id == role.Id);
            return Task.FromResult(result.IsAcknowledged ? IdentityResult.Success : IdentityResult.Failed());
        }

        public Task<string> GetRoleIdAsync(RoleEntity role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return Task.FromResult(role.Id.ToString());
        }

        public Task<string> GetRoleNameAsync(RoleEntity role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return Task.FromResult(role.Name);
        }

        public Task SetRoleNameAsync(RoleEntity role, string? roleName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            role.Name = roleName;
            return Task.CompletedTask;
        }

        public Task<string?> GetNormalizedRoleNameAsync(RoleEntity role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return Task.FromResult(role.NormalizedName);
        }

        public Task SetNormalizedRoleNameAsync(RoleEntity role, string? normalizedName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            role.NormalizedName = normalizedName;
            return Task.CompletedTask;
        }

        public Task<RoleEntity?> FindByIdAsync(string roleId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return Task.FromResult(_roleCollection.Find(r => r.Id.ToString() == roleId).FirstOrDefault());
        }

        public Task<RoleEntity?> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return Task.FromResult(_roleCollection.Find(r => r.NormalizedName == normalizedRoleName).FirstOrDefault());
        }

        public void Dispose()
        {
            // Dispose resources if necessary
        }
    }
}