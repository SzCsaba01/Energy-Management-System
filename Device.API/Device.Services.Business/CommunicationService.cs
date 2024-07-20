using Device.Data.Objects.Helpers;
using Device.Services.Contracts;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using User.Data.Contracts.Helpers.DTO.User;

namespace Device.Services.Business;
public class CommunicationService : ICommunicationService {
    private readonly HttpClient _httpClient;
    private readonly IHttpContextAccessor _httpContextAccessor;
    public CommunicationService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor = null)
    {
        _httpClient = httpClient;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task RemoveDeviceFromUserAsync(UserToDeviceDto userToDeviceDto) {
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", GetAccessToken());

        var content = new StringContent(JsonSerializer.Serialize(userToDeviceDto), Encoding.UTF8, "application/json");
        HttpResponseMessage response =
            await _httpClient.PutAsync(Constants.USER_API_URL + "RemoveDeviceFromUser", content);

        if (!response.IsSuccessStatusCode) {
            throw new Exception("Error while removing user from device!");
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
