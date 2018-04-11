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
using Plugin.CurrentActivity;

[assembly: Xamarin.Forms.Dependency(typeof(GeolocationApp.Droid.MainActivity))]

namespace GeolocationApp.Droid
{
    [Activity(Label = "GeolocationApp", Icon = "@drawable/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity, IPlacePicker
    {
        TaskCompletionSource<TryResult<Place>> TaskCompletionSource { get; set; }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);
            Xamarin.FormsMaps.Init(this, bundle);
            CrossCurrentActivity.Current.Activity = (Activity)Forms.Context;
            LoadApplication(new App());

        }

        // Context context = Android.App.Application.Context;

        //  private static readonly int PLACE_PICKER_REQUEST = 1;

        //private Button _pickAPlaceButton;
        //private TextView _placeNameTextView;
        //private TextView _placeAddressTextView;
        //private TextView _placePhoneNumberTextView;
        //private TextView _placeWebSiteTextView;



        public Task<TryResult<Place>> PickPlace() //((MainActivity)CrossCurrentActivity.Current.Activity) добавлено везде, так как создавалось несколько активити и возвращался null от PickPlace
        {
            ((MainActivity)CrossCurrentActivity.Current.Activity).TaskCompletionSource?.TrySetResult(TryResult<Place>.Unsucceed()); //сбрасываем статус результата
            ((MainActivity)CrossCurrentActivity.Current.Activity).TaskCompletionSource = new TaskCompletionSource<TryResult<Place>>();
            OnPickAPlaceButtonTapped();

            return ((MainActivity)CrossCurrentActivity.Current.Activity).TaskCompletionSource.Task;
        }

        public void OnPickAPlaceButtonTapped()
        {
            //Android.App.Application.SynchronizationContext.Post(ignored =>
            //    {
            var builder = new PlacePicker.IntentBuilder();
            //builder.SetLatLngBounds(new Android.Gms.Maps.Model.LatLngBounds() {  }) //testing
            ((Activity)Forms.Context).StartActivityForResult(builder.Build((Activity)Forms.Context), 99); // ОБЯЗАТЕЛЬНО ТОЛЬКО в двух местах прописать "((Activity)Forms.Context", иначе ошибка!!!!!!!!!!!!!!!!!!!!!!!!!!!!

            //   }, null);

        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            if (requestCode == 99 && resultCode == Result.Ok)
            {
                GetPlaceFromPicker(data);
            }

            base.OnActivityResult(requestCode, resultCode, data);
        }

        private void GetPlaceFromPicker(Intent data)
        {
          //  taskCompletionSource = new TaskCompletionSource<TryResult<Place>>();  //ошибка, если нет строки;

            var placePicked = PlacePicker.GetPlace(this, data);

            var place = new Place(placePicked?.NameFormatted?.ToString(), placePicked.LatLng.Latitude, placePicked.LatLng.Longitude);
            
            ((MainActivity)CrossCurrentActivity.Current.Activity).TaskCompletionSource.TrySetResult(TryResult<Place>.Succeed(place));
            
        }


    }

}


