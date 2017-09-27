using System;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using Android.Content.PM;
using System.Threading.Tasks;
using Android.Graphics;
using Android.Content.Res;

namespace App5
{
    [Activity(Label = "@string/ApplicationName", ScreenOrientation = ScreenOrientation.Portrait)]
    public class LocalSelectActivity : Activity
    {
        LinearLayout LinearLayout1;
        //Bitmap bitmapToDisplay;
        Bitmap[] btm_Regions;
        public static bool backBTNClick = false;
        protected override async void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            if (MainActivity.Activities.ContainsKey("LocalSelectActivity"))
            {
                MainActivity.Activities["LocalSelectActivity"].Finish();
                MainActivity.Activities.Remove("LocalSelectActivity");
                MainActivity.Activities.Add("LocalSelectActivity", this);
            }
            else
            {
                MainActivity.Activities.Add("LocalSelectActivity", this);
            }
            RequestWindowFeature(WindowFeatures.NoTitle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            //BitmapResize br = new BitmapResize();
            //BitmapFactory.Options options = await br.GetBitmapOptionsOfImageAsync(Resources, Resource.Drawable.MainImage2);
            //bitmapToDisplay = await br.LoadScaledDownBitmapForDisplayAsync(Resources, Resource.Drawable.MainImage2, options, 50, 50);

            //backgroundImg.SetImageBitmap(bitmapToDisplay);
            ImageView[] iv_regions = {
                FindViewById<ImageView>(Resource.Id.BTN_ULGIRO),
                FindViewById<ImageView>(Resource.Id.BTN_JONGRO),
                FindViewById<ImageView>(Resource.Id.BTN_MYENGDONG),
                FindViewById<ImageView>(Resource.Id.BTN_GANGNAM),
                FindViewById<ImageView>(Resource.Id.BTN_TERMINAL),
                FindViewById<ImageView>(Resource.Id.BTN_YEONGDEONGPO)
            };
            int[] regionImgs =
            {
                Resource.Drawable.ulgiroIcon,
                Resource.Drawable.JongroIcon,
                Resource.Drawable.MyeongdongIcon,
                Resource.Drawable.gangnamIcon,
                Resource.Drawable.TerminalIcon,
                Resource.Drawable.yeongIcon
            };
            btm_Regions = new Bitmap[iv_regions.Length];
            for(int i =0; i < iv_regions.Length; i++)
            {
                RegisterForContextMenu(iv_regions[i]);
                iv_regions[i].Click += new EventHandler(OnButtonClick);

                BitmapResize br2 = new BitmapResize();
                BitmapFactory.Options options2 = await br2.GetBitmapOptionsOfImageAsync(Resources, regionImgs[i]);
                btm_Regions[i] = await br2.LoadScaledDownBitmapForDisplayAsync(Resources, regionImgs[i], options2, 64,64);
                //btm_Members[i] = BitmapFactory.DecodeResource(Resources, memberImgs[i]);
                iv_regions[i].SetImageBitmap(btm_Regions[i]);
            }
            //ImageView BTN_ULGIRO = FindViewById<ImageView>(Resource.Id.BTN_ULGIRO);
            //RegisterForContextMenu(BTN_ULGIRO);
            //BTN_ULGIRO.Click += new EventHandler(OnButtonClick);
            //ImageView BTN_JONGRO = FindViewById<ImageView>(Resource.Id.BTN_JONGRO);
            //RegisterForContextMenu(BTN_JONGRO);
            //BTN_JONGRO.Click += new EventHandler(OnButtonClick);
            //ImageView BTN_MYENGDONG = FindViewById<ImageView>(Resource.Id.BTN_MYENGDONG);
            //RegisterForContextMenu(BTN_MYENGDONG);
            //BTN_MYENGDONG.Click += new EventHandler(OnButtonClick);
            //ImageView BTN_GANGNAM = FindViewById<ImageView>(Resource.Id.BTN_GANGNAM);
            //RegisterForContextMenu(BTN_GANGNAM);
            //BTN_GANGNAM.Click += new EventHandler(OnButtonClick);
            //ImageView BTN_TERMINAL = FindViewById<ImageView>(Resource.Id.BTN_TERMINAL);
            //RegisterForContextMenu(BTN_TERMINAL);
            //BTN_TERMINAL.Click += new EventHandler(OnButtonClick);
            //ImageView BTN_YEONGDEONGPO = FindViewById<ImageView>(Resource.Id.BTN_YEONGDEONGPO);
            //RegisterForContextMenu(BTN_YEONGDEONGPO);
            //BTN_YEONGDEONGPO.Click += new EventHandler(OnButtonClick);


            LinearLayout1 = FindViewById<LinearLayout>(Resource.Id.LinearLayout1);

            Button BTN_SEARCH = FindViewById<Button>(Resource.Id.BTN_SEARCH);
            BTN_SEARCH.Click += OnSearchClick;

            LinearLayout BTN_BOOKMARK = FindViewById<LinearLayout>(Resource.Id.BTN_BOOKMARK);
            BTN_BOOKMARK.Click += BTN_BOOKMARK_Click;
            LinearLayout BTN_EVENT = FindViewById<LinearLayout>(Resource.Id.BTN_EVENT);
            BTN_EVENT.Click += BTN_EVENT_Click;
            LinearLayout BTN_DEVELOP = FindViewById<LinearLayout>(Resource.Id.BTN_DEVELOP);
            BTN_DEVELOP.Click += BTN_DEVELOP_Click;
        }

