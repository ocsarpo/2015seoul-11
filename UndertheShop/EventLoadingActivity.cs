using System;
using System.Collections.Generic;

using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using System.Net.Http;
using System.Threading.Tasks;
using Android.Content.PM;

namespace App5
{
    [Activity(Label = "progressActivyti", Theme = "@android:style/Theme.NoTitleBar", ScreenOrientation = ScreenOrientation.Portrait)]
    public class EventLoadingActivity : Activity
    {
        public static List<UnderShopEventInfo> list_UnderShopInfo = new List<UnderShopEventInfo>();
        ProgressBar pb_Loading;

        private int count = 0;
          
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            RequestWindowFeature(WindowFeatures.NoTitle);
            SetContentView(Resource.Layout.EventLoading);

            pb_Loading = FindViewById<ProgressBar>(Resource.Id.pb_EventLoading);
            ////////////////////////////////////////////////////
            LoadCalendarData();
        }
        public void LoadCalendarData()
        {
            list_UnderShopInfo.Clear();

            for (int date = 1; date <= 31; date++)
            {
                GetUnderShopEventInfo(date);
            }
        }

        public async void GetUnderShopEventInfo(int date)
        {
            try
            {

                string url = (@"http://www.sisul.or.kr/open_content/sub/schedule/detail.do?year=" + DateTime.Now.Year + @"&month=" + DateTime.Now.Month + @"&day=" + date + @"&site_div=undershop").Trim();
                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();

                bool hasMarkString = responseBody.Contains("event_list");

                if (hasMarkString)
                {
                    string splitText = responseBody.Split(new string[] { "event_list" }, StringSplitOptions.None)[1];

                    hasMarkString = splitText.Contains("margin_t10");

                    if (hasMarkString)
                    {
                        splitText = splitText.Split(new string[] { "margin_t10" }, StringSplitOptions.None)[0]; //remove not need data

                        hasMarkString = splitText.Contains("<p class=\"tit\">");

                        if (hasMarkString)
                        {
                            string[] needData = splitText.Split(new string[] { "<p class=\"tit\">" }, StringSplitOptions.None);
                            List<string> dataList = new List<string>(needData);
                            dataList.RemoveAt(0);


                            hasMarkString = splitText.Contains("img src");

                            if (hasMarkString)
                            {
                                foreach (string tempEvent in dataList)
                                {
                                    UnderShopEventInfo tempInfo = new UnderShopEventInfo();
                                    tempInfo.EventYear = DateTime.Now.Year;
                                    tempInfo.EventMonth = DateTime.Now.Month;
                                    tempInfo.EventDate = date;

                                    string[] tempS = tempEvent.Split('[');
                                    tempInfo.PERIOD = tempS[1].Split(']')[0];
                                    tempInfo.PLACE = tempS[2].Split(']')[0];
                                    tempInfo.NAME = (tempS[2].Split(']')[1]).Split(new string[] { "</span>" }, StringSplitOptions.None)[0];

                                    string result = await AddUnderShopInfo(tempInfo);
                                }
                            }
                        }
                    }
                }
                IncreaseCount();
            }
            catch
            {
                Finish();
            }
        }


        public async Task<string> AddUnderShopInfo(UnderShopEventInfo tempInfo)
        {
            list_UnderShopInfo.Add(tempInfo);
            return "finish";
        }

        private void IncreaseCount()
        {
            if (++count >= 31)
            {
                count = 0;
                ChangeActivity();
            }
        }

        private void ChangeActivity()
        {
            StartActivity(typeof(EventPageActivity));
            Finish();
        }
    }
}