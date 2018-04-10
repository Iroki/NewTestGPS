using System;
using System.Collections.Generic;
using System.Text;

namespace GeolocationApp
{
    public class Place
    {
        public Place(string address, string latLng)
        {
            Address = address;
            Latitude = latLng.Substring(0, latLng.IndexOf(',')).Trim();
            Longitude = latLng.Substring(latLng.IndexOf(',')).Trim();
        }

        public string Address { get; }
        public string Latitude { get; }
        public string Longitude { get; }
    }
}
