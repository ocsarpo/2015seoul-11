using System.Collections.Generic;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using Android.Content.PM;
using Xamarin.Auth;
using System;

namespace App5
{
    [Activity(Label = "Activity1", Theme = "@android:style/Theme.NoTitleBar", ScreenOrientation = ScreenOrientation.Portrait)]
    public class ShopInfoActivity : Activity
    {
        int count = 0;
        float myRate = 0;
        private static Account loggedInAccount = null;
        //public static Account LoggedInAccount
        //{
        //    get { return  loggedInAccount; } 
        //}
        string local = "";
        string shopID = "";

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            if (MainActivity.Activities.ContainsKey("ShopInfoActivity"))
            {
                MainActivity.Activities["ShopInfoActivity"].Finish();
                MainActivity.Activities.Remove("ShopInfoActivity");
                MainActivity.Activities.Add("ShopInfoActivity", this);
            }
            else
            {
                MainActivity.Activities.Add("ShopInfoActivity", this);
            }
            // Create your application here
            SetContentView(Resource.Layout.ShopInfoLayout2);
            shopID = Intent.GetStringExtra("SHOP_ID_NUMBER");
            local = Intent.GetStringExtra("Local");

            ShopDBInformation DBShopContext = null;
            if (Data_ShopInfo.DIC_SHOP_DB_INFO_OVERALL_INFO.ContainsKey(shopID))
                DBShopContext = Data_ShopInfo.DIC_SHOP_DB_INFO_OVERALL_INFO[shopID];
            else
            {
                ShopDBInformation shopData = new ShopDBInformation()
                {
                    BookMark = 0,
                    Comment = "",
                    Rating = 0f,
                    IdentificationNumber = shopID
                };
                Data_ShopInfo.DIC_SHOP_DB_INFO_OVERALL_INFO.Add(shopData.IdentificationNumber, shopData);
                DBShopContext = Data_ShopInfo.DIC_SHOP_DB_INFO_OVERALL_INFO[shopID];
                Data_ShopInfo.InsertShopInfoData(shopData);
            }

            ShopInformation XMLShopContext = Data_ShopInfo.DIC_SHOP_XML_INFO_OVERALL_INFO[shopID];

            TextView tv_ShopName = FindViewById<TextView>(Resource.Id.tv_ShopName);
            tv_ShopName.Text = XMLShopContext.ShopName;
            TextView tv_ShopAddress = FindViewById<TextView>(Resource.Id.tv_ShopAddress);
            tv_ShopAddress.Text = XMLShopContext.LocalNam;
            TextView tv_SaleKind = FindViewById<TextView>(Resource.Id.tv_SaleKind);
            tv_SaleKind.Text = XMLShopContext.Category;

            RatingBar ratingbar = FindViewById<RatingBar>(Resource.Id.ratingbar);
            ratingbar.Rating = DBShopContext.Rating;

            ratingbar.RatingBarChange += (o, e) =>
            {
                Toast.MakeText(this, "New Rating: " + ratingbar.Rating.ToString(), ToastLength.Short).Show();
                DBShopContext.Rating = ratingbar.Rating;
                Data_ShopInfo.SHOP_DB.UpdateShopData("Rating", DBShopContext.Rating, DBShopContext.IdentificationNumber);
            };

            RatingBar bookmark = FindViewById<RatingBar>(Resource.Id.BookMark);
            bookmark.Rating = DBShopContext.BookMark;
            bookmark.Touch += (o, e) =>
            {
                if (e.Event.ActionMasked == MotionEventActions.Up)
                {
                    count++;
                    if (count % 2 == 0)
                    {
                        bookmark.Rating = 0;
                        DBShopContext.BookMark = 0;
                        Data_ShopInfo.SHOP_DB.UpdateShopData("BookMark", DBShopContext.BookMark, DBShopContext.IdentificationNumber);
                    }
                    else
                    {
                        bookmark.Rating = 1;
                        DBShopContext.BookMark = 1;
                        Data_ShopInfo.SHOP_DB.UpdateShopData("BookMark", DBShopContext.BookMark, DBShopContext.IdentificationNumber);
                    }
                }
            };

            EditText et = FindViewById<EditText>(Resource.Id.et_comment);

            et.Text = DBShopContext.Comment.ToString();
            et.TextChanged += (o, e) =>
            {
                DBShopContext.Comment = et.Text;
                Data_ShopInfo.SHOP_DB.UpdateShopData("Comment", DBShopContext.Comment, DBShopContext.IdentificationNumber);
            };
            int tweetcount = ("#" + XMLShopContext.ShopName + "(#" + XMLShopContext.LocalNam + GetString(Resource.String.UnderShopdddd) + "\n" +
                   GetString(Resource.String.SaleKind) + " " + XMLShopContext.Category + "\n" +
                   GetString(Resource.String.MyRating) + " " + ratingbar.Rating.ToString() + "\n" +
                   GetString(Resource.String.MyComment) + " ").Length;
            global::Android.Text.IInputFilter[] fA = new Android.Text.IInputFilter[1];
            fA[0] = new Android.Text.InputFilterLengthFilter(139 - tweetcount);
            et.SetFilters(fA);

            LinearLayout twit_sharing = FindViewById<LinearLayout>(Resource.Id.twit_sharing);
            LinearLayout twit_others_opinion = FindViewById<LinearLayout>(Resource.Id.twit_others_opinion);

            twit_sharing.Click += (o, e) =>
            {
                Android.Net.ConnectivityManager connectivityManager = (Android.Net.ConnectivityManager)GetSystemService(ConnectivityService);
                Android.Net.NetworkInfo activeConnection = connectivityManager.ActiveNetworkInfo;
                bool isOnline = (activeConnection != null) && activeConnection.IsConnected;

                if (isOnline)
                {
                    if (loggedInAccount == null)
                    {
                        Toast.MakeText(this, GetString(Resource.String.acquireLogin), ToastLength.Short).Show();
                        var auth = new OAuth1Authenticator(
            consumerKey: "qjXcnuCmZKPLU9Mupr0nkEcCv",
            consumerSecret: "q3c4nxTk1jqsLowujLxTmugsmdaLZFVq5cLI4SmWA1pRSlPcaD",
            requestTokenUrl: new Uri("https://api.twitter.com/oauth/request_token"),
            authorizeUrl: new Uri("https://api.twitter.com/oauth/authorize"),
            accessTokenUrl: new Uri("https://api.twitter.com/oauth/access_token"),
            callbackUrl: new Uri("http://www.hansei.ac.kr/"));

                        //save the account data in the authorization completed even handler
                        auth.Completed += (sender, eventArgs) =>
                        {
                            if (eventArgs.IsAuthenticated)
                            {
                                loggedInAccount = eventArgs.Account;
                                AccountStore.Create(this).Save(loggedInAccount, "UndertheShopTwitApp");

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
                                TwitterCtx.TweetAsync("#" + XMLShopContext.ShopName + "(#" + XMLShopContext.LocalNam + GetString(Resource.String.UnderShopdddd) + "\n" +
                       GetString(Resource.String.SaleKind) + " " + XMLShopContext.Category + "\n" +
                       GetString(Resource.String.MyRating) + " " + ratingbar.Rating.ToString() + "\n" +
                       GetString(Resource.String.MyComment) + " " + et.Text);

                                Toast.MakeText(this, GetString(Resource.String.TwitSharingComment), ToastLength.Short).Show();
                            }
                        };
                        auth.Error += (sender, eventArgs) =>
                        {
                            Toast.MakeText(this, GetString(Resource.String.authfail), ToastLength.Short).Show();
                            var next = new Intent(this.ApplicationContext, typeof(ShopInfoActivity));
                            next.PutExtra("SHOP_ID_NUMBER", shopID);
                            next.PutExtra("Local", local);
                            StartActivity(next);
                            Finish();
                        };

                        var ui = auth.GetUI(this);
                        StartActivityForResult(ui, -1);
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
                        TwitterCtx.TweetAsync("#" + XMLShopContext.ShopName + "(#" + XMLShopContext.LocalNam + GetString(Resource.String.UnderShopdddd) + "\n" +
               GetString(Resource.String.SaleKind) + " " + XMLShopContext.Category + "\n" +
               GetString(Resource.String.MyRating) + " " + ratingbar.ToString() + "\n" +
               GetString(Resource.String.MyComment) + " " + et.Text);

                        Toast.MakeText(this, GetString(Resource.String.TwitSharingComment), ToastLength.Short).Show();
                    }
                }
                else
                {
                    AlertDialog.Builder dlg = new AlertDialog.Builder(this);
                    dlg.SetTitle(GetString(Resource.String.InternetCheck));
                    dlg.SetMessage(GetString(Resource.String.InternetCheckMessage));
                    dlg.SetIcon(Resource.Drawable.AppIcon);
                    dlg.SetPositiveButton(GetString(Resource.String.confirm), (s, o2) =>
                    {

                    });
                    dlg.Show();
                }
            };

