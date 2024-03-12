using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ClassSystemManager.model
{
    public class 子訂單商家資料
    {
        public int 店家ID { get; set; }
        public string 店家名稱 { get; set; }
        public string 店家電話 { get; set; }
        public string 店家郵件 { get; set; }
        public List<子訂單商家資料> gettable(int ID) 
        {
            order_meal_systemEntities db = new order_meal_systemEntities();
            var store_information = from a in db.t訂餐_訂單詳細資訊表
                                    join b in db.t訂餐_店家資料表 on a.店家ID equals b.店家ID
                                    where a.訂單ID == ID
                                    select new 子訂單商家資料
                                    {
                                        店家ID = b.店家ID,
                                        店家名稱 = b.店家名稱,
                                        店家電話 = b.電話,
                                        店家郵件 = b.電子信箱
                                    };
            return store_information.ToList();
        }
    }
}
