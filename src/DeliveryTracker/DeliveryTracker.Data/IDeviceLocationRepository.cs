using System.Collections.Generic;
using System.Threading.Tasks;
using DeliveryTracker.Model;

namespace DeliveryTracker.Data
{
    public interface IDeviceLocationRepository
    {
        Task AddDeviceLocation(DeviceLocation deviceLocation);

         Task<DeviceLocation> GetDeviceLocation(string macId);

        Task<IReadOnlyCollection<DeviceLocation>> GetDeviceLocations(string[] macId);
    }
}
