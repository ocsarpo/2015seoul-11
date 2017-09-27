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
using Android.Graphics;
using Android.Content.PM;

namespace App5
{
    [Activity(Label = "BookmarkActivity", Theme = "@android:style/Theme.NoTitleBar", ScreenOrientation = ScreenOrientation.Portrait)]
    public class BookmarkActivity : Activity
    {
        private List<ShopInformation> tempShopList;
        private ListView lvResult;
        private CustomListAdapter cusAdapter;
        int itemPostion = 0;
        //ImageView backgroundImg;
        //Bitmap bitmapToDisplay;

    protected override async void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.BookMarkResult);
            if (MainActivity.Activities.ContainsKey("BookmarkActivity"))
            {
                MainActivity.Activities["BookmarkActivity"].Finish();
                MainActivity.Activities.Remove("BookmarkActivity");
                MainActivity.Activities.Add("BookmarkActivity", this);
            }
            else
            {
                MainActivity.Activities.Add("BookmarkActivity", this);
            }
            // Create your application here
            //backgroundImg = FindViewById<ImageView>(Resource.Id.backgroundImg);
            //BitmapResize br = new BitmapResize();
            //BitmapFactory.Options options = await br.GetBitmapOptionsOfImageAsync(Resources, Resource.Drawable.MainImage2);
            //bitmapToDisplay = await br.LoadScaledDownBitmapForDisplayAsync(Resources, Resource.Drawable.MainImage2, options, 50, 50);
            //backgroundImg.SetImageBitmap(bitmapToDisplay);

            tempShopList = new List<ShopInformation>();
            lvResult = FindViewById<ListView>(Resource.Id.LV_SEARCHRESULT);
            cusAdapter = new CustomListAdapter(this.ApplicationContext, tempShopList);
            cusAdapter.GetData().Clear();
            SearchFunc();
            lvResult.Adapter = cusAdapter;
            lvResult.ItemClick += LvResult_ItemClick;
            RegisterForContextMenu(lvResult);
        }
        protected override void OnRestart()
        {

            cusAdapter.GetData().Clear();
            SearchFunc();
            lvResult.Adapter = cusAdapter;
            base.OnRestart();
        }
        private void LvResult_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            ListView lv = sender as ListView;
            itemPostion = e.Position;
            if (lv != null)
            {
                lv.ShowContextMenu();
            }
        }

        public override void OnCreateContextMenu(IContextMenu menu, View v, IContextMenuContextMenuInfo menuInfo)
        {
            if (v.Id == Resource.Id.LV_SEARCHRESULT)
            {
                menu.SetHeaderTitle(GetString(Resource.String.ListViewItemLongClickHeader));
                menu.Add(0, 1, 0, GetString(Resource.String.mapLayout));
                menu.Add(0, 2, 1, GetString(Resource.String.shopinfo));
            }
        }
        public override bool OnContextItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case 1:
                    {
                        ShopInformation selectShop = cusAdapter.GetData()[itemPostion];
                        foreach (var i in Data_ShopInfo.DIC_SHOP_XML_INFO_OVERALL_INFO)
                        {
                            if (i.Key.Equals(selectShop.IdentificationNumber))
                            {
                                var next = new Intent(this.ApplicationContext, typeof(UnderShopMapActivity));
                                next.PutExtra("Local", selectShop.Local);
                                next.PutExtra("SHOP_ID_NUMBER", selectShop.IdentificationNumber);
                                StartActivity(next);
                            }
                        }
                        break;
                    }
                case 2:
                    {
                        ShopInformation selectShop = cusAdapter.GetData()[itemPostion];
                        foreach (var i in Data_ShopInfo.DIC_SHOP_XML_INFO_OVERALL_INFO)
                        {
                            if (i.Key.Equals(selectShop.IdentificationNumber))
                            {
                                var next = new Intent(this.ApplicationContext, typeof(ShopInfoActivity));
                                next.PutExtra("SHOP_ID_NUMBER", selectShop.IdentificationNumber);
                                next.PutExtra("Local", selectShop.Local);
                                StartActivity(next);
                            }
                        }
                        break;
                    }
            }
            return base.OnContextItemSelected(item);
        }


        private void SearchFunc()
        {
            foreach(var i in Data_ShopInfo.DIC_SHOP_DB_INFO_OVERALL_INFO)
            {
                if(i.Value.BookMark == 1)
                {
                    tempShopList.Add(Data_ShopInfo.DIC_SHOP_XML_INFO_OVERALL_INFO[i.Value.IdentificationNumber]);
                }
            }
        }
        protected override void OnDestroy()
        {
            //bitmapToDisplay.Recycle();
            //bitmapToDisplay = null;
            //backgroundImg.Dispose();
            if (MainActivity.Activities.ContainsKey("BookmarkActivity"))
            {
                MainActivity.Activities.Remove("BookmarkActivity");
            }
            base.OnDestroy();
        }
    }
}