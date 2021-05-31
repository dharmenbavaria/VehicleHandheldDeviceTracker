using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DeliveryTracker.Data;
using DeliveryTracker.Messaging;
using DeliveryTracker.Model;

namespace DeliveryTracker.Business
{
    public class SendAlertToPostnlService : ISendAlertToPostnlService
    {
        private readonly IAlertedVehicleHandheldRepository _alertedVehicleHandheldRepository;
        private readonly IPostnlAlertMessagePublishingClient _postnlAlertMessagePublishingClient;

        public SendAlertToPostnlService(IAlertedVehicleHandheldRepository alertedVehicleHandheldRepository,
            IPostnlAlertMessagePublishingClient postnlAlertMessagePublishingClient)
        {
            _alertedVehicleHandheldRepository = alertedVehicleHandheldRepository;
            _postnlAlertMessagePublishingClient = postnlAlertMessagePublishingClient;
        }

        public async Task Handle(AlertedVehicleHandheld alertedVehicleHandheld, CancellationToken cancellationToken)
        {
            var alertVehicleHandheld =
                _alertedVehicleHandheldRepository.GetAlertVehicleHandheld(alertedVehicleHandheld.MacId,
                    alertedVehicleHandheld.LinkedMacId);

            if (alertedVehicleHandheld.IsMoreThanAllowedDistance)
            {
                if (alertVehicleHandheld == null)
                {
                    await _alertedVehicleHandheldRepository.AddAlertVehicleHandheld(alertedVehicleHandheld);
                    await _postnlAlertMessagePublishingClient.PublishAsync(new PostNlMessage()
                    {
                        LinkedLat = alertedVehicleHandheld.LinkMacLat,
                        LinkedLong = alertedVehicleHandheld.LinkMacLong,
                        MacId = alertedVehicleHandheld.MacId,
                        LinkedDeviceMacId = alertedVehicleHandheld.LinkedMacId,
                        MacLat = alertedVehicleHandheld.MacLat,
                        MacLong = alertedVehicleHandheld.MacLong
                    }, cancellationToken);
                }
            }
            else
            {
                if (alertVehicleHandheld != null)
                {
                    await _alertedVehicleHandheldRepository.RemoveAlertVehicleHandheld(alertedVehicleHandheld);
                }
            }
        }
    }
}
