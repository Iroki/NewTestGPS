using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Gms.Location.Places.UI;
using Android.Content;
using Xamarin.Forms;
using System.Threading.Tasks;

[assembly: Xamarin.Forms.Dependency(typeof(GeolocationApp.Droid.MainActivity))]

namespace GeolocationApp.Droid
{
    [Activity(Label = "GeolocationApp", Icon = "@drawable/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity, IPlacePicker
    {
        

        TaskCompletionSource<TryResult<Place>> taskCompletionSource;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);
            Xamarin.FormsMaps.Init(this, bundle);
            Plugin.CurrentActivity.CrossCurrentActivity.Current.Activity = this;
            LoadApplication(new App());

        }

       // Context context = Android.App.Application.Context;

            private static readonly int PLACE_PICKER_REQUEST = 1;

        //private Button _pickAPlaceButton;
        //private TextView _placeNameTextView;
        //private TextView _placeAddressTextView;
        //private TextView _placePhoneNumberTextView;
        //private TextView _placeWebSiteTextView;

        //testing

        public string placeName;
        public string placeLatLng;


            
        public void OnPickAPlaceButtonTapped()
        {
            //Android.App.Application.SynchronizationContext.Post(ignored =>
            //    {
            var builder = new PlacePicker.IntentBuilder();
            //builder.SetLatLngBounds(new Android.Gms.Maps.Model.LatLngBounds() {  }) //testing
            ((Activity)Forms.Context).StartActivityForResult(builder.Build((Activity)Forms.Context), PLACE_PICKER_REQUEST); // ОБЯЗАТЕЛЬНО ТОЛЬКО ТАК, иначе ошибка!!!!!!!!!!!!!!!!!!!!!!!!!!!!
             
             //   }, null);

        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            if (requestCode == PLACE_PICKER_REQUEST && resultCode == Result.Ok)
            {
                GetPlaceFromPicker(data);
            }

            base.OnActivityResult(requestCode, resultCode, data);
        }

        private void GetPlaceFromPicker(Intent data)
        {
            var placePicked = PlacePicker.GetPlace(this, data);
            


            placeName = placePicked?.NameFormatted?.ToString();

            taskCompletionSource.TrySetResult(TryResult<Place>.Unsucceed());          
            


            //_placeNameTextView.Text = placePicked?.NameFormatted?.ToString();
            //_placeAddressTextView.Text = placePicked?.AddressFormatted?.ToString();
            //_placePhoneNumberTextView.Text = placePicked?.PhoneNumberFormatted?.ToString();
            //_placeWebSiteTextView.Text = placePicked?.WebsiteUri?.ToString();
        }

        public Task<TryResult<Place>> PickPlace()
        {
            taskCompletionSource?.TrySetResult(TryResult<Place>.Unsucceed());
            taskCompletionSource = new TaskCompletionSource<TryResult<Place>>();            
            OnPickAPlaceButtonTapped();
            return taskCompletionSource.Task;
        }
    }
    }


