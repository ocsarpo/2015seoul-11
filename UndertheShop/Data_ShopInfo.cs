using Android.Graphics;
using SQLite;
using System.Collections.Generic;

namespace App5
{
    public class Data_ShopInfo
    {
        private static Dictionary<string, int> dic_shopMap = new Dictionary<string, int>();
        public static Dictionary<string, int> DIC_SHOPMAP
        {
            get { return dic_shopMap; }
        }
        private static Dictionary<string, List<ShopInformation>> dic_shopInfo_overallInfo = new Dictionary<string, List<ShopInformation>>();
        public static Dictionary<string, List<ShopInformation>> DIC_SHOPINFO_OVERALL_INFO
        {
            get { return dic_shopInfo_overallInfo; }
        }

        private static Dictionary<string, ShopInformation> dic_shopXMLInfo_overallInfo = new Dictionary<string, ShopInformation>();
        public static Dictionary<string, ShopInformation> DIC_SHOP_XML_INFO_OVERALL_INFO
        {
            get { return dic_shopXMLInfo_overallInfo; }
        }
        private static Dictionary<string, ShopDBInformation> dic_shopDBInfo_overallInfo = new Dictionary<string, ShopDBInformation>();
        public static Dictionary<string, ShopDBInformation> DIC_SHOP_DB_INFO_OVERALL_INFO
        {
            get { return dic_shopDBInfo_overallInfo; }
        }
        public static void AddShopXMLInfo(string local, System.Xml.XmlDocument xml)
        {
            List<ShopInformation> list = new List<ShopInformation>();            
            System.Xml.XmlNodeList nodeList = xml.SelectNodes("UnderShopInfo/" + local + "/ShopInfo");
            foreach (System.Xml.XmlNode i in nodeList)
            {
                ShopInformation si = new ShopInformation(float.Parse(i["PointX"].InnerText), float.Parse(i["PointY"].InnerText))
                {
                    Local = local,
                    LocalNam = i["LocalNam"].InnerText,
                    ShopName = i["ShopName"].InnerText,
                    EngCategory = i["EngCategory"].InnerText,
                    Category = i["Category"].InnerText,
                    IdentificationNumber = i["IdentificationNumber"].InnerText,
                };
                list.Add(si);
                if (!DIC_SHOP_XML_INFO_OVERALL_INFO.ContainsKey(si.IdentificationNumber))
                    DIC_SHOP_XML_INFO_OVERALL_INFO.Add(si.IdentificationNumber, si);
            }
            if (!DIC_SHOPINFO_OVERALL_INFO.ContainsKey(local))
                DIC_SHOPINFO_OVERALL_INFO.Add(local, list);
        }
        /// <summary>
        /// AddShopXMLInfo 메서드가 선 수행 되어야만 한다.
        /// </summary>
        public static void InsertShopInfoData(ShopDBInformation shopData)
        {
            SHOP_DB.InsertData(shopData);
            SIZE_DB.IncreaseData("ShopDBInformation");
        }


        private static ShopInformationDB shop_DB = new ShopInformationDB();
        public static ShopInformationDB SHOP_DB
        {
            get { return shop_DB; }
        }

        private static DBSizeDB size_DB = new DBSizeDB();
        public static DBSizeDB SIZE_DB
        {
            get { return size_DB; }
        }

        private static int data_Num = 0;
        public static int DATA_NUM
        {
            get { return data_Num; }
        }
    }

    public class ShopInformation
    {
        //public enum ShopCategory
        //{
        //    DIGITAL_APPLIANCE, FASHION_CLOTHES, BEAUTY, CONVENIENCE, FOOD , INTERIOR, OTHER
        //}
        private PointF shopLocation;

        public string Local { get; set; }
        public string LocalNam { get; set; }
        public string ShopName { get; set; }
        public string Category { get; set; }
        public string EngCategory { get; set; }
        public PointF ShopLocation { get { return shopLocation; } }
        public string IdentificationNumber { get; set; }

        public ShopInformation(float x, float y)
        {
            shopLocation = new PointF(x, y);
        }
    }
    public class ShopDBInformation
    {
        [PrimaryKey]
        public string IdentificationNumber { get; set; }
        public int BookMark { get; set; }
        public float Rating { get; set; }
        public string Comment { get; set; }
    }

    class UnderShopLocationMapInfo
    {
        public string ShopName { get; set; }
        public string ShopAddr { get; set; }
        public string Shop_LAT { get; set; }
        public string Shop_LNG { get; set; }
    }

}