using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DeliveryTracker.Data;
using DeliveryTracker.Environment;
using DeliveryTracker.Messaging;
using DeliveryTracker.Model;
using Moq;
using Xunit;

namespace DeliveryTracker.Business.Tests
{
    public class FunctionTest
    {
        private MockRepository _mockRepository = new MockRepository(MockBehavior.Strict);
        private Mock<ICalculateLocationDistanceService> _calculateLocationDistanceServiceMock;
        private Mock<IDeviceLocationRepository> _deviceLocationRepoMock;
        private Mock<IDeviceLookupRepository> _deviceLookupRepoMock;
        private Mock<IDistanceMoreMessagePublishingClient> _distanceMorePublishClient;


        [Fact]
        public async Task DeviceLocationDistanceMoreThanAllowedShouldPublishAlertVehicleHandHeldMessage()
        {
            var processCalculateDeviceLocationService = Setup(50);
            _deviceLocationRepoMock.Setup(p => p.AddDeviceLocation(It.IsAny<DeviceLocation>())).Returns(Task.FromResult(true));
            _deviceLookupRepoMock.Setup(p => p.GetDeviceLinked("def-1")).ReturnsAsync(new List<DeviceLookup>()
            {
                new DeviceLookup() { Mac = "def-1", LinkedDeviceMac = "abc-1" }
            });
            _deviceLocationRepoMock.Setup(p => p.GetDeviceLocations(new string[] { "abc-1" })).ReturnsAsync(
                new List<DeviceLocation>()
                {
                    new DeviceLocation()
                    {
                        Longitude = 10,
                        Latitude = 10,
                        DeviceMacId = "abc-1"
                    }
                });


            _calculateLocationDistanceServiceMock
                .Setup(p =>
                    p.HaversineDistance(It.IsAny<GeoCordinate>()
                            , It.IsAny<GeoCordinate>())).Returns(60);


            /*new AlertedVehicleHandheld()
                {
                    IsMoreThanAllowedDistance = true,
                    LinkedMacId = "abc-1",
                    MacId = "def-1",
                    LinkMacLat = 10,
                    LinkMacLong = 10,
                    MacLat = 20,
                    MacLong = 20
                }*/

            _distanceMorePublishClient
                .Setup(p => p.PublishAsync(
                    It.IsAny<AlertedVehicleHandheld>()
                    , CancellationToken.None))
                .ReturnsAsync(() => "abc-1");

            await processCalculateDeviceLocationService.Handle(new DeviceLocation()
            {
                Latitude = 20,
                Longitude = 20,
                DeviceMacId = "def-1"
            }, CancellationToken.None);


        }

        [Fact]
        public async Task DeviceLocationDistanceNotMoreThanAllowedShouldPublishAlertVehicleHandHeldMessage()
        {
            var processCalculateDeviceLocationService = Setup(100);
            _deviceLocationRepoMock.Setup(p => p.AddDeviceLocation(It.IsAny<DeviceLocation>())).Returns(Task.FromResult(true));
            _deviceLookupRepoMock.Setup(p => p.GetDeviceLinked("def-1")).ReturnsAsync(new List<DeviceLookup>()
            {
                new DeviceLookup() { Mac = "def-1", LinkedDeviceMac = "abc-1" }
            });
            _deviceLocationRepoMock.Setup(p => p.GetDeviceLocations(new string[] { "abc-1" })).ReturnsAsync(
                new List<DeviceLocation>()
                {
                    new DeviceLocation()
                    {
                        Longitude = 10,
                        Latitude = 10,
                        DeviceMacId = "abc-1"
                    }
                });


            _calculateLocationDistanceServiceMock
                .Setup(p =>
                    p.HaversineDistance(It.IsAny<GeoCordinate>()
                        , It.IsAny<GeoCordinate>())).Returns(30);


            /*
             * new AlertedVehicleHandheld
                    {
                        IsMoreThanAllowedDistance = false,
                        LinkedMacId = "abc-1",
                        MacId = "def-1",
                        LinkMacLat = 10,
                        LinkMacLong = 10,
                        MacLat = 20,
                        MacLong = 20
                    }
             */
            _distanceMorePublishClient
                .Setup(p => p.PublishAsync(
                    It.IsAny<AlertedVehicleHandheld>()
                    , CancellationToken.None))
                .ReturnsAsync(() => "abc-1");

            await processCalculateDeviceLocationService.Handle(new DeviceLocation()
            {
                Latitude = 20,
                Longitude = 20,
                DeviceMacId = "def-1"
            }, CancellationToken.None);
        }

        [Fact]
        public async Task DeviceLocationWithNoLinkShouldNotPublishMessage()
        {
            var processCalculateDeviceLocationService = Setup(100);
            _deviceLocationRepoMock.Setup(p => p.AddDeviceLocation(It.IsAny<DeviceLocation>())).Returns(Task.FromResult(true));
            _deviceLookupRepoMock.Setup(p => p.GetDeviceLinked("def-1")).ReturnsAsync(new List<DeviceLookup>()
            {
            });
            _deviceLocationRepoMock.Setup(p => p.GetDeviceLocations(new string[] { "abc-1" })).ReturnsAsync(
                new List<DeviceLocation>()
                {
                    new DeviceLocation()
                    {
                        Longitude = 10,
                        Latitude = 10,
                        DeviceMacId = "abc-1"
                    }
                });

            _calculateLocationDistanceServiceMock
                .Setup(p =>
                    p.HaversineDistance(It.IsAny<GeoCordinate>()
                        , It.IsAny<GeoCordinate>())).Returns(30);


            /*
             * new AlertedVehicleHandheld
                    {
                        IsMoreThanAllowedDistance = false,
                        LinkedMacId = "abc-1",
                        MacId = "def-1",
                        LinkMacLat = 10,
                        LinkMacLong = 10,
                        MacLat = 20,
                        MacLong = 20
                    }
             */
            
            await processCalculateDeviceLocationService.Handle(new DeviceLocation()
            {
                Latitude = 20,
                Longitude = 20,
                DeviceMacId = "def-1"
            }, CancellationToken.None);
        }

        [Fact]
        public async Task LinkedDeviceButNoDeviceLocationShouldPublishMessage()
        {
            var processCalculateDeviceLocationService = Setup(100);
            _deviceLocationRepoMock.Setup(p => p.AddDeviceLocation(It.IsAny<DeviceLocation>())).Returns(Task.FromResult(true));
            _deviceLookupRepoMock.Setup(p => p.GetDeviceLinked("def-1")).ReturnsAsync(new List<DeviceLookup>()
            {
                new DeviceLookup() { Mac = "def-1", LinkedDeviceMac = "abc-1" }
            });
            _deviceLocationRepoMock.Setup(p => p.GetDeviceLocations(new string[] { "abc-1" })).ReturnsAsync(
                new List<DeviceLocation>()
                {
                });


            _calculateLocationDistanceServiceMock
                .Setup(p =>
                    p.HaversineDistance(It.IsAny<GeoCordinate>()
                        , It.IsAny<GeoCordinate>())).Returns(30);

            await processCalculateDeviceLocationService.Handle(new DeviceLocation()
            {
                Latitude = 20,
                Longitude = 20,
                DeviceMacId = "def-1"
            }, CancellationToken.None);
        }

        private IProcessCalculateDeviceLocationService Setup(int maxDistance)
        {
            _calculateLocationDistanceServiceMock = _mockRepository.Create<ICalculateLocationDistanceService>();
            _deviceLocationRepoMock = _mockRepository.Create<IDeviceLocationRepository>();
            _deviceLookupRepoMock = _mockRepository.Create<IDeviceLookupRepository>();
            _distanceMorePublishClient = _mockRepository.Create<IDistanceMoreMessagePublishingClient>();

            return new ProcessCalculateCalculateDeviceLocationService(
                _deviceLocationRepoMock.Object,
                _calculateLocationDistanceServiceMock.Object,
                _deviceLookupRepoMock.Object,
                _distanceMorePublishClient.Object,
                new EnvironmentSettings() { MaxDistanceAllowed = maxDistance });
        }
    }
}
