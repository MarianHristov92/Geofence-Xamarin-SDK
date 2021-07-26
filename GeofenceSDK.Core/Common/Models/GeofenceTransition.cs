using System;
namespace GeofenceSDK.Common.Models
{
    public enum GeofenceTransition
    {
        /// <summary>
        /// Entry transition
        /// </summary>
        Entered,
        /// <summary>
        /// Exit transition
        /// </summary>
        Exited,
        /// <summary>
        /// Stayed in transition
        /// </summary>
        Stayed,
        /// <summary>
        /// Unknown transition
        /// </summary>
        Unknown
    }
}
