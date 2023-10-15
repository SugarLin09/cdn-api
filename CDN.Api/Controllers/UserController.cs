using CDN.Application.Interfaces;
using CDN.Application.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace CDN.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly ILogger<UserController> _logger;

    public UserController(ILogger<UserController> logger, IUserService userService)
    {
        _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [HttpGet(Name = "GetAllUsers")]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetAllUsers()
    {
        _logger.LogInformation("[Controller {Controller}, Method {Method}]: Get all users",
            nameof(UserController),
            nameof(GetAllUsers));

        var users = await _userService.GetAllUsersAsync();
        return Ok(users);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<UserDto>> GetUserById(string id)
    {
        _logger.LogInformation("[Controller {Controller}, Method {Method}]: Get user by Id: {id}",
            nameof(UserController),
            nameof(GetUserById),
            id);

        var userDto = await _userService.GetUserByIdAsync(id);
        return userDto == null? NotFound(): Ok(userDto);
    }

    [HttpPost]
    public async Task<ActionResult<UserDto>> CreateUser(UserDto userDto)
    {
        _logger.LogInformation("[Controller {Controller}, Method {Method}]: Create user",
            nameof(UserController),
            nameof(CreateUser));

        await _userService.CreateUserAsync(userDto);
        return Ok(userDto);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<UserDto>> UpdateUser(string id, UserDto userDto)
    {
        _logger.LogInformation("[Controller {Controller}, Method {Method}]: Update user, Id: {id}",
            nameof(UserController),
            nameof(UpdateUser),
            id);

        var existingUser = await _userService.GetUserByIdAsync(id);
        if (existingUser == null) return NotFound();

        await _userService.UpdateUserAsync(userDto);
        return Ok(userDto);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<UserDto>> DeleteUser(string id)
    {
        _logger.LogInformation("[Controller {Controller}, Method {Method}]: Delete user, Id: {id}",
            nameof(UserController),
            nameof(DeleteUser),
            id);

        var userDto = await _userService.GetUserByIdAsync(id);
        if (userDto == null) return NotFound();

        await _userService.DeleteUserAsync(id);
        return Ok(userDto);
    }
}
