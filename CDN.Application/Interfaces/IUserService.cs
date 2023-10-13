using CDN.Application.Models;

namespace CDN.Application.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserDto>> GetAllUsersAsync();
        Task<UserDto?> GetUserByIdAsync(string userId);
        Task<bool> CreateUserAsync(UserDto userDto);
        Task<bool> UpdateUserAsync(UserDto userDto);
        Task<bool> DeleteUserAsync(string userId);
    }
}