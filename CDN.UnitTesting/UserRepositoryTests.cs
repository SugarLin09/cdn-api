using CDN.Infrastructure.Repositories;
using CDN.Core.Entities;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using CDN.Infrastructure.Data;

namespace CDN.UnitTesting
{
    public class UserRepositoryTests
    {
        private readonly Mock<ILogger<UserRepository>> mockLogger;
        private AppDbContext mockAppDbContext;

        public UserRepositoryTests()
        {
            mockLogger = new Mock<ILogger<UserRepository>>();
            var _dbContextOptionsMock = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
            mockAppDbContext = new AppDbContext(_dbContextOptionsMock);
        }

        private void DisposeDBCOntext()
        {
            mockAppDbContext.Database.EnsureCreated();
            mockAppDbContext.Dispose();
        }

        [Fact]
        public async Task GetAllUsersAsync_ReturnAllUsers()
        {
            // Arrange
            var users = new List<User>
            {
                new User { Id = "1", Username = "JohnDoe", Email = "johndoe@cdn.com", PhoneNumber = "123", Skillsets = "Skill A, Skill B", Hobbies = "swimming" },
                new User { Id = "2", Username = "JaneDoe", Email = "janedoe@cdn.com", PhoneNumber = "123", Skillsets = "Skill A", Hobbies = "reading" }
            };

            mockAppDbContext.Users.AddRange(users);
            await mockAppDbContext.SaveChangesAsync();

            var userRepository = new UserRepository(mockLogger.Object, mockAppDbContext);

            // Act
            var result = await userRepository.GetAllUsersAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());

            DisposeDBCOntext();
        }

        [Fact]
        public async Task GetUserByIdAsync_ReturnCorrectUser()
        {
            // Arrange
            var userId = "1";
            var user = new User { Id = userId, Username = "JohnDoe", Email = "johndoe@cdn.com", PhoneNumber = "123", Skillsets = "Skill A, Skill B", Hobbies = "swimming" };
            mockAppDbContext.Users.AddRange(user);
            await mockAppDbContext.SaveChangesAsync();

            var userRepository = new UserRepository(mockLogger.Object, mockAppDbContext);

            // Act
            var result = await userRepository.GetUserByIdAsync(userId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(userId, result.Id);


            DisposeDBCOntext();
        }

        [Fact]
        public async Task CreateUserAsync_CreateUserCorrectly()
        {
            // Arrange
            var newUser = new User { Id = "4", Username = "BobDoe", Email = "bobdoe@cdn.com", PhoneNumber = "123", Skillsets = "Skill A, Skill B", Hobbies = "swimming, reading" };
            var userRepository = new UserRepository(mockLogger.Object, mockAppDbContext);

            // Act
            await userRepository.CreateUserAsync(newUser);

            // Assert
            var userFromDb = mockAppDbContext.Users.Find("4");
            Assert.NotNull(userFromDb);
            Assert.Equal("BobDoe", userFromDb.Username);


            DisposeDBCOntext();
        }

        [Fact]
        public async Task UpdateUserAsync_UpdateUserCorrectly()
        {
            // Arrange
            var originalUser = new User { Id = "5", Username = "OriginalName", Email = "original@email.com", PhoneNumber = "123", Skillsets = "Skill A, Skill B", Hobbies = "swimming, reading" };
            var updatedUser = new User { Id = "5", Username = "UpdatedName", Email = "updated@email.com", PhoneNumber = "123", Skillsets = "Skill A, Skill B", Hobbies = "swimming, reading" };
            mockAppDbContext.Users.Add(originalUser);
            await mockAppDbContext.SaveChangesAsync();

            var userRepository = new UserRepository(mockLogger.Object, mockAppDbContext);

            // Act
            await userRepository.UpdateUserAsync(updatedUser);

            // Assert
            var userFromDb = mockAppDbContext.Users.Find("5");
            Assert.Equal("UpdatedName", userFromDb?.Username);
            Assert.Equal("updated@email.com", userFromDb?.Email);


            DisposeDBCOntext();
        }

        [Fact]
        public async Task DeleteUserAsync_DeletesUserCorrectly()
        {
            // Arrange
            var userToDelete = new User { Id = "6", Username = "DeleteMe", Email = "deleteme@email.com", PhoneNumber = "123", Skillsets = "Skill A, Skill B", Hobbies = "swimming, reading" };
            mockAppDbContext.Users.Add(userToDelete);
            await mockAppDbContext.SaveChangesAsync();

            var userRepository = new UserRepository(mockLogger.Object, mockAppDbContext);

            // Act
            await userRepository.DeleteUserAsync("6");

            // Assert
            var deletedUser = mockAppDbContext.Users.Find("6");
            Assert.Null(deletedUser);

            DisposeDBCOntext();
        }


    }

}

