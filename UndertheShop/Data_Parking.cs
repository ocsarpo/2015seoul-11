using SQLite;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace App5
{
    public class Data_Parking
    {
        private static Dictionary<string, string[]> dic_parkingLotString = new Dictionary<string, string[]>();
        public static Dictionary<string, string[]> DIC_PARKINGLOTSTRING
        {
            get { return dic_parkingLotString; }
        }

        private static Dictionary<string, Dictionary<string, ParkingLotInformation>> dic_parkinglot_overallInfo = new Dictionary<string, Dictionary<string, ParkingLotInformation>>();
        public static Dictionary<string, Dictionary<string, ParkingLotInformation>> DIC_PARKINGLOT_OVERALL_INFO
        {
            get { return dic_parkinglot_overallInfo; }
        }

        //private static ParkingLotInformationDB parking_DB = new ParkingLotInformationDB();
        //public static ParkingLotInformationDB PARKING_DB
        //{
        //    get { return parking_DB; }
        //}

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

        public static void SaveURL(string key, params string[] urls)
        {
            if(!DIC_PARKINGLOTSTRING.ContainsKey(key))
            DIC_PARKINGLOTSTRING.Add(key, urls);
        }

        public static void LoadXMLData(string key, params string[] url)
        {
            Dictionary<string, ParkingLotInformation> dic_parkinglotInfo = new Dictionary<string, ParkingLotInformation>();
            for (int idx = 0; idx < url.Length; idx++)
            {
                XmlDocument xml = new XmlDocument();
                xml.Load(url[idx]);
                XmlNodeList xmList = xml.SelectNodes("SearchParkingInfo/row");

                foreach (XmlNode i in xmList)
                {
                    ParkingLotInformation pli = new ParkingLotInformation();

                    pli.PARKING_CODE = i["PARKING_CODE"].InnerText;
                    pli.PARKING_NAME = i["PARKING_NAME"].InnerText;
                    pli.ADDR = i["ADDR"].InnerText;
                    pli.TEL = i["TEL"].InnerText;
                    pli.CAPACITY = i["CAPACITY"].InnerText;
                    pli.PAY_NM = i["PAY_NM"].InnerText;
                    pli.NIGHT_FREE_OPEN_NM = i["NIGHT_FREE_OPEN_NM"].InnerText;
                    pli.WEEKDAY_BEGIN_TIME = i["WEEKDAY_BEGIN_TIME"].InnerText;
                    pli.WEEKDAY_END_TIME = i["WEEKDAY_END_TIME"].InnerText;
                    pli.WEEKEND_BEGIN_TIME = i["WEEKEND_BEGIN_TIME"].InnerText;
                    pli.WEEKEND_END_TIME = i["WEEKEND_END_TIME"].InnerText;
                    pli.HOLIDAY_BEGIN_TIME = i["HOLIDAY_BEGIN_TIME"].InnerText;
                    pli.HOLIDAY_END_TIME = i["HOLIDAY_END_TIME"].InnerText;
                    pli.SATURDAY_PAY_NM = i["SATURDAY_PAY_NM"].InnerText;
                    pli.HOLIDAY_PAY_NM = i["HOLIDAY_PAY_NM"].InnerText;
                    pli.LAT = i["LAT"].InnerText;
                    pli.LNG = i["LNG"].InnerText;

                    if ((!dic_parkinglotInfo.ContainsKey(pli.PARKING_CODE)) && pli.LAT != "" && pli.LNG != "")
                    {
                        pli.PARKING_REGION = key;
                        dic_parkinglotInfo.Add(pli.PARKING_CODE, pli);
                        //InsertParkingData(pli);

                        //SIZE_DB.IncreaseData("ParkingLotInformation");
                    }
                }
            }
            OverAllParkingLotInfoAdd(key, dic_parkinglotInfo);
        }

        public static void OverAllParkingLotInfoAdd(string key, Dictionary<string, ParkingLotInformation> dic_parkinglotInfo)
        {
            if(!DIC_PARKINGLOT_OVERALL_INFO.ContainsKey(key))
            DIC_PARKINGLOT_OVERALL_INFO.Add(key, dic_parkinglotInfo);
        }

        //public static void InsertParkingData(ParkingLotInformation parkingData)
        //{
        //    PARKING_DB.InsertData(parkingData);
        //}
    }

    public class DBAllSize
    {
        [PrimaryKey, NotNull]
        public string DBName { get; set; }

        public int DB_Num { get; set; }
    }


    public class ParkingLotInformation
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        public string ADDR { get; set; }
        public string PARKING_CODE { get; set; }
        public string PARKING_REGION { get; set; }
        public string PARKING_NAME { get; set; }
        public string TEL { get; set; }
        public string CAPACITY { get; set; }
        public string PAY_NM { get; set; }
        public string NIGHT_FREE_OPEN_NM { get; set; }
        public string WEEKDAY_BEGIN_TIME { get; set; }
        public string WEEKDAY_END_TIME { get; set; }
        public string WEEKEND_BEGIN_TIME { get; set; }
        public string WEEKEND_END_TIME { get; set; }
        public string HOLIDAY_BEGIN_TIME { get; set; }
        public string HOLIDAY_END_TIME { get; set; }
        public string SATURDAY_PAY_NM { get; set; }
        public string HOLIDAY_PAY_NM { get; set; }
        public string LAT { get; set; }
        public string LNG { get; set; }
    }

}

//  for (int idx = 0; idx<url.Length; idx++)
//            {
//                System.Net.Http.HttpClient client = new System.Net.Http.HttpClient();
//HttpResponseMessage response = await client.GetAsync(url[idx]);
//response.EnsureSuccessStatusCode();

//                string responseBody = await response.Content.ReadAsStringAsync();   // 값이 들어옴
//XDocument xDoc = XDocument.Parse(responseBody);

//var parkinglot = from ParkingLotInformation in xDoc.Descendants("row")
//                 select new ParkingLotInformation
//                 {
//                     PARKING_CODE = (string)ParkingLotInformation.Element("PARKING_CODE"),
//                     PARKING_NAME = (string)ParkingLotInformation.Element("PARKING_NAME"),
//                     ADDR = (string)ParkingLotInformation.Element("ADDR"),
//                     TEL = (string)ParkingLotInformation.Element("TEL"),
//                     CAPACITY = (string)ParkingLotInformation.Element("CAPACITY"),
//                     PAY_NM = (string)ParkingLotInformation.Element("PAY_NM"),
//                     NIGHT_FREE_OPEN_NM = (string)ParkingLotInformation.Element("NIGHT_FREE_OPEN_NM"),
//                     WEEKDAY_BEGIN_TIME = (string)ParkingLotInformation.Element("WEEKDAY_BEGIN_TIME"),
//                     WEEKDAY_END_TIME = (string)ParkingLotInformation.Element("WEEKDAY_END_TIME"),
//                     WEEKEND_BEGIN_TIME = (string)ParkingLotInformation.Element("WEEKEND_BEGIN_TIME"),
//                     WEEKEND_END_TIME = (string)ParkingLotInformation.Element("WEEKEND_END_TIME"),
//                     HOLIDAY_BEGIN_TIME = (string)ParkingLotInformation.Element("HOLIDAY_BEGIN_TIME"),
//                     HOLIDAY_END_TIME = (string)ParkingLotInformation.Element("HOLIDAY_END_TIME"),
//                     SATURDAY_PAY_NM = (string)ParkingLotInformation.Element("SATURDAY_PAY_NM"),
//                     HOLIDAY_PAY_NM = (string)ParkingLotInformation.Element("HOLIDAY_PAY_NM"),
//                     LAT = (string)ParkingLotInformation.Element("LAT"),
//                     LNG = (string)ParkingLotInformation.Element("LNG")
//                 };
//List<ParkingLotInformation> booklist = parkinglot.ToList(); // 디버깅으로 값 확인(여기서 값이 들어감)

//                foreach (var i in parkinglot)
//                {
//                    if (!dic_parkinglotInfo.ContainsKey(i.PARKING_CODE) && i.LAT != "" && i.LNG != "")
//                        dic_parkinglotInfo.Add(i.PARKING_CODE, i);
//                }
//            }