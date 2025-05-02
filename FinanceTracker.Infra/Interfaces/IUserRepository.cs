using FinanceTracker.Domain.Identity.Entities;

namespace FinanceTracker.Infra.Interfaces
{
    public interface IUserRepository
    {
        Task<UserEntity> GetUserByIdAsync(string id);
        Task<UserEntity> GetByEmailAsync(string email);
        Task AddAsync(UserEntity user);
    }
}