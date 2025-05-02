using FinanceTracker.Domain.Identity.Entities;
using FinanceTracker.Domain.Identity.Interfaces;
using FinanceTracker.Infra.Identity.Services;
using FinanceTracker.Infra.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace FinanceTracker.Application.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher<UserEntity> _passwordHasher;
    private readonly JwtTokenGenerator _jwtTokenGenerator;

    public AuthService
    (
        IUserRepository userRepository,
        IPasswordHasher<UserEntity> passwordHasher,
        JwtTokenGenerator jwtTokenGenerator
    )
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

    public async Task RegisterAsync(string user, string password, string role)
    {
        var existingUser = await _userRepository.GetByEmailAsync(user);
        if (existingUser != null)
        {
            throw new InvalidOperationException("User already exists.");
        }
        var hashedPassword = _passwordHasher.HashPassword(new UserEntity(), password);
        var newUser = new UserEntity
        {
            Email = user,
            PasswordHash = hashedPassword,
            Role = role
        };
        await _userRepository.AddAsync(newUser);
    }
}