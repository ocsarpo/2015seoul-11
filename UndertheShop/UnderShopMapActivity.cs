using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Content.PM;
using Android.Graphics;
using System.Threading.Tasks;
using Android.Content.Res;

namespace App5
{
    [Activity(Label = "UnderShopMapActivity", Theme = "@android:style/Theme.NoTitleBar", ScreenOrientation = ScreenOrientation.Portrait)]
    public class UnderShopMapActivity : Activity
    {
        string local = "";
        string id = "";
        ScaleImageView image;
        //ImageView imageView1;
        TextView iv= null;
        Bitmap bitmapToDisplay;
        List<ShopNameLocation> shopNameList;
        TextView[] TV_Categories;
    protected override async void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            if (MainActivity.Activities.ContainsKey("UnderShopMapActivity"))
            {
                MainActivity.Activities["UnderShopMapActivity"].Finish();
                MainActivity.Activities.Remove("UnderShopMapActivity");
                MainActivity.Activities.Add("UnderShopMapActivity", this);
            }
            else
            {
                MainActivity.Activities.Add("UnderShopMapActivity", this);
            }
            // Create your application here
            SetContentView(Resource.Layout.ScaleImage);
            
            FrameLayout fr = FindViewById<FrameLayout>(Resource.Id.fr);

            local = Intent.GetStringExtra("Local");
            id = Intent.GetStringExtra("SHOP_ID_NUMBER");

            image = FindViewById<ScaleImageView>(Resource.Id.AnimateImage);

            //foreach(var i in Data_ShopInfo.DIC_SHOP_XML_INFO_OVERALL_INFO)
            //{
            //    if (i.Value.IdentificationNumber.Equals(id))
            //    {
            //        iv = FindViewById<ImageView>(Resource.Id.iv);
            //        iv.SetImageResource(Resource.Drawable.directions9);
            //        iv.TranslationX = i.Value.ShopLocation.X;
            //        iv.TranslationY = i.Value.ShopLocation.Y - 10;
            //        iv.Tag = i.Value.IdentificationNumber;
            //    }
            //}

            TextView iv=null;
            shopNameList = new List<ShopNameLocation>();
            foreach (var i in Data_ShopInfo.DIC_SHOPINFO_OVERALL_INFO[local])
            {
                TextView tv = new TextView(this);
                tv.SetTextColor(Android.Graphics.Color.Black);
                if (i.Category.Equals(GetString(Resource.String.station)))
                {
                    tv.Text = i.ShopName;
                    tv.Click += Tv_Subway_Click;
                    tv.Tag = i.IdentificationNumber.Split(',')[0] + "," + i.EngCategory;
                    tv.SetTypeface(null, TypefaceStyle.Bold);
                    tv.SetTextColor(Color.White);
                }
                else if (i.Category.Equals(GetString(Resource.String.rest)))
                {
                    tv.SetBackgroundResource(Resource.Drawable.restroom16);
                    tv.Tag = i.IdentificationNumber + "," + i.EngCategory;
                }
                else if (i.Category.Equals(GetString(Resource.String.gateway)))
                {
                    tv.Text = i.ShopName;
                    tv.Tag = i.IdentificationNumber + "," + i.EngCategory;
                }
                else if(i.Category.Equals(GetString(Resource.String.move)))
                {
                    tv.Text = i.ShopName;
                    tv.Click += MapTransfer;
                    tv.SetTextColor(Color.White);
                    tv.Tag = i.IdentificationNumber.Split(',')[0]+","+i.EngCategory;
                }
                else
                {
                    tv.Text = i.ShopName;
                    tv.Click += Tv_Click;
                    tv.Tag = i.IdentificationNumber +","+ i.EngCategory;

                    if (id != null)
                    {
                        if (id.Equals(i.IdentificationNumber))
                        {
                            iv = tv;
                            iv.SetBackgroundColor(Color.Red);
                            iv.SetTextColor(Color.White);
                        }
                    }
                }
                var lparam = new FrameLayout.LayoutParams(ViewGroup.LayoutParams.WrapContent,
                    ViewGroup.LayoutParams.WrapContent);
                tv.LayoutParameters = lparam;
                ShopNameLocation snl = new ShopNameLocation()
                {
                    myView = tv,
                    myLocationX = i.ShopLocation.X,
                    myLocationY = i.ShopLocation.Y
                };
                shopNameList.Add(snl);
                fr.AddView(tv);
            }
            image.LocalKey = local;
            try
            {
                BitmapResize br = new BitmapResize();
                BitmapFactory.Options options = await br.GetBitmapOptionsOfImageAsync(Resources, Data_ShopInfo.DIC_SHOPMAP[local]);
                bitmapToDisplay = await br.LoadScaledDownBitmapForDisplayAsync(Resources, Data_ShopInfo.DIC_SHOPMAP[local], options, 370, 370);

                if (iv != null)
                    image.SetImageBitmapAndMarker(bitmapToDisplay, iv, iv.TranslationX, iv.TranslationY, shopNameList);
                else
                    image.SetImageBitmapAndMarker(bitmapToDisplay, shopNameList);
            }
            catch
            {
                
            }


