using CDN.Application.Interfaces;
using CDN.Application.Models;
using CDN.Core.Entities;
using Microsoft.Extensions.Logging;

namespace CDN.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<UserService> _logger;

        public UserService(ILogger<UserService> logger, IUserRepository userRepository)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            _logger.LogInformation("[Service {Service}, Method {Method}]: Get all users",
                nameof(UserService),
                nameof(GetAllUsersAsync));

            var users = await _userRepository.GetAllUsersAsync();
            return users.Select(user => ConvertToUserDto(user));
        }

        public async Task<UserDto?> GetUserByIdAsync(string userId)
        {
            _logger.LogInformation("[Service {Service}, Method {Method}]: Get user by Id: {userId}",
                nameof(UserService),
                nameof(GetUserByIdAsync),
                userId);

            var user = await _userRepository.GetUserByIdAsync(userId);
            return user != null? ConvertToUserDto(user): null;
        }

        public async Task<bool> CreateUserAsync(UserDto userDto)
        {
            _logger.LogInformation("[Service {Service}, Method {Method}]: Create user",
                nameof(UserService),
                nameof(CreateUserAsync));

            await _userRepository.CreateUserAsync(ConvertToUserEntity(userDto));
            return true;
        }

        public async Task<bool> UpdateUserAsync(UserDto userDto)
        {
            _logger.LogInformation("[Service {Service}, Method {Method}]: Update user",
                nameof(UserService),
                nameof(UpdateUserAsync));

            await _userRepository.UpdateUserAsync(ConvertToUserEntity(userDto));
            return true;
        }

        public async Task<bool> DeleteUserAsync(string userId)
        {
            _logger.LogInformation("[Service {Service}, Method {Method}]: Delete user, Id: {userId}",
                nameof(UserService),
                nameof(DeleteUserAsync),
                userId);

            await _userRepository.DeleteUserAsync(userId);
            return true;
        }

        private static UserDto ConvertToUserDto(User user)
        {
            return new UserDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Skillsets = user.Skillsets?.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList() ?? new List<string>(),
                Hobbies = user.Hobbies?.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList() ?? new List<string>()
            };
        }

        private static User ConvertToUserEntity(UserDto userDto)
        {
            return new User
            {
                Id = userDto.Id,
                Username = userDto.Username,
                Email = userDto.Email,
                PhoneNumber = userDto.PhoneNumber,
                Skillsets = string.Join(',', userDto.Skillsets),
                Hobbies = string.Join(',', userDto.Hobbies)
            };
        }
    }
}