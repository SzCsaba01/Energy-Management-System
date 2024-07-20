using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MonitoringAndCommunication.Services.Contracts;

namespace MonitoringAndCommunication.Microservice.Controllers;
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class MonitoringController : ControllerBase
{
    private readonly IMonitoringService _monitoringService;
    public MonitoringController(IMonitoringService monitoringService)
    {
        _monitoringService = monitoringService;
    }

    [HttpPost("GetMonitoringsByDeviceIds")]
    [Authorize(Roles = "Admin, User")]
    public async Task<IActionResult> GetMonitoringsByDeviceIdsAsync([FromBody] ICollection<Guid> deviceIds)
    {
        return Ok(await _monitoringService.GetMonitoringsByDeviceIds(deviceIds));
    }

}
