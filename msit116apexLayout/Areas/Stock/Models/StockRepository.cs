using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using msit116apexLayout.Models;

namespace msit116apexLayout.Areas.Stock.Models
{
    public class StockRepository : IStockRepository
    {
        MSIT116APEXEntities db = new MSIT116APEXEntities();

        public IEnumerable<MainStockbySSSModel> MainStockbySSS(string EmpID,int ListID)
        {
            IEnumerable<MainStockbySSSModel> q =
                    from sss in db.SelfSelectedStockList
                    join sif in db.StockInfoHistory
                    on sss.SSS_StockID equals sif.SIH_StockID
                    where sss.SSS_EmpID == EmpID && sss.SSS_ListID == ListID
                    orderby sss.SSS_ListNumberID
                    select new MainStockbySSSModel
                    {
                        SSS_ListNumberID =  sss.SSS_ListNumberID,
                        SI_StockID = sss.StockInfo.SI_StockID,
                        SI_StockName = sss.StockInfo.SI_StockName,
                        SIH_Price = sif.SIH_Price,
                        SIH_Volume = sif.SIH_Volume
                    };

            return q;
        }

        public IEnumerable<MainStockbyCategoryModel> MainStockbyCategory(int CategoryID)
        {
            IEnumerable<MainStockbyCategoryModel> q = from si in db.StockInfo
                    join sif in db.StockInfoHistory
                    on si.SI_StockID equals sif.SIH_StockID
                    where si.SI_CategoryID == CategoryID
                    orderby si.SI_StockID
                    select new MainStockbyCategoryModel
                    {
                        SI_StockID = si.SI_StockID,
                        SI_StockName = si.SI_StockName,
                        SIH_Price = sif.SIH_Price,
                        SIH_Volume = sif.SIH_Volume
                    };
            return q;
        }       

        public IEnumerable<SelfSelectedStockListNumber_Table_AddModel> SelfSelectedStockListNumber_Table_Add(string EmpID, int ListID, string StockID)
        {
            var finallistnumber = (from s in db.SelfSelectedStockList
                                   where s.SSS_EmpID == EmpID && s.SSS_ListID == ListID
                                   orderby s.SSS_ListNumberID descending
                                   select s.SSS_ListNumberID).First();
            var stockrepeated = (from s in db.SelfSelectedStockList
                                 where s.SSS_EmpID == EmpID && s.SSS_ListID == ListID && s.SSS_StockID == StockID
                                 select s.SSS_StockID).Count();
            var listname = (from s in db.SelfSelectedStockList
                            where s.SSS_EmpID == EmpID && s.SSS_ListID == ListID
                            select s.SSS_ListName).First();

            if (finallistnumber == 0)
            {
                var update = from s in db.SelfSelectedStockList
                             where s.SSS_EmpID == EmpID && s.SSS_ListID == ListID
                             select s;

                foreach (var sss in update)
                {
                    sss.SSS_ListNumberID = 1;
                    sss.SSS_StockID = StockID;
                }
                db.SaveChanges();
            }
            else
            {
                if (stockrepeated == 0)
                {
                    var insert = new SelfSelectedStockList();
                    insert.SSS_EmpID = EmpID;
                    insert.SSS_ListID = ListID;
                    insert.SSS_ListName = listname;
                    insert.SSS_ListNumberID = (finallistnumber + 1);
                    insert.SSS_StockID = StockID;

                    db.SelfSelectedStockList.Add(insert);
                    db.SaveChanges();
                }
            }

            IEnumerable<SelfSelectedStockListNumber_Table_AddModel> q = from sss in db.SelfSelectedStockList
                    join si in db.StockInfo
                    on sss.SSS_StockID equals si.SI_StockID
                    where sss.SSS_EmpID == EmpID && sss.SSS_ListID == ListID
                    orderby sss.SSS_ListNumberID
                    select new SelfSelectedStockListNumber_Table_AddModel
                    {
                       SSS_ListNumberID = sss.SSS_ListNumberID,
                       SSS_StockID = sss.SSS_StockID,
                       SI_StockName = si.SI_StockName
                    };
            return q;

        }
        public void SelfSelectedStockListNumber_Table_ChangeID(string EmpID, int ListID, int or, int nr)
        {
            if (or < nr)
            {
                var zerosort = (from s in db.SelfSelectedStockList
                                where s.SSS_EmpID == EmpID && s.SSS_ListID == ListID && s.SSS_ListNumberID == or
                                select s).First();
                zerosort.SSS_ListNumberID = 0;
                db.SaveChanges();


                var beforesort = from bs in db.SelfSelectedStockList
                                 where bs.SSS_EmpID == EmpID && bs.SSS_ListID == ListID && bs.SSS_ListNumberID <= nr && bs.SSS_ListNumberID > or
                                 select bs;
                foreach (var sss in beforesort)
                {
                    sss.SSS_ListNumberID = sss.SSS_ListNumberID - 1;

                }

                zerosort.SSS_ListNumberID = nr;
                db.SaveChanges();

            }
            else if (or > nr)
            {
                var zerosort = (from s in db.SelfSelectedStockList
                                where s.SSS_EmpID == EmpID && s.SSS_ListID == ListID && s.SSS_ListNumberID == or
                                select s).First();
                zerosort.SSS_ListNumberID = 0;
                db.SaveChanges();

                var beforesort = from bs in db.SelfSelectedStockList
                                 where bs.SSS_EmpID == EmpID && bs.SSS_ListID == ListID && bs.SSS_ListNumberID >= nr && bs.SSS_ListNumberID < or
                                 select bs;
                foreach (var sss in beforesort)
                {
                    sss.SSS_ListNumberID = sss.SSS_ListNumberID + 1;

                }

                zerosort.SSS_ListNumberID = nr;
                db.SaveChanges();

            }
        }

