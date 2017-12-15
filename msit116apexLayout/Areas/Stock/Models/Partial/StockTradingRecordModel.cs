using msit116apexLayout.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace msit116apexLayout.Areas.Stock.Models.Partial
{
    [MetadataType(typeof(StockTradingRecordModel))]
    public partial class StockTradingRecordModel
    {
        [DisplayName("序號")]
        public int STR_ID { get; set; }

        [DisplayName("股票代碼")]
        public string STR_StockID { get; set; }

        [DisplayName("買/賣")]
        public bool STR_BuySold { get; set; }

        [DisplayName("訂單價")]
        public decimal STR_OrderPrice { get; set; }

        [DisplayName("成交價")]
        public decimal STR_Price { get; set; }

        [DisplayName("成交量")]
        public int STR_Volume { get; set; }

        [DisplayName("訂單時間")]
        public System.DateTime STR_OrderTime { get; set; }

        [DisplayName("員工編號")]
        public string STR_EmpID { get; set; }

        [DisplayName("入庫時間")]
        public System.DateTime STR_STR_EnterTreasuryTime { get; set; }

        [DisplayName("交易帳戶")]
        public string STR_AccountID { get; set; }

        public virtual msit116apexLayout.Areas.Stock.Models.Partial.StockInfoModel StockInfoModel { get; set; }
    }

}