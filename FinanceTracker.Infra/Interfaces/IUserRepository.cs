using FinanceTracker.Domain.Entities;

namespace FinanceTracker.Infra.Interfaces
{
    public interface IUserRepository
    {
        Task<UserEntity> GetUserByIdAsync(Guid id);
        Task AddAsync(UserEntity user);
    }
}