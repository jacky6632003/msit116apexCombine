using Microsoft.AspNet.Identity;
using msit116apexLayout.Areas.Stock.Models;
using msit116apexLayout.Areas.Stock.Models.Partial;
using msit116apexLayout.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace msit116apexLayout.Areas.Stock.Controllers
{ 
    [Authorize]
    public class StockController : Controller
    {
        public static string EmpID
        {
            get; set;
        }
        MSIT116APEXEntities db = new MSIT116APEXEntities();
        StockRepository dbr = new StockRepository();

        // GET: Stock/Stock
        public ActionResult Index()
        {
            EmpID = User.Identity.GetUserId();
            return View();
        }

        public ActionResult MainStockbySSS(int ListID = 0)
        {
            //var q = from sss in db.SelfSelectedStockList
            //        join sif in db.StockInfoHistory
            //        on sss.SSS_StockID equals sif.SIH_StockID
            //        where sss.SSS_EmpID == 1 && sss.SSS_ListID == ListID
            //        orderby sss.SSS_ListNumberID
            //        select new
            //        {
            //            sss.SSS_ListNumberID,
            //            sss.StockInfo.SI_StockID,
            //            sss.StockInfo.SI_StockName,
            //            sif.SIH_Price,
            //            sif.SIH_Volume
            //        };
            //return Json(q.ToList(), JsonRequestBehavior.AllowGet);
            return Json(dbr.MainStockbySSS(EmpID, ListID).ToList(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult MainStockbyCategory(int CategoryID = 0)
        {
            //var q = from si in db.StockInfo
            //        join sif in db.StockInfoHistory
            //        on si.SI_StockID equals sif.SIH_StockID
            //        where si.SI_CategoryID == CategoryID
            //        orderby si.SI_StockID
            //        select new
            //        {
            //            si.SI_StockID,
            //            si.SI_StockName,
            //            sif.SIH_Price,
            //            sif.SIH_Volume
            //        };
            //return Json(q.ToList(), JsonRequestBehavior.AllowGet);
            return Json(dbr.MainStockbyCategory(CategoryID).ToList(), JsonRequestBehavior.AllowGet);
        }



        public ActionResult SelfSelectedStockListNumber_Table_load(int ListID = 0)
        {
            //var q = from sss in db.SelfSelectedStockList
            //        join si in db.StockInfo
            //        on sss.SSS_StockID equals si.SI_StockID
            //        where sss.SSS_EmpID == EmpID && sss.SSS_ListID == ListID
            //        orderby sss.SSS_ListNumberID
            //        select new
            //        {
            //            sss.SSS_ListNumberID,
            //            sss.SSS_StockID,
            //            si.SI_StockName
            //        };
            //return Json(q.ToList(), JsonRequestBehavior.AllowGet);
            return Json(dbr.SelfSelectedStockListNumber_Table_load(EmpID, ListID).ToList(), JsonRequestBehavior.AllowGet);
        }


        public ActionResult StockCategories_DDL_load()
        {
            //var q = from c in db.StockCategoriesList
            //        select new
            //        {
            //            c.SCL_CategoryID,
            //            c.SCL_CategoryName
            //        };
            //return Json(q.ToList(), JsonRequestBehavior.AllowGet);
            return Json(dbr.StockCategories_DDL_load().ToList(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult StockbyCategories_Table_load(int CategoryID = 1)
        {
            //var q = from c in db.StockInfo
            //        where c.SI_CategoryID == CategoryID
            //        select new
            //        {
            //            c.SI_StockID,
            //            c.SI_StockName
            //        };
            //return Json(q.ToList(), JsonRequestBehavior.AllowGet);
            return Json(dbr.StockbyCategories_Table_load(CategoryID).ToList(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult SelfSelectedStockList_Table_load()
        {
            //var q = from sss in db.SelfSelectedStockList
            //        where sss.SSS_EmpID == EmpID
            //        orderby sss.SSS_ListID
            //        select new
            //        {
            //            sss.SSS_ListID,
            //            sss.SSS_ListName
            //        };
            //return Json(q.Distinct().ToList(), JsonRequestBehavior.AllowGet);
            return Json(dbr.SelfSelectedStockList_Table_load(EmpID).ToList(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult SelfSelectedStockList_DDL_load()
        {
            //var q = from sss in db.SelfSelectedStockList
            //        where sss.SSS_EmpID == EmpID
            //        orderby sss.SSS_ListID
            //        select new
            //        {
            //            sss.SSS_ListID,
            //            sss.SSS_ListName
            //        };
            //return Json(q.Distinct().ToList(), JsonRequestBehavior.AllowGet);
            return Json(dbr.SelfSelectedStockList_DDL_load(EmpID).Distinct().ToList(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult SelfSelectedStockListNumber_Table_Del(int ListID = 0, int ListNumberID = 0)
        {
            //var d = (from s in db.SelfSelectedStockList
            //        where s.SSS_EmpID == EmpID && s.SSS_ListID == ListID && s.SSS_ListNumberID == ListNumberID
            //        select s).First();

            //db.SelfSelectedStockList.Remove(d);
            //db.SaveChanges();
            dbr.SelfSelectedStockListNumber_Table_Del(EmpID, ListID, ListNumberID);
            return RedirectToAction("SelfSelectedStockListNumber_Table_Update", new { EmpID = EmpID, ListID = ListID, ListNumberID = ListNumberID });
        }

        public ActionResult SelfSelectedStockListNumber_Table_Update(string EmpID = "" ,int ListID = 0, int ListNumberID = 0)
        {
            //var afterdellistcount = (from s in db.SelfSelectedStockList
            //                         where s.SSS_EmpID == EmpID && s.SSS_ListID == ListID
            //                         select s).Count();
            //var newfinallistnumber = (from s in db.SelfSelectedStockList
            //                          where s.SSS_EmpID == EmpID && s.SSS_ListID == ListID
            //                          orderby s.SSS_ListNumberID descending
            //                          select s.SSS_ListNumberID).First();
            //if (newfinallistnumber != afterdellistcount)
            //{
            //    var update = from s in db.SelfSelectedStockList
            //                 where s.SSS_EmpID == EmpID && s.SSS_ListID == ListID && s.SSS_ListNumberID > ListNumberID
            //                 select s;

            //    foreach (var sss in update)
            //    {
            //        sss.SSS_ListNumberID = sss.SSS_ListNumberID - 1;
            //    }
            //    db.SaveChanges();
            //}  

            //var q = from sss in db.SelfSelectedStockList
            //        join si in db.StockInfo
            //        on sss.SSS_StockID equals si.SI_StockID
            //        where sss.SSS_EmpID == EmpID && sss.SSS_ListID == ListID
            //        orderby sss.SSS_ListNumberID
            //        select new
            //        {
            //            sss.SSS_ListNumberID,
            //            sss.SSS_StockID,
            //            si.SI_StockName
            //        };

            //return Json(q.Distinct().ToList(), JsonRequestBehavior.AllowGet);
            return Json(dbr.SelfSelectedStockListNumber_Table_Update(EmpID, ListID, ListNumberID).Distinct().ToList(), JsonRequestBehavior.AllowGet);

        }

        public ActionResult SelfSelectedStockListNumber_Table_Add(int ListID = 0, string StockID = "0")
        {
            //var finallistnumber = (from s in db.SelfSelectedStockList
            //                       where s.SSS_EmpID == EmpID && s.SSS_ListID == ListID
            //                       orderby s.SSS_ListNumberID descending
            //                       select s.SSS_ListNumberID).First();
            //var stockrepeated = (from s in db.SelfSelectedStockList
            //                     where s.SSS_EmpID == EmpID && s.SSS_ListID == ListID && s.SSS_StockID == StockID
            //                     select s.SSS_StockID).Count();
            //var listname = (from s in db.SelfSelectedStockList
            //               where s.SSS_EmpID == EmpID && s.SSS_ListID == ListID
            //               select s.SSS_ListName).First();

            //if (finallistnumber == 0)
            //{
            //    var update = from s in db.SelfSelectedStockList
            //                 where s.SSS_EmpID == EmpID && s.SSS_ListID == ListID 
            //                 select s;

            //    foreach (var sss in update)
            //    {
            //        sss.SSS_ListNumberID = 1;
            //        sss.SSS_StockID = StockID;
            //    }
            //    db.SaveChanges();
            //}
            //else
            //{
            //    if (stockrepeated == 0)
            //    {
            //        var insert = new SelfSelectedStockList();
            //        insert.SSS_EmpID = EmpID;
            //        insert.SSS_ListID = ListID;
            //        insert.SSS_ListName = listname;
            //        insert.SSS_ListNumberID = (finallistnumber + 1);
            //        insert.SSS_StockID = StockID;

            //        db.SelfSelectedStockList.Add(insert);
            //        db.SaveChanges();
            //    }
            //}  

            //var q = from sss in db.SelfSelectedStockList
            //        join si in db.StockInfo
            //        on sss.SSS_StockID equals si.SI_StockID
            //        where sss.SSS_EmpID == EmpID && sss.SSS_ListID == ListID
            //        orderby sss.SSS_ListNumberID
            //        select new
            //        {
            //            sss.SSS_ListNumberID,
            //            sss.SSS_StockID,
            //            si.SI_StockName
            //        };

            //return Json(q.Distinct().ToList(), JsonRequestBehavior.AllowGet);
            var EmpID = User.Identity.GetUserId();
            return Json(dbr.SelfSelectedStockListNumber_Table_Add(EmpID, ListID, StockID).Distinct().ToList(), JsonRequestBehavior.AllowGet);

        }

       


        public ActionResult SelfSelectedStockListNumber_Table_ChangeID(int ListID = 0,int or = 0,int nr = 0)
        {
            //if (or < nr)
            //{
            //    var beforesort = from bs in db.SelfSelectedStockList
            //                     where bs.SSS_EmpID == EmpID && bs.SSS_ListID == ListID && bs.SSS_ListNumberID <= nr
            //                     select bs;
            //    foreach (var sss in beforesort)
            //    {
            //        sss.SSS_ListNumberID = sss.SSS_ListNumberID - 1;

            //    }
            //    db.SaveChanges();
            //    var nor = or - 1;
            //    var aftersort = from afs in db.SelfSelectedStockList
            //                     where afs.SSS_EmpID == EmpID && afs.SSS_ListID == ListID && afs.SSS_ListNumberID == nor
            //                     select afs;
            //    foreach (var sss in aftersort)
            //    {
            //        sss.SSS_ListNumberID = nr;
            //    }
            //    db.SaveChanges();
            //}
            //else if (or > nr)
            //{
            //    var beforesort = from bs in db.SelfSelectedStockList
            //                     where bs.SSS_EmpID == EmpID && bs.SSS_ListID == ListID && bs.SSS_ListNumberID >= nr
            //                     select bs;
            //    foreach (var sss in beforesort)
            //    {
            //        sss.SSS_ListNumberID = sss.SSS_ListNumberID + 1;

            //    }
            //    db.SaveChanges();
            //    var nor = or + 1;
            //    var aftersort = from afs in db.SelfSelectedStockList
            //                    where afs.SSS_EmpID == EmpID && afs.SSS_ListID == ListID && afs.SSS_ListNumberID == nor
            //                    select afs;
            //    foreach (var sss in aftersort)
            //    {
            //        sss.SSS_ListNumberID = nr;
            //    }
            //    db.SaveChanges();
            //}

            dbr.SelfSelectedStockListNumber_Table_ChangeID(EmpID, ListID, or, nr);
            return RedirectToAction("SelfSelectedStockListNumber_Table_load", new { EmpID = EmpID , ListID = ListID});

        }

        public ActionResult SelfSelectedStockList_Table_Add(int ListID = 0,string ListName = "")
        {
            //var insert = new SelfSelectedStockList();
            //insert.SSS_EmpID = EmpID;
            //insert.SSS_ListID = ListID;
            //insert.SSS_ListName = ListName;
            //insert.SSS_ListNumberID = 0;

            //db.SelfSelectedStockList.Add(insert);
            //db.SaveChanges();

            //var q = from s in db.SelfSelectedStockList
            //        where s.SSS_EmpID == EmpID
            //        orderby s.SSS_ListID
            //        select new
            //        {
            //            s.SSS_ListID,
            //            s.SSS_ListName
            //        };

            //return Json(q.Distinct().ToList(), JsonRequestBehavior.AllowGet);
            return Json(dbr.SelfSelectedStockList_Table_Add(EmpID ,ListID, ListName).ToList(), JsonRequestBehavior.AllowGet);
        }        
        
        public ActionResult SelfSelectedStockList_Table_Del(int ListID = 0)
        {
            //var d = from s in db.SelfSelectedStockList
            //         where s.SSS_EmpID == EmpID && s.SSS_ListID == ListID 
            //         select s;

            //db.SelfSelectedStockList.RemoveRange(d);
            //db.SaveChanges();
            dbr.SelfSelectedStockList_Table_Del(EmpID, ListID);
            return RedirectToAction("SelfSelectedStockList_Table_ChangeID", new { EmpID = EmpID, ListID = ListID });
        }

        public ActionResult SelfSelectedStockList_Table_ChangeID(string EmpID ="",int ListID = 0)
        {
            //var afterdellistcount = (from s in db.SelfSelectedStockList
            //                         where s.SSS_EmpID == EmpID && s.SSS_ListNumberID == 1 || s.SSS_ListNumberID == 0
            //                         select s.SSS_ListID).Distinct().Count();

            //var newfinallist = (from s in db.SelfSelectedStockList
            //                    where s.SSS_EmpID == EmpID
            //                    orderby s.SSS_ListID descending
            //                    select s.SSS_ListID).First();

            //if (newfinallist != afterdellistcount)
            //{
            //    var update = from s in db.SelfSelectedStockList
            //                 where s.SSS_EmpID == EmpID && s.SSS_ListID > ListID
            //                 select s;

            //    foreach (var sss in update)
            //    {
            //        sss.SSS_ListID = sss.SSS_ListID - 1;
            //    }
            //    db.SaveChanges();
            //}

            //var q = from sss in db.SelfSelectedStockList
            //        where sss.SSS_EmpID == EmpID
            //        orderby sss.SSS_ListID
            //        select new
            //        {
            //            sss.SSS_ListID,
            //            sss.SSS_ListName
            //        };

            //return Json(q.Distinct().ToList(), JsonRequestBehavior.AllowGet);
            return Json(dbr.SelfSelectedStockList_Table_ChangeID(EmpID, ListID).ToList(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult SelfSelectedStockList_Table_Update(int ListID = 0,string ListName ="")
        {
            //var update = from s in db.SelfSelectedStockList
            //             where s.SSS_EmpID == EmpID && s.SSS_ListID == ListID
            //             select s;

            //foreach (var sss in update)
            //{
            //    sss.SSS_ListName = ListName;
            //}
            //db.SaveChanges();

            //var q = from sss in db.SelfSelectedStockList
            //        where sss.SSS_EmpID == EmpID
            //        orderby sss.SSS_ListID
            //        select new
            //        {
            //            sss.SSS_ListID,
            //            sss.SSS_ListName
            //        };

            //return Json(q.Distinct().ToList(), JsonRequestBehavior.AllowGet);
            return Json(dbr.SelfSelectedStockList_Table_Update(EmpID,ListID , ListName).ToList(), JsonRequestBehavior.AllowGet);

        }

        public ActionResult StockbyStockIDorStockName_Table_load(string Search)
        {
            //Regex NumberPattern = new Regex("[0-9]");


            //Search = Search.Trim();            
            //var q = from c in db.StockInfo
            //        where c.SI_StockID.Contains(Search) || c.SI_StockName.Contains(Search)
            //        select new
            //        {
            //            c.SI_StockID,
            //            c.SI_StockName
            //        };
            //return Json(q.Distinct().ToList(), JsonRequestBehavior.AllowGet);

            return Json(dbr.StockbyStockIDorStockName_Table_load(Search).Distinct().ToList(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult MainTreasuryStock_Table_load(string AC)
        {
            var q = from s in db.StockTreasury
                    join sih in db.StockInfoHistory
                    on s.ST_StockID equals sih.SIH_StockID
                    where s.ST_EmpID == EmpID && s.ST_Volume >0
                    orderby s.ST_StockID,s.ST_EnterTreasuryTime
                    select new
                    {
                        ST_StockID = s.ST_StockID,
                        SI_StockName = s.StockInfo.SI_StockName,
                        ST_Price = s.ST_Price,
                        ST_Volume = s.ST_Volume,
                        ST_EnterTreasuryTime = s.ST_EnterTreasuryTime,
                        SIH_Price = sih.SIH_Price                        
                    };

            return Json(q.ToList(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult PendingOrderHistory_Table_load(string AC, int Check)
        {
            var q = from p in db.PendingOrderHistory
                    where p.POH_EmpID == EmpID && p.POH_AccountID == AC && p.POH_CheckCancel == Check
                    select new
                    {
                        POH_ID = p.POH_ID,
                        POH_StockID = p.POH_StockID,
                        SI_StockName = p.StockInfo.SI_StockName,
                        POH_BuySold = (p.POH_BuySold == true ? "買" : p.POH_BuySold == false ? "賣" : "Wrong"),
                        POH_Price = p.POH_Price,
                        POH_Volume = p.POH_Volume,
                        POH_OrderTime = p.POH_OrderTime,
                        POH_AccountID = p.POH_AccountID
                    };

            return Json(q.ToList(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult PendingOrderHistory_Table_Cancel(string AC, int Check,int OID)
        {

            var update = from up in db.PendingOrderHistory
                         where up.POH_ID == OID
                         select up;

            foreach (var p in update)
            {
                p.POH_CheckCancel = 1;
            }
            db.SaveChanges();
            return RedirectToAction("PendingOrderHistory_Table_load", new { EmpID = EmpID, AC = AC, Check = Check });
        }        

        public ActionResult PendingOrderHistory_Table_Update(string AC, int Check, int OID,string BS)
        {

            var update = (from up in db.PendingOrderHistory
                         where up.POH_ID == OID
                         select up).First();

            update.POH_CheckCancel = 2;    
            db.SaveChanges();

            bool? bs=null;
            if (BS == "買")
            { bs = true; }
            if (BS == "賣")
            { bs = false; }

            return RedirectToAction("StockTradingRecordStockTreasury_Add", new { EmpID = EmpID, AC = AC, Check = Check ,OID = OID,bs=bs });
        }

        public ActionResult StockTradingRecordStockTreasury_Add(string AC, int Check, int OID,bool? bs)
        {

            var update = (from up in db.PendingOrderHistory
                          where up.POH_ID == OID
                          select new
                          {
                              up.POH_ID,
                              up.POH_StockID,
                              up.POH_BuySold,
                              up.POH_Price,
                              up.POH_Volume,
                              up.POH_OrderTime,
                              up.POH_EmpID,
                              up.POH_AccountID   
                          }).First();

            var str = new StockTradingRecord();
            
            str.STR_StockID = update.POH_StockID;
            str.STR_BuySold = update.POH_BuySold;
            str.STR_OrderPrice = update.POH_Price;
            str.STR_Price = update.POH_Price - 1;//暫時寫死
            str.STR_Volume = update.POH_Volume;
            str.STR_OrderTime = update.POH_OrderTime;
            str.STR_STR_EnterTreasuryTime = DateTime.Now;
            str.STR_EmpID = update.POH_EmpID;
            str.STR_AccountID = update.POH_AccountID;

            
            if (bs == true)
            {   
                var st = new StockTreasury();
                st.ST_StockID = update.POH_StockID;
                st.ST_Price = update.POH_Price - 1;
                st.ST_Volume = update.POH_Volume;
                st.ST_EmpID = EmpID;
                st.ST_EnterTreasuryTime = DateTime.Now;
                st.ST_AccountID = update.POH_AccountID;
                db.StockTreasury.Add(st);
            }
            if (bs == false)
            {
                var st = from s in db.StockTreasury
                         where s.ST_EmpID == EmpID && s.ST_AccountID == AC && s.ST_StockID == update.POH_StockID
                         select s;

                var V = update.POH_Volume;

                foreach (var sti in st)
                {
                    if ((V - sti.ST_Volume) > 0)
                    {
                        V = V - sti.ST_Volume;
                        sti.ST_Volume = 0;                        
                    }
                    else if ((V - sti.ST_Volume) <= 0)
                    {
                        sti.ST_Volume = sti.ST_Volume - V;
                        V = 0;
                    }
                }
            }

            db.StockTradingRecord.Add(str);    
            db.SaveChanges();

            //return Json(update, JsonRequestBehavior.AllowGet);

            return RedirectToAction("PendingOrderHistory_Table_load", new { EmpID = EmpID, AC = AC, Check = Check });
        }



        public ActionResult StockTradingRecord_load(int Day = 1)
        {
            var date1 = DateTime.Today;
            var date7 = DateTime.Today.AddDays(-7);
            var date30 = DateTime.Today.AddDays(-30);

            var q = from s in db.StockTradingRecord
                    where s.STR_EmpID == EmpID
                    select new
                    {
                        STR_ID = s.STR_ID,
                        STR_StockID = s.STR_StockID,
                        SI_StockName = s.StockInfo.SI_StockName,
                        STR_BuySold = (s.STR_BuySold == true ? "買" : s.STR_BuySold == false ? "賣" : "Wrong"),
                        STR_OrderPrice = s.STR_OrderPrice,
                        STR_Price = s.STR_Price,
                        STR_Volume = s.STR_Volume,
                        STR_OrderTime = s.STR_OrderTime,
                        STR_STR_EnterTreasuryTime = s.STR_STR_EnterTreasuryTime,
                        STR_AccountID = s.STR_AccountID
                    };

            if (Day == 1)
            {
                    q = from s in db.StockTradingRecord
                        where s.STR_EmpID == EmpID && s.STR_STR_EnterTreasuryTime > date1
                        select new
                        {
                            STR_ID = s.STR_ID,
                            STR_StockID = s.STR_StockID,
                            SI_StockName = s.StockInfo.SI_StockName,
                            STR_BuySold = (s.STR_BuySold == true ? "買" : s.STR_BuySold == false ? "賣" : "Wrong"),
                            STR_OrderPrice = s.STR_OrderPrice,
                            STR_Price = s.STR_Price,
                            STR_Volume = s.STR_Volume,
                            STR_OrderTime = s.STR_OrderTime,
                            STR_STR_EnterTreasuryTime = s.STR_STR_EnterTreasuryTime,
                            STR_AccountID = s.STR_AccountID
                        };
            }
            else if (Day == 7)
            {
                q = from s in db.StockTradingRecord
                    where s.STR_EmpID == EmpID && s.STR_STR_EnterTreasuryTime > date7
                    select new
                    {
                        STR_ID = s.STR_ID,
                        STR_StockID = s.STR_StockID,
                        SI_StockName = s.StockInfo.SI_StockName,
                        STR_BuySold = (s.STR_BuySold == true ? "買" : s.STR_BuySold == false ? "賣" : "Wrong"),
                        STR_OrderPrice = s.STR_OrderPrice,
                        STR_Price = s.STR_Price,
                        STR_Volume = s.STR_Volume,
                        STR_OrderTime = s.STR_OrderTime,
                        STR_STR_EnterTreasuryTime = s.STR_STR_EnterTreasuryTime,
                        STR_AccountID = s.STR_AccountID
                    };
            }
            else if (Day == 30)
            {
                q = from s in db.StockTradingRecord
                    where s.STR_EmpID == EmpID && s.STR_STR_EnterTreasuryTime > date30
                    select new
                    {
                        STR_ID = s.STR_ID,
                        STR_StockID = s.STR_StockID,
                        SI_StockName = s.StockInfo.SI_StockName,
                        STR_BuySold = (s.STR_BuySold == true ? "買" : s.STR_BuySold == false ? "賣" : "Wrong"),
                        STR_OrderPrice = s.STR_OrderPrice,
                        STR_Price = s.STR_Price,
                        STR_Volume = s.STR_Volume,
                        STR_OrderTime = s.STR_OrderTime,
                        STR_STR_EnterTreasuryTime = s.STR_STR_EnterTreasuryTime,
                        STR_AccountID = s.STR_AccountID
                    };
            }
           

            return Json(q.ToList(), JsonRequestBehavior.AllowGet);
        }



        

        public ActionResult StockTradeAdvancedsearch(string strac="",string strsid = "",string strbs="", string dates = "", string datee = "")
        {
            //var d = from str in db.StockTradingRecord
            //        select str.STR_ID;
            //List<SelectListItem> items = new List<SelectListItem>();
            //foreach (var dd in d)
            //{
            //    items.Add(new SelectListItem()
            //    {
            //        Text = dd.ToString(),
            //        Value = dd.ToString()  
            //    });
            //}      
            //ViewBag.STRIDDDL = items;
            ViewBag.strac = strac;
            ViewBag.strsid = strsid;
            ViewBag.strbs = strbs;
            ViewBag.dates = dates;
            ViewBag.datee = datee;

            EmpID = User.Identity.GetUserId();

            List<StockTradingRecordModel> lstrm = new List<StockTradingRecordModel>();
           

            var q = from str in db.StockTradingRecord
                    where str.STR_EmpID == EmpID
                    select str;


            if (strac != "")
            {
                q = q.Where(w => w.STR_AccountID == strac);
            }
            if (strsid != "")
            {
                q = q.Where(w => w.STR_StockID == strsid);
            }
            if (strbs != "")
            {
                Boolean boolean = Boolean.Parse(strbs);
                q = q.Where(w => w.STR_BuySold == boolean);
            }
            if (dates != "")
            {
                DateTime d = DateTime.Parse(dates);
                q = q.Where(w => w.STR_STR_EnterTreasuryTime > d);
            }
            if (datee != "")
            {
                DateTime d = DateTime.Parse(datee).AddDays(1);
                q = q.Where(w => w.STR_STR_EnterTreasuryTime <= d);
            }


            foreach (var item in q.OrderByDescending(o=>o.STR_STR_EnterTreasuryTime))
            {
                StockInfoModel sim = new StockInfoModel();
                sim.SI_StockID = item.StockInfo.SI_StockID;
                sim.SI_StockName = item.StockInfo.SI_StockName;
                sim.SI_CategoryID = item.StockInfo.SI_CategoryID;

                lstrm.Add(new StockTradingRecordModel()
                {
                    STR_ID = item.STR_ID,
                    STR_StockID = item.STR_StockID,
                    STR_BuySold = item.STR_BuySold,
                    STR_OrderPrice = item.STR_OrderPrice,
                    STR_Price = item.STR_Price,
                    STR_Volume = item.STR_Volume,
                    STR_OrderTime = item.STR_OrderTime,
                    STR_EmpID = item.STR_EmpID,
                    STR_STR_EnterTreasuryTime = item.STR_STR_EnterTreasuryTime,
                    STR_AccountID = item.STR_AccountID,

                    StockInfoModel = sim,    
                });

            }

            return View(lstrm);
        }

        public ActionResult STA_Partialview(string STRAC = "", string STRSID = "", string STRBS = "", string DATES = "", string DATEE = "")
        {
            var qac = (from s in db.StockTradingRecord
                       where s.STR_EmpID == EmpID
                       select s.STR_AccountID).Distinct();
            List<SelectListItem> strac = new List<SelectListItem>();
            strac.Add(new SelectListItem() { Text = "請選擇", Value = "" });
            foreach (var d in qac)
            {
                strac.Add(new SelectListItem()
                {
                    Text = d.ToString(),
                    Value = d.ToString(),
                    Selected = (d == STRAC)

                });
            }
            ViewBag.strac = strac;

            var qsid = (from s in db.StockTradingRecord
                        where s.STR_EmpID == EmpID
                        select new
                        {
                            STR_StockID = s.STR_StockID,
                            SI_StockName = s.StockInfo.SI_StockName
                        }).Distinct();
            List<SelectListItem> strsid = new List<SelectListItem>();
            strsid.Add(new SelectListItem() { Text = "請選擇", Value = "" });
            foreach (var d in qsid)
            {
                strsid.Add(new SelectListItem()
                {
                    Text = $"{d.STR_StockID.ToString()}({d.SI_StockName})",
                    Value = d.STR_StockID.ToString(),
                    Selected = (d.STR_StockID == STRSID)

                });
            }
            ViewBag.strsid = strsid;

            Boolean? boolean = null;
            if (STRBS != "")
            { boolean = Boolean.Parse(STRBS); }
            var q2 = (from s in db.StockTradingRecord
                      where s.STR_EmpID == EmpID
                      select s.STR_BuySold).Distinct();
            List<SelectListItem> strbs = new List<SelectListItem>();
            strbs.Add(new SelectListItem() { Text = "請選擇", Value = "" });
            foreach (var d in q2)
            {
                strbs.Add(new SelectListItem()
                {

                    Text = d == true ? "買" : d == false ? "賣" : "Wrong",
                    Value = d.ToString(),
                    Selected = d == boolean


                });
            }
            ViewBag.strbs = strbs;

            ViewBag.strds = DATES;
            ViewBag.strde = DATEE;






            return PartialView();
        }

        public ActionResult Category_Accordion_Load()
        {
            var q = from c in db.StockInfo
                    group c by new
                    {
                        c.StockCategoriesList.SCL_CategoryID,
                        c.StockCategoriesList.SCL_CategoryName

                    } into g
                    select new
                    {
                        ID = g.Key.SCL_CategoryID,
                        Key = g.Key.SCL_CategoryName,                        
                        items = g.Select(item => new {
                            SI_StockID = item.SI_StockID,
                            SI_StockName = item.SI_StockName
                           
                        })
                    };
            q = q.OrderBy(w => w.ID);
          

            return Json(q.ToList(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult FastStockID_Input_load(string StockID="")
        {
            var q = from s in db.StockInfo
                    where s.SI_StockID == StockID
                    select s.SI_StockName;
            return Json(q.ToList(), JsonRequestBehavior.AllowGet);
        }


        public ActionResult StockTrade_Price_load(string StockID = "")
        {
            var q = from s in db.StockInfoHistory
                    where s.SIH_StockID == StockID
                    select s.SIH_Price;
            return Json(q.ToList(), JsonRequestBehavior.AllowGet);
        }


        public ActionResult Place_An_Orders(string AC = "", string StockID = "" ,bool bs = true,decimal Price =0,int Volume =0)
        {
            var stockid = StockID;

            var buyorder = new PendingOrderHistory();
            buyorder.POH_StockID = StockID;
            buyorder.POH_BuySold = bs;
            buyorder.POH_Price = Price;
            buyorder.POH_Volume = Volume;
            buyorder.POH_OrderTime = DateTime.Now;
            buyorder.POH_EmpID = EmpID;
            buyorder.POH_CheckCancel = 0;
            buyorder.POH_AccountID = AC;

            db.PendingOrderHistory.Add(buyorder);
            db.SaveChanges();

            var si = (from s in db.StockInfo
                      where s.SI_StockID == StockID
                      select new
                      {
                          SI_StockID = s.SI_StockID,
                          SI_StockName = s.SI_StockName,
                      }).First();

            var infostring = $"{si.SI_StockName}({si.SI_StockID}) <br /> 掛單成功!!";

            return Json(infostring, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CheckTreasuryStockCanSell( string AC = "", string StockID = "", bool bs = true, decimal Price = 0, int Volume = 0)
        {
            var q = from st in db.StockTreasury
                     where st.ST_EmpID == EmpID && st.ST_AccountID == AC && st.ST_StockID == StockID 
                     select st.ST_Volume;
            var V = 0;
            foreach (var vol in q)
            {
                V  += vol;
            }
            if (V >= Volume)
            {
                return RedirectToAction("Place_An_Orders", new { EmpID = EmpID, AC = AC, StockID = StockID, bs =false, Price = Price , Volume = Volume });
            }
            else
            {
                var si = (from s in db.StockInfo
                          where s.SI_StockID == StockID
                          select new
                          {
                              SI_StockID = s.SI_StockID,
                              SI_StockName = s.SI_StockName,
                          }).First();

                var infostring = $"{si.SI_StockName}({si.SI_StockID}) <br /> 掛單失敗，庫存不足!!";

                return Json(infostring, JsonRequestBehavior.AllowGet);
            }

            
        }
        

        public ActionResult AccountID_Load()
        {
            var em = User.Identity.GetUserName();
            var DPN = from anu in db.AspNetUsers
                     join uru in db.uRoleUsers on anu.UserName equals uru.uUserID
                     join ur in db.uRoles on uru.uRoleID equals ur.uRoleID
                     join dp in db.Department on ur.departmentID equals dp.departmentID
                     where anu.UserName == em
                     select dp.departmentName;
            var s = DPN.FirstOrDefault() +"_" + EmpID;


            return Json(s, JsonRequestBehavior.AllowGet);
        }


        public ActionResult PerformanceReportJson(int month = 10)
        {

            //db.uRoles 部門別>>職稱
            //db.uRoleUsers 職稱>>員工帳號(信箱)

            var i = "股票";
            List<PerformanceModel> model = new List<PerformanceModel>();
            List<PerformanceModel> model2 = new List<PerformanceModel>();
            Random rd = new Random();
            
            var monthstart = new DateTime(DateTime.Now.Year, 10, 1);
            var monthend = new DateTime(DateTime.Now.Year, 11, 1);

            if(month == 11)
            {
               monthstart = new DateTime(DateTime.Now.Year, 11, 1);
               monthend = new DateTime(DateTime.Now.Year, 12, 1);
            }
            if (month == 12)
            {
                monthstart = new DateTime(DateTime.Now.Year, 12, 1);
                monthend = new DateTime(DateTime.Now.Year, 12, 31);
            }

            var PRDP = from dp in db.Department
                       select new
                       {
                           departmentID = dp.departmentID,
                           departmentName = dp.departmentName,
                           rgolbs = from str in db.StockTradingRecord
                                    join ur in db.uRoles on dp.departmentID equals ur.departmentID
                                    join uru in db.uRoleUsers on ur.uRoleID equals uru.uRoleID
                                    join anu in db.AspNetUsers on uru.uUserID equals anu.UserName
                                    where str.STR_EmpID == anu.Id
                                    orderby str.STR_STR_EnterTreasuryTime
                                    group str by str.STR_StockID into g
                                    select new
                                    {
                                        R = g.Select(s => new {
                                            s.STR_STR_EnterTreasuryTime,
                                            s.STR_BuySold,
                                            s.STR_Price,
                                            s.STR_Volume,
                                            s.STR_StockID
                                        }),
                                    },
                           ugolbs = from st in db.StockTreasury
                                    join sih in db.StockInfoHistory on st.ST_StockID equals sih.SIH_StockID
                                    join ur in db.uRoles on dp.departmentID equals ur.departmentID
                                    join uru in db.uRoleUsers on ur.uRoleID equals uru.uRoleID
                                    join anu in db.AspNetUsers on uru.uUserID equals anu.UserName
                                    where st.ST_EmpID == anu.Id && st.ST_EnterTreasuryTime > monthstart && st.ST_EnterTreasuryTime < monthend
                                    select new
                                    {
                                        SIH_Price = sih.SIH_Price,
                                        ST_Volume = st.ST_Volume,
                                        ST_Price = st.ST_Price,
                                        ST_EnterTreasuryTime = st.ST_EnterTreasuryTime,
                                    },
                           PR = from uru in db.uRoleUsers
                                join ur in db.uRoles on uru.uRoleID equals ur.uRoleID
                                join anu in db.AspNetUsers on uru.uUserID equals anu.UserName
                                where ur.departmentID == dp.departmentID
                                select new
                                {
                                    uUserID = uru.uUserID,
                                    _rgolbs = from str in db.StockTradingRecord
                                              where str.STR_EmpID == anu.Id
                                              orderby str.STR_STR_EnterTreasuryTime
                                              group str by str.STR_StockID into g
                                              select new
                                              {
                                                  R = g.Select(s => new {
                                                      s.STR_BuySold,
                                                      s.STR_Price,
                                                      s.STR_Volume,
                                                      s.STR_StockID,
                                                      s.STR_STR_EnterTreasuryTime,
                                                  }),
                                              },
                                    _ugolbs = from st in db.StockTreasury
                                              join sih in db.StockInfoHistory on st.ST_StockID equals sih.SIH_StockID
                                              where st.ST_EmpID == anu.Id
                                              select new
                                              {
                                                  SIH_Price = sih.SIH_Price,
                                                  ST_Volume = st.ST_Volume,
                                                  ST_Price = st.ST_Price
                                              }
                                }
                       };


            foreach (var items in PRDP)
            {
                decimal r = 0;
                foreach (var im in items.rgolbs)
                {
                    List<STRTemp> DPsold = new List<STRTemp>();
                    foreach (var ims in im.R.Where(w => w.STR_BuySold == false && w.STR_STR_EnterTreasuryTime > monthstart && w.STR_STR_EnterTreasuryTime < monthend))
                    {
                        STRTemp t = new STRTemp();
                        t.price = ims.STR_Price;
                        t.volume = ims.STR_Volume;
                        DPsold.Add(t);
                    }
                    var svt = 0;
                    foreach (var v in DPsold)
                    {
                        r += v.volume * v.price;
                        svt += v.volume;
                    }
                    foreach (var ims in im.R.Where(w => w.STR_BuySold == true && w.STR_STR_EnterTreasuryTime > monthstart && w.STR_STR_EnterTreasuryTime < monthend))
                    {
                        if (svt - ims.STR_Volume > 0)
                        {
                            r -= ims.STR_Volume * ims.STR_Price;

                            svt -= ims.STR_Volume;
                        }
                        if (svt != 0 && svt - ims.STR_Volume <= 0)
                        {
                            r -= svt * ims.STR_Price;
                            svt = 0;
                        }
                    }
                }



                decimal? u = 0;
                foreach (var item in items.ugolbs)
                {
                    //var Yprice = item.SIH_Price;
                    //var LiftUnit = 0.00;
                    //if (Yprice < 10)
                    //{ LiftUnit = 0.01; }
                    //else if (Yprice >= 10 && Yprice < 50)
                    //{ LiftUnit = 0.05; }
                    //else if (Yprice >= 50 && Yprice < 100)
                    //{ LiftUnit = 0.1; }
                    //else if (Yprice >= 100 && Yprice < 500)
                    //{ LiftUnit = 0.5; }
                    //else if (Yprice >= 500 && Yprice < 1000)
                    //{ LiftUnit = 1; }
                    //else if (Yprice >= 1000)
                    //{ LiftUnit = 5; }
                    //var rp = rd.Next(-5, 16);
                    //var Nprice = Yprice;
                    //Nprice += ((decimal)rp * (decimal)LiftUnit);

                    u += ((item.SIH_Price - item.ST_Price) * item.ST_Volume);
                }


                List<EMPPRModel> _m2 = new List<EMPPRModel>();
                foreach (var item in items.PR)
                {
                    decimal _r = 0;
                    foreach (var im in item._rgolbs)
                    {
                        List<STRTemp> sold = new List<STRTemp>();

                        foreach (var ims in im.R.Where(w => w.STR_BuySold == false))
                        {
                            STRTemp t = new STRTemp();
                            t.price = ims.STR_Price;
                            t.volume = ims.STR_Volume;
                            sold.Add(t);
                        }

                        var svt = 0;
                        foreach (var v in sold)
                        {
                            _r += v.volume * v.price;
                            svt += v.volume;
                        }
                        foreach (var ims in im.R.Where(w => w.STR_BuySold == true))
                        {
                            if (svt - ims.STR_Volume > 0)
                            {
                                _r -= ims.STR_Volume * ims.STR_Price;

                                svt -= ims.STR_Volume;
                            }
                            if (svt != 0 && svt - ims.STR_Volume <= 0)
                            {
                                _r -= svt * ims.STR_Price;
                                svt = 0;
                            }
                        }

                    }



                    decimal? _u = 0;
                    foreach (var im in item._ugolbs)
                    {


                        //var Yprice = im.SIH_Price;
                        //var LiftUnit = 0.00;
                        //if (Yprice < 10)
                        //{ LiftUnit = 0.01; }
                        //else if (Yprice >= 10 && Yprice < 50)
                        //{ LiftUnit = 0.05; }
                        //else if (Yprice >= 50 && Yprice < 100)
                        //{ LiftUnit = 0.1; }
                        //else if (Yprice >= 100 && Yprice < 500)
                        //{ LiftUnit = 0.5; }
                        //else if (Yprice >= 500 && Yprice < 1000)
                        //{ LiftUnit = 1; }
                        //else if (Yprice >= 1000)
                        //{ LiftUnit = 5; }
                        //var rp = rd.Next(-10,11);
                        //var Nprice = Yprice;
                        //Nprice += ((decimal)rp * (decimal)LiftUnit);

                        _u += ((im.SIH_Price - im.ST_Price) * im.ST_Volume);
                    }
                    _m2.Add(new EMPPRModel()
                    {
                        _UserName = item.uUserID,
                        _unrealizedgainsorlosses = _u * 1000,
                        _realizedgainsorlosses = _r * 1000,
                    });
                }


                model2.Add(new PerformanceModel()
                {
                    departmentID = items.departmentID,
                    departmentName = items.departmentName,
                    unrealizedgainsorlosses = u * 1000,
                    realizedgainsorlosses = r * 1000,
                    EMPPR = _m2

                });
            }

            return Json(model2, JsonRequestBehavior.AllowGet);           
        }






        public ActionResult PerformanceReport()
        {
            //db.uRoles 部門別>>職稱
            //db.uRoleUsers 職稱>>員工帳號(信箱)

            var i = "股票";
            List<PerformanceModel> model2 = new List<PerformanceModel>();
            Random rd = new Random();

            var PRDP = from dp in db.Department
                       select new
                       {
                           departmentID = dp.departmentID,
                           departmentName = dp.departmentName,
                           rgolbs =from str in db.StockTradingRecord
                                   join ur in db.uRoles on dp.departmentID equals ur.departmentID
                                   join uru in db.uRoleUsers on ur.uRoleID equals uru.uRoleID
                                   join anu in db.AspNetUsers on uru.uUserID equals anu.UserName
                                   where str.STR_EmpID == anu.Id
                                   orderby str.STR_STR_EnterTreasuryTime
                                   group str by str.STR_StockID into g
                                   select new
                                   {
                                       R = g.Select(s=> new {
                                           s.STR_BuySold,
                                           s.STR_Price,
                                           s.STR_Volume,
                                           s.STR_StockID
                                       }),
                                   },
                           ugolbs = from st in db.StockTreasury
                                    join sih in db.StockInfoHistory on st.ST_StockID equals sih.SIH_StockID
                                    join ur in db.uRoles on dp.departmentID equals ur.departmentID
                                    join uru in db.uRoleUsers on ur.uRoleID equals uru.uRoleID
                                    join anu in db.AspNetUsers on uru.uUserID equals anu.UserName
                                    where st.ST_EmpID == anu.Id
                                    select new
                                    {
                                        SIH_Price = sih.SIH_Price,
                                        ST_Volume = st.ST_Volume,
                                        ST_Price = st.ST_Price
                                    },
                           PR = from uru in db.uRoleUsers
                                join ur in db.uRoles on uru.uRoleID equals ur.uRoleID
                                join anu in db.AspNetUsers on uru.uUserID equals anu.UserName
                                where ur.departmentID == dp.departmentID
                                select new
                                {
                                    uUserID = uru.uUserID,
                                    _rgolbs = from str in db.StockTradingRecord
                                              where str.STR_EmpID == anu.Id
                                              orderby str.STR_STR_EnterTreasuryTime
                                              group str by str.STR_StockID into g
                                              select new
                                              {
                                                  R = g.Select(s => new {
                                                      s.STR_BuySold,
                                                      s.STR_Price,
                                                      s.STR_Volume,
                                                      s.STR_StockID
                                                  }),
                                              },
                                    _ugolbs = from st in db.StockTreasury
                                              join sih in db.StockInfoHistory on st.ST_StockID equals sih.SIH_StockID
                                              where st.ST_EmpID == anu.Id
                                              select new
                                              {
                                                  SIH_Price = sih.SIH_Price,
                                                  ST_Volume = st.ST_Volume,
                                                  ST_Price = st.ST_Price
                                              }
                                }
                       };

            
            foreach (var items in PRDP)
            {
                decimal r = 0;
                foreach (var im in items.rgolbs)
                {
                    List<STRTemp> DPsold = new List<STRTemp>();
                    foreach (var ims in im.R.Where(w => w.STR_BuySold == false))
                    {
                        STRTemp t = new STRTemp();
                        t.price = ims.STR_Price;
                        t.volume = ims.STR_Volume;
                        DPsold.Add(t);
                    }
                    var svt = 0;
                    foreach (var v in DPsold)
                    {
                        r += v.volume * v.price;
                        svt += v.volume;
                    }
                    foreach (var ims in im.R.Where(w => w.STR_BuySold == true))
                    {
                        if (svt - ims.STR_Volume > 0)
                        {
                            r -= ims.STR_Volume * ims.STR_Price;

                            svt -= ims.STR_Volume;
                        }
                        if (svt != 0 && svt - ims.STR_Volume <= 0)
                        {
                            r -= svt * ims.STR_Price;
                            svt = 0;
                        }
                    }
                }

                

                decimal? u = 0;
                foreach (var item in items.ugolbs)
                {
                    //var Yprice = item.SIH_Price;
                    //var LiftUnit = 0.00;
                    //if (Yprice < 10)
                    //{ LiftUnit = 0.01; }
                    //else if (Yprice >= 10 && Yprice < 50)
                    //{ LiftUnit = 0.05; }
                    //else if (Yprice >= 50 && Yprice < 100)
                    //{ LiftUnit = 0.1; }
                    //else if (Yprice >= 100 && Yprice < 500)
                    //{ LiftUnit = 0.5; }
                    //else if (Yprice >= 500 && Yprice < 1000)
                    //{ LiftUnit = 1; }
                    //else if (Yprice >= 1000)
                    //{ LiftUnit = 5; }
                    //var rp = rd.Next(-5, 16);
                    //var Nprice = Yprice;
                    //Nprice += ((decimal)rp * (decimal)LiftUnit);

                    u += ((item.SIH_Price - item.ST_Price) * item.ST_Volume);
                }

                
                List<EMPPRModel> _m2 = new List<EMPPRModel>();
                foreach (var item in items.PR)
                {
                    decimal _r = 0;
                    foreach (var im in item._rgolbs)
                    {                        
                        List<STRTemp> sold = new List<STRTemp>();                        

                        foreach (var ims in im.R.Where(w=>w.STR_BuySold == false))
                        {
                            STRTemp t = new STRTemp();
                            t.price = ims.STR_Price;
                            t.volume = ims.STR_Volume;
                            sold.Add(t);
                        }

                        var svt = 0;
                        foreach (var v in sold)
                        {
                            _r += v.volume * v.price;
                            svt += v.volume;
                        }
                        foreach (var ims in im.R.Where(w=>w.STR_BuySold == true))
                        {
                            if (svt - ims.STR_Volume > 0)
                            {
                                _r -= ims.STR_Volume * ims.STR_Price;

                                svt -= ims.STR_Volume;
                            }
                            if (svt != 0 && svt - ims.STR_Volume <= 0)
                            {
                                _r -= svt * ims.STR_Price;
                                svt = 0;
                            }
                        }
                        
                    }



                    decimal? _u = 0;
                    foreach (var im in item._ugolbs)
                    {

                        
                        //var Yprice = im.SIH_Price;
                        //var LiftUnit = 0.00;
                        //if (Yprice < 10)
                        //{ LiftUnit = 0.01; }
                        //else if (Yprice >= 10 && Yprice < 50)
                        //{ LiftUnit = 0.05; }
                        //else if (Yprice >= 50 && Yprice < 100)
                        //{ LiftUnit = 0.1; }
                        //else if (Yprice >= 100 && Yprice < 500)
                        //{ LiftUnit = 0.5; }
                        //else if (Yprice >= 500 && Yprice < 1000)
                        //{ LiftUnit = 1; }
                        //else if (Yprice >= 1000)
                        //{ LiftUnit = 5; }
                        //var rp = rd.Next(-10,11);
                        //var Nprice = Yprice;
                        //Nprice += ((decimal)rp * (decimal)LiftUnit);

                        _u += ((im.SIH_Price - im.ST_Price) * im.ST_Volume);
                    }
                    _m2.Add(new EMPPRModel()
                    {
                        _UserName = item.uUserID,
                        _unrealizedgainsorlosses = _u*1000 ,
                        _realizedgainsorlosses = _r *1000,
                    });
                }


                model2.Add(new PerformanceModel()
                {
                    departmentID = items.departmentID,
                    departmentName = items.departmentName,
                    unrealizedgainsorlosses = u*1000 ,
                    realizedgainsorlosses = r*1000 ,
                    EMPPR = _m2

                });
            }

            //return Json(model2, JsonRequestBehavior.AllowGet);
            return View(model2);
        }

        public class STRTemp
        {
            public decimal price { get; set; }
            public int volume { get; set; }

        }
       
        


        public ActionResult trendchart_select(string Stock = "0")
        {
            var q = from s in db.StockInfoHistory
                    where s.SIH_StockID == Stock
                    select new
                    {
                        SIH_StockID = s.SIH_StockID,
                        SI_StockName = s.StockInfo.SI_StockName,
                        SIH_Price = s.SIH_Price

                    };

            return Json(q.First(), JsonRequestBehavior.AllowGet);
        }



        ////////////////////////////////////////////////////////  

        public ActionResult Data_ADD()
        {
            var q = from uru in db.uRoleUsers
                    join anu in db.AspNetUsers on uru.uUserID equals anu.UserName
                    join ur in db.uRoles on uru.uRoleID equals ur.uRoleID
                    join dp in db.Department on ur.departmentID equals dp.departmentID
                    select new
                    {
                        uru.uUserID,
                        anu.Id,
                        dp.departmentName,
                    };

            var startI = 1;
            var endI = 300;

            Random r = new Random();

            foreach (var item in q)
            {
                var userhashid = item.Id;
                var dpname = item.departmentName;

                //
                for (int i = startI; i <= endI; i++)
                {
                    var SRandom = (from si in db.StockInfo
                                   join sih in db.StockInfoHistory on si.SI_StockID equals sih.SIH_StockID
                                   select new
                                   {
                                       SI_StockID = si.SI_StockID,
                                       SI_StockName = si.SI_StockName,
                                       SIH_Price = sih.SIH_Price
                                   }).OrderBy(o => Guid.NewGuid()).Take(1).First();



                    var Yprice = SRandom.SIH_Price;
                    var Maxprice = (double)Yprice * 1.1;
                    var Minprice = (double)Yprice * 0.9;
                    var LiftUnit = 0.01;
                    var Volume = r.Next(3, 11);
                    var Volume2 = Volume - r.Next(0, 4);

                    if (Yprice < 10)
                    { LiftUnit = 0.01; }
                    else if (Yprice >= 10 && Yprice < 50)
                    { LiftUnit = 0.05; }
                    else if (Yprice >= 50 && Yprice < 100)
                    { LiftUnit = 0.1; }
                    else if (Yprice >= 100 && Yprice < 500)
                    { LiftUnit = 0.5; }
                    else if (Yprice >= 500 && Yprice < 1000)
                    { LiftUnit = 1; }
                    else if (Yprice >= 1000)
                    { LiftUnit = 5; }

                    var Oprice = (r.Next(-1, 2) * LiftUnit) + (double)Yprice;
                    var Nprice = (r.Next(-5, 6) * LiftUnit) + (double)Yprice;
                    var Nprice2 = (r.Next(-4, 13) * LiftUnit) + (double)Yprice; // -3~13

                    var poh = new PendingOrderHistory();
                    poh.POH_StockID = SRandom.SI_StockID;
                    poh.POH_BuySold = true;
                    poh.POH_Price = (decimal)Oprice;
                    poh.POH_Volume = Volume;
                    poh.POH_OrderTime = DateTime.Now.AddDays(-90 + (i / 10));
                    poh.POH_EmpID = userhashid;
                    poh.POH_CheckCancel = 2;
                    poh.POH_AccountID = dpname + "_" + userhashid;
                    db.PendingOrderHistory.Add(poh);

                    var str = new StockTradingRecord();
                    str.STR_StockID = SRandom.SI_StockID;
                    str.STR_BuySold = true;
                    str.STR_OrderPrice = (decimal)Oprice;
                    str.STR_Price = (decimal)Nprice;
                    str.STR_Volume = Volume;
                    str.STR_OrderTime = DateTime.Now.AddDays(-90 + i);
                    str.STR_EmpID = userhashid;
                    str.STR_STR_EnterTreasuryTime = DateTime.Now.AddDays(-90 + (i / 10) + 2);
                    str.STR_AccountID = dpname + "_" + userhashid;
                    db.StockTradingRecord.Add(str);

                    var st = new StockTreasury();
                    st.ST_StockID = SRandom.SI_StockID;
                    st.ST_Price = (decimal)Nprice;
                    st.ST_Volume = Volume;
                    st.ST_EmpID = userhashid;
                    st.ST_EnterTreasuryTime = DateTime.Now.AddDays(-90 + (i / 10) + 2);
                    st.ST_AccountID = dpname + "_" + userhashid;
                    db.StockTreasury.Add(st);

                    //////////////////////////////////////////////////////////
                    if (r.Next(1, 21) <= 19)
                    {

                        var poh2 = new PendingOrderHistory();
                        poh2.POH_StockID = SRandom.SI_StockID;
                        poh2.POH_BuySold = false;
                        poh2.POH_Price = (decimal)Oprice;
                        poh2.POH_Volume = Volume2;
                        poh2.POH_OrderTime = DateTime.Now.AddDays(-90 + (i / 10) + 7);
                        poh2.POH_EmpID = userhashid;
                        poh2.POH_CheckCancel = 2;
                        poh2.POH_AccountID = dpname + "_" + userhashid;
                        db.PendingOrderHistory.Add(poh2);

                        var str2 = new StockTradingRecord();
                        str2.STR_StockID = SRandom.SI_StockID;
                        str2.STR_BuySold = false;
                        str2.STR_OrderPrice = (decimal)Oprice;
                        str2.STR_Price = (decimal)Nprice2;
                        str2.STR_Volume = Volume2;
                        str2.STR_OrderTime = DateTime.Now.AddDays(-90 + i + 7);
                        str2.STR_EmpID = userhashid;
                        str2.STR_STR_EnterTreasuryTime = DateTime.Now.AddDays(-90 + (i / 10) + 2 + 7);
                        str2.STR_AccountID = dpname + "_" + userhashid;
                        db.StockTradingRecord.Add(str2);

                        var st2 = new StockTreasury();
                        st2.ST_StockID = SRandom.SI_StockID;
                        st2.ST_Price = (decimal)Nprice2;
                        st2.ST_Volume = Volume2;
                        st2.ST_EmpID = userhashid;
                        st2.ST_EnterTreasuryTime = DateTime.Now.AddDays(-90 + (i / 10) + 2 + 7);
                        st2.ST_AccountID = dpname + "_" + userhashid;
                        db.StockTreasury.Add(st2);
                    }


                }

                for (int i = startI; i <= endI; i++)
                {                   
                    if (r.Next(1, 11) <= 1)
                    {


                        var SRandom = (from si in db.StockInfo
                                       join sih in db.StockInfoHistory on si.SI_StockID equals sih.SIH_StockID
                                       select new
                                       {
                                           SI_StockID = si.SI_StockID,
                                           SI_StockName = si.SI_StockName,
                                           SIH_Price = sih.SIH_Price
                                       }).OrderBy(o => Guid.NewGuid()).Take(1).First();


                        var Yprice = SRandom.SIH_Price;
                        var Maxprice = (double)Yprice * 1.1;
                        var Minprice = (double)Yprice * 0.9;
                        var LiftUnit = 0.01;
                        var Volume = r.Next(2, 11);

                        if (Yprice < 10)
                        { LiftUnit = 0.01; }
                        else if (Yprice >= 10 && Yprice < 50)
                        { LiftUnit = 0.05; }
                        else if (Yprice >= 50 && Yprice < 100)
                        { LiftUnit = 0.1; }
                        else if (Yprice >= 100 && Yprice < 500)
                        { LiftUnit = 0.5; }
                        else if (Yprice >= 500 && Yprice < 1000)
                        { LiftUnit = 1; }
                        else if (Yprice >= 1000)
                        { LiftUnit = 5; }

                        var Oprice = (r.Next(-1, 2) * LiftUnit) + (double)Yprice;
                        var Nprice = (r.Next(-5, 6) * LiftUnit) + (double)Yprice;

                        var poh = new PendingOrderHistory();
                        poh.POH_StockID = SRandom.SI_StockID;
                        poh.POH_BuySold = true;
                        poh.POH_Price = (decimal)Oprice;
                        poh.POH_Volume = Volume;
                        poh.POH_OrderTime = DateTime.Now.AddDays(-90 + (i / 10));
                        poh.POH_EmpID = userhashid;
                        poh.POH_CheckCancel = 2;
                        poh.POH_AccountID = dpname + "_" + userhashid;
                        db.PendingOrderHistory.Add(poh);

                        var str = new StockTradingRecord();
                        str.STR_StockID = SRandom.SI_StockID;
                        str.STR_BuySold = true;
                        str.STR_OrderPrice = (decimal)Oprice;
                        str.STR_Price = (decimal)Nprice;
                        str.STR_Volume = Volume;
                        str.STR_OrderTime = DateTime.Now.AddDays(-90 + i);
                        str.STR_EmpID = userhashid;
                        str.STR_STR_EnterTreasuryTime = DateTime.Now.AddDays(-90 + (i / 10) + 2);
                        str.STR_AccountID = dpname + "_" + userhashid;
                        db.StockTradingRecord.Add(str);

                        var st = new StockTreasury();
                        st.ST_StockID = SRandom.SI_StockID;
                        st.ST_Price = (decimal)Nprice;
                        st.ST_Volume = Volume;
                        st.ST_EmpID = userhashid;
                        st.ST_EnterTreasuryTime = DateTime.Now.AddDays(-90 + (i / 10) + 2);
                        st.ST_AccountID = dpname + "_" + userhashid;
                        db.StockTreasury.Add(st);
                    }

                }
            }
            db.SaveChanges();

            var count_poh = (from poh in db.PendingOrderHistory
                             select poh.POH_ID).Count().ToString();
            var count_str = (from str in db.StockTradingRecord
                             select str.STR_ID).Count().ToString();
            var count_st = (from st in db.StockTreasury
                            select st.ST_ID).Count().ToString();


            return Json("count_poh =" + count_poh + " ,count_str =" + count_str + " ,count_st =" + count_st, JsonRequestBehavior.AllowGet);
        }








        //////////////////////////////////////////
        //首頁小格子PartialView
        public ActionResult FastStock()
        {
            ViewBag.id = User.Identity.GetUserId();


            var SSSRandom = (from sss in db.SelfSelectedStockList
                             join sih in db.StockInfoHistory on sss.SSS_StockID equals sih.SIH_StockID
                             join si in db.StockInfo on sih.SIH_StockID equals si.SI_StockID
                             select new
                             {
                                 SI_StockName = si.SI_StockName,
                                 SIH_Price = sih.SIH_Price
                             }).OrderBy(o => Guid.NewGuid()).Take(3).ToArray();


            DashboardModel model = new DashboardModel();

            model.SSSRandomName1 = SSSRandom[0].SI_StockName;
            model.SSSRandomPrice1 = SSSRandom[0].SIH_Price;
            model.SSSRandomName2 = SSSRandom[1].SI_StockName;
            model.SSSRandomPrice2 = SSSRandom[1].SIH_Price;
            model.SSSRandomName3 = SSSRandom[2].SI_StockName;
            model.SSSRandomPrice3 = SSSRandom[2].SIH_Price;


            return PartialView(model);
        }

        public class DashboardModel
        {
            public string SSSRandomName1 { get; set; }
            public decimal? SSSRandomPrice1 { get; set; }
            public string SSSRandomName2 { get; set; }
            public decimal? SSSRandomPrice2 { get; set; }
            public string SSSRandomName3 { get; set; }
            public decimal? SSSRandomPrice3 { get; set; }
        }







    }
}