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
    
    public partial class FundDeploy
    {
        public int FundID { get; set; }
        public decimal StockPT { get; set; }
        public decimal BondPT { get; set; }
        public decimal CashPT { get; set; }
        public decimal OtherPT { get; set; }
    
        public virtual FundMaster FundMaster { get; set; }
    }
}
