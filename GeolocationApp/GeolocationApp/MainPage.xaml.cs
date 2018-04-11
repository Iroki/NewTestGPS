using Plugin.Geolocator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace GeolocationApp
{
	public partial class MainPage : ContentPage
	{
		public MainPage()
		{
			InitializeComponent();
            mainViewModel = new MainViewModel();
            BindingContext = mainViewModel;
            MyMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(50.470691, 30.465316), Distance.FromKilometers(1.00))); //change later somehow
            CrossGeolocator.Current.PositionChanged += (sender, e) =>
            {
                Current_PositionChanged(sender, e);
            };

        }

        MainViewModel mainViewModel;

        Plugin.Geolocator.Abstractions.Position position;

        private void Current_PositionChanged(object sender, Plugin.Geolocator.Abstractions.PositionEventArgs e)
        {
            MyMap.Pins.Clear();
            position = e.Position;
            MyMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(position.Latitude, position.Longitude), Distance.FromKilometers(1.00)));
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            mainViewModel.OnViewModelAppear();
        }

    }
}
