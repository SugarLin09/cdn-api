using CDN.Application.Interfaces;
using CDN.Application.Services;
using Microsoft.Extensions.Logging;
using CDN.Core.Entities;
using CDN.Application.Models;

namespace CDN.UnitTesting
{
    public class UserServiceTests
    {
        private UserService GetUserServiceTests(Mock<IUserRepository> mockUserRepository)
        {
            var mockLogger = new Mock<ILogger<UserService>>();
            return new UserService(mockLogger.Object, mockUserRepository.Object);
        }

        [Fact]
        public async Task GetAllUsersAsync_ShouldReturnAllUsers()
        {
            // Arrange
            var mockUserRepository = new Mock<IUserRepository>();
            var users = new List<User>
            {
                new User { Id = "1", Username = "JohnDoe", Email = "johndoe@cdn.com", PhoneNumber = "123", Skillsets = "Skill A, Skill B", Hobbies = "swimming" },
                new User { Id = "2", Username = "JaneDoe", Email = "janedoe@cdn.com", PhoneNumber = "123", Skillsets = "Skill A", Hobbies = "reading" }
            };
            mockUserRepository.Setup(repo => repo.GetAllUsersAsync()).ReturnsAsync(users);

            var service = GetUserServiceTests(mockUserRepository);

            // Act
            var result = await service.GetAllUsersAsync();

            // Assert
            var firstUser = result.FirstOrDefault();
            Assert.Equal(2, result.Count());
            Assert.Equal(users.First().Username, firstUser?.Username);
        }

        [Fact]
        public async Task GetUserByIdAsync_ShouldReturnUser_WhenUserExists()
        {
            // Arrange
            var mockUserRepository = new Mock<IUserRepository>();
            var userId = "foo";
            var user = new User { Id = userId, Username = "JohnDoe", Email = "johndoe@cdn.com", PhoneNumber = "123", Skillsets = "Skill A, Skill B", Hobbies = "swimming" };
            mockUserRepository.Setup(repo => repo.GetUserByIdAsync(userId)).ReturnsAsync(user);

            var service = GetUserServiceTests(mockUserRepository);

            // Act
            var result = await service.GetUserByIdAsync(userId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(userId, result.Id);
        }

        [Fact]
        public async Task CreateUserAsync_CreateUserCorrectly()
        {
            // Arrange
            var mockUserRepository = new Mock<IUserRepository>();
            var userDto = new UserDto { Id = "foo", Username = "JohnDoe", Email = "johndoe@cdn.com", PhoneNumber = "123", Skillsets = new List<string>{"Skill A", "Skill B"}, Hobbies = new List<string>{"swimming"} };
            var user = new User { Id = "foo", Username = "JohnDoe", Email = "johndoe@cdn.com", PhoneNumber = "123", Skillsets = "Skill A, Skill B", Hobbies = "swimming" };
            mockUserRepository.Setup(repo => repo.CreateUserAsync(It.IsAny<User>()));

            var service = GetUserServiceTests(mockUserRepository);

            // Act
            var result = await service.CreateUserAsync(userDto);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task UpdateUserAsync_UpdateUserCorrectly()
        {
            // Arrange
            var mockUserRepository = new Mock<IUserRepository>();
            var userDto = new UserDto { Id = "foo", Username = "JohnDoe", Email = "johndoe@cdn.com", PhoneNumber = "123456", Skillsets = new List<string>{"Skill A", "Skill B"}, Hobbies = new List<string>{"swimming", "reading"} };
            var user = new User { Id = "foo", Username = "JohnDoe", Email = "johndoe@cdn.com", PhoneNumber = "123456", Skillsets = "Skill A, Skill B", Hobbies = "swimming, reading" };
            mockUserRepository.Setup(repo => repo.UpdateUserAsync(user));

            var service = GetUserServiceTests(mockUserRepository);

            // Act
            var result = await service.UpdateUserAsync(userDto);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task DeleteUserAsync_DeleteeUserCorrectly()
        {
            // Arrange
            var mockUserRepository = new Mock<IUserRepository>();
            var userId = "foo";
            var user = new User { Id = userId, Username = "JohnDoe", Email = "johndoe@cdn.com", PhoneNumber = "123456", Skillsets = "Skill A, Skill B", Hobbies = "swimming, reading" };
            mockUserRepository.Setup(repo => repo.CreateUserAsync(user));
            mockUserRepository.Setup(repo => repo.DeleteUserAsync(userId));

            var service = GetUserServiceTests(mockUserRepository);

            // Act
            var result = await service.DeleteUserAsync(userId);

            // Assert
            Assert.True(result);
        }

    }
}