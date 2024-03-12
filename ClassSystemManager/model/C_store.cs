using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassSystemManager.model
{
    public class C_store
    {
        public int 店家ID { get; set; }
        public string 名稱 { get; set; }
        public string 地址 { get; set; }
        public string 電話 { get; set; }

        public string 餐廳照片 { get; set; }
        public string 信箱 { get; set; }
        public List<C_store> get_table() 
        {
            order_meal_systemEntities db = new order_meal_systemEntities();
            var result = from a in db.t訂餐_店家資料表
                         select new C_store
                         {
                             店家ID = a.店家ID,
                             名稱 = a.店家名稱,
                             地址 = a.地址,
                             電話 = a.電話,
                             餐廳照片 = a.餐廳照片,
                             信箱 = a.電子信箱
                         };
            return result.ToList();
        }
        public List<C_store> search(string result) 
        {
            order_meal_systemEntities db = new order_meal_systemEntities();
            var table = from a in db.t訂餐_店家資料表
                        where a.店家名稱.Contains(result) || a.電話.Contains(result) || a.地址.Contains(result)
                        select new C_store
                        {
                            店家ID = a.店家ID,
                            名稱 = a.店家名稱,
                            地址 = a.地址,
                            電話 = a.電話,
                            餐廳照片 = a.餐廳照片,
                            信箱 = a.電子信箱
                        };
            return table.ToList();
        }
        public t訂餐_店家資料表 tryswitch_DBtable(C_store store,string information,string password) 
        {
            t訂餐_店家資料表 a=new t訂餐_店家資料表();
            a.店家ID = store.店家ID;
            a.店家名稱 = store.名稱;
            a.地址 = store.地址;
            a.電子信箱 = store.信箱;
            a.電話 = store.電話;
            a.餐廳照片 = store.餐廳照片;
            a.餐廳介紹 = information;
            a.密碼 = password;
            return a;
        }
    }
}
