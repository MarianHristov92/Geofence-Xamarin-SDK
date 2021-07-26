using System;
using Android.App;
using Android.Content;

namespace GeofenceSDK.Droid.Models
{
    /// <summary>
    /// GeofenceBootReceiver class
    /// Receives BOOT_COMPLETED event
    /// </summary>
    [BroadcastReceiver]
    [IntentFilter(new[] { Intent.ActionBootCompleted })]
    public class GeofenceBootReceiver : BroadcastReceiver
    {
        /// <summary>
        /// On boot completed restores all persisted regions
        /// </summary>
        /// <param name="context"></param>
        /// <param name="intent"></param>
        public override void OnReceive(Context context, Intent intent)
        {
            System.Diagnostics.Debug.WriteLine(string.Format("{0} - {1}", GeofenceCrossService.Id, "Boot Received"));
            var geofenceInstance = GeofenceCrossService.Current;
        }
    }
}