        public void SelfSelectedStockListNumber_Table_Del(string EmpID, int ListID, int ListNumberID)
        {
            var ssslncount = (from s in db.SelfSelectedStockList
                              where s.SSS_EmpID == EmpID && s.SSS_ListID == ListID
                              select s.SSS_ListNumberID).Count();


            if (ssslncount != 1)
            {
                var d = (from s in db.SelfSelectedStockList
                         where s.SSS_EmpID == EmpID && s.SSS_ListID == ListID && s.SSS_ListNumberID == ListNumberID
                         select s).First();

                db.SelfSelectedStockList.Remove(d);
            }
            else if (ssslncount == 1)
            {
                var d = (from s in db.SelfSelectedStockList
                       where s.SSS_EmpID == EmpID && s.SSS_ListID == ListID && s.SSS_ListNumberID == ListNumberID
                       select s).First();
                d.SSS_ListNumberID = 0;
                d.SSS_StockID = null;
            }
            
            db.SaveChanges();          
           
        }

        public IEnumerable<SelfSelectedStockListNumber_Table_loadModel> SelfSelectedStockListNumber_Table_load(string EmpID, int ListID)
        {
            IEnumerable<SelfSelectedStockListNumber_Table_loadModel> q = from sss in db.SelfSelectedStockList
                    join si in db.StockInfo
                    on sss.SSS_StockID equals si.SI_StockID
                    where sss.SSS_EmpID == EmpID && sss.SSS_ListID == ListID
                    orderby sss.SSS_ListNumberID
                    select new SelfSelectedStockListNumber_Table_loadModel
                    {
                        SSS_ListNumberID = sss.SSS_ListNumberID,
                        SSS_StockID = sss.SSS_StockID,
                        SI_StockName = si.SI_StockName
                    };
            return q;
        }

