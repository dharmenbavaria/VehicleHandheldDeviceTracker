namespace DeliveryTracker.Business
{
    public interface ICalculateLocationDistanceService
    {
        double HaversineDistance(GeoCordinate pos1, GeoCordinate pos2);
    }
}