using System.Threading;
using System.Threading.Tasks;
using DeliveryTracker.Model;

namespace DeliveryTracker.Business
{
    public interface ISendAlertToPostnlService
    {
        Task Handle(AlertedVehicleHandheld alertedVehicleHandheld, CancellationToken cancellationToken);
    }
}