            TV_Categories = new TextView[] {
            FindViewById<TextView>(Resource.Id.TV_ALL),
            FindViewById<TextView>(Resource.Id.TV_FOOD),
            FindViewById<TextView>(Resource.Id.TV_FASHION),
            FindViewById<TextView>(Resource.Id.TV_CONVE),
            FindViewById<TextView>(Resource.Id.TV_INTERIOR),
            FindViewById<TextView>(Resource.Id.TV_BEAUTY),
            FindViewById<TextView>(Resource.Id.TV_OTHERS),
            FindViewById<TextView>(Resource.Id.TV_DIGITAL)
            };
            for(int i =0; i < TV_Categories.Length; i++)
            {
                TV_Categories[i].Click += LEGEND_TV_CLICK;
            }
            Toast.MakeText(this, GetString(Resource.String.UnderMapComment), ToastLength.Short).Show();

        }

        private void MapTransfer(object sender, EventArgs e)
        {
            TextView tv = sender as TextView;

            if (tv != null)
            {
                var second = new Intent(this, typeof(UnderShopMapActivity));
                second.PutExtra("Local", tv.Tag.ToString().Split(',')[0]);
                this.StartActivity(second);
                Finish();
            }
        }

        private void LEGEND_TV_CLICK(object sender, EventArgs e)
        {
            TextView legend_tv = sender as TextView;
            if (legend_tv != null)
            {
                string tv_tag = legend_tv.Tag.ToString();

                foreach (var i in TV_Categories)
                {
                    if(i.Tag.ToString().Equals(tv_tag))
                        i.SetBackgroundColor(Color.DarkRed);                    
                    else
                        i.SetBackgroundColor(new Color(byte.Parse("00"),byte.Parse("00"), byte.Parse("00"), byte.Parse("59")));
                }

                foreach (var i in shopNameList)
                {
                    if (!i.myView.Tag.ToString().Split(',')[1].Equals(tv_tag))
                        i.myView.Visibility = ViewStates.Invisible;
                    else
                        i.myView.Visibility = ViewStates.Visible;

                    if (tv_tag.Equals("ALL"))
                        i.myView.Visibility = ViewStates.Visible;

                    if (i.myView.Tag.ToString().Split(',')[1].Equals("REST") ||
                        i.myView.Tag.ToString().Split(',')[1].Equals("GATEWAY") ||
                        i.myView.Tag.ToString().Split(',')[1].Equals("SUBWAY")||
                        i.myView.Tag.ToString().Split(',')[1].Equals("MOVE"))
                        i.myView.Visibility = ViewStates.Visible;

                }
            }
        }

