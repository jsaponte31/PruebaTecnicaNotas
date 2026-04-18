using Microsoft.EntityFrameworkCore;
using SecurityService.Api.Data;
using SecurityService.Api.Authentication;
using SecurityService.Domain.DTOs;
using SecurityService.Domain.Entities;
using SecurityService.Domain.Interfaces;

namespace SecurityService.Api.Services;

public class AuthService : IAuthService
{
    private readonly SecurityDbContext _context;
    private readonly JwtProvider _jwtProvider;

    public AuthService(SecurityDbContext context, JwtProvider jwtProvider)
    {
        _context = context;
        _jwtProvider = jwtProvider;
    }

    public async Task<bool> RegisterAsync(LoginRequest request)
    {
        if (await _context.Users.AnyAsync(u => u.Username == request.Username))
            return false;

        var user = new User
        {
            Username = request.Username,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password)
        };

        _context.Users.Add(user);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<AuthResponse?> LoginAsync(LoginRequest request)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == request.Username);

        if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            return null;

        var token = _jwtProvider.Generate(user);
        return new AuthResponse(token, user.Username);
    }
}