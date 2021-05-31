using System.Threading;
using System.Threading.Tasks;
using DeliveryTracker.Model;

namespace DeliveryTracker.Business
{
    public interface IProcessCalculateDeviceLocationService
    {
        Task Handle(DeviceLocation deviceLocation, CancellationToken cancellationToken);
    }
}