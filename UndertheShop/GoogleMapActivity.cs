using System;
using System.Collections.Generic;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Content.PM;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;

namespace App5
{
    [Activity(Label = "@string/ParkingLotInfo", Theme = "@android:style/Theme.NoTitleBar", ScreenOrientation = ScreenOrientation.Portrait)]
    public class GoogleMapActivity : Activity, IOnMapReadyCallback
    {
        private GoogleMap mMap;
        private string local = "";
        private UnderShopLocationMapInfo underShopLoca; 

    protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.GoogleMapLayout);
            if (MainActivity.Activities.ContainsKey("GoogleMapActivity"))
            {
                MainActivity.Activities["GoogleMapActivity"].Finish();
                MainActivity.Activities.Remove("GoogleMapActivity");
                MainActivity.Activities.Add("GoogleMapActivity", this);
            }
            else
            {
                MainActivity.Activities.Add("GoogleMapActivity", this);
            }
            local = Intent.GetStringExtra("Local");

           
            Data_Parking.LoadXMLData(local,Data_Parking.DIC_PARKINGLOTSTRING[local]);


            System.IO.StreamReader sr = new System.IO.StreamReader(Assets.Open("UnderShopLocationInfo.xml"));
            System.Xml.XmlDocument xml = new System.Xml.XmlDocument();
            xml.Load(sr);
            System.Xml.XmlNodeList nodeList = xml.SelectNodes("UnderShopLocationInfo/" + local);
            foreach(System.Xml.XmlNode i in nodeList)
            { 
            underShopLoca = new UnderShopLocationMapInfo() {
                ShopName = i["ShopName"].InnerText,
                ShopAddr = i["ShopAddr"].InnerText,
                Shop_LAT = i["Shop_LAT"].InnerText,
                Shop_LNG = i["Shop_LNG"].InnerText
            };
            }
            SetUpMap();
        }

        List<LatLng> list_latlan = new List<LatLng>();
        private void SetUpMap()
        {
            if (mMap == null)
            {
                FragmentManager.FindFragmentById<MapFragment>(Resource.Id.map).GetMapAsync(this);
            }
        }
        public void OnMapReady(GoogleMap googleMap)
        {
            mMap = googleMap;

            //샵 마커
            if (underShopLoca != null)
            {
                LatLng latlng = new LatLng(Double.Parse(underShopLoca.Shop_LAT), Double.Parse(underShopLoca.Shop_LNG));
                CameraUpdate cam = CameraUpdateFactory.NewLatLngZoom(latlng, 15);
                mMap.MoveCamera(cam);
                MarkerOptions opt = new MarkerOptions()
                    .SetPosition(latlng)
                    .SetTitle(underShopLoca.ShopName)
                    .SetSnippet(underShopLoca.ShopAddr);

                mMap.AddMarker(opt)
                    .SetIcon(BitmapDescriptorFactory.FromResource(Resource.Drawable.shopIcon));
                    //.SetIcon(BitmapDescriptorFactory.DefaultMarker(BitmapDescriptorFactory.HueBlue));
            }

            //주차장 마커
            foreach (var i in Data_Parking.DIC_PARKINGLOT_OVERALL_INFO[local])
            {
                LatLng latlng = new LatLng(Double.Parse(i.Value.LAT), Double.Parse(i.Value.LNG));
                
                MarkerOptions opt = new MarkerOptions()
                    .SetPosition(latlng)
                    .SetTitle(i.Value.ADDR)
                    .SetSnippet(i.Value.PARKING_NAME);

                mMap.AddMarker(opt)
                    .SetIcon(BitmapDescriptorFactory.FromResource(Resource.Drawable.parkingIcon));
            }

            //MarkerOptions options = new MarkerOptions()
            //    .SetPosition(latlng)
            //    .SetTitle("여긴어디")
            //    .SetSnippet("AKA: 나는누규");
            //    //.Draggable(true);

            ////marker1
            //mMap.AddMarker(options);
            //    //.SetIcon(BitmapDescriptorFactory.FromResource(Resource.Drawable.our));

            //////marker2
            ////mMap.AddMarker(new MarkerOptions()
            ////    .SetPosition(latlng2)
            ////    .SetTitle("Marker 2"))
            ////    .SetIcon(BitmapDescriptorFactory.DefaultMarker(BitmapDescriptorFactory.HueBlue));

            ////marker2
            //mMap.AddMarker(new MarkerOptions()
            //    .SetPosition(latlng2)
            //    .SetTitle("Marker 2"));
            //    //.SetIcon(BitmapDescriptorFactory.FromResource(Resource.Drawable.our));


            ////mMap.MarkerClick += MMap_MarkerClick;
            ////mMap.MarkerDrag += MMap_MarkerDrag;
        }

        private void MMap_MarkerClick(object sender, GoogleMap.MarkerClickEventArgs e)
        {
            LatLng pos = e.Marker.Position;
            mMap.AnimateCamera(CameraUpdateFactory.NewLatLngZoom(pos, 10));
        }

        private void MMap_MarkerDrag(object sender, GoogleMap.MarkerDragEventArgs e)
        {
            LatLng pos = e.Marker.Position;
            Console.WriteLine(pos.ToString());
        }
        protected override void OnDestroy()
        {
            mMap.Clear();
            mMap.Dispose();
            mMap = null;
            if (MainActivity.Activities.ContainsKey("GoogleMapActivity"))
            {
                MainActivity.Activities.Remove("GoogleMapActivity");
            }
            base.OnDestroy();
        }
    }
}
