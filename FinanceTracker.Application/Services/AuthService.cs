using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace FinanceTracker.Application.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher<UserEntity> _passwordHasher;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public AuthService(IUserRepository userRepository, IPasswordHasher<UserEntity> passwordHasher, IJwtTokenGenerator jwtTokenGenerator)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public async Task<string> AuthenticateAsync(string email, string password)
    {
        var user = await _userRepository.GetByEmailAsync(email);
        if (user == null || _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password) != PasswordVerificationResult.Success)
        {
            throw new UnauthorizedAccessException("Invalid credentials.");
        }

        return _jwtTokenGenerator.GenerateToken(user);
    }

    public async Task RegisterAsync(UserEntity user, string password)
    {
        user.PasswordHash = _passwordHasher.HashPassword(user, password);
        await _userRepository.AddAsync(user);
    }
}