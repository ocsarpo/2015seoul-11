using System.Collections.Generic;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using Android.Content.PM;
using System.Xml;
using Android.Support.V4.Widget;
using System.IO;

namespace App5
{
    [Activity(Label = "NextActiviy", ScreenOrientation = ScreenOrientation.Portrait)]
    public class ActionBarActivity : Activity
    {
        DrawerLayout mDrawerLayout;
        List<string> mLeftItems = new List<string>();
        ArrayAdapter mLeftAdapter;
        ListView mLeftDrawer;
        List<IMenuItem> mLeftMenu = new List<IMenuItem>();

        XmlNodeList nodeList;
        string local = "";
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.ContentSelectLayout);

            Toast msg = Toast.MakeText(this, GetString(Resource.String.ActionBarComment), ToastLength.Short);
            msg.SetGravity(GravityFlags.Top | GravityFlags.Left, 0, 250);
            msg.Show();
            mDrawerLayout = FindViewById<DrawerLayout>(Resource.Id.myDrawer);
            mLeftDrawer = FindViewById<ListView>(Resource.Id.leftListView);
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

            local = Intent.GetStringExtra("Local");

            this.ActionBar.NavigationMode = ActionBarNavigationMode.Tabs;

            ActionBar.SetDisplayShowHomeEnabled(false);
            ActionBar.SetDisplayShowTitleEnabled(false);
            ActionBar.SetDisplayShowCustomEnabled(true);

            //adding audio tab
            AddTab("infomation", Resource.Drawable.Icon, new Tab_Information(local));

            ////adding video tab 
            AddTab("undershopMap", Resource.Drawable.Icon, new Tab_UnderShopMap(local));
            AddTab("parkingLot", Resource.Drawable.Icon, new Tab_ParkingLot(local));
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
            Finish();
            var next = new Intent(this, typeof(ActionBarActivity));
            next.PutExtra("Sliding_item", "Undershop/" + areaname);
            next.PutExtra("Local", local);
            StartActivity(next);
        }


        void AddTab(string tabText, int iconResourceId, Fragment view)
        {
            var tab = this.ActionBar.NewTab();
            tab.SetText(tabText);
            tab.SetIcon(iconResourceId);

            tab.TabSelected += delegate (object sender, ActionBar.TabEventArgs e)
            {
                var fragment = this.FragmentManager.FindFragmentById(Resource.Id.fragmentContainer);
                if (fragment != null)
                    e.FragmentTransaction.Remove(fragment);
                e.FragmentTransaction.Add(Resource.Id.fragmentContainer, view);
            };
            tab.TabUnselected += delegate (object sender, ActionBar.TabEventArgs e)
            {
                e.FragmentTransaction.Remove(view);
            };
            this.ActionBar.AddTab(tab);
        }
        protected override void OnDestroy()
        {
            base.OnDestroy();
        }

    }

}