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
    
    public partial class StockTradingRecord
    {
        public int STR_ID { get; set; }
        public string STR_StockID { get; set; }
        public bool STR_BuySold { get; set; }
        public decimal STR_OrderPrice { get; set; }
        public decimal STR_Price { get; set; }
        public int STR_Volume { get; set; }
        public System.DateTime STR_OrderTime { get; set; }
        public string STR_EmpID { get; set; }
        public System.DateTime STR_STR_EnterTreasuryTime { get; set; }
        public string STR_AccountID { get; set; }
    
        public virtual StockInfo StockInfo { get; set; }
    }
}
