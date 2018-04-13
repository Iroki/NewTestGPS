using System;
using System.Collections.Generic;
using System.Text;

namespace GeolocationApp
{
    public class Place
    {
        public Place(string address, double latitude, double longitude)
        {
            Address = address;
            Latitude = latitude; //latLng.Substring(0, latLng.IndexOf(',')).Trim();
            Longitude = longitude; //latLng.Substring(latLng.IndexOf(',')).Trim();
        }

        public string Address { get; }
        public double Latitude { get; }
        public double Longitude { get; }


        public override string ToString()
        {
            return Address;
        }
    }
}
