//------------------------------------------------------------------------------
// <auto-generated>
//     這個程式碼是由範本產生。
//
//     對這個檔案進行手動變更可能導致您的應用程式產生未預期的行為。
//     如果重新產生程式碼，將會覆寫對這個檔案的手動變更。
// </auto-generated>
//------------------------------------------------------------------------------

namespace msit116apexLayout.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class StockInfoHistory
    {
        public int SIH_ID { get; set; }
        public string SIH_StockID { get; set; }
        public Nullable<decimal> SIH_Price { get; set; }
        public Nullable<int> SIH_Volume { get; set; }
        public System.DateTime SIH_Time { get; set; }
    
        public virtual StockInfo StockInfo { get; set; }
    }
}
