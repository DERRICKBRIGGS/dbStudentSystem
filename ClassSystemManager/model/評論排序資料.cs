using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassSystemManager.model
{
    public class 評論排序資料
    {
        public int 訂單ID { get; set; }
        public int 學員ID { get; set; }
        public string 學生姓名 { get; set; }
        public string 訂單時間 { get; set; }
         public string 評價星數 { get; set; }

        public List<評論排序資料> gettable_feedback()
        {
            order_meal_systemEntities db = new order_meal_systemEntities();
            var datatable = from a in db.t訂餐_訂單資訊表
                            join b in db.t會員_學生 on a.學員ID equals b.學生ID
                            join c in db.t訂餐_評論表 on a.訂單ID equals c.訂單ID
                            select new 評論排序資料
                            {
                                訂單ID = a.訂單ID,
                                學員ID = b.學生ID,
                                學生姓名 = b.姓名,
                                訂單時間 = a.訂單時間,
                                評價星數 = c.滿意度_星數_
                            };
            return datatable.ToList();
        }
    }
}