        private void BTN_DEVELOP_Click(object sender, EventArgs e)
        {
            StartActivity(typeof(DevelopStoryActivity));
        }

        private void BTN_EVENT_Click(object sender, EventArgs e)
        {
            Android.Net.ConnectivityManager connectivityManager = (Android.Net.ConnectivityManager)GetSystemService(ConnectivityService);
            Android.Net.NetworkInfo activeConnection = connectivityManager.ActiveNetworkInfo;
            bool isOnline = (activeConnection != null) && activeConnection.IsConnected;

            if (isOnline)
            {
                StartActivity(typeof(EventLoadingActivity));
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

        private void BTN_BOOKMARK_Click(object sender, EventArgs e)
        {
            Intent next = new Intent(this, typeof(BookmarkActivity));
           StartActivity(next);      
        }

        private void OnSearchClick(object sender, EventArgs e)
        {
            EditText EDT_SEARCH = FindViewById<EditText>(Resource.Id.EDT_SEARCH);
            if (EDT_SEARCH.Text != "")
            {
                string searchContent = EDT_SEARCH.Text.ToString().Trim();

                Intent next = new Intent(this, typeof(SearchResultActivity));
                next.PutExtra("searchString", searchContent);
               StartActivity(next);    

            }
            else
                Toast.MakeText(this, GetString(Resource.String.searchAlert), ToastLength.Short).Show();
        }

        private void OnButtonClick(object sender, EventArgs e)
        {
            ImageView btn = sender as ImageView;
            if (btn != null)
            {
                btn.ShowContextMenu();
            }
        }


        public override void OnCreateContextMenu(IContextMenu menu, View v, IContextMenuContextMenuInfo menuInfo)
        {
            base.OnCreateOptionsMenu(menu);
            MenuInflater inflater = this.MenuInflater;
            switch (v.Tag.ToString())
            {
                case "ULGIRO":
                    inflater.Inflate(Resource.Menu.EulgiMenu, menu);
                    break;
                case "JONGRO":
                    inflater.Inflate(Resource.Menu.JongroMenu, menu);
                    break;
                case "MYENGDONG":
                    inflater.Inflate(Resource.Menu.MyeongdongMenu, menu);
                    break;
                case "GANGNAM":
                    inflater.Inflate(Resource.Menu.GangnamMenu, menu);
                    break;
                case "TERMINAL":
                    inflater.Inflate(Resource.Menu.TerminalMenu, menu);
                    break;
                case "YEONGDEONGPO":
                    inflater.Inflate(Resource.Menu.YeongdeungpoMenu, menu);
                    break;
            }
        }

        public override bool OnContextItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.eulgiro3:
                    {
                        var next = new Intent(this, typeof(InforActivity));
                        next.PutExtra("Sliding_item", "Undershop/eulgiro");
                        next.PutExtra("Local", MainActivity.Ulgiro3);
                        StartActivity(next);
                        break;
                    }
                case Resource.Id.eulgiro4:
                    {
                        var next = new Intent(this, typeof(InforActivity));
                        next.PutExtra("Sliding_item", "Undershop/eulgiro");
                        next.PutExtra("Local", MainActivity.Ulgiro4);
                        StartActivity(next);
                        break;
                    }
                case Resource.Id.eulgientrance:
                    {
                        var next = new Intent(this, typeof(InforActivity));
                        next.PutExtra("Sliding_item", "Undershop/eulgiro");
                        next.PutExtra("Local", MainActivity.UlgiEnterance);
                       StartActivity(next); 
                        break;
                    }
                case Resource.Id.cityhallsquare:
                    {
                        var next = new Intent(this, typeof(InforActivity));
                        next.PutExtra("Sliding_item", "Undershop/eulgiro");
                        next.PutExtra("Local", MainActivity.CityHall);
                       StartActivity(next); 
                        break;
                    }
                case Resource.Id.inhyeon:
                    {
                        var next = new Intent(this, typeof(InforActivity));
                        next.PutExtra("Sliding_item", "Undershop/eulgiro");
                        next.PutExtra("Local", MainActivity.InHyeon);
                       StartActivity(next); 
                        break;
                    }
                case Resource.Id.shindang:
                    {
                        var next = new Intent(this, typeof(InforActivity));
                        next.PutExtra("Sliding_item", "Undershop/eulgiro");
                        next.PutExtra("Local", MainActivity.Sindang);
                       StartActivity(next); 
                        break;
                    }
                case Resource.Id.jongkak:
                    {
                        var next = new Intent(this, typeof(InforActivity));
                        next.PutExtra("Sliding_item", "Undershop/jongro");
                        next.PutExtra("Local", MainActivity.JongKak);
                       StartActivity(next); 
                        break;
                    }
                case Resource.Id.jongro4:
                    {
                        var next = new Intent(this, typeof(InforActivity));
                        next.PutExtra("Sliding_item", "Undershop/jongro");
                        next.PutExtra("Local", MainActivity.Jongro4);
                       StartActivity(next); 
                        break;
                    }
                case Resource.Id.majeon:
                    {
                        var next = new Intent(this, typeof(InforActivity));
                        next.PutExtra("Sliding_item", "Undershop/jongro");
                        next.PutExtra("Local", MainActivity.Majeon);
                       StartActivity(next); 
                        break;
                    }
                case Resource.Id.chonggye5:
                    {
                        var next = new Intent(this, typeof(InforActivity));
                        next.PutExtra("Sliding_item", "Undershop/jongro");
                        next.PutExtra("Local", MainActivity.ChongGye5);
                       StartActivity(next); 
                        break;
                    }
                case Resource.Id.jongoh:
                    {
                        var next = new Intent(this, typeof(InforActivity));
                        next.PutExtra("Sliding_item", "Undershop/jongro");
                        next.PutExtra("Local", MainActivity.Jongoh);
                       StartActivity(next); 
                        break;
                    }
                case Resource.Id.dongdaemoon:
                    {
                        var next = new Intent(this, typeof(InforActivity));
                        next.PutExtra("Sliding_item", "Undershop/jongro");
                        next.PutExtra("Local", MainActivity.DongDaeMun);
                       StartActivity(next); 
                        break;
                    }
                case Resource.Id.chonggye6:
                    {
                        var next = new Intent(this, typeof(InforActivity));
                        next.PutExtra("Sliding_item", "Undershop/eulgiro");
                        next.PutExtra("Local", MainActivity.ChongGye6);
                       StartActivity(next); 
                        break;
                    }
                case Resource.Id.myeongdong:
                    { 
                        var next = new Intent(this, typeof(InforActivity));
                        next.PutExtra("Sliding_item", "Undershop/myeongdong");
                        next.PutExtra("Local", MainActivity.MyeongDong);
                        StartActivity(next);
                        break;
                    }
                case Resource.Id.myeongdongstation:
                    {
                        var next = new Intent(this, typeof(InforActivity));
                        next.PutExtra("Sliding_item", "Undershop/myeongdong");
                        next.PutExtra("Local", MainActivity.MyeongDongStation);
                        StartActivity(next);
                        break;
                    }
                case Resource.Id.sogong:
                    {
                        var next = new Intent(this, typeof(InforActivity));
                        next.PutExtra("Sliding_item", "Undershop/myeongdong");
                        next.PutExtra("Local", MainActivity.Sogong);
                       StartActivity(next); 
                        break;
                    }
                case Resource.Id.namdaemun:
                    {
                        var next = new Intent(this, typeof(InforActivity));
                        next.PutExtra("Sliding_item", "Undershop/myeongdong");
                        next.PutExtra("Local", MainActivity.NamDaeMun);
                        StartActivity(next);
                        break;
                    }
                case Resource.Id.hoehyeon:
                    {
                        var next = new Intent(this, typeof(InforActivity));
                        next.PutExtra("Sliding_item", "Undershop/myeongdong");
                        next.PutExtra("Local", MainActivity.HoeHyeon);
                        StartActivity(next);
                        break;
                    }
                case Resource.Id.gangnam:
                    {
                        var next = new Intent(this, typeof(InforActivity));
                        next.PutExtra("Sliding_item", "Undershop/gangnam");
                        next.PutExtra("Local", MainActivity.Gangnam);
                       StartActivity(next); 
                        break;

                    }
                case Resource.Id.jamsil:
                    {
                        var next = new Intent(this, typeof(InforActivity));
                        next.PutExtra("Sliding_item", "Undershop/gangnam");
                        next.PutExtra("Local", MainActivity.Jamsil);
                       StartActivity(next); 
                        break;
                    }
                case Resource.Id.gangnamterminal1:
                    {
                        var next = new Intent(this, typeof(InforActivity));
                        next.PutExtra("Sliding_item", "Undershop/termianl");
                        next.PutExtra("Local", MainActivity.GangNamTerminalPart1);
                        StartActivity(next);
                        break;
                    }
                case Resource.Id.gangnamterminal2:
                    {
                        var next = new Intent(this, typeof(InforActivity));
                        next.PutExtra("Sliding_item", "Undershop/termianl");
                        next.PutExtra("Local", MainActivity.GangNamTerminalPart2);
                        StartActivity(next);
                        break;
                    }
                case Resource.Id.gangnamterminal3:
                    {
                        var next = new Intent(this, typeof(InforActivity));
                        next.PutExtra("Sliding_item", "Undershop/termianl");
                        next.PutExtra("Local", MainActivity.GangNamTerminalPart3);
                        StartActivity(next);
                        break;
                    }
                case Resource.Id.yeongdeungpo:
                    {
                        var next = new Intent(this, typeof(InforActivity));
                        next.PutExtra("Sliding_item", "Undershop/yeongdeungpo");
                        next.PutExtra("Local", MainActivity.YeongDeungPoStation);
                        StartActivity(next);
                        break;
                    }
                case Resource.Id.yeongdeungporotary:
                    {
                        var next = new Intent(this, typeof(InforActivity));
                        next.PutExtra("Sliding_item", "Undershop/yeongdeungpo");
                        next.PutExtra("Local", MainActivity.YeongDeungPoRotary);
                        StartActivity(next);
                        break;
                    }
                case Resource.Id.yeongdeungpomarket:
                    {
                        var next = new Intent(this, typeof(InforActivity));
                        next.PutExtra("Sliding_item", "Undershop/yeongdeungpo");
                        next.PutExtra("Local", MainActivity.YeongDeungPoMarketNewtown);
                        StartActivity(next);
                        break;
                    }
            }
            return base.OnOptionsItemSelected(item);
        }
        public override void OnBackPressed()
        {
            base.OnBackPressed();
            backBTNClick = true;
        }

        protected override void OnDestroy()
        {
            //bitmapToDisplay.Recycle();
            //bitmapToDisplay = null;
            //backgroundImg.Dispose();
            if (MainActivity.Activities.ContainsKey("LocalSelectActivity"))
            {
                MainActivity.Activities.Remove("LocalSelectActivity");
            }
            for (int i = 0; i < btm_Regions.Length; i++)
            {
                btm_Regions[i].Recycle();
                btm_Regions[i] = null;
            }
            base.OnDestroy();
        }
    }
}