        public IEnumerable<SelfSelectedStockListNumber_Table_UpdateModel> SelfSelectedStockListNumber_Table_Update(string EmpID, int ListID, int ListNumberID)
        {
            var afterdellistcount = (from s in db.SelfSelectedStockList
                                     where s.SSS_EmpID == EmpID && s.SSS_ListID == ListID && s.SSS_ListNumberID >= 1
                                     select s).Count();

            var newfinallistnumber = (from s in db.SelfSelectedStockList
                                      where s.SSS_EmpID == EmpID && s.SSS_ListID == ListID && s.SSS_ListNumberID >= 0
                                      orderby s.SSS_ListNumberID descending
                                      select s.SSS_ListNumberID).First();
            if (newfinallistnumber != afterdellistcount)
            {
                var update = from s in db.SelfSelectedStockList
                             where s.SSS_EmpID == EmpID && s.SSS_ListID == ListID && s.SSS_ListNumberID > ListNumberID
                             select s;

                foreach (var sss in update)
                {
                    sss.SSS_ListNumberID = sss.SSS_ListNumberID - 1;
                }
                db.SaveChanges();
            }


            IEnumerable<SelfSelectedStockListNumber_Table_UpdateModel> q = from sss in db.SelfSelectedStockList
                    join si in db.StockInfo
                    on sss.SSS_StockID equals si.SI_StockID
                    where sss.SSS_EmpID == EmpID && sss.SSS_ListID == ListID
                    orderby sss.SSS_ListNumberID
                    select new SelfSelectedStockListNumber_Table_UpdateModel
                    {
                        SSS_ListNumberID = sss.SSS_ListNumberID,
                        SSS_StockID = sss.SSS_StockID,
                        SI_StockName = si.SI_StockName
                    };
            return q;
        }

        public IEnumerable<SelfSelectedStockList_DDL_loadModel> SelfSelectedStockList_DDL_load(string EmpID)
        {
            IEnumerable<SelfSelectedStockList_DDL_loadModel> q = (from sss in db.SelfSelectedStockList
                    where sss.SSS_EmpID == EmpID
                    orderby sss.SSS_ListID
                    select new SelfSelectedStockList_DDL_loadModel
                    {
                        SSS_ListID = sss.SSS_ListID,
                        SSS_ListName = sss.SSS_ListName
                    }).Distinct();
            return q;
        }

        public IEnumerable<SelfSelectedStockList_Table_AddModel> SelfSelectedStockList_Table_Add(string EmpID, int ListID, string ListName)
        {
            var insert = new SelfSelectedStockList();
            insert.SSS_EmpID = EmpID;
            insert.SSS_ListID = ListID;
            insert.SSS_ListName = ListName;
            insert.SSS_ListNumberID = 0;

            db.SelfSelectedStockList.Add(insert);
            db.SaveChanges();

            IEnumerable<SelfSelectedStockList_Table_AddModel> q = (from s in db.SelfSelectedStockList
                    where s.SSS_EmpID == EmpID
                    orderby s.SSS_ListID
                    select new SelfSelectedStockList_Table_AddModel
                    {
                        SSS_ListID = s.SSS_ListID,
                        SSS_ListName = s.SSS_ListName
                    }).Distinct();
            return q;
        }

        public IEnumerable<SelfSelectedStockList_Table_ChangeIDModel> SelfSelectedStockList_Table_ChangeID(string EmpID, int ListID)
        {
            var afterdellistcount = (from s in db.SelfSelectedStockList
                                     where s.SSS_EmpID == EmpID && s.SSS_ListNumberID <= 1 
                                     select s.SSS_ListID).Distinct().Count();

            if (afterdellistcount >= 1)
            {

                var newfinallist = (from s in db.SelfSelectedStockList
                                    where s.SSS_EmpID == EmpID
                                    orderby s.SSS_ListID descending
                                    select s.SSS_ListID).First();

                if (newfinallist != afterdellistcount)
                {
                    var update = from s in db.SelfSelectedStockList
                                 where s.SSS_EmpID == EmpID && s.SSS_ListID > ListID
                                 select s;

                    foreach (var sss in update)
                    {
                        sss.SSS_ListID = sss.SSS_ListID - 1;
                    }
                    db.SaveChanges();
                }
            }
            IEnumerable<SelfSelectedStockList_Table_ChangeIDModel> q = (from sss in db.SelfSelectedStockList
                    where sss.SSS_EmpID == EmpID
                    orderby sss.SSS_ListID
                    select new SelfSelectedStockList_Table_ChangeIDModel
                    {
                        SSS_ListID = sss.SSS_ListID,
                        SSS_ListName = sss.SSS_ListName
                    }).Distinct();
            return q;
        }

