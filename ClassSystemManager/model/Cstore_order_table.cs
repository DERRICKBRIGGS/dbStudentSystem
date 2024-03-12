using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassSystemManager.model
{
    public class Cstore_order_table
    {
        //public int 訂單編號 { get; set; }
        public int 子訂單編號 { get; set; }
        public string 訂單狀態 { get; set; }
        public string 訂單時間 { get; set; }
        public string 餐點名稱 { get; set; }
        public int 餐點數量 { get; set; }
        public decimal 金額小記 { get; set; }
        public List<Cstore_order_table> gettable(int ID) 
        {
            order_meal_systemEntities db = new order_meal_systemEntities();
            var store_information = from a in db.t訂餐_店家資料表
                                    join b in db.t訂餐_訂單詳細資訊表 on a.店家ID equals b.店家ID
                                    join c in db.t訂餐_訂單資訊表 on b.訂單ID equals c.訂單ID
                                    join d in db.t訂餐_餐點資訊表 on b.餐點ID equals d.餐點ID
                                    where a.店家ID==ID
                                    orderby c.訂單時間 descending
                                    select new Cstore_order_table
                                    {
                                       // 訂單編號=c.訂單ID,
                                        子訂單編號= b.訂單詳細表ID,
                                        訂單狀態 =c.訂單狀態,
                                        訂單時間=c.訂單時間,
                                        餐點名稱=d.餐點名稱,
                                        餐點數量=(int)b.餐點數量,
                                        金額小記= (Decimal)(b.餐點數量 * d.餐點定價)
                                    };
            return store_information.ToList();
        }
        public void getupdate_type(int ID,int order_ID,string type) 
        {
            order_meal_systemEntities db = new order_meal_systemEntities();
            var store_information = (from a in db.t訂餐_店家資料表
                                    join b in db.t訂餐_訂單詳細資訊表 on a.店家ID equals b.店家ID
                                    join c in db.t訂餐_訂單資訊表 on b.訂單ID equals c.訂單ID
                                    join d in db.t訂餐_餐點資訊表 on b.餐點ID equals d.餐點ID
                                    where a.店家ID == ID && b.訂單詳細表ID== order_ID
                                    orderby c.訂單時間 descending
                                    select new Cstore_order_table
                                    {
                                        //訂單編號 = c.訂單ID,
                                        子訂單編號 = b.訂單詳細表ID,
                                        訂單狀態 = c.訂單狀態,
                                        訂單時間 = c.訂單時間,
                                        餐點名稱 = d.餐點名稱,
                                        餐點數量 = (int)b.餐點數量,
                                        金額小記 = (Decimal)(b.餐點數量 * d.餐點定價)
                                    }).Single();
            if (store_information != null) 
            {
                store_information.訂單狀態 = type;
            }
            db.SaveChanges();
        }
    }
}
