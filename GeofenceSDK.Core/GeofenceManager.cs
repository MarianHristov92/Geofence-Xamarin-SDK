using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using GeofenceSDK.Common;
using GeofenceSDK.Common.Interface;
using GeofenceSDK.Common.Models;
using Real.AppPushSDK;
using Real.AppPushSDK.Services;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace GeofenceSDK
{
    public class GeofenceManager
    {
        #region Attributes
        public Location CurrentLocation { get; set; }

        bool _update;
        IPlatformPushManagerService _native;

        private static object _syncroot = new object();
        private static GeofenceManager _instance;

        public static GeofenceManager Instance
        {
            get
            {
                lock (_syncroot)
                    if (_instance == null) _instance = new GeofenceManager();
                return _instance;
            }
        }

        public ObservableCollection<GeofenceCircularRegion> GeofenceLocations { get; set; }

        static Lazy<IGeofenceService> Implementation = new Lazy<IGeofenceService>(() => CreateGeofence(), System.Threading.LazyThreadSafetyMode.PublicationOnly);

        /// <summary>
        /// Checks if plugin is initialized
        /// </summary>
        public static bool IsInitialized => (GeofenceListener != null);
        /// <summary>
        /// Plugin id
        /// </summary>
        public const string Id = "GeofenceCrossService";
        /// <summary>
        /// Geofence state events listener
        /// </summary>
        public static IGeofenceListener GeofenceListener { get; private set; }
        /// <summary>
        /// Geofence location accuracy priority
        /// </summary>
        public static GeofencePriority GeofencePriority { get; set; }

        /// <summary>
        /// Smallest displacement for location updates
        /// </summary>
        public static float SmallestDisplacement { get; set; }

        /// <summary>
        /// Request the user for Notifications Permission.  Set to false if this is already handled in the client application.
        /// </summary>
        /// <value><c>true</c> if request notification permission; otherwise, <c>false</c>.</value>
        public static bool RequestNotificationPermission { get; set; }

        /// <summary>
        /// Request the user for Location Services Permissions. Set to false if this is already handled in the client application. 
        /// </summary>
        /// <value><c>true</c> if request location permission; otherwise, <c>false</c>.</value>
        public static bool RequestLocationPermission { get; set; }

        // ANDROID SPECIFIC 
        /// <summary>
        /// Icon resource used for notification
        /// </summary>
        public static int IconResource { get; set; }
        /// <summary>
        /// ARGB Color used for notification
        /// </summary>
        public static int Color { get; set; }
        /// <summary>
        /// Location updates internal
        /// </summary>
        public static int LocationUpdatesInterval { get; set; }
        /// <summary>
        /// Fastest location updates interval
        /// </summary>
        public static int FastestLocationUpdatesInterval { get; set; }
        #endregion

        #region Ctor
        private GeofenceManager()
        {
            var tag = this + ".Ctor";
            try
            {
                LogWriter("Ctor", "Ctor called");

                Initialize();
                SubscribeEvents();

            }
            catch (Exception ex)
            {
                Track.Exception(tag, ex);
            }
        }

        public void Initialize()
        {
            string tag = this + ".Initialize";
            try
            {
                GeofenceLocations = new ObservableCollection<GeofenceCircularRegion>();
                CurrentLocation = new Location();
                _update = false;
                _native = DependencyService.Get<IPlatformPushManagerService>();
            }
            catch (Exception ex)
            {
                Track.Exception(tag, ex);
            }
        }

        ~GeofenceManager()
        {
            UnsubscribeEvents();
        }
        #endregion


        #region Events
        public void SubscribeEvents()
        {
            string tag = this + ".SubscribeEvents";
            try
            {
                LogWriter("SubscribeEvents", "Subscribe to events");
                UnsubscribeEvents();
            }
            catch (Exception ex)
            {
                Track.Exception(tag, ex);
            }
        }

        public void UnsubscribeEvents()
        {
            string tag = this + ".UnsubscribeEvents";
            try
            {
                LogWriter("UnsubscribeEvents", "Unsubscribe from events");
            }
            catch (Exception ex)
            {
                Track.Exception(tag, ex);
            }
        }
        #endregion


        #region Location Finder
        public async void GetCurrentLocation()
        {
            string tag = this + ".GetCurrentLocation";
            try
            {
                CurrentLocation = await Geolocation.GetLastKnownLocationAsync();

                if (CurrentLocation != null)
                {
                    Console.WriteLine($"Latitude: {CurrentLocation.Latitude}, Longitude: {CurrentLocation.Longitude}, Altitude: {CurrentLocation.Altitude}");
                    GetLocalGeofences(CurrentLocation);
                }
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Handle not supported on device exception
            }
            catch (FeatureNotEnabledException fneEx)
            {
                // Handle not enabled on device exception
            }
            catch (PermissionException pEx)
            {
                // Handle permission exception
            }
            catch (Exception ex)
            {
                // Unable to get location
            }
        }

        public void GetLocalGeofences(Location currentLocation)
        {
            string tag = this + ".GetLocalGeofences";
            try
            {

                //TODO: Post Location to backend

                //POST(currentLocation)

            }
            catch (Exception ex)
            {
                Track.Exception(tag, ex);
            }
        }

        public void OnGeofenceLocationsReceived()
        {
            //TODO: Add the logic event when is done.
            foreach (GeofenceCircularRegion geofenceCircularRegion in GeofenceLocations)
            {
                Current.StartMonitoring(new GeofenceCircularRegion(geofenceCircularRegion.Id, geofenceCircularRegion.Latitude, geofenceCircularRegion.Longitude, geofenceCircularRegion.Radius)
                {
                    NotifyOnStay = true,
                    StayedInThresholdDuration = TimeSpan.FromMinutes(5)
                });
            }
        }
        #endregion



        #region Test Environment
        private async Task AddGeofence()
        {
            string tag = this + ".AddGeofence";
            try
            {
                LogWriter("Enter geofence region name", "Add Geofence");
                double radius = 50;

                var position1 = new GeofenceCircularRegion()
                {
                    Id = "Stadium",
                    Latitude = 51.174778,
                    Longitude = 6.386332,
                    Radius = 52220
                };
                GeofenceLocations.Add(position1);

                var position2 = new GeofenceCircularRegion()
                {
                    Id = "Kaufland",
                    Latitude = 51.182997,
                    Longitude = 6.412036,
                    Radius = 52220
                };
                GeofenceLocations.Add(position2);

                var position3 = new GeofenceCircularRegion()
                {
                    Id = "Real",
                    Latitude = 51.208155,
                    Longitude = 6.460733,
                    Radius = 52220
                };
                GeofenceLocations.Add(position3);

                foreach (GeofenceCircularRegion geofenceCircularRegion in GeofenceLocations)
                {
                    Current.StartMonitoring(new GeofenceCircularRegion(geofenceCircularRegion.Id, geofenceCircularRegion.Latitude, geofenceCircularRegion.Longitude, geofenceCircularRegion.Radius)
                    {
                        NotifyOnStay = true,
                        StayedInThresholdDuration = TimeSpan.FromMinutes(5)
                    });
                }
            }
            catch (Exception ex)
            {
                Track.Exception(tag, ex);
            }
        }

        #endregion

        #region Logs
        private void LogWriter(string method, string message)
        {
            string tag = this + ".LogWriter";
            try
            {
                Track.Info($"[{this}.{method}]: {message}");
                System.Diagnostics.Debug.WriteLine($"[{this}.{method}]: {message}");
            }
            catch (Exception ex)
            {
                Track.Exception(tag, ex);
            }
        }
        #endregion


        /// <summary>
        /// Initializes geofence plugin
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="priority"></param>
        /// <param name="smallestDisplacement"></param>
        /// <param name="requestNotificationPermission"></param>
        /// <param name="requestLocationPermission"></param>
        public static void Initialize<T>(GeofencePriority priority = GeofencePriority.BalancedPower, float smallestDisplacement = 0, bool requestNotificationPermission = true, bool requestLocationPermission = true)
     where T : IGeofenceListener, new()
        {
            if (GeofenceListener == null)
            {

                GeofenceListener = (IGeofenceListener)Activator.CreateInstance(typeof(T));
                Debug.WriteLine("Geofence plugin initialized.");
            }
            else
            {
                Debug.WriteLine("Geofence plugin already initialized.");
            }
            GeofencePriority = priority;
            SmallestDisplacement = smallestDisplacement;

            RequestNotificationPermission = requestNotificationPermission;
            RequestLocationPermission = requestLocationPermission;
        }
        /// <summary>
        /// Current settings to use
        /// </summary>
        public static IGeofenceService Current
        {
            get
            {
                //Should always initialize plugin before use
                if (!IsInitialized)
                {
                    throw GeofenceNotInitializedException();
                }
                var ret = Implementation.Value;
                if (ret == null)
                {
                    throw NotImplementedInReferenceAssembly();
                }
                return ret;

            }
        }

        static IGeofenceService CreateGeofence()
        {
            #if NETSTANDARD2_0
                        return null;
            #else
                        System.Diagnostics.Debug.WriteLine("Creating GeofenceImplementation");
                        var geofenceImplementation = new GeofenceImplementation();
                        geofenceImplementation.RequestNotificationPermission = RequestNotificationPermission;
                        geofenceImplementation.RequestLocationPermission = RequestLocationPermission;
                        return geofenceImplementation;
            #endif
        }

        internal static Exception NotImplementedInReferenceAssembly()
        {
            return new NotImplementedException("This functionality is not implemented in the portable version of this assembly.  You should reference the NuGet package from your main application project in order to reference the platform-specific implementation.");
        }

        internal static GeofenceException GeofenceNotInitializedException()
        {
            string description = string.Format("{0} - {1}", Id, "Plugin is not initialized. Should initialize before use with CrossGeofence Initialize method. Example:  GeofenceSDK.Initialize<GeofenceCrossListener>()");

            return new GeofenceException(description);
        }


    }
}
