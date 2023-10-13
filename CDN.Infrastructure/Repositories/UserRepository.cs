using CDN.Application.Interfaces;
using CDN.Core.Entities;
using CDN.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CDN.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<UserRepository> _logger;

        public UserRepository(ILogger<UserRepository> logger, AppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            _logger.LogInformation("[Repository {Repository}, Method {Method}]: Get all users",
                nameof(UserRepository),
                nameof(GetAllUsersAsync));

            return await _context.Users.ToListAsync();
        }

        public async Task<User?> GetUserByIdAsync(string id)
        {
            _logger.LogInformation("[Repository {Repository}, Method {Method}]: Get user by Id: {id}",
                nameof(UserRepository),
                nameof(GetUserByIdAsync),
                id);

            return await _context.Users.FindAsync(id);
        }

        public async Task CreateUserAsync(User user)
        {
            _logger.LogInformation("[Repository {Repository}, Method {Method}]: Create user",
                nameof(UserRepository),
                nameof(CreateUserAsync));

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateUserAsync(User user)
        {
            _logger.LogInformation("[Repository {Repository}, Method {Method}]: Update user",
                nameof(UserRepository),
                nameof(UpdateUserAsync));


            var existingUser = await _context.Users.FindAsync(user.Id);
            if (existingUser != null)
            {
                existingUser.Username = user.Username;
                existingUser.Email = user.Email;
                existingUser.PhoneNumber = user.PhoneNumber;
                existingUser.Skillsets = user.Skillsets;
                existingUser.Hobbies = user.Hobbies;
                _context.Users.Update(existingUser);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteUserAsync(string id)
        {
            _logger.LogInformation("[Repository {Repository}, Method {Method}]: Delete user, Id: {id}",
                nameof(UserRepository),
                nameof(DeleteUserAsync),
                id);

            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
        }
    }
}