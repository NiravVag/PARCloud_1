namespace Par.CommandCenter.Domain.Model
{
    public class GeoCoordinate
    {
        public GeoCoordinate(double latitude, double longitude)
        {
            this.Latitude = latitude;
            this.Longitude = longitude;
        }

        public GeoCoordinate(double latitude, double longitude, Address address)
            : this(latitude, longitude)
        {
            this.Address = address;
        }

        public double Latitude { get; private set; }

        public double Longitude { get; private set; }

        public Address Address { get; private set; }
    }
}
