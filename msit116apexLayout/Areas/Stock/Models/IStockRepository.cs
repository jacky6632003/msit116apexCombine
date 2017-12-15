using msit116apexLayout.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace msit116apexLayout.Areas.Stock.Models
{
    interface IStockRepository
    {
        IEnumerable<MainStockbySSSModel> MainStockbySSS(string EmpID, int ListID);
        IEnumerable<MainStockbyCategoryModel> MainStockbyCategory(int CategoryID);
        IEnumerable<SelfSelectedStockListNumber_Table_loadModel> SelfSelectedStockListNumber_Table_load(string EmpID,int ListID);
        IEnumerable<StockCategories_DDL_loadModel> StockCategories_DDL_load();
        IEnumerable<StockbyCategories_Table_loadModel> StockbyCategories_Table_load(int CategoryID);
        IEnumerable<SelfSelectedStockList_Table_loadModel> SelfSelectedStockList_Table_load(string EmpID);
        IEnumerable<SelfSelectedStockList_DDL_loadModel> SelfSelectedStockList_DDL_load(string EmpID);
        void SelfSelectedStockListNumber_Table_Del(string EmpID,int ListID, int ListNumberID);
        IEnumerable<SelfSelectedStockListNumber_Table_UpdateModel> SelfSelectedStockListNumber_Table_Update(string EmpID,int ListID,int ListNumberID);
        IEnumerable<SelfSelectedStockListNumber_Table_AddModel> SelfSelectedStockListNumber_Table_Add(string EmpID, int ListID, string StockID);
        void SelfSelectedStockListNumber_Table_ChangeID(string EmpID, int ListID, int or, int nr);
        IEnumerable<SelfSelectedStockList_Table_AddModel> SelfSelectedStockList_Table_Add(string EmpID, int ListID, string ListName);
        void SelfSelectedStockList_Table_Del(string EmpID, int ListID);
        IEnumerable<SelfSelectedStockList_Table_ChangeIDModel> SelfSelectedStockList_Table_ChangeID(string EmpID, int ListID );
        IEnumerable<SelfSelectedStockList_Table_UpdateModel> SelfSelectedStockList_Table_Update(string EmpID, int ListID, string ListName);

        IEnumerable<StockbyStockIDorStockName_Table_loadModel> StockbyStockIDorStockName_Table_load(string Search);

   
    }

    public partial class MainStockbySSSModel
    {
        public int SSS_ListNumberID { get; set; }
        public string SI_StockID { get; set; }
        public string SI_StockName { get; set; }
        public Nullable<decimal> SIH_Price { get; set; }
        public Nullable<int> SIH_Volume { get; set; }
    }

    public partial class MainStockbyCategoryModel
    {
        public string SI_StockID { get; set; }
        public string SI_StockName { get; set; }
        public Nullable<decimal> SIH_Price { get; set; }
        public Nullable<int> SIH_Volume { get; set; }
    }

    public partial class SelfSelectedStockListNumber_Table_loadModel
    {        
        public int SSS_ListNumberID { get; set; }
        public string SSS_StockID { get; set; }
        public string SI_StockName { get; set; }
    }

    public partial class StockCategories_DDL_loadModel
    {
        public int SCL_CategoryID { get; set; }
        public string SCL_CategoryName { get; set; }
    }

    public partial class StockbyCategories_Table_loadModel
    {
        public string SI_StockID { get; set; }
        public string SI_StockName { get; set; }
    }

    public partial class SelfSelectedStockList_Table_loadModel
    {      
        public int SSS_ListID { get; set; }
        public string SSS_ListName { get; set; }
    }

    public partial class SelfSelectedStockList_DDL_loadModel
    {        
        public int SSS_ListID { get; set; }
        public string SSS_ListName { get; set; }        
    }
    public partial class SelfSelectedStockListNumber_Table_DelModel
    {
        //多餘
    }
    public partial class SelfSelectedStockListNumber_Table_UpdateModel
    {  
        public int SSS_ListNumberID { get; set; }
        public string SSS_StockID { get; set; }
        public string SI_StockName { get; set; }
    }
    public partial class SelfSelectedStockListNumber_Table_AddModel
    {
        public int SSS_ListNumberID { get; set; }
        public string SSS_StockID { get; set; }
        public string SI_StockName { get; set; }

    }
    public partial class SelfSelectedStockListNumber_Table_ChangeIDModel
    {
        //多餘
    }
    public partial class SelfSelectedStockList_Table_AddModel
    {
        public int SSS_ListID { get; set; }
        public string SSS_ListName { get; set; }
    }
    public partial class SelfSelectedStockList_Table_DelModel
    {
        //多餘
    }
    public partial class SelfSelectedStockList_Table_ChangeIDModel
    {
        public int SSS_ListID { get; set; }
        public string SSS_ListName { get; set; }
    }
    public partial class SelfSelectedStockList_Table_UpdateModel
    {
        public int SSS_ListID { get; set; }
        public string SSS_ListName { get; set; }
    }
    public partial class StockbyStockIDorStockName_Table_loadModel
    {
        public string SI_StockID { get; set; }
        public string SI_StockName { get; set; }
    }




}