        private void Tv_Subway_Click(object sender, EventArgs e)
        {
            TextView t = sender as TextView;
            if(t != null)
            {
                Android.Net.ConnectivityManager connectivityManager = (Android.Net.ConnectivityManager)GetSystemService(ConnectivityService);
                Android.Net.NetworkInfo activeConnection = connectivityManager.ActiveNetworkInfo;
                bool isOnline = (activeConnection != null) && activeConnection.IsConnected;

                if (isOnline)
                {
                    var second = new Intent(this, typeof(SubwayInfoActivity));
                    second.PutExtra("StationName", t.Tag.ToString().Split(',')[0]);
                    this.StartActivity(second);
                }
                else
                {
                    AlertDialog.Builder dlg = new AlertDialog.Builder(this);
                    dlg.SetTitle(GetString(Resource.String.InternetCheck));
                    dlg.SetMessage(GetString(Resource.String.InternetCheckMessage));
                    dlg.SetIcon(Resource.Drawable.AppIcon);
                    dlg.SetPositiveButton(GetString(Resource.String.confirm), (s, o) =>
                    {

                    });
                    dlg.Show();
                }
            }
        }

        private void Tv_Click(object sender, EventArgs e)
        {
            TextView t = sender as TextView;
            if (t != null)
            {
                var second = new Intent(this, typeof(ShopInfoActivity));
                second.PutExtra("SHOP_ID_NUMBER", t.Tag.ToString().Split(',')[0]);
                second.PutExtra("Local", local);
                this.StartActivity(second);
            }
        }
        
        private int ConvertPixelsToDp(float pixelValue)
        {
            var dp = (int)((pixelValue) / Resources.DisplayMetrics.Density);
            return dp;
        }
        public override void OnBackPressed()
        {
            Finish();
            base.OnBackPressed();
        }

        protected override void OnDestroy()
        {
            shopNameList.Clear();
            image.Resources.Dispose();
            image.Dispose();
            bitmapToDisplay.Recycle();
            if (MainActivity.Activities.ContainsKey("UnderShopMapActivity"))
            {
                MainActivity.Activities.Remove("UnderShopMapActivity");
            }
            System.GC.Collect();
            System.GC.WaitForPendingFinalizers();
            base.OnDestroy();
        }
        //async Task<BitmapFactory.Options> GetBitmapOptionsOfImageAsync()
        //{
        //    BitmapFactory.Options options = new BitmapFactory.Options
        //    {
        //        InJustDecodeBounds = true
        //    };

        //    // The result will be null because InJustDecodeBounds == true.
        //    Bitmap result = await BitmapFactory.DecodeResourceAsync(Resources, Data_ShopInfo.DIC_SHOPMAP[local], options);

        //    int imageHeight = options.OutHeight;
        //    int imageWidth = options.OutWidth;

        //    //_originalDimensions.Text = string.Format("Original Size= {0}x{1}", imageWidth, imageHeight);

        //    return options;
        //}
        //public static int CalculateInSampleSize(BitmapFactory.Options options, int reqWidth, int reqHeight)
        //{
        //    // Raw height and width of image
        //    float height = options.OutHeight;
        //    float width = options.OutWidth;
        //    double inSampleSize = 1D;

        //    if (height > reqHeight || width > reqWidth)
        //    {
        //        int halfHeight = (int)(height / 2);
        //        int halfWidth = (int)(width / 2);

        //        // Calculate a inSampleSize that is a power of 2 - the decoder will use a value that is a power of two anyway.
        //        while ((halfHeight / inSampleSize) > reqHeight && (halfWidth / inSampleSize) > reqWidth)
        //        {
        //            inSampleSize *= 2;
        //        }
        //    }

        //    return (int)inSampleSize;
        //}
        //public async Task<Bitmap> LoadScaledDownBitmapForDisplayAsync(Resources res, BitmapFactory.Options options, int reqWidth, int reqHeight)
        //{
        //    // Calculate inSampleSize
        //    options.InSampleSize = CalculateInSampleSize(options, reqWidth, reqHeight);

        //    // Decode bitmap with inSampleSize set
        //    options.InJustDecodeBounds = false;

        //    return await BitmapFactory.DecodeResourceAsync(res, Data_ShopInfo.DIC_SHOPMAP[local], options);
        //}

    }
    public class ShopNameLocation
    {
        public TextView myView {get;set;}
        public float myLocationX { get; set; }
        public float myLocationY { get; set; }
    }
}