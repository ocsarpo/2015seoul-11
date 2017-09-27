
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;

namespace App5
{
    public class Tab_ParkingLot : Fragment
    {
        string local = "";
        public Tab_ParkingLot(string local)
        {
            this.local = local;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            var view = inflater.Inflate(Resource.Layout.GoogleMapTab, container, false);
            var sampleTextView = view.FindViewById<TextView>(Resource.Id.textView);

            sampleTextView.Click += (o, e) => {
                var nex_acc = new Intent(this.Activity, typeof(GoogleMapActivity));
                nex_acc.PutExtra("Local", local);
                StartActivity(nex_acc);
            };
            
            var next = new Intent(this.Activity, typeof(GoogleMapActivity));
            next.PutExtra("Local",local);
            StartActivity(next);

            return view;
        }
    }
}
