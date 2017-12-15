using msit116apexLayout.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace msit116apexLayout.Areas.Stock.Models.Partial
{

    [MetadataType(typeof(StockInfoModel))]
    public partial class StockInfoModel
    {
        [DisplayName("股票代碼")]
        public string SI_StockID { get; set; }

        [DisplayName("股票名稱")]
        public string SI_StockName { get; set; }

        [DisplayName("股票類別")]
        public int SI_CategoryID { get; set; }
    }
}