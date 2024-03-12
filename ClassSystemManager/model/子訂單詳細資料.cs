using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ClassSystemManager.model
{
    public class 子訂單詳細資料
    {
        public int 子訂單ID { get; set; }
        public string 店家姓名 { get; set; }
        public string 餐點名稱 { get; set; }
        public int 數量 { get; set; }
        public decimal 小記 { get; set; }
        public List<子訂單詳細資料> gettable(int ID) 
        {
            order_meal_systemEntities db = new order_meal_systemEntities();
            var result = from a in db.t訂餐_訂單詳細資訊表
                         join b in db.t訂餐_餐點資訊表
                         on a.餐點ID equals b.餐點ID
                         join c in db.t訂餐_店家資料表 on b.店家ID equals c.店家ID
                         where a.訂單ID == ID
                         select new 子訂單詳細資料
                         {
                             子訂單ID=a.訂單詳細表ID,
                             店家姓名 = c.店家名稱,
                             餐點名稱 = b.餐點名稱,
                             數量 = (int)a.餐點數量,
                             小記 = (Decimal)(a.餐點數量 * b.餐點定價)
                         };
            return result.ToList();
        }
    }
}
