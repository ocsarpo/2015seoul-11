using System;
using Android.App;
using Android.OS;
using Android.Content.PM;
using System.Collections.Generic;
using Android.Net;

namespace App5
{
    [Activity(Label = "언더더샵", MainLauncher = true, Theme = "@style/Theme.Splash", NoHistory = true, ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : Activity
    {
        public static readonly string Ulgiro3 = "Ulgiro3";
        public static readonly string Ulgiro4 = "Ulgiro4";
        public static readonly string UlgiEnterance = "UlgiEnterance";
        public static readonly string CityHall = "CityHall";
        public static readonly string InHyeon = "InHyeon";
        public static readonly string Sindang = "Sindang";
        public static readonly string JongKak = "JongKak";
        public static readonly string Jongro4 = "Jongro4";
        public static readonly string Majeon = "Majeon";
        public static readonly string ChongGye5 = "ChongGye5";
        public static readonly string Jongoh = "Jongoh";
        public static readonly string DongDaeMun = "DongDaeMun";
        public static readonly string ChongGye6 = "ChongGye6";
        public static readonly string MyeongDong = "MyeongDong";
        public static readonly string MyeongDongStation = "MyeongDongStation";
        public static readonly string Sogong = "Sogong";
        public static readonly string NamDaeMun = "NamDaeMun";
        public static readonly string HoeHyeon = "HoeHyeon";
        public static readonly string Gangnam = "Gangnam";
        public static readonly string Jamsil = "Jamsil";
        public static readonly string GangNamTerminalPart1 = "GangNamTerminalPart1";
        public static readonly string GangNamTerminalPart2 = "GangNamTerminalPart2";
        public static readonly string GangNamTerminalPart3 = "GangNamTerminalPart3";

        public static readonly string YeongDeungPoStation = "YeongDeungPoStation";
        public static readonly string YeongDeungPoRotary = "YeongDeungPoRotary";
        public static readonly string YeongDeungPoMarketNewtown = "YeongDeungPoMarketNewtown";
        static float dpixScale; static float dpiyScale = 0f;
        public static float DPI_XSCALE
        { get { return dpixScale; } }
        public static float DPI_YSCALE
        { get { return dpiyScale; } }
        public static Dictionary<string, Activity> Activities = new Dictionary<string, Activity>();

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            if (Activities.ContainsKey("MainActivity"))
            {
                Activities["MainActivity"].Finish();
                Activities.Remove("MainActivity");
                Activities.Add("MainActivity", this);
            }
            else
            {
                Activities.Add("MainActivity", this);
            }
            Data_Parking.DIC_PARKINGLOT_OVERALL_INFO.Clear();
            Data_Parking.DIC_PARKINGLOTSTRING.Clear();
            Data_ShopInfo.DIC_SHOPINFO_OVERALL_INFO.Clear();
            Data_ShopInfo.DIC_SHOP_XML_INFO_OVERALL_INFO.Clear();
            Data_ShopInfo.DIC_SHOPMAP.Clear();
            Data_ShopInfo.DIC_SHOP_DB_INFO_OVERALL_INFO.Clear();

            if (Data_Parking.DIC_PARKINGLOT_OVERALL_INFO.Count == 0)
            {
                Data_Parking.SaveURL(Ulgiro3, GetString(Resource.String.sogonddong), GetString(Resource.String.kawncheldong),
                    GetString(Resource.String.sanlimdong), GetString(Resource.String.inhyendong),
                    GetString(Resource.String.jukyodong), GetString(Resource.String.bangsandong));
                Data_Parking.SaveURL(Ulgiro4, GetString(Resource.String.sogonddong), GetString(Resource.String.kawncheldong),
                    GetString(Resource.String.sanlimdong), GetString(Resource.String.inhyendong),
                    GetString(Resource.String.jukyodong), GetString(Resource.String.bangsandong));
                Data_Parking.SaveURL(UlgiEnterance, GetString(Resource.String.sogonddong), GetString(Resource.String.kawncheldong),
                    GetString(Resource.String.sanlimdong), GetString(Resource.String.inhyendong),
                    GetString(Resource.String.jukyodong), GetString(Resource.String.bangsandong));
                Data_Parking.SaveURL(CityHall, GetString(Resource.String.seosomundong), GetString(Resource.String.sogonddong),
                    GetString(Resource.String.taepyeongro));
                Data_Parking.SaveURL(InHyeon, GetString(Resource.String.sanlimdong), GetString(Resource.String.inhyendong),
                    GetString(Resource.String.yeekhawndong), GetString(Resource.String.chungmuro));
                Data_Parking.SaveURL(Sindang, GetString(Resource.String.heongindong), GetString(Resource.String.sangwangsipli));
                Data_Parking.SaveURL(JongKak, GetString(Resource.String.kawncheldong), GetString(Resource.String.insadong));
                Data_Parking.SaveURL(Jongro4, GetString(Resource.String.yejidong), GetString(Resource.String.hunjeongdong), GetString(Resource.String.sanlimdong), GetString(Resource.String.jukyodong), GetString(Resource.String.inoedong));
                Data_Parking.SaveURL(Majeon, GetString(Resource.String.bangsandong), GetString(Resource.String.jukyodong), GetString(Resource.String.yejidong), GetString(Resource.String.hunjeongdong));
                Data_Parking.SaveURL(ChongGye5, GetString(Resource.String.bangsandong), GetString(Resource.String.jukyodong), GetString(Resource.String.yejidong), GetString(Resource.String.hunjeongdong));
                Data_Parking.SaveURL(Jongoh, GetString(Resource.String.yejidong), GetString(Resource.String.inoedong));
                Data_Parking.SaveURL(DongDaeMun, GetString(Resource.String.bangsandong), GetString(Resource.String.jongro5), GetString(Resource.String.jongro6), GetString(Resource.String.eulgiro6));
                Data_Parking.SaveURL(ChongGye6, GetString(Resource.String.jongro5), GetString(Resource.String.jongro6), GetString(Resource.String.bangsandong));
                Data_Parking.SaveURL(MyeongDong, GetString(Resource.String.sogonddong), GetString(Resource.String.chungmuro1));
                Data_Parking.SaveURL(MyeongDongStation, GetString(Resource.String.namdaemoonro4), GetString(Resource.String.hoehyeondong2ga), GetString(Resource.String.chungmuro1));
                Data_Parking.SaveURL(Sogong, GetString(Resource.String.sogonddong), GetString(Resource.String.chungmuro1));
                Data_Parking.SaveURL(NamDaeMun, GetString(Resource.String.namdaemoonro4), GetString(Resource.String.bongraedong1));
                Data_Parking.SaveURL(HoeHyeon, GetString(Resource.String.namdaemoonro3), GetString(Resource.String.chungmuro1));
                Data_Parking.SaveURL(Gangnam, GetString(Resource.String.yeoksamdong), GetString(Resource.String.seochodong));
                Data_Parking.SaveURL(Jamsil, GetString(Resource.String.sincheondong));
                Data_Parking.SaveURL(GangNamTerminalPart1, GetString(Resource.String.banpodong));
                Data_Parking.SaveURL(GangNamTerminalPart2, GetString(Resource.String.banpodong));
                Data_Parking.SaveURL(GangNamTerminalPart3, GetString(Resource.String.banpodong));
                Data_Parking.SaveURL(YeongDeungPoStation, GetString(Resource.String.yeongdeungpo));
                Data_Parking.SaveURL(YeongDeungPoRotary, GetString(Resource.String.yeongdeungpo));
                Data_Parking.SaveURL(YeongDeungPoMarketNewtown, GetString(Resource.String.yeongdeungpo));
            }

            //배율구하기
            {
                float defalt_dpi = 160.0f;
                float basic_dpix; float basic_dpiy;
                float dpixScaleSurplus = 0f; float dpiyScaleSurplus = 0f;
                basic_dpix = Resources.DisplayMetrics.Xdpi;
                basic_dpiy = Resources.DisplayMetrics.Ydpi;

                if (basic_dpix > 320)
                {
                    dpixScaleSurplus = basic_dpix % defalt_dpi;
                    dpiyScaleSurplus = basic_dpiy % defalt_dpi;
                }
                dpixScale = (float)Math.Round(basic_dpix / defalt_dpi + dpixScaleSurplus / defalt_dpi);
                dpiyScale = (float)Math.Round(basic_dpiy / defalt_dpi + dpiyScaleSurplus / defalt_dpi);
                //dpixScale = Resources.DisplayMetrics.ScaledDensity;
                //dpiyScale = Resources.DisplayMetrics.ScaledDensity;
            }

            if (Data_ShopInfo.DIC_SHOPINFO_OVERALL_INFO.Count == 0)
            {
                using (System.IO.StreamReader sr = new System.IO.StreamReader(Assets.Open("UnderShopInfo.xml")))
                {
                    System.Xml.XmlDocument xml = new System.Xml.XmlDocument();
                    xml.Load(sr);

                    Data_ShopInfo.AddShopXMLInfo(Ulgiro3, xml);
                    Data_ShopInfo.AddShopXMLInfo(Ulgiro4, xml);
                    Data_ShopInfo.AddShopXMLInfo(UlgiEnterance, xml);
                    Data_ShopInfo.AddShopXMLInfo(CityHall, xml);
                    Data_ShopInfo.AddShopXMLInfo(InHyeon, xml);
                    Data_ShopInfo.AddShopXMLInfo(Sindang, xml);

                    Data_ShopInfo.AddShopXMLInfo(JongKak, xml);
                    Data_ShopInfo.AddShopXMLInfo(Jongro4, xml);
                    Data_ShopInfo.AddShopXMLInfo(Majeon, xml);
                    Data_ShopInfo.AddShopXMLInfo(ChongGye5, xml);
                    Data_ShopInfo.AddShopXMLInfo(Jongoh, xml);
                    Data_ShopInfo.AddShopXMLInfo(DongDaeMun, xml);
                    Data_ShopInfo.AddShopXMLInfo(ChongGye6, xml);

                    Data_ShopInfo.AddShopXMLInfo(MyeongDong, xml);
                    Data_ShopInfo.AddShopXMLInfo(MyeongDongStation, xml);
                    Data_ShopInfo.AddShopXMLInfo(NamDaeMun, xml);
                    Data_ShopInfo.AddShopXMLInfo(Sogong, xml);
                    Data_ShopInfo.AddShopXMLInfo(HoeHyeon, xml);

                    Data_ShopInfo.AddShopXMLInfo(Gangnam, xml);
                    Data_ShopInfo.AddShopXMLInfo(Jamsil, xml);

                    Data_ShopInfo.AddShopXMLInfo(GangNamTerminalPart1, xml);
                    Data_ShopInfo.AddShopXMLInfo(GangNamTerminalPart2, xml);
                    Data_ShopInfo.AddShopXMLInfo(GangNamTerminalPart3, xml);

                    Data_ShopInfo.AddShopXMLInfo(YeongDeungPoStation, xml);
                    Data_ShopInfo.AddShopXMLInfo(YeongDeungPoRotary, xml);
                    Data_ShopInfo.AddShopXMLInfo(YeongDeungPoMarketNewtown, xml);
                }

            }
            // Data_ShopInfo.InsertShopInfoData();

            Data_ShopInfo.SHOP_DB.GetDicShopInfo();

            Data_ShopInfo.DIC_SHOPMAP.Clear();
            if (Data_ShopInfo.DIC_SHOPMAP.Count == 0)
            {
                Data_ShopInfo.DIC_SHOPMAP.Add(Ulgiro3, Resource.Drawable.UGR3_final);
                Data_ShopInfo.DIC_SHOPMAP.Add(Ulgiro4, Resource.Drawable.UGR4_final);
                Data_ShopInfo.DIC_SHOPMAP.Add(UlgiEnterance, Resource.Drawable.UlgiEntranceFin);
                Data_ShopInfo.DIC_SHOPMAP.Add(CityHall, Resource.Drawable.CityHallFin);
                Data_ShopInfo.DIC_SHOPMAP.Add(InHyeon, Resource.Drawable.InHyeonFin);
                Data_ShopInfo.DIC_SHOPMAP.Add(Sindang, Resource.Drawable.SindangFinal);

                Data_ShopInfo.DIC_SHOPMAP.Add(JongKak, Resource.Drawable.jongak_final);
                Data_ShopInfo.DIC_SHOPMAP.Add(Jongro4, Resource.Drawable.jongro4_final);
                Data_ShopInfo.DIC_SHOPMAP.Add(Majeon, Resource.Drawable.majeongyo_final);
                Data_ShopInfo.DIC_SHOPMAP.Add(ChongGye5, Resource.Drawable.chungye5_final);
                Data_ShopInfo.DIC_SHOPMAP.Add(Jongoh, Resource.Drawable.jongO_final);
                Data_ShopInfo.DIC_SHOPMAP.Add(DongDaeMun, Resource.Drawable.Dongdaemoon_final);
                Data_ShopInfo.DIC_SHOPMAP.Add(ChongGye6, Resource.Drawable.chungye6_final);

                Data_ShopInfo.DIC_SHOPMAP.Add(MyeongDong, Resource.Drawable.Myeongdong_final);
                Data_ShopInfo.DIC_SHOPMAP.Add(MyeongDongStation, Resource.Drawable.myeongdongstaition_final);
                Data_ShopInfo.DIC_SHOPMAP.Add(NamDaeMun, Resource.Drawable.namdaemoon_final);
                Data_ShopInfo.DIC_SHOPMAP.Add(Sogong, Resource.Drawable.sogong_final);
                Data_ShopInfo.DIC_SHOPMAP.Add(HoeHyeon, Resource.Drawable.huihyun_final);

                Data_ShopInfo.DIC_SHOPMAP.Add(GangNamTerminalPart1, Resource.Drawable.Express1);
                Data_ShopInfo.DIC_SHOPMAP.Add(GangNamTerminalPart2, Resource.Drawable.Express2);
                Data_ShopInfo.DIC_SHOPMAP.Add(GangNamTerminalPart3, Resource.Drawable.Express3_fin);

                Data_ShopInfo.DIC_SHOPMAP.Add(Gangnam, Resource.Drawable.GangNamFinal);
                Data_ShopInfo.DIC_SHOPMAP.Add(Jamsil, Resource.Drawable.JamsilFinal);

                Data_ShopInfo.DIC_SHOPMAP.Add(YeongDeungPoStation, Resource.Drawable.YDPstaiton_final);
                Data_ShopInfo.DIC_SHOPMAP.Add(YeongDeungPoRotary, Resource.Drawable.YDProtary_final);
                Data_ShopInfo.DIC_SHOPMAP.Add(YeongDeungPoMarketNewtown, Resource.Drawable.YDPnewtown_final);
            }
            //Finish();
            System.Threading.Thread.Sleep(2000);
            StartActivity(typeof(LocalSelectActivity));

        }
        protected override void OnDestroy()
        {
            base.OnDestroy();
            if (MainActivity.Activities.ContainsKey("MainActivity"))
            {
                MainActivity.Activities.Remove("MainActivity");
            }
        }



    }
}