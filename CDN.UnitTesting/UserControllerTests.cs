using CDN.Application.Models;
using CDN.Api.Controllers;
using Microsoft.Extensions.Logging;
using CDN.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CDN.UnitTesting
{
    public class UserControllerTests
    {
        private UserController GetUserControllerTests(Mock<IUserService> mockUserService)
        {
            var mockLogger = new Mock<ILogger<UserController>>();
            return new UserController(mockLogger.Object, mockUserService.Object);
        }

        [Fact]
        public async Task GetAllUsers_ShouldReturnAllUsers()
        {
            // Arrange
            var mockUserService = new Mock<IUserService>();
            var users = new List<UserDto>
            {
                new UserDto { Id = "1", Username = "JohnDoe", Email = "johndoe@cdn.com", PhoneNumber = "123", Skillsets = new List<string>{"Skill A", "Skill B"}, Hobbies = new List<string>{"swimming"} },
                new UserDto { Id = "2", Username = "JaneDoe", Email = "janedoe@cdn.com", PhoneNumber = "123", Skillsets = new List<string>{"Skill A"}, Hobbies = new List<string>{"reading"} }
            };
            mockUserService.Setup(service => service.GetAllUsersAsync()).ReturnsAsync(users);

            var controller = GetUserControllerTests(mockUserService);

            // Act
            var result = await controller.GetAllUsers();

            // Assert
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result.Result);

            var dataFromResult = (OkObjectResult)result.Result;

            var firstUser = (dataFromResult.Value as IEnumerable<UserDto>).FirstOrDefault();
            Assert.Equal(2, (dataFromResult.Value as IEnumerable<UserDto>).Count());
            Assert.Equal(users.First().Username, firstUser?.Username);
        }

        [Fact]
        public async Task GetUserById_ShouldReturnUser_WhenUserExists()
        {
            // Arrange
            var mockUserService = new Mock<IUserService>();
            var userId = "foo";
            var userDto = new UserDto { Id = userId, Username = "JohnDoe", Email = "johndoe@cdn.com", PhoneNumber = "123", Skillsets = new List<string>{"Skill A", "Skill B"}, Hobbies = new List<string>{"swimming"} };
            mockUserService.Setup(service => service.GetUserByIdAsync(userId)).ReturnsAsync(userDto);

            var controller = GetUserControllerTests(mockUserService);

            // Act
            var result = await controller.GetUserById(userId);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result.Result);

            var dataFromResult = (OkObjectResult)result.Result;
            Assert.Equal(userId, (dataFromResult.Value as UserDto).Id);
        }

        [Fact]
        public async Task GetUserById_ShouldReturnNotFound_WhenUserNotExists()
        {
            // Arrange
            var mockUserService = new Mock<IUserService>();
            mockUserService.Setup(service => service.GetUserByIdAsync(It.IsAny<string>())).ReturnsAsync(() => null);

            var controller = GetUserControllerTests(mockUserService);

            // Act
            var result = await controller.GetUserById("1");

            // Assert
            Assert.NotNull(result);
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task CreateUser_CreateUserCorrectly()
        {
            // Arrange
            var mockService = new Mock<IUserService>();
            var userDto = new UserDto { Id = "foo", Username = "JohnDoe", Email = "johndoe@cdn.com", PhoneNumber = "123", Skillsets = new List<string>{"Skill A", "Skill B"}, Hobbies = new List<string>{"swimming"} };
            mockService.Setup(service => service.CreateUserAsync(It.IsAny<UserDto>()));

            var controller = GetUserControllerTests(mockService);

            // Act
            var result = await controller.CreateUser(userDto);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result.Result);

            var dataFromResult = (OkObjectResult)result.Result;
            Assert.Equal("foo", (dataFromResult.Value as UserDto).Id);
        }

        [Fact]
        public async Task UpdateUser_UpdateUserCorrectly_WhenUserExists()
        {
            // Arrange
            var mockUserService = new Mock<IUserService>();
            var userDto = new UserDto { Id = "foo", Username = "JohnDoe", Email = "johndoe@cdn.com", PhoneNumber = "123456", Skillsets = new List<string>{"Skill A", "Skill B"}, Hobbies = new List<string>{"swimming", "reading"} };
            mockUserService.Setup(service => service.UpdateUserAsync(userDto));
            mockUserService.Setup(service => service.GetUserByIdAsync("foo")).ReturnsAsync(userDto);

            var controller = GetUserControllerTests(mockUserService);

            // Act
            var result = await controller.UpdateUser("foo", userDto);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result.Result);

            var dataFromResult = (OkObjectResult)result.Result;
            Assert.Equal("foo", (dataFromResult.Value as UserDto).Id);
        }

        [Fact]
        public async Task UpdateUser_ShouldReturnNotFound_WhenUserNotExists()
        {
            // Arrange
            var mockUserService = new Mock<IUserService>();
            var userDto = new UserDto { Id = "foo", Username = "JohnDoe", Email = "johndoe@cdn.com", PhoneNumber = "123456", Skillsets = new List<string>{"Skill A", "Skill B"}, Hobbies = new List<string>{"swimming", "reading"} };
            mockUserService.Setup(service => service.UpdateUserAsync(userDto));
            mockUserService.Setup(service => service.GetUserByIdAsync("foo")).ReturnsAsync(() => null);

            var controller = GetUserControllerTests(mockUserService);

            // Act
            var result = await controller.UpdateUser("foo", userDto);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task DeleteUser_DeleteUserCorrectly_WhenUserExists()
        {
            // Arrange
            var mockUserService = new Mock<IUserService>();
            var userDto = new UserDto { Id = "foo", Username = "JohnDoe", Email = "johndoe@cdn.com", PhoneNumber = "123456", Skillsets = new List<string>{"Skill A", "Skill B"}, Hobbies = new List<string>{"swimming", "reading"} };
            mockUserService.Setup(service => service.DeleteUserAsync("foo"));
            mockUserService.Setup(service => service.GetUserByIdAsync("foo")).ReturnsAsync(userDto);

            var controller = GetUserControllerTests(mockUserService);

            // Act
            var result = await controller.DeleteUser("foo");

            // Assert
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result.Result);

            var dataFromResult = (OkObjectResult)result.Result;
            Assert.Equal("foo", (dataFromResult.Value as UserDto).Id);
        }

        [Fact]
        public async Task DeleteUser_ShouldReturnNotFound_WhenUserNotExists()
        {
            // Arrange
            var mockUserService = new Mock<IUserService>();
            var userDto = new UserDto { Id = "foo", Username = "JohnDoe", Email = "johndoe@cdn.com", PhoneNumber = "123456", Skillsets = new List<string>{"Skill A", "Skill B"}, Hobbies = new List<string>{"swimming", "reading"} };
            mockUserService.Setup(service => service.DeleteUserAsync("foo"));
            mockUserService.Setup(service => service.GetUserByIdAsync("foo")).ReturnsAsync(() => null);

            var controller = GetUserControllerTests(mockUserService);

            // Act
            var result = await controller.DeleteUser("foo");

            // Assert
            Assert.NotNull(result);
            Assert.IsType<NotFoundResult>(result.Result);
        }
    }
}