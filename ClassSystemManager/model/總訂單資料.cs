using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassSystemManager
{
    public class 總訂單資料
    {
        public int 訂單ID { get; set; }
        public int 學員ID { get; set; }
        public string 學生姓名 { get; set; }
        public string 訂單時間 { get; set; }
        public string 支付方式 { get; set; }
        public string 訂單狀態 { get; set; }
        public decimal 總額 { get; set; }
        public List<總訂單資料> gettable()
        {
            order_meal_systemEntities db = new order_meal_systemEntities();
            var datatable = from a in db.t訂餐_訂單資訊表
                            join b in db.t會員_學生 on a.學員ID equals b.學生ID
                            join c in db.t訂餐_訂單詳細資訊表 on a.訂單ID equals c.訂單ID
                            group c by new
                            {
                                訂單ID = a.訂單ID,
                                學員ID = a.學員ID,
                                學生姓名 = b.姓名,
                                訂單時間 = a.訂單時間,
                                支付方式 = a.支付方式,
                                訂單狀態 = a.訂單狀態,
                            } into grouped
                            select new 總訂單資料
                            {
                                訂單ID = grouped.Key.訂單ID,
                                學員ID = grouped.Key.學員ID,
                                學生姓名 = grouped.Key.學生姓名,
                                訂單時間 = grouped.Key.訂單時間,
                                支付方式 = grouped.Key.支付方式,
                                訂單狀態 = grouped.Key.訂單狀態,
                                總額 = (decimal)grouped.Sum(c => c.金額小記)
                            };
            return datatable.ToList();
        }
    }
}
