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
    
    public partial class t訂餐_學員口味表
    {
        public int 識別碼ID { get; set; }
        public int 學員ID { get; set; }
        public int 口味ID { get; set; }
    
        public virtual t訂餐_口味總表 t訂餐_口味總表 { get; set; }
        public virtual t會員_學生 t會員_學生 { get; set; }
    }
}
