using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using User.Data.Contracts.Helpers.DTO.User;
using User.Data.Object.Helpers.DTO.Device;
using User.Data.Object.Helpers.DTO.User;
using User.Services.Contracts;

namespace User.Microservice.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class UserController : ControllerBase {
    private readonly IUserService _userService;
    public UserController(IUserService userService) {
        _userService = userService;
    }

    [HttpGet("GetAllUsers")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAllUsersAsync() {
        try {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }
        catch (Exception ex) {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("GetAdminId")]
    [Authorize(Roles = "Admin, User")]
    public async Task<IActionResult> GetAdminIdAsync()
    {
        try
        {
            var adminId = await _userService.GetAdminIdAsync();
            return Ok(adminId);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("GetUsernames")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetUsernamesAsync()
    {
        try
        {
            var usernames = await _userService.GetUsernamesAsync();
            return Ok(usernames);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("AddUser")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> AddUserAsync(UserDto userDto) {
        try {
            await _userService.AddUserAsync(userDto);
            return Ok();
        }
        catch (Exception ex) {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("UpdateUser")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateUserAsync(UserDto userDto) {
        try {
            await _userService.UpdateUserAsync(userDto);
            return Ok();
        }
        catch (Exception ex) {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("AssignDeviceToUser")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> AssignDeviceToUserAsync(DeviceToUserDto deviceToUser) {
        try {
            await _userService.AssignDeviceToUserAsync(deviceToUser);
            return Ok();
        }
        catch (Exception ex) {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("RemoveDeviceFromUserByUserIdAndDeviceId")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> RemoveDeviceFromUserByUserIdAndDeviceIdAsync(DeviceToUserDto deviceToUserDto) {
        try {
            await _userService.RemoveDeviceFromUserByUsernameAndDeviceIdAsync(deviceToUserDto);
            return Ok();
        }
        catch (Exception ex) {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("RemoveDeviceFromUser")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> RemoveDeviceFromUserAsync(UserToDeviceDto userToDeviceDto) {
        try {
            await _userService.RemoveDeviceFromUserAsync(userToDeviceDto);
            return Ok();
        }
        catch (Exception ex) {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("DeleteUser")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteUserAsync(string username) {
        try {
            await _userService.DeleteUserByUsernameAsync(username);
            return Ok();
        }
        catch (Exception ex) {
            return BadRequest(ex.Message);
        }
    }
}
