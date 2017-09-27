using System.Collections.Generic;
using Android.Content;
using Android.Views;
using Android.Widget;

namespace App5
{
    class EventListAdapter : BaseAdapter<UnderShopEventInfo>
    {
        private List<UnderShopEventInfo> mItems;
        private Context mContext;

        public EventListAdapter(Context context, List<UnderShopEventInfo> items)
        {
            this.mContext = context;
            this.mItems = items;
        }

        public override UnderShopEventInfo this[int position]
        {
            get
            {
                return mItems[position];
            }
        }

        public override int Count
        {
            get
            {
                return mItems.Count;
            }
        }

        public override long GetItemId(int position)
        {

            return position;
        }

        public List<UnderShopEventInfo> GetData()
        {
            return mItems;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View row = convertView;

            if (row == null)
            {
                row = LayoutInflater.From(mContext).Inflate(Resource.Layout.EventElement, null, false);
            }

            TextView tvName = row.FindViewById<TextView>(Resource.Id.tv_EventRegion);
            tvName.Text = mItems[position].PLACE;
            TextView tvRegion = row.FindViewById<TextView>(Resource.Id.tv_EventName);
            tvRegion.Text = mItems[position].NAME;
            TextView tvCategory = row.FindViewById<TextView>(Resource.Id.tv_Period);
            tvCategory.Text = mItems[position].PERIOD;

            return row;
        }
    }

    public class UnderShopEventInfo
    {
        private int eventYear;
        public int EventYear
        {
            get { return eventYear; }
            set { eventYear = value; }
        }

        private int eventMonth;
        public int EventMonth
        {
            get { return eventMonth; }
            set { eventMonth = value; }
        }

        private int eventDate;
        public int EventDate
        {
            get { return eventDate; }
            set { eventDate = value; }
        }

        private string period;
        public string PERIOD
        {
            get { return period; }
            set { period = value; }
        }
        private string place;
        public string PLACE
        {
            get { return place; }
            set { place = value; }
        }
        private string name;
        public string NAME
        {
            get { return name; }
            set { name = value; }
        }
    }

}