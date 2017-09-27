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
using Android.Webkit;
using Java.Net;
using Android.Content.PM;

namespace App5
{
    [Activity(Label = "App3", ScreenOrientation = ScreenOrientation.Portrait)]
    public class SubwayInfoActivity : Activity
    {
        FrameLayout mWebContainer;
        WebView mWebView;
        WebSettings webSettings;
        WebViewClient mWebClient;

    protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            if (MainActivity.Activities.ContainsKey("SubwayInfoActivity"))
            {
                MainActivity.Activities["SubwayInfoActivity"].Finish();
                MainActivity.Activities.Remove("SubwayInfoActivity");
                MainActivity.Activities.Add("SubwayInfoActivity", this);
            }
            else
            {
                MainActivity.Activities.Add("SubwayInfoActivity", this);
            }
            // Set our view from the "main" layout resource
            RequestWindowFeature(WindowFeatures.NoTitle);

            SetContentView(Resource.Layout.WebviewLayout);
            string stationName = Intent.GetStringExtra("StationName");
            string strURL = @"http://m.seoul.go.kr/traffic/SubInfoSearchList.do?subSearch=" + URLEncoder.Encode(stationName, "EUC-KR") + "&flag=4";
            mWebContainer = FindViewById<FrameLayout>(Resource.Id.frameLayout1);
            mWebView = new WebView(this);
            mWebContainer.AddView(mWebView);
            webSettings = mWebView.Settings;

            mWebClient = new WebViewClient();
            mWebView.SetWebViewClient(mWebClient);

            mWebView.LoadUrl(strURL);
        }

        protected override void OnDestroy()
        {
            if (mWebView != null)
            {
                long timeout = ViewConfiguration.ZoomControlsTimeout;

                mWebView.Destroy();
                mWebView = null;
            }
            if (MainActivity.Activities.ContainsKey("SubwayInfoActivity"))
            {
                MainActivity.Activities.Remove("SubwayInfoActivity");
            }
            base.OnDestroy();
        }
    }
}