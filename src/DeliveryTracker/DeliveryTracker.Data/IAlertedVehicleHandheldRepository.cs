using System.Threading.Tasks;
using DeliveryTracker.Model;

namespace DeliveryTracker.Data
{
    public interface IAlertedVehicleHandheldRepository
    {
        Task AddAlertVehicleHandheld(AlertedVehicleHandheld alertedVehicleHandheld);

        Task<AlertedVehicleHandheld> GetAlertVehicleHandheld(string vehicleMacId, string deviceMacId);

        Task RemoveAlertVehicleHandheld(AlertedVehicleHandheld alertedVehicleHandheld);

    }
}