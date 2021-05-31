namespace DeliveryTracker.Business
{
    public interface ICalculateTwoLocationDistanceService
    {
        double HaversineDistance(GeoCordinate pos1, GeoCordinate pos2);
    }
}