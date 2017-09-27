
using Android.App;
using Android.OS;
using Android.Content.PM;
using Android.Support.V4.Widget;
using System.Collections.Generic;
using Android.Widget;
using Android.Views;
using System.Xml;
using System.IO;
using Android.Content;
using Android.Net;

namespace App5
{
    [Activity(Label = "inforactiviry", ScreenOrientation = ScreenOrientation.Portrait, Theme = "@android:style/Theme.NoTitleBar")]
    public class InforActivity : Activity
    {
        DrawerLayout mDrawerLayout;
        List<string> mLeftItems = new List<string>();
        ArrayAdapter mLeftAdapter;
        ListView mLeftDrawer;
        List<IMenuItem> mLeftMenu = new List<IMenuItem>();
        XmlNodeList nodeList;
        XmlNodeList strList;

        Context thisContext = Application.Context;

        string local = "";
        string[] str_name; string[] str_address; string[] str_scale;
        string[] str_admin; string[] str_operation; string[] str_industry;
        string[] str_surrounding; string[] str_direction;

        string shop_name; string shop_address; string shop_scale;
        string shop_admin; string shop_operation; string shop_industry;
        string shop_surrounding; string shop_direction;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            if (MainActivity.Activities.ContainsKey("InforActivity"))
            {
                MainActivity.Activities["InforActivity"].Finish();
                MainActivity.Activities.Remove("InforActivity");
                MainActivity.Activities.Add("InforActivity", this);
            }
            else
            {
                MainActivity.Activities.Add("InforActivity", this);
            }

            SetContentView(Resource.Layout.RegionInfoLayout);
            Toast msg = Toast.MakeText(this, GetString(Resource.String.ActionBarComment), ToastLength.Short);
            msg.SetGravity(GravityFlags.Top | GravityFlags.Left, 0, 250);
            msg.Show();

            mDrawerLayout = FindViewById<DrawerLayout>(Resource.Id.myDrawer2);
            mLeftDrawer = FindViewById<ListView>(Resource.Id.leftListView2);
            StreamReader sr = new StreamReader(Assets.Open("UnderShop.xml"));

            XmlDocument xml = new XmlDocument();
            xml.Load(sr);

            string slindg_item = Intent.GetStringExtra("Sliding_item");
            nodeList = xml.SelectNodes(slindg_item);

            ////Change the price on the books.
            foreach (XmlNode i in nodeList)
            {
                mLeftItems.Add(i["undershop_name"].InnerText);
            }

            //mDrawerToggle = new ActionBarDrawerToggle(this, mDrawerLayout, Resource.Drawable.Icon,Resource.String.open_drawer,Resource.String.close_drawer);
            mLeftAdapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleListItem1, mLeftItems);
            mLeftDrawer.Adapter = mLeftAdapter;

            mLeftDrawer.ItemClick += listView_ItemClick;
            ///////slide¸Þ´º ³¡/////////

            local = Intent.GetStringExtra("Local");

            TextView tv_address = FindViewById<TextView>(Resource.Id.tv_address);
            TextView tv_scale = FindViewById<TextView>(Resource.Id.tv_scale);
            TextView tv_admin = FindViewById<TextView>(Resource.Id.tv_admin);
            TextView tv_operation = FindViewById<TextView>(Resource.Id.tv_operation);
            TextView tv_industry = FindViewById<TextView>(Resource.Id.tv_industry);
            TextView tv_surround = FindViewById<TextView>(Resource.Id.tv_surround);
            TextView tv_direction = FindViewById<TextView>(Resource.Id.tv_direction);
            TextView tv_localName = FindViewById<TextView>(Resource.Id.tv_localName);

            LinearLayout shop_layout = FindViewById<LinearLayout>(Resource.Id.shop_layout);
            LinearLayout parking_info = FindViewById<LinearLayout>(Resource.Id.parking_info);

            StreamReader bsr = new StreamReader(Assets.Open("UnderShopBasicInfo.xml"));

            XmlDocument bxml = new XmlDocument();
            bxml.Load(bsr);

            strList = bxml.SelectNodes("UnderShopBasicInfomation/" + local);

            foreach (XmlNode i in strList)
            {
                shop_name = (i["shop_name"].InnerText);
                shop_address = (i["shop_address"].InnerText);
                shop_scale = (i["shop_scale"].InnerText);
                shop_admin = (i["shop_administration"].InnerText);
                shop_operation = (i["shop_operation"].InnerText);
                shop_industry = (i["shop_industry"].InnerText);
                shop_surrounding = (i["shop_surroundings"].InnerText);
                shop_direction = (i["shop_direction"].InnerText);
            }

            str_name = shop_name.Split('_');
            str_address = shop_address.Split('_');
            str_scale = shop_scale.Split('_');
            str_admin = shop_admin.Split('_');
            str_operation = shop_operation.Split('_');
            str_industry = shop_industry.Split('_');
            str_surrounding = shop_surrounding.Split('_');
            str_direction = shop_direction.Split('_');

            tv_localName.Append(str_name[0]);

            //tv_address.Append("\n");
            //tv_scale.Append("\n");
            //tv_admin.Append("\n");
            //tv_operation.Append("\n");
            //tv_industry.Append("\n");
            //tv_surround.Append("\n");
            //tv_direction.Append("\n");
            tv_address.Text = "";
            tv_scale.Text = "";
            tv_admin.Text = "";
            tv_operation.Text = "";
            tv_industry.Text = "";
            tv_surround.Text = "";
            tv_direction.Text = "";

            foreach (string s in str_address)
            {
                tv_address.Append(s + "\n");
            }
            foreach (string s in str_scale)
            {
                tv_scale.Append(s + "\n");
            }
            foreach (string s in str_admin)
            {
                tv_admin.Append(s + "\n");
            }
            foreach (string s in str_operation)
            {
                tv_operation.Append(s + "\n");
            }
            foreach (string s in str_industry)
            {
                tv_industry.Append(s + "\n");
            }
            foreach (string s in str_surrounding)
            {
                tv_surround.Append(s + "\n");
            }
            foreach (string s in str_direction)
            {
                tv_direction.Append(s + "\n");
            }

            shop_layout.Click += delegate
            {
                var next = new Intent(this, typeof(UnderShopMapActivity));
                next.PutExtra("Local", local);
                StartActivity(next);
            };
            parking_info.Click += delegate
            {
                ConnectivityManager connectivityManager = (ConnectivityManager)GetSystemService(ConnectivityService);
                NetworkInfo activeConnection = connectivityManager.ActiveNetworkInfo;
                bool isOnline = (activeConnection != null) && activeConnection.IsConnected;

                if (isOnline)
                {
                    var nex_acc = new Intent(thisContext, typeof(GoogleMapActivity));
                    nex_acc.PutExtra("Local", local);
                    StartActivity(nex_acc);
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
            };
        }
        private void listView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            var item = mLeftItems[e.Position];
            string areaname = "";

            //Change the price on the books.
            foreach (XmlNode i in nodeList)
            {
                if (item == (i["undershop_name"].InnerText))
                {
                    local = i["menu_name"].InnerText;
                    areaname = i["area_name"].InnerText;
                }
            }
            var next = new Intent(this, typeof(InforActivity));
            next.PutExtra("Sliding_item", "Undershop/" + areaname);
            next.PutExtra("Local", local);
            Finish();
            StartActivity(next);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
        }
    }
}