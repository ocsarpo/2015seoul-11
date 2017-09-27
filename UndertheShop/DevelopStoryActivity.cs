using Android.App;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Graphics;
using Android.Content.PM;

namespace App5
{
    [Activity(Label = "App3", Theme = "@android:style/Theme.NoTitleBar", ScreenOrientation = ScreenOrientation.Portrait)]
    public class DevelopStoryActivity : Activity
    {
        Bitmap[] btm_Members = null;
        Bitmap[] btm_Tools = null;
        Bitmap[] btm_APIs = null;
        Bitmap btm_belong = null;
        //ImageView backgroundImg;
        //Bitmap bitmapToDisplay;

        protected override async void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            RequestWindowFeature(WindowFeatures.NoTitle);


            if (MainActivity.Activities.ContainsKey("DevelopStoryActivity"))
            {
                MainActivity.Activities["DevelopStoryActivity"].Finish();
                MainActivity.Activities.Remove("DevelopStoryActivity");
                MainActivity.Activities.Add("DevelopStoryActivity", this);
            }
            else
            {
                MainActivity.Activities.Add("DevelopStoryActivity", this);
            }

            SetContentView(Resource.Layout.DevelopStoryLayout);

            //backgroundImg = FindViewById<ImageView>(Resource.Id.backgroundImg);
            //BitmapResize br = new BitmapResize();
            //BitmapFactory.Options options = await br.GetBitmapOptionsOfImageAsync(Resources, Resource.Drawable.blurUnderShop2);
            //bitmapToDisplay = await br.LoadScaledDownBitmapForDisplayAsync(Resources, Resource.Drawable.blurUnderShop2, options, 100, 100);
            //backgroundImg.SetImageBitmap(bitmapToDisplay);

            int[] memberImgs =
            {
                Resource.Drawable.Ahn,
                Resource.Drawable.kimchangwoo,
                Resource.Drawable.nakim,
                Resource.Drawable.yang,
                Resource.Drawable.joo
            };

            int[] toolImgs =
            {
                Resource.Drawable.visual_studio_logo,
                Resource.Drawable.xamarin_logo
            };

            int[] APIImgs =
            {
                Resource.Drawable.seoul_logo,
                Resource.Drawable.Google_logo
            };

            int belongImg = Resource.Drawable.hansei;

            ImageView[] iv_Members =
            {
                FindViewById<ImageView>(Resource.Id.iv_Member1),
                FindViewById<ImageView>(Resource.Id.iv_Member2),
                FindViewById<ImageView>(Resource.Id.iv_Member3),
                FindViewById<ImageView>(Resource.Id.iv_Member4),
                FindViewById<ImageView>(Resource.Id.iv_Member5)
            };

            ImageView[] iv_Tools =
            {
                FindViewById<ImageView>(Resource.Id.iv_VSC),
                FindViewById<ImageView>(Resource.Id.iv_Xamarin)
            };
            ImageView[] iv_APIs =
{
                FindViewById<ImageView>(Resource.Id.iv_SeoulLogo),
                FindViewById<ImageView>(Resource.Id.iv_GoogleLogo)
            };
            ImageView iv_belong = FindViewById<ImageView>(Resource.Id.iv_belong);
            
           btm_Members = new Bitmap[5];
            for (int i = 0; i < btm_Members.Length; i++)
            {
                BitmapResize br2 = new BitmapResize();
                BitmapFactory.Options options2 = await br2.GetBitmapOptionsOfImageAsync(Resources, memberImgs[i]);
                btm_Members[i] = await br2.LoadScaledDownBitmapForDisplayAsync(Resources, memberImgs[i], options2, 64, 64);
                //btm_Members[i] = BitmapFactory.DecodeResource(Resources, memberImgs[i]);
                iv_Members[i].SetImageBitmap(btm_Members[i]);
            }

            btm_Tools = new Bitmap[2];
            for (int i = 0; i < btm_Tools.Length; i++)
            {
                BitmapResize br2 = new BitmapResize();
                BitmapFactory.Options options2 = await br2.GetBitmapOptionsOfImageAsync(Resources, toolImgs[i]);
                btm_Tools[i] = await br2.LoadScaledDownBitmapForDisplayAsync(Resources, toolImgs[i], options2, 48, 48);
                //btm_Tools[i] = BitmapFactory.DecodeResource(Resources, toolImgs[i]);
                iv_Tools[i].SetImageBitmap(btm_Tools[i]);
            }

            btm_APIs = new Bitmap[2];
            for (int i = 0; i < btm_APIs.Length; i++)
            {
                BitmapResize br2 = new BitmapResize();
                BitmapFactory.Options options2 = await br2.GetBitmapOptionsOfImageAsync(Resources, APIImgs[i]);
                btm_APIs[i] = await br2.LoadScaledDownBitmapForDisplayAsync(Resources, APIImgs[i], options2, 32, 32);
                                //btm_APIs[i] = BitmapFactory.DecodeResource(Resources, APIImgs[i]);
                iv_APIs[i].SetImageBitmap(btm_APIs[i]);
            }

            BitmapResize br3 = new BitmapResize();
            BitmapFactory.Options options3 = await br3.GetBitmapOptionsOfImageAsync(Resources, belongImg);
            btm_belong = await br3.LoadScaledDownBitmapForDisplayAsync(Resources, belongImg, options3, 32, 32);
            //btm_APIs[i] = BitmapFactory.DecodeResource(Resources, APIImgs[i]);
            iv_belong.SetImageBitmap(btm_belong);
        }

        protected override void OnDestroy()
        {
            //bitmapToDisplay.Recycle();
            //bitmapToDisplay = null;
            //backgroundImg.Dispose();
            if (MainActivity.Activities.ContainsKey("DevelopStoryActivity"))
            {
                MainActivity.Activities.Remove("DevelopStoryActivity");
            }
            for (int i = 0; i < btm_Members.Length; i++)
            {
                btm_Members[i].Recycle();
                btm_Members[i] = null;
            }

            for (int i = 0; i < btm_Tools.Length; i++)
            {
                btm_Tools[i].Recycle();
                btm_Tools[i] = null;
            }

            for (int i = 0; i < btm_APIs.Length; i++)
            {
                btm_APIs[i].Recycle();
                btm_APIs[i] = null;
            }

            btm_belong.Recycle();
            btm_belong = null;
            
            base.OnDestroy();
        }
    }
}

