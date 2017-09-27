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
    [Activity(Label = "SearchResult", Theme = "@android:style/Theme.NoTitleBar", ScreenOrientation = ScreenOrientation.Portrait)]
    public class SearchResultActivity : Activity
    {
        private List<ShopInformation> tempShopList;
        private ListView lvResult;
        private EditText et;
        //ImageView backgroundImg;
        private CustomListAdapter cusAdapter;
        int itemPostion = 0;
        //private Bitmap bitmapToDisplay;

        protected override async void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.SearchResult);

            if (MainActivity.Activities.ContainsKey("SearchResultActivity"))
            {
                MainActivity.Activities["SearchResultActivity"].Finish();
                MainActivity.Activities.Remove("SearchResultActivity");
                MainActivity.Activities.Add("SearchResultActivity", this);
            }
            else
            {
                MainActivity.Activities.Add("SearchResultActivity", this);
            }

            tempShopList = new List<ShopInformation>();
            string mSearchString = Intent.GetStringExtra("searchString");
            //backgroundImg = FindViewById<ImageView>(Resource.Id.backgroundImg);
            //BitmapResize br = new BitmapResize();
            //BitmapFactory.Options options = await br.GetBitmapOptionsOfImageAsync(Resources, Resource.Drawable.MainImage2);
            //bitmapToDisplay = await br.LoadScaledDownBitmapForDisplayAsync(Resources, Resource.Drawable.MainImage2, options, 50, 50);
            
            //backgroundImg.SetImageBitmap(bitmapToDisplay);

            lvResult = FindViewById<ListView>(Resource.Id.LV_SEARCHRESULT);
            et = FindViewById<EditText>(Resource.Id.EDT_RESEARCH);
            Button bt = FindViewById<Button>(Resource.Id.BTN_RESEARCH);

            et.Text = mSearchString;
            cusAdapter = new CustomListAdapter(this.ApplicationContext, tempShopList);
            cusAdapter.GetData().Clear();
            lvResult.Adapter = cusAdapter;
            if (mSearchString != null)
            {
                SearchFunc(mSearchString);
            }

            bt.Click += new EventHandler(OnResearchClick);
            lvResult.ItemClick += LvResult_ItemClick;
            RegisterForContextMenu(lvResult);

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

        private void OnResearchClick(object sender, EventArgs e)
        {
            cusAdapter.GetData().Clear();
            if(et.Text != "")
                SearchFunc(et.Text.ToString());
            lvResult.InvalidateViews();
        }

        private void SearchFunc(string mSearchString)
        {
            List<ShopInformation> tempShopInfo = null;

            if (mSearchString == "")
            {
                ShopInformation failGetSearch = new ShopInformation(0, 0);
                failGetSearch.LocalNam = "not found";
                tempShopList.Add(failGetSearch);
            }
            else
            {//검색 추가
                tempShopInfo = Data_ShopInfo.DIC_SHOPINFO_OVERALL_INFO[MainActivity.Ulgiro3];
                SearchInDicShopInfo(tempShopInfo, mSearchString);
                tempShopInfo = Data_ShopInfo.DIC_SHOPINFO_OVERALL_INFO[MainActivity.Ulgiro4];
                SearchInDicShopInfo(tempShopInfo, mSearchString);
                tempShopInfo = Data_ShopInfo.DIC_SHOPINFO_OVERALL_INFO[MainActivity.UlgiEnterance];
                SearchInDicShopInfo(tempShopInfo, mSearchString);
                tempShopInfo = Data_ShopInfo.DIC_SHOPINFO_OVERALL_INFO[MainActivity.CityHall];
                SearchInDicShopInfo(tempShopInfo, mSearchString);
                tempShopInfo = Data_ShopInfo.DIC_SHOPINFO_OVERALL_INFO[MainActivity.InHyeon];
                SearchInDicShopInfo(tempShopInfo, mSearchString);
                tempShopInfo = Data_ShopInfo.DIC_SHOPINFO_OVERALL_INFO[MainActivity.Sindang];
                SearchInDicShopInfo(tempShopInfo, mSearchString);

                tempShopInfo = Data_ShopInfo.DIC_SHOPINFO_OVERALL_INFO[MainActivity.JongKak];
                SearchInDicShopInfo(tempShopInfo, mSearchString);
                tempShopInfo = Data_ShopInfo.DIC_SHOPINFO_OVERALL_INFO[MainActivity.Jongro4];
                SearchInDicShopInfo(tempShopInfo, mSearchString);
                tempShopInfo = Data_ShopInfo.DIC_SHOPINFO_OVERALL_INFO[MainActivity.Majeon];
                SearchInDicShopInfo(tempShopInfo, mSearchString);
                tempShopInfo = Data_ShopInfo.DIC_SHOPINFO_OVERALL_INFO[MainActivity.ChongGye5];
                SearchInDicShopInfo(tempShopInfo, mSearchString);
                tempShopInfo = Data_ShopInfo.DIC_SHOPINFO_OVERALL_INFO[MainActivity.Jongoh];
                SearchInDicShopInfo(tempShopInfo, mSearchString);
                tempShopInfo = Data_ShopInfo.DIC_SHOPINFO_OVERALL_INFO[MainActivity.DongDaeMun];
                SearchInDicShopInfo(tempShopInfo, mSearchString);
                tempShopInfo = Data_ShopInfo.DIC_SHOPINFO_OVERALL_INFO[MainActivity.ChongGye6];
                SearchInDicShopInfo(tempShopInfo, mSearchString);

                tempShopInfo = Data_ShopInfo.DIC_SHOPINFO_OVERALL_INFO[MainActivity.MyeongDong];
                SearchInDicShopInfo(tempShopInfo, mSearchString);
                tempShopInfo = Data_ShopInfo.DIC_SHOPINFO_OVERALL_INFO[MainActivity.MyeongDongStation];
                SearchInDicShopInfo(tempShopInfo, mSearchString);
                tempShopInfo = Data_ShopInfo.DIC_SHOPINFO_OVERALL_INFO[MainActivity.NamDaeMun];
                SearchInDicShopInfo(tempShopInfo, mSearchString);
                tempShopInfo = Data_ShopInfo.DIC_SHOPINFO_OVERALL_INFO[MainActivity.Sogong];
                SearchInDicShopInfo(tempShopInfo, mSearchString);
                tempShopInfo = Data_ShopInfo.DIC_SHOPINFO_OVERALL_INFO[MainActivity.HoeHyeon];
                SearchInDicShopInfo(tempShopInfo, mSearchString);

                tempShopInfo = Data_ShopInfo.DIC_SHOPINFO_OVERALL_INFO[MainActivity.Gangnam];
                SearchInDicShopInfo(tempShopInfo, mSearchString);
                tempShopInfo = Data_ShopInfo.DIC_SHOPINFO_OVERALL_INFO[MainActivity.Jamsil];
                SearchInDicShopInfo(tempShopInfo, mSearchString);

                tempShopInfo = Data_ShopInfo.DIC_SHOPINFO_OVERALL_INFO[MainActivity.GangNamTerminalPart1];
                SearchInDicShopInfo(tempShopInfo, mSearchString);
                tempShopInfo = Data_ShopInfo.DIC_SHOPINFO_OVERALL_INFO[MainActivity.GangNamTerminalPart2];
                SearchInDicShopInfo(tempShopInfo, mSearchString);
                tempShopInfo = Data_ShopInfo.DIC_SHOPINFO_OVERALL_INFO[MainActivity.GangNamTerminalPart3];
                SearchInDicShopInfo(tempShopInfo, mSearchString);


                tempShopInfo = Data_ShopInfo.DIC_SHOPINFO_OVERALL_INFO[MainActivity.YeongDeungPoStation];
                SearchInDicShopInfo(tempShopInfo, mSearchString);
                tempShopInfo = Data_ShopInfo.DIC_SHOPINFO_OVERALL_INFO[MainActivity.YeongDeungPoMarketNewtown];
                SearchInDicShopInfo(tempShopInfo, mSearchString);
                tempShopInfo = Data_ShopInfo.DIC_SHOPINFO_OVERALL_INFO[MainActivity.YeongDeungPoRotary];
                SearchInDicShopInfo(tempShopInfo, mSearchString);
            }

            tempShopInfo = null;
        }

        private void SearchInDicShopInfo(List<ShopInformation> tempShopInfo, string searchContent)
        {

            foreach (ShopInformation shopInfo in tempShopInfo)
            {
                if (shopInfo.LocalNam.Contains(searchContent)
                    || shopInfo.ShopName.Contains(searchContent)
                    || shopInfo.Category.Contains(searchContent))
                {
                    tempShopList.Add(shopInfo);
                }
            }
        }
        protected override void OnDestroy()
        {
            //bitmapToDisplay.Recycle();
            //bitmapToDisplay = null;
            //backgroundImg.Dispose();
            if (MainActivity.Activities.ContainsKey("SearchResultActivity"))
            {
                MainActivity.Activities.Remove("SearchResultActivity");
            }
            base.OnDestroy();

        }

    }
}