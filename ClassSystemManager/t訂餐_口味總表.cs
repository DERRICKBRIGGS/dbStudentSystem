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
    
    public partial class t訂餐_口味總表
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public t訂餐_口味總表()
        {
            this.t訂餐_店家風味表 = new HashSet<t訂餐_店家風味表>();
            this.t訂餐_學員口味表 = new HashSet<t訂餐_學員口味表>();
        }
    
        public int 口味ID { get; set; }
        public string 風味名稱 { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<t訂餐_店家風味表> t訂餐_店家風味表 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<t訂餐_學員口味表> t訂餐_學員口味表 { get; set; }
    }
}
