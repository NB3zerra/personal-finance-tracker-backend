using Microsoft.AspNetCore.Identity;
using MongoDB.Driver;

namespace FinanceTracker.Infra.Repositories;

public class MongoUserStore : IUserStore<IdentityUser>, IUserPasswordStore<IdentityUser>
{
    private readonly IMongoCollection<IdentityUser> _users;

    public MongoUserStore(IMongoDatabase database)
    {
        _users = database.GetCollection<IdentityUser>("Users");
    }

    public async Task<IdentityResult> CreateAsync(IdentityUser user, CancellationToken cancellationToken)
    {
        await _users.InsertOneAsync(user, cancellationToken: cancellationToken);
        return IdentityResult.Success;
    }

    public async Task<IdentityResult> DeleteAsync(IdentityUser user, CancellationToken cancellationToken)
    {
        var result = await _users.DeleteOneAsync(u => u.Id == user.Id, cancellationToken);
        return result.DeletedCount > 0 ? IdentityResult.Success : IdentityResult.Failed();
    }

    public async Task<IdentityUser?> FindByIdAsync(string userId, CancellationToken cancellationToken)
    {
        return await _users.Find(u => u.Id == userId).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IdentityUser?> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
    {
        return await _users.Find(u => u.NormalizedUserName == normalizedUserName).FirstOrDefaultAsync(cancellationToken);
    }

    public Task<string?> GetNormalizedUserNameAsync(IdentityUser user, CancellationToken cancellationToken)
    {
        return Task.FromResult(user.NormalizedUserName);
    }

    public Task<string> GetUserIdAsync(IdentityUser user, CancellationToken cancellationToken)
    {
        return Task.FromResult(user.Id);
    }

    public Task<string?> GetUserNameAsync(IdentityUser user, CancellationToken cancellationToken)
    {
        return Task.FromResult(user.UserName);
    }

    public Task SetNormalizedUserNameAsync(IdentityUser user, string? normalizedName, CancellationToken cancellationToken)
    {
        user.NormalizedUserName = normalizedName;
        return Task.CompletedTask;
    }

    public Task SetUserNameAsync(IdentityUser user, string? userName, CancellationToken cancellationToken)
    {
        user.UserName = userName;
        return Task.CompletedTask;
    }

    public Task SetPasswordHashAsync(IdentityUser user, string? passwordHash, CancellationToken cancellationToken)
    {
        user.PasswordHash = passwordHash;
        return Task.CompletedTask;
    }

    public Task<string?> GetPasswordHashAsync(IdentityUser user, CancellationToken cancellationToken)
    {
        return Task.FromResult(user.PasswordHash);
    }

    public Task<bool> HasPasswordAsync(IdentityUser user, CancellationToken cancellationToken)
    {
        return Task.FromResult(!string.IsNullOrEmpty(user.PasswordHash));
    }

    public Task<IdentityResult> UpdateAsync(IdentityUser user, CancellationToken cancellationToken)
    {
        var filter = Builders<IdentityUser>.Filter.Eq(u => u.Id, user.Id);
        var update = Builders<IdentityUser>.Update
            .Set(u => u.UserName, user.UserName)
            .Set(u => u.NormalizedUserName, user.NormalizedUserName)
            .Set(u => u.PasswordHash, user.PasswordHash);

        _users.UpdateOne(filter, update, new UpdateOptions { IsUpsert = true }, cancellationToken);
        return Task.FromResult(IdentityResult.Success);
    }

    public void Dispose() { }
}