using System;
using System.Collections.Generic;
using System.Linq;

using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using System.Threading.Tasks;
using LinqToTwitter;
using Android.Content;
using Android.Content.PM;

namespace App5
{
    [Activity(Label = "Search Twitter", Theme = "@android:style/Theme.NoTitleBar", ScreenOrientation = ScreenOrientation.Portrait)]
    public class SearchTwitter : ListActivity
    {
        private string SearchTerm;
        Xamarin.Auth.Account loggedInAccount;
        List<LinqToTwitter.Search> _searches;
        private string shopID;
        private string local;

    async protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            if (MainActivity.Activities.ContainsKey("SearchTwitter"))
            {
                MainActivity.Activities["SearchTwitter"].Finish();
                MainActivity.Activities.Remove("SearchTwitter");
                MainActivity.Activities.Add("SearchTwitter", this);
            }
            else
            {
                MainActivity.Activities.Add("SearchTwitter", this);
            }
            //has the user already authenticated from a previous session? see AccountStore.Create().Save() later
            IEnumerable<Xamarin.Auth.Account> accounts = Xamarin.Auth.AccountStore.Create(this).FindAccountsForService("UndertheShopTwitApp");

            SearchTerm = Intent.GetStringExtra("keyword");
            shopID = Intent.GetStringExtra("SHOP_ID_NUMBER");
            local = Intent.GetStringExtra("Local");

            //check the account store for a valid account marked as "Twitter" and then hold on to it for future requests
            foreach (Xamarin.Auth.Account account in accounts)
            {
                loggedInAccount = account;
                break;
            }
            if (loggedInAccount == null)
            {
                Toast.MakeText(this, GetString(Resource.String.authfail), ToastLength.Short).Show();
                var next = new Intent(this.ApplicationContext, typeof(ShopInfoActivity));
                next.PutExtra("SHOP_ID_NUMBER", shopID);
                next.PutExtra("Local", local);
                StartActivity(next);
                Finish();
            }

            else
            {
                var cred = new LinqToTwitter.InMemoryCredentialStore();
                cred.ConsumerKey = loggedInAccount.Properties["oauth_consumer_key"];
                cred.ConsumerSecret = loggedInAccount.Properties["oauth_consumer_secret"];
                cred.OAuthToken = loggedInAccount.Properties["oauth_token"];
                cred.OAuthTokenSecret = loggedInAccount.Properties["oauth_token_secret"];
                var auth0 = new LinqToTwitter.PinAuthorizer()
                {
                    CredentialStore = cred,
                };
                var TwitterCtx = new LinqToTwitter.TwitterContext(auth0);
                Console.WriteLine(TwitterCtx.User);
                _searches = await (from tweet in TwitterCtx.Search
                                   where (tweet.Type == SearchType.Search) &&
                                   (tweet.Query == SearchTerm)
                                   select tweet).ToListAsync();
                this.ListAdapter = new SearchAdapter(this, _searches[0].Statuses);
            }
        }
        protected override void OnDestroy()
        {
            base.OnDestroy();
            if (MainActivity.Activities.ContainsKey("SearchTwitter"))
            {
                MainActivity.Activities.Remove("SearchTwitter");
            }
        }
    }

    public class SearchAdapter : BaseAdapter<LinqToTwitter.Status>
    {
        List<LinqToTwitter.Status> _items;
        Activity _context;
        public SearchAdapter(Activity context,
          List<LinqToTwitter.Status> items)
          : base()
        {
            this._context = context;
            this._items = items;
        }
        public override long GetItemId(int position)
        {
            return position;
        }
        public override LinqToTwitter.Status this[int position]
        {
            get { return _items[position]; }
        }

        public override Java.Lang.Object GetItem(int position)
        {
            return _items[position].Text;
        }
        public override int Count
        {
            get { return _items.Count; }
        }
        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view = convertView; // re-use an existing view, if one is available
            if (view == null) // otherwise create a new one
                view = _context.LayoutInflater.Inflate(Android.Resource.Layout.ActivityListItem, null);
            view.FindViewById<TextView>(Android.Resource.Id.Text1).Text = _items[position].Text;

            var imageUrl = new Java.Net.URL(_items[position].User.ProfileImageUrl);
            Task.Factory.StartNew(() =>
            {
                System.IO.Stream stream = imageUrl.OpenStream();
                var bitmap = Android.Graphics.BitmapFactory.DecodeStream(stream);
                (view.FindViewById<ImageView>(Android.Resource.Id.Icon)).SetImageBitmap(bitmap);
            });
            return view;
        }
    }
}