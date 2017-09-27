using System.Collections.Generic;
using SQLite;

namespace App5
{
    public abstract class SQLiteDB
    {
        public SQLiteConnection SQLCon;
        public string pathToDatabase;

        public SQLiteDB()
        {
            string docsFolder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments); //db경로
            pathToDatabase = System.IO.Path.Combine(docsFolder, "UnderShopDB.db");
            SQLCon = new SQLiteConnection(pathToDatabase); //db연결

        }

        public abstract void CreateTable();
        public abstract void InsertData(object o);
        public abstract void DeleteTable();
    }

    public class ShopInformationDB : SQLiteDB
    {
        public ShopInformationDB() : base()
        {
            SQLCon.CreateTable<ShopDBInformation>();
        }

        public override void CreateTable()
        {
            SQLCon.CreateTable<ShopDBInformation>();
        }

        public override void InsertData(object o)
        {
            ShopDBInformation shopInfo = o as ShopDBInformation;
            if (shopInfo != null)
                SQLCon.Insert(shopInfo);
        }

        public override void DeleteTable()
        {
            SQLCon.DropTable<ShopDBInformation>();
        }

        public void GetDicShopInfo()
        {
            var temp = SQLCon.Query<ShopDBInformation>("select * from ShopDBInformation");
            foreach (ShopDBInformation pInfo in temp)
            {
                if (!Data_ShopInfo.DIC_SHOP_DB_INFO_OVERALL_INFO.ContainsKey(pInfo.IdentificationNumber))
                    Data_ShopInfo.DIC_SHOP_DB_INFO_OVERALL_INFO.Add(pInfo.IdentificationNumber, pInfo);
            }

        }
        public void UpdateShopData(string col, string val, string id)
        {
            var temp = SQLCon.Query<int>("update ShopDBInformation set " + col + " = '" + val + "' where IdentificationNumber = ?", id);
        }
        public void UpdateShopData(string col, float val, string id)
        {
            var temp = SQLCon.Query<int>("update ShopDBInformation set " + col + " = " + val + " where IdentificationNumber = ?", id);
        }
        public void UpdateShopData(string col, int val, string id)
        {
            var temp = SQLCon.Query<int>("update ShopDBInformation set " + col + " = " + val + " where IdentificationNumber = ?", id);
        }

    }

    public class DBSizeDB : SQLiteDB
    {
        private int dbSize;

        public DBSizeDB() : base()
        {
            SQLCon.CreateTable<DBAllSize>();
            dbSize = 0;
        }

        public override void CreateTable()
        {
            SQLCon.CreateTable<DBAllSize>();
        }

        public override void InsertData(object o)
        {
            DBAllSize dbInfo = o as DBAllSize;
            if (dbInfo != null)
                SQLCon.Insert(dbInfo);
        }

        public override void DeleteTable()
        {
            SQLCon.DropTable<DBAllSize>();
        }

        public int GetDBSize(string dbKind)
        {
            List<DBAllSize> tempSize = SQLCon.Query<DBAllSize>("select * from DBAllSize where DBName = ?", dbKind);
            try
            {
                dbSize = tempSize[0].DB_Num;
            }
            catch
            {
                return 0;
            }
            return dbSize;
        }

        public void IncreaseData(string dbKind)
        {
            var updateMarks = SQLCon.Query<int>("UPDATE DBAllSize Set DB_Num = (DB_Num+1) WHERE DBName = ?", dbKind);
        }
    }
}