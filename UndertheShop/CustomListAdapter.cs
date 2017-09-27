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

namespace App5
{
    class CustomListAdapter : BaseAdapter<ShopInformation>
    {
        private List<ShopInformation> mItems;
        private Context mContext;

        public CustomListAdapter(Context context, List<ShopInformation> items)
        {
            this.mContext = context;
            this.mItems = items;
        }

        public override ShopInformation this[int position]
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

        public List<ShopInformation> GetData()
        {
            return mItems;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View row = convertView;

            if (row == null)
            {
                row = LayoutInflater.From(mContext).Inflate(Resource.Layout.SearchResultItem, null, false);
            }

            TextView tvName = row.FindViewById<TextView>(Resource.Id.TV_RESULTNAME);
            tvName.Text = mItems[position].ShopName;
            TextView tvRegion = row.FindViewById<TextView>(Resource.Id.TV_RESULTREGION);
            tvRegion.Text = mItems[position].LocalNam;
            TextView tvCategory = row.FindViewById<TextView>(Resource.Id.TV_CATEGORY);
            tvCategory.Text = mItems[position].Category;
            return row;
        }
    }
}