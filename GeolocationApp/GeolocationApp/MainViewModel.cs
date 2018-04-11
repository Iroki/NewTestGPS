using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace GeolocationApp
{
	public class MainViewModel : INotifyPropertyChanged
	{
		public MainViewModel()
		{
           
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public Command OnPickAPlaceButtonTapped
        {
            get
            {
                return new Command(async () =>
                {
                    var placeResult = await DependencyService.Get<IPlacePicker>().PickPlace();
                    if (placeResult.OperationSucceed)
                    {
                        PickedPlace = placeResult.Data;
                        PickedPlacesCollection.Add(PickedPlace);
                    }

                });
            }
        }

        internal async void OnViewModelAppear()
        {
            await StartListening();
            await GetCurrentPosition();
        }

        private ObservableCollection<Place> _pickedPlaceCollection = new ObservableCollection<Place>();
        public ObservableCollection<Place> PickedPlacesCollection
        {
            get
            {
                return _pickedPlaceCollection;
            }
            set
            {
                _pickedPlaceCollection = value;
                RaisePropertyChanged();
            }
        }

        private Place _pickedPlace;
        public Place PickedPlace
        {
            get
            {
                return _pickedPlace;
            }
            set
            {
                _pickedPlace = value;
                RaisePropertyChanged();
            }
        }

        //Geolocation -copy-paste 

        public static async Task<Plugin.Geolocator.Abstractions.Position> GetCurrentPosition()
        {
            Plugin.Geolocator.Abstractions.Position position = null;
            try
            {
                var locator = CrossGeolocator.Current;
                locator.DesiredAccuracy = 100;

                position = await locator.GetLastKnownLocationAsync();

                if (position != null)
                {
                    //got a cached position, so let's use it.
                    return position;
                }

                if (!locator.IsGeolocationAvailable || !locator.IsGeolocationEnabled)
                {
                    //not available or enabled
                    return null;
                }

                position = await locator.GetPositionAsync(TimeSpan.FromSeconds(10), null, true);

            }
            catch (Exception ex)
            {
                Debug.WriteLine("Unable to get location: " + ex);
            }

            if (position == null)
                return null;

            //необязательно
            var output = string.Format("Time: {0} \nLat: {1} \nLong: {2} \nAltitude: {3} \nAltitude Accuracy: {4} \nAccuracy: {5} \nHeading: {6} \nSpeed: {7}",
                    position.Timestamp, position.Latitude, position.Longitude,
                    position.Altitude, position.AltitudeAccuracy, position.Accuracy, position.Heading, position.Speed);

            Debug.WriteLine(output);

            return position;
        }


        //LISTENER GEOLOCATION COPY-PASTE

        private Plugin.Geolocator.Abstractions.Position _currentPosition;

        public Plugin.Geolocator.Abstractions.Position CurrentPosition
        {
            get
            {
                return _currentPosition;
            }
            set
            {
                _currentPosition = value;
                RaisePropertyChanged();
            }
        }


        async Task StartListening()
        {
            if (CrossGeolocator.Current.IsListening)
                return;

            await CrossGeolocator.Current.StartListeningAsync(TimeSpan.FromSeconds(10), 10, true);

            CrossGeolocator.Current.PositionChanged += PositionChanged;
            CrossGeolocator.Current.PositionError += PositionError;
        }

        public void PositionChanged(object sender, PositionEventArgs e)
        {

            //If updating the UI, ensure you invoke on main thread
            CurrentPosition = e.Position;
            //необязательно
            var output = "Full: Lat: " + CurrentPosition.Latitude + " Long: " + CurrentPosition.Longitude;
            output += "\n" + $"Time: {CurrentPosition.Timestamp}";
            output += "\n" + $"Heading: {CurrentPosition.Heading}";
            output += "\n" + $"Speed: {CurrentPosition.Speed}";
            output += "\n" + $"Accuracy: {CurrentPosition.Accuracy}";
            output += "\n" + $"Altitude: {CurrentPosition.Altitude}";
            output += "\n" + $"Altitude Accuracy: {CurrentPosition.AltitudeAccuracy}";
            Debug.WriteLine(output);
        }

        private void PositionError(object sender, PositionErrorEventArgs e)
        {
            Debug.WriteLine(e.Error);
            //Handle event here for errors
        }

        async Task StopListening()
        {
            if (!CrossGeolocator.Current.IsListening)
                return;

            await CrossGeolocator.Current.StopListeningAsync();

            CrossGeolocator.Current.PositionChanged -= PositionChanged;
            CrossGeolocator.Current.PositionError -= PositionError;
        }
    }


}
