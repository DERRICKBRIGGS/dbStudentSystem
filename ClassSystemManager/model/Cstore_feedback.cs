using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassSystemManager.model
{
    public class Cstore_feedback
    {
        public int 訂單編號 { get; set; }
        public int 子訂單編號 { get; set; }
        public string 餐點名稱 { get; set; }
        public int 餐點數量 { get; set; }
        public string 訂單時間 { get; set; }
        public string 評價星數 { get; set; }
        public List<Cstore_feedback> getdata(int ID) 
        {
            order_meal_systemEntities db = new order_meal_systemEntities();
            var store_information = from a in db.t訂餐_店家資料表
                                    join b in db.t訂餐_訂單詳細資訊表 on a.店家ID equals b.店家ID
                                    join c in db.t訂餐_訂單資訊表 on b.訂單ID equals c.訂單ID
                                    join d in db.t訂餐_餐點資訊表 on b.餐點ID equals d.餐點ID
                                    join e in db.t訂餐_評論表 on c.訂單ID equals e.訂單ID
                                    where a.店家ID == ID
                                    orderby c.訂單時間 descending
                                    select new Cstore_feedback
                                    {
                                        訂單編號=c.訂單ID,
                                        子訂單編號 = b.訂單詳細表ID,
                                        餐點名稱=d.餐點名稱,
                                        餐點數量=(int)b.餐點數量,
                                        訂單時間=c.訂單時間,
                                        評價星數=e.滿意度_星數_
                                    };
            return store_information.ToList();
        }
             

    }
}