        public void SelfSelectedStockList_Table_Del(string EmpID, int ListID)
        {
            var d = from s in db.SelfSelectedStockList
                    where s.SSS_EmpID == EmpID && s.SSS_ListID == ListID
                    select s;

            db.SelfSelectedStockList.RemoveRange(d);
            db.SaveChanges();
        }

        public IEnumerable<SelfSelectedStockList_Table_loadModel> SelfSelectedStockList_Table_load(string EmpID)
        {
            IEnumerable<SelfSelectedStockList_Table_loadModel> q = (from sss in db.SelfSelectedStockList
                    where sss.SSS_EmpID == EmpID
                    orderby sss.SSS_ListID
                    select new SelfSelectedStockList_Table_loadModel
                    {
                       SSS_ListID = sss.SSS_ListID,
                       SSS_ListName =  sss.SSS_ListName
                    }).Distinct();
            return q;
        }

        public IEnumerable<SelfSelectedStockList_Table_UpdateModel> SelfSelectedStockList_Table_Update(string EmpID, int ListID, string ListName)
        {
            var update = from s in db.SelfSelectedStockList
                         where s.SSS_EmpID == EmpID && s.SSS_ListID == ListID
                         select s;

            foreach (var sss in update)
            {
                sss.SSS_ListName = ListName;
            }
            db.SaveChanges();

            IEnumerable<SelfSelectedStockList_Table_UpdateModel> q = (from sss in db.SelfSelectedStockList
                    where sss.SSS_EmpID == EmpID
                    orderby sss.SSS_ListID
                    select new SelfSelectedStockList_Table_UpdateModel
                    {
                        SSS_ListID = sss.SSS_ListID,
                        SSS_ListName = sss.SSS_ListName
                    }).Distinct();
            return q;
        }

        public IEnumerable<StockbyCategories_Table_loadModel> StockbyCategories_Table_load(int CategoryID)
        {
            IEnumerable<StockbyCategories_Table_loadModel> q = from c in db.StockInfo
                    where c.SI_CategoryID == CategoryID
                    select new StockbyCategories_Table_loadModel
                    {
                        SI_StockID = c.SI_StockID,
                        SI_StockName = c.SI_StockName
                    };
            return q;
        }

        public IEnumerable<StockbyStockIDorStockName_Table_loadModel> StockbyStockIDorStockName_Table_load(string Search)
        {
            Search = Search.Trim();
            IEnumerable<StockbyStockIDorStockName_Table_loadModel> q = from c in db.StockInfo
                    where c.SI_StockID.Contains(Search) || c.SI_StockName.Contains(Search)
                    select new StockbyStockIDorStockName_Table_loadModel
                    {
                        SI_StockID = c.SI_StockID,
                        SI_StockName = c.SI_StockName
                    };
            return q;
        }

        public IEnumerable<StockCategories_DDL_loadModel> StockCategories_DDL_load()
        {
            IEnumerable<StockCategories_DDL_loadModel> q = from c in db.StockCategoriesList
                    select new StockCategories_DDL_loadModel
                    {
                        SCL_CategoryID = c.SCL_CategoryID,
                        SCL_CategoryName = c.SCL_CategoryName
                    };
            return q;
        }
    }
}