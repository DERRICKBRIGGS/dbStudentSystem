//------------------------------------------------------------------------------
// <auto-generated>
//     這個程式碼是由範本產生。
//
//     對這個檔案進行手動變更可能導致您的應用程式產生未預期的行為。
//     如果重新產生程式碼，將會覆寫對這個檔案的手動變更。
// </auto-generated>
//------------------------------------------------------------------------------

namespace ClassSystemManager
{
    using System;
    using System.Collections.Generic;
    
    public partial class t訂餐_營業時間表
    {
        public int 營業時間表ID { get; set; }
        public int 店家ID { get; set; }
        public string 星期 { get; set; }
        public string 時段_早_中_晚_全_ { get; set; }
        public string 開始營業時間 { get; set; }
        public string 結束營業時間 { get; set; }
    
        public virtual t訂餐_店家資料表 t訂餐_店家資料表 { get; set; }
    }
}
