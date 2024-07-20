using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using User.Data.Object.Helpers;
using User.Data.Object.Helpers.DTO.Device;
using User.Data.Object.Helpers.DTO.User;
using User.Services.Contracts;

namespace User.Services.Business;
public class CommunicationService : ICommunicationService {
    private readonly HttpClient _httpClient;
    private readonly IHttpContextAccessor _httpContextAccessor;
    public CommunicationService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor = null)
    {
        _httpClient = httpClient;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<List<DeviceDto>> GetDevicesByUserIdAsync(Guid id) {
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", GetAccessToken());

        HttpResponseMessage response =
            await _httpClient.GetAsync(Constants.DEVICE_API_URL + $"GetDevicesByUserId?userId={id}");

        if (!response.IsSuccessStatusCode) {
            throw new Exception("Error while assigning user to device!");
        }

        return JsonSerializer.Deserialize<List<DeviceDto>>(await response.Content.ReadAsStringAsync(), new JsonSerializerOptions {
            PropertyNameCaseInsensitive = true
        });
    }

    public async Task AssignUserToDeviceAsync(UserToDeviceDto userToDeviceDto) {
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", GetAccessToken());

        var content = new StringContent(JsonSerializer.Serialize(userToDeviceDto), Encoding.UTF8, "application/json");
        HttpResponseMessage response =
            await _httpClient.PutAsync(Constants.DEVICE_API_URL + "AssignUserToDevice", content);

        if (!response.IsSuccessStatusCode) {
            throw new Exception("Error while assigning user to device!");
        }
    }

    public async Task RemoveUserFromDeviceAsync(UserToDeviceDto userToDeviceDto) {
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", GetAccessToken());

        var content = new StringContent(JsonSerializer.Serialize(userToDeviceDto), Encoding.UTF8, "application/json");
        HttpResponseMessage response =
            await _httpClient.PutAsync(Constants.DEVICE_API_URL + "RemoveUserFromDevice", content);

        if (!response.IsSuccessStatusCode) {
              throw new Exception("Error while removing user from device!");
        }
    }

    public async Task RemoveUserFromAllDevicesByUserIdAsync(Guid userId) {
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", GetAccessToken());

        var content = new StringContent(JsonSerializer.Serialize(userId), Encoding.UTF8, "application/json");
        HttpResponseMessage response =
            await _httpClient.PutAsync(Constants.DEVICE_API_URL + "RemoveUserFromAllDevicesByUserId", content);

        if (!response.IsSuccessStatusCode) {
              throw new Exception("Error while deleting all devices by user id!");
        }
    }

    private string GetAccessToken()
    {
        var context = _httpContextAccessor.HttpContext;

        var authorizationHeader = context.Request.Headers["Authorization"].ToString();

        if (!string.IsNullOrEmpty(authorizationHeader) && authorizationHeader.StartsWith("bearer "))
        {
            return authorizationHeader.Substring("bearer ".Length);
        }

        throw new InvalidOperationException("Invalid Authorization header format");
    }
}
