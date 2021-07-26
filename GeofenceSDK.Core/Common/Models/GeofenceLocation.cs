using System;
namespace GeofenceSDK.Common.Models
{
    public class GeofenceLocation
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public DateTime Date { get; set; }
        public double Accuracy { get; set; }

        public GeofenceLocation()
        {
        }

        public GeofenceLocation(GeofenceLocation geofenceLocation)
        {
            Latitude = geofenceLocation.Latitude;
            Longitude = geofenceLocation.Longitude;
            Date = geofenceLocation.Date;
            Accuracy = geofenceLocation.Accuracy;
        }

    }
}
