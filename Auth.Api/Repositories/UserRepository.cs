using Auth.Api.Data;
using Auth.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Auth.Api.Repositories;

public class UserRepository(AuthDbContext dbContext) : IUserRepository
{
    private readonly AuthDbContext _context = dbContext;

    public async Task AddAsync(User user, CancellationToken cancellationToken)
    {
        await _context.Users.AddAsync(user, cancellationToken);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken)
    {
        return await _context.Users.AnyAsync(u => u.Email == email, cancellationToken);
    }

    public async Task<User?> GetByEmail(string email, CancellationToken cancellationToken)
    {
        return await _context.Users.FirstOrDefaultAsync(user =>  user.Email == email, cancellationToken);
    }
}
