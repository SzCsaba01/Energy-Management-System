namespace User.Data.Contracts.Helpers.DTO.Device;
public class DeviceDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Address { get; set; }
    public int MaxHourlyEnergyConsumption { get; set; }
}