            twit_others_opinion.Click += (o, e) =>
            {
                Android.Net.ConnectivityManager connectivityManager = (Android.Net.ConnectivityManager)GetSystemService(ConnectivityService);
                Android.Net.NetworkInfo activeConnection = connectivityManager.ActiveNetworkInfo;
                bool isOnline = (activeConnection != null) && activeConnection.IsConnected;

                if (isOnline)
                {
                    if (loggedInAccount == null)
                    {
                        Toast.MakeText(this, GetString(Resource.String.acquireLogin), ToastLength.Short).Show();
                        var auth = new OAuth1Authenticator(
        consumerKey: "qjXcnuCmZKPLU9Mupr0nkEcCv",
        consumerSecret: "q3c4nxTk1jqsLowujLxTmugsmdaLZFVq5cLI4SmWA1pRSlPcaD",
        requestTokenUrl: new Uri("https://api.twitter.com/oauth/request_token"),
        authorizeUrl: new Uri("https://api.twitter.com/oauth/authorize"),
        accessTokenUrl: new Uri("https://api.twitter.com/oauth/access_token"),
        callbackUrl: new Uri("http://www.hansei.ac.kr/"));

                        //save the account data in the authorization completed even handler
                        auth.Completed += (sender, eventArgs) =>
                        {
                            if (eventArgs.IsAuthenticated)
                            {
                                loggedInAccount = eventArgs.Account;
                                AccountStore.Create(this).Save(loggedInAccount, "UndertheShopTwitApp");
                            }
                            if (loggedInAccount != null)
                            {
                                var ne = new Intent(this, typeof(SearchTwitter));
                                ne.PutExtra("keyword", "#" + XMLShopContext.ShopName);
                                StartActivity(ne);
                            }
                            else
                            {
                                Toast.MakeText(this, GetString(Resource.String.authfail), ToastLength.Short).Show();
                                var next = new Intent(this.ApplicationContext, typeof(ShopInfoActivity));
                                next.PutExtra("SHOP_ID_NUMBER", shopID);
                                next.PutExtra("Local", local);
                                StartActivity(next);
                                Finish();
                            }
                        };

                        var ui = auth.GetUI(this);
                        StartActivityForResult(ui, -1);
                    }
                    else
                    {
                        var ne = new Intent(this, typeof(SearchTwitter));
                        ne.PutExtra("keyword", "#" + XMLShopContext.ShopName);
                        ne.PutExtra("SHOP_ID_NUMBER", shopID);
                        ne.PutExtra("Local", local);
                        StartActivity(ne);
                    }
                }
                else
                {
                    AlertDialog.Builder dlg = new AlertDialog.Builder(this);
                    dlg.SetTitle(GetString(Resource.String.InternetCheck));
                    dlg.SetMessage(GetString(Resource.String.InternetCheckMessage));
                    dlg.SetIcon(Resource.Drawable.AppIcon);
                    dlg.SetPositiveButton(GetString(Resource.String.confirm), (s, o2) =>
                    {

                    });
                    dlg.Show();
                }
            };
            //twit_sharing.Click += (o, e) => { Tweet("#" + shopContext.ShopName + "(#"+shopContext.LocalNam+ GetString(Resource.String.UnderShopdddd) + "\n" + 
            //   GetString(Resource.String.SaleKind) +" " + shopContext.Category + "\n" +
            //   GetString(Resource.String.MyRating) + " " + myRate.ToString() + "\n" +
            //   GetString(Resource.String.MyComment) + " " + tv.Text    , "Nothing"); };

            //twit_others_opinion.Click += (o, e) => {
            //    var next = new Intent(this, typeof(OthersOpinionActivity));
            //    next.PutExtra("ShopName", shopContext.ShopName);
            //    StartActivity(next);

            //};

            LinearLayout shopLoca = FindViewById<LinearLayout>(Resource.Id.shopLoca);
            shopLoca.Click += ShopLoca_Click;
        }

        private void ShopLoca_Click(object sender, EventArgs e)
        {
            if(MainActivity.Activities.ContainsKey("UnderShopMapActivity"))
            {
                MainActivity.Activities["UnderShopMapActivity"].Finish();
                MainActivity.Activities.Remove("UnderShopMapActivity");
            }

            var next = new Intent(this.ApplicationContext, typeof(UnderShopMapActivity));
            next.PutExtra("SHOP_ID_NUMBER", shopID);
            next.PutExtra("Local", local);
            StartActivity(next);

            Finish();
        }
        protected override void OnDestroy()
        {
            base.OnDestroy();
            if (MainActivity.Activities.ContainsKey("ShopInfoActivity"))
            {
                MainActivity.Activities.Remove("ShopInfoActivity");
            }
        }
        //public bool Tweet(string text, string imagePath)
        //{
        //    Intent tweetIntent = new Intent(Intent.ActionSend);
        //    tweetIntent.PutExtra(Intent.ExtraText, text);
        //    {
        //        tweetIntent.SetType("text/plain");
        //    }

        //    PackageManager pm = this.PackageManager;
        //    IList<ResolveInfo> resolvedInfoList = pm.QueryIntentActivities(tweetIntent, PackageInfoFlags.MatchDefaultOnly);

        //    foreach (ResolveInfo resolveInfo in resolvedInfoList)
        //    {
        //        if (resolveInfo.ActivityInfo.PackageName.StartsWith("com.twitter.android"))
        //        {
        //            tweetIntent.SetClassName(resolveInfo.ActivityInfo.PackageName, resolveInfo.ActivityInfo.Name);
        //            this.StartActivity(tweetIntent);
        //            return true;
        //        }
        //    }
        //    return false;
        //}
    }
}

