
using Android.App;
using Android.Content.PM;
using Android.Graphics;
using Android.OS;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace App5
{
    [Activity(Label = "dataPageActivity", Theme = "@android:style/Theme.NoTitleBar", ScreenOrientation = ScreenOrientation.Portrait)]
    public class EventPageActivity : Activity
    {
        private EventListAdapter cusAdapter;
        private Dictionary<int, UnderShopEventInfo> dic_eventInfo;
        List<UnderShopEventInfo> tempUEI;
        ListView lv_EventNotice;
        LinearLayout preSelectLayout = null;
        int m_month = 0;
        int m_day = 0;

    protected override async void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            RequestWindowFeature(WindowFeatures.NoTitle);
            SetContentView(Resource.Layout.EventPage);

            if (MainActivity.Activities.ContainsKey("EventPageActivity"))
            {
                MainActivity.Activities["EventPageActivity"].Finish();
                MainActivity.Activities.Remove("EventPageActivity");
                MainActivity.Activities.Add("EventPageActivity", this);
            }
            else
            {
                MainActivity.Activities.Add("EventPageActivity", this);
            }

            lv_EventNotice = FindViewById<ListView>(Resource.Id.lv_EventList);

            dic_eventInfo = new Dictionary<int, UnderShopEventInfo>();
            tempUEI = new List<UnderShopEventInfo>();

            foreach (UnderShopEventInfo usi in EventLoadingActivity.list_UnderShopInfo)
            {
                dic_eventInfo.Add(usi.EventDate, usi);
            }

            cusAdapter = new EventListAdapter(this.ApplicationContext, tempUEI);
            lv_EventNotice.Adapter = cusAdapter;

            //////////////////////////////////////////////////////

            m_month = DateTime.Now.Month;
            m_day = DateTime.Now.Day;

            int daysOfMonth = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month); //해당달의 일수 구하기
            string dayOfWeek = DateTime.Now.DayOfWeek.ToString();

            int startDay = dayOfWeek == "Sunday" ? 0 : dayOfWeek == "Monday" ? 1 : dayOfWeek == "Tuesday" ? 2 :
                dayOfWeek == "Wednesday" ? 3 : dayOfWeek == "Thursday" ? 4 : dayOfWeek == "Friday" ? 5 : 6;
            startDay = (8 + startDay - m_day % 7) % 7; //find start day num

            TextView tv_CalendarMonth = FindViewById<TextView>(Resource.Id.tv_CalendarMonth);
            tv_CalendarMonth.Text = m_month + tv_CalendarMonth.Text; //chnage Month String

            LinearLayout ll_Calendar = FindViewById<LinearLayout>(Resource.Id.ll_CalendarEle); //True Calendar
            LinearLayout ll_CalendarElement = FindViewById<LinearLayout>(Resource.Id.ll_CalendarElement);
            LinearLayout ll_RowElement = new LinearLayout(this);
            LinearLayout ll_tempCalendar = new LinearLayout(this);

            LinearLayout.LayoutParams llParam =
                new LinearLayout.LayoutParams(LinearLayout.LayoutParams.WrapContent, LinearLayout.LayoutParams.WrapContent, 1f);
            llParam.Gravity = GravityFlags.Center;

            ll_tempCalendar.LayoutParameters = llParam;
            ll_tempCalendar.Orientation = Orientation.Vertical;

            /////////////////////////////////////////////////////////////

            View tempView;
            TextView tv_CalendarDate;
            ImageView tv_CalendarDot;
            int calDate = daysOfMonth + startDay;

            for (int i = 0; i < 42; i++) // 42 is maximum Calendal Count
            {
                if (calDate <= i && i % 7 == 0)
                {
                    break;
                }

                llParam = new LinearLayout.LayoutParams(0, LinearLayout.LayoutParams.WrapContent, 1f);
                llParam.Gravity = GravityFlags.Center;

                tempView = View.Inflate(this.ApplicationContext, Resource.Layout.CalendarElement, ll_CalendarElement);
                tempView.LayoutParameters = llParam;
                tempView.Click += new EventHandler(EventTouchEvent);
                //////////////////////////////////////////////////////////////////////////////////////

                tv_CalendarDate = tempView.FindViewById<TextView>(Resource.Id.tv_DateElement);
                if (startDay <= i && i < calDate)
                {
                    tv_CalendarDate.SetTextColor(Android.Graphics.Color.ParseColor("#FF000000"));
                    tv_CalendarDate.Text = (i - startDay + 1).ToString();
                }
                //////////////////////////////////////////////////////////////////////////////////////

                tv_CalendarDot = tempView.FindViewById<ImageView>(Resource.Id.iv_CheckElement);

                if (dic_eventInfo.ContainsKey(i - startDay + 1))
                {
                    tv_CalendarDot.SetImageResource(Resource.Drawable.eventCheck3);
                }
                //////////////////////////////////////////////////////////////////////////////////////
                ll_RowElement.AddView(tempView);

                if (i % 7 == 6)
                {
                    ll_tempCalendar.AddView(ll_RowElement);
                    ll_RowElement = new LinearLayout(this);
                }
            }

            ll_Calendar.AddView(ll_tempCalendar);
        }

        private async void EventTouchEvent(object sender, EventArgs e)
        {
            try
            {
                string result = await ChangeChooseEvent(sender);
            }
            catch
            {
                Finish();
            }
        }

        private async Task<string> ChangeChooseEvent(object sender)
        {
            LinearLayout eventLayout = (LinearLayout)sender;
            List<UnderShopEventInfo> selectList;

            if (preSelectLayout != null)
                preSelectLayout.SetBackgroundResource(Resource.Drawable.calendarBackground);

            preSelectLayout = eventLayout;
            eventLayout.SetBackgroundColor(new Android.Graphics.Color(3,169,244));

            selectList = new List<UnderShopEventInfo>();
            string testString = eventLayout.FindViewById<TextView>(Resource.Id.tv_DateElement).Text.ToString();
            int testInt = 0;
            if (testString != "")
                testInt = int.Parse(testString);
            else
                return "fail";

            if (dic_eventInfo.ContainsKey(testInt))
            {
                tempUEI.Clear();
                cusAdapter.GetData().Clear();

                tempUEI.Add(dic_eventInfo[testInt]);

                lv_EventNotice.InvalidateViews();
            }
            else
            {
                tempUEI.Clear();
                lv_EventNotice.InvalidateViews();
            }

            return "finish";
        }
        protected override void OnDestroy()
        {
            base.OnDestroy();
            if (MainActivity.Activities.ContainsKey("EventPageActivity"))
            {
                MainActivity.Activities.Remove("EventPageActivity");
            }
        }
    }
}