
using Device.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using User.Data.Contracts.Helpers.DTO.Device;
using User.Data.Contracts.Helpers.DTO.User;

namespace Device.Microservice.Controllers;
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class DeviceController : ControllerBase {
    private readonly IDeviceService _deviceService;
    public DeviceController(IDeviceService deviceService) {
        _deviceService = deviceService;
    }

    [HttpPost("AddDevice")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> AddDeviceAsync(DeviceDto deviceDto) {
        try {
            return Ok(await _deviceService.AddDeviceAsync(deviceDto));
        } catch (Exception e) {
            return BadRequest(e.Message);
        }
    }

    [HttpGet("GetAllDevices")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAllDevices() {
        try {
            var devices = await _deviceService.GetAllDevices();
            return Ok(devices);
        } catch (Exception e) {
            return BadRequest(e.Message);
        }
    }

    [HttpGet("GetDevicesByUserId")]
    [Authorize(Roles = "Admin, User")]
    public async Task<IActionResult> GetDevicesByUserId(Guid userId) {
        try {
            var devices = await _deviceService.GetDevicesByUserId(userId);
            return Ok(devices);
        } catch (Exception e) {
            return BadRequest(e.Message);
        }
    }

    [HttpGet("GetUnassignedDevices")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetUnassignedDevices()
    {
        try
        {
            var devices = await _deviceService.GetUnassignedDevices();
            return Ok(devices);
        } catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPut("AssignUserToDevice")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> AssignUserToDeviceAsync(UserToDeviceDto deviceToUserDto) {
        try {
            await _deviceService.AssignUserToDeviceAsync(deviceToUserDto);
            return Ok();
        } catch (Exception e) {
            return BadRequest(e.Message);
        }
    }

    [HttpPut("UpdateDevice")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateDeviceAsync(DeviceDto deviceDto) {
        try {
            await _deviceService.UpdateDeviceAsync(deviceDto);
            return Ok();
        } catch (Exception e) {
            return BadRequest(e.Message);
        }
    }

    [HttpPut("RemoveUserFromAllDevicesByUserId")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> RemoveUserFromAllDevicesByUserIdAsync([FromBody]Guid userId)
    {
        try
        {
            await _deviceService.RemoveUserFromAllDevicesByUserIdAsync(userId);
            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPut("RemoveUserFromDevice")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> RemoveUserFromDevice(UserToDeviceDto deviceToUserDto) {
        try {
            await _deviceService.RemoveUserFromDeviceAsync(deviceToUserDto);
            return Ok();
        } catch (Exception e) {
            return BadRequest(e.Message);
        }
    }

    [HttpDelete("DeleteDeviceById")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteDeviceByIdAsync(Guid id) {
        try {
            await _deviceService.DeleteDeviceByIdAsync(id);
            return Ok();
        } catch (Exception e) {
            return BadRequest(e.Message);
        }
    }
}
