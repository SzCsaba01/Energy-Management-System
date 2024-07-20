using User.Data.Contracts.Helpers.DTO.User;

namespace Device.Services.Contracts;
public interface ICommunicationService {
    public Task RemoveDeviceFromUserAsync(UserToDeviceDto userToDeviceDto);
}
