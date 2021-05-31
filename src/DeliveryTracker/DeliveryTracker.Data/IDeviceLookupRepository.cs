using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using DeliveryTracker.Model;

namespace DeliveryTracker.Data
{
    public interface IDeviceLookupRepository
    {
        Task<IReadOnlyCollection<DeviceLookup>> GetDeviceLinked(string macId);
    }

}