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
    
    public partial class FundOrderDetail
    {
        public string OrderDetailID { get; set; }
        public string OrderlID { get; set; }
        public string GoodID { get; set; }
        public string TradeTypeCode { get; set; }
        public Nullable<int> TradePrice { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public string ChargeDateCode { get; set; }
        public Nullable<decimal> TradePricePerTime { get; set; }
        public string BankAccountID { get; set; }
    
        public virtual FundChargeDate FundChargeDate { get; set; }
        public virtual FundOrder FundOrder { get; set; }
        public virtual FundTradeType FundTradeType { get; set; }
        public virtual FundUserBankAccount FundUserBankAccount { get; set; }
    }
}
