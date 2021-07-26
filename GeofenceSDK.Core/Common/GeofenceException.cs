using System;
namespace GeofenceSDK.Common
{
    public class GeofenceException : Exception
    {
        public GeofenceException()
        {

        }

        public GeofenceException(string message) : base(message)
        {

        }
    }
}
