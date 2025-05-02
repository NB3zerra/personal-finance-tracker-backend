using FinanceTracker.Infra.Interfaces;
using FinanceTracker.Domain.Identity.Entities;
using MongoDB.Driver;

namespace FinanceTracker.Infra.Identity.Repositories
{
    public class UserRepository : IUserRepository
    { 
        private readonly IMongoCollection<UserEntity> _users;

        public UserRepository(IMongoDatabase database)
        {
            _users = database.GetCollection<UserEntity>("Users");
        }

        public async Task<UserEntity> GetUserByIdAsync(string id)
        {
            return await _users.Find(user => user.Id == id).FirstOrDefaultAsync();
        }

        public async Task<UserEntity> GetByEmailAsync(string email)
        {
            return await _users.Find(user => user.Email == email).FirstOrDefaultAsync();
        }

        public async Task AddAsync(UserEntity user)
        {
            await _users.InsertOneAsync(user);
        }
    }
}