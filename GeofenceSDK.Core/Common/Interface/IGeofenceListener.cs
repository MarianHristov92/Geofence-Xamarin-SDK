using System;
using GeofenceSDK.Common.Models;

namespace GeofenceSDK.Common.Interface
{
    public interface IGeofenceListener
    {
        void OnMonitoringStarted(string identifier);
        void OnMonitoringStopped();
        void OnMonitoringStopped(string identifier);
        void OnRegionStateChanged(GeofenceResult result);
        void OnError(String error);
        void OnAppStarted();
        void OnLocationChanged(GeofenceLocation location);
    }
}
