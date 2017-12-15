using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;

namespace msit116apexLayout.Models
{
    public class ConfirmUserPowerMethod
    {
        MSIT116APEXEntities db = new MSIT116APEXEntities();
        public bool checkIsEmployee(string userName)
        {
            bool checkResult = false;
            if (db.IsEmployee.Where(n => n.UserId == userName).Count() > 0)
                checkResult = true;
            return checkResult;
        }

        public bool checkHasPower(string userName, int powerID)
        {
            bool checkResult = false;
            int count = (from r in db.uRoles
                         join ru in db.uRoleUsers on r.uRoleID equals ru.uRoleID
                         join rp in db.uRolePowers on r.uRoleID equals rp.uRoleID
                         where ru.uUserID == userName && rp.powerID == powerID
                         select rp.powerID).Count();

            if (count > 0)
                checkResult = true;
            return checkResult;
        }

        public String checkNeedConfirm(out int? outurpchSn, string userName, int powerID, string confirmUrl, string confirmDescription = "")
        {
            string checkResult = "";
            outurpchSn = null;

            IEnumerable<uRoles> uRolesie = db.uRoles.ToList();
            IEnumerable<uRoleUsers> uRoleUsersie = db.uRoleUsers.ToList();
            IEnumerable<uRolePowers> uRolePowersie = db.uRolePowers.ToList();
            IEnumerable<uRolePowerConfirm> uRolePowerConfirmie = db.uRolePowerConfirm.ToList();
            IEnumerable<uRolePowerConfirmRole> uRolePowerConfirmRoleie = db.uRolePowerConfirmRole.ToList();
            #region 取出歷史覆核清單
            uRolePowerConfirmHistory olduRolePowerConfirmHistory = (from r in uRolesie
                                                                    join ru in uRoleUsersie on r.uRoleID equals ru.uRoleID
                                                                    join rp in uRolePowersie on r.uRoleID equals rp.uRoleID
                                                                    join rpc in uRolePowerConfirmie on rp.urpSn equals rpc.uRolePowerSn
                                                                    join rpch in db.uRolePowerConfirmHistory on rpc.ucrSn equals rpch.uRolePowerConfirmSn
                                                                    where ru.uUserID == userName && rp.powerID == powerID && (rpch.state == 0 || rpch.state == 1)
                                                                    select rpch).FirstOrDefault();
            #endregion
            if (olduRolePowerConfirmHistory == null)
            {
                #region 取出所有擁有角色覆核設定
                List<uRolePowerConfirmDecide> urpcli = (from r in uRolesie
                                                        join ru in uRoleUsersie on r.uRoleID equals ru.uRoleID
                                                        join rp in uRolePowersie on r.uRoleID equals rp.uRoleID
                                                        join rpc in uRolePowerConfirmie on rp.urpSn equals rpc.uRolePowerSn
                                                        where ru.uUserID == userName && rp.powerID == powerID && (rpc.uEachRoleMinNum.HasValue || rpc.uTotlaRoleMinNum.HasValue)
                                                        select new uRolePowerConfirmDecide
                                                        {
                                                            uRolePowerConfirm = rpc,
                                                            roleRegisteredDate = r.registedDate.Value,
                                                            firstSurRoleID = r.supervisorID,
                                                            uRoleID = r.uRoleID
                                                        }).ToList();
                for (int i = 0; i < urpcli.Count; i++)
                {
                    int roleNums = (from rpc in uRolePowerConfirmie
                                    join rpcr in uRolePowerConfirmRoleie on rpc.ucrSn equals rpcr.uRolePowerConfirmSn
                                    where rpc.ucrSn == urpcli[i].uRolePowerConfirm.ucrSn
                                    select rpc).Count();
                    int roleLevel = 0;
                    bool checkRoleLevelEnd = true;
                    int? tmpSurRoleID = urpcli[i].firstSurRoleID;
                    if (tmpSurRoleID.HasValue)
                    {
                        while (checkRoleLevelEnd)
                        {
                            tmpSurRoleID = uRolesie.Where(n => n.uRoleID == tmpSurRoleID).Select(n => n.supervisorID).FirstOrDefault();
                            if (!tmpSurRoleID.HasValue)
                                checkRoleLevelEnd = false;
                            roleLevel++;
                        }
                    }
                    urpcli[i].roleNums = roleNums;
                    urpcli[i].roleLevel = roleLevel;
                }
                #endregion
                bool needConfirm = false;
                if (urpcli.Count() > 0)
                    needConfirm = true;
                if (needConfirm)
                {
                    List<uRolePowerConfirmDecide> decidedConfirmSet = new List<uRolePowerConfirmDecide>();
                    decidedConfirmSet.AddRange(urpcli);
                    #region 選擇覆核設定
                    while (decidedConfirmSet.Count() > 1)
                    {
                        if (decidedConfirmSet[1].uRolePowerConfirm.rolePriority > decidedConfirmSet[0].uRolePowerConfirm.rolePriority)
                        {
                            decidedConfirmSet.RemoveAt(0);
                            continue;
                        }
                        if (decidedConfirmSet[1].uRolePowerConfirm.rolePriority < decidedConfirmSet[0].uRolePowerConfirm.rolePriority)
                        {
                            decidedConfirmSet.RemoveAt(1);
                            continue;
                        }
                        if (decidedConfirmSet[1].roleLevel < decidedConfirmSet[0].roleLevel)
                        {
                            decidedConfirmSet.RemoveAt(0);
                            continue;
                        }
                        if (decidedConfirmSet[1].roleLevel > decidedConfirmSet[0].roleLevel)
                        {
                            decidedConfirmSet.RemoveAt(1);
                            continue;
                        }
                        if (decidedConfirmSet[1].roleNums > decidedConfirmSet[0].roleNums)
                        {
                            decidedConfirmSet.RemoveAt(0);
                            continue;
                        }
                        if (decidedConfirmSet[1].roleNums < decidedConfirmSet[0].roleNums)
                        {
                            decidedConfirmSet.RemoveAt(1);
                            continue;
                        }
                        if (decidedConfirmSet[1].uRolePowerConfirm.uTotlaRoleMinNum > decidedConfirmSet[0].uRolePowerConfirm.uTotlaRoleMinNum)
                        {
                            decidedConfirmSet.RemoveAt(0);
                            continue;
                        }
                        if (decidedConfirmSet[1].uRolePowerConfirm.uTotlaRoleMinNum < decidedConfirmSet[0].uRolePowerConfirm.uTotlaRoleMinNum)
                        {
                            decidedConfirmSet.RemoveAt(1);
                            continue;
                        }
                        if (decidedConfirmSet[1].uRolePowerConfirm.uEachRoleMinNum > decidedConfirmSet[0].uRolePowerConfirm.uEachRoleMinNum)
                        {
                            decidedConfirmSet.RemoveAt(0);
                            continue;
                        }
                        if (decidedConfirmSet[1].uRolePowerConfirm.uEachRoleMinNum < decidedConfirmSet[0].uRolePowerConfirm.uEachRoleMinNum)
                        {
                            decidedConfirmSet.RemoveAt(1);
                            continue;
                        }
                        if (decidedConfirmSet[1].roleRegisteredDate > decidedConfirmSet[0].roleRegisteredDate)
                        {
                            decidedConfirmSet.RemoveAt(0);
                            continue;
                        }
                        if (decidedConfirmSet[1].roleRegisteredDate < decidedConfirmSet[0].roleRegisteredDate)
                        {
                            decidedConfirmSet.RemoveAt(1);
                            continue;
                        }
                        if (decidedConfirmSet[1].uRoleID < decidedConfirmSet[0].uRoleID)
                        {
                            decidedConfirmSet.RemoveAt(0);
                            continue;
                        }
                        if (decidedConfirmSet[1].uRoleID > decidedConfirmSet[0].uRoleID)
                        {
                            decidedConfirmSet.RemoveAt(1);
                            continue;
                        }
                    }
                    #endregion
                    checkResult = "開始處理覆核，開始時間為" + DateTime.Now.ToString() + "。";
                    #region 寫入發派覆核清單
                    List<uRoleUsers> needConfirmUserWithRole = (from urpcr in uRolePowerConfirmRoleie
                                                                join urpc in uRolePowerConfirmie on urpcr.uRolePowerConfirmSn equals urpc.ucrSn
                                                                join uru in uRoleUsersie on urpcr.uConfirmRoleID equals uru.uRoleID
                                                                where urpc.ucrSn == decidedConfirmSet[0].uRolePowerConfirm.ucrSn
                                                                select uru).ToList();
                    uRolePowerConfirmHistory newurpch = new uRolePowerConfirmHistory
                    {
                        uRolePowerConfirmSn = decidedConfirmSet[0].uRolePowerConfirm.ucrSn,
                        startDate = DateTime.Now,
                        state = 0,
                        UserID = userName,
                        powerID = powerID
                    };
                    if (decidedConfirmSet[0].uRolePowerConfirm.maxDay.HasValue)
                        newurpch.limitDate = DateTime.Now.AddDays(decidedConfirmSet[0].uRolePowerConfirm.maxDay.Value);
                    db.uRolePowerConfirmHistory.Add(newurpch);
                    db.SaveChanges();


                    List<uRolePowerConfirmHistoryDetail> newpchdList = new List<uRolePowerConfirmHistoryDetail>();
                    foreach (uRoleUsers uru in needConfirmUserWithRole)
                    {
                        uRolePowerConfirmHistoryDetail urpchd = new uRolePowerConfirmHistoryDetail
                        {
                            uRolePowerConfirmHistorySn = newurpch.urpchSn,
                            ConfirmRoleID = uru.uRoleID,
                            ConfirmUserID = uru.uUserID,
                            state = 0
                        };
                        db.uRolePowerConfirmHistoryDetail.Add(urpchd);
                        newpchdList.Add(urpchd);
                    }
                    db.SaveChanges();

                    outurpchSn = newurpch.urpchSn;
                    #endregion
                    int toUserNewsurpchSn = newurpch.urpchSn;
                    //寫入發派覆核資料庫
                    HubServerMethods NewsHub = new HubServerMethods();
                    string powerName = db.uPowers.Where(n => n.powerID == powerID).Select(n => n.powerName).FirstOrDefault();
                    int drid = decidedConfirmSet[0].uRoleID;
                    uRolesDpmt urd = (from ur in db.uRoles
                                      join d in db.Department on ur.departmentID equals d.departmentID
                                      where ur.uRoleID == drid
                                      select new uRolesDpmt
                                      {
                                          uRoles = ur,
                                          Department = d.departmentName
                                      }).FirstOrDefault();
                    AspNetUsers asu = db.AspNetUsers.Where(n => n.UserName == userName).FirstOrDefault();
                    List<int> returnUserNewsSnList = new List<int>();
                    foreach (var newpchd in newpchdList)
                    {
                        List<UserNewsUrls> confirmUrls = new List<UserNewsUrls>();
                        UserNewsUrls confirmUrl1 = new UserNewsUrls
                        {
                            UserNewsTitle = "核准覆核",
                            UserNewsUrl = confirmUrl + "?needConfirmUserID=" + userName + "&needConfirmPowerID=" + powerID + "&ConfirmResult=1",
                            UserNewsCSS = "btn btn-warning"
                        };
                        UserNewsUrls confirmUrl2 = new UserNewsUrls
                        {
                            UserNewsTitle = "否決覆核",
                            UserNewsUrl = confirmUrl + "?needConfirmUserID=" + userName + "&needConfirmPowerID=" + powerID + "&ConfirmResult=0",
                            UserNewsCSS = "btn btn-danger"
                        };
                        confirmUrls.Add(confirmUrl1);
                        confirmUrls.Add(confirmUrl2);
                        string fromUser = userName;
                        string toUser = newpchd.ConfirmUserID;
                        string msgTitle = "執行權限需要被覆核";
                        string msgContent = "使用者：'" + asu.Name + "'&lt帳號：" + userName + "&gt<br/>"
                            + "使用角色：'" + urd.uRoles.uRoleName + "'&lt部門：" + urd.Department + "&gt<br/>"
                            + "使用權限：'" + powerName + "'&gt<br/>"
                            + "核准內容：" + confirmDescription;
                        int returnUserNewsSn = NewsHub.SendMessageToUser(fromUser, toUser, msgTitle, msgContent, "", confirmUrls, "True");
                        returnUserNewsSnList.Add(returnUserNewsSn);
                    }
                    //"~/ConfirmUserPower/UserConfirmUserPower?needConfirmUserID=xxx&needConfirmPowerID=xxx&ConfirmResult=xxx"

                    //紀錄發送覆核消息與覆核紀錄關聯
                    AddUserNewsConfirmList(returnUserNewsSnList, toUserNewsurpchSn);
                }
            }
            else
            {
                Repository<uRolePowerConfirmHistory> dburpch = new Repository<uRolePowerConfirmHistory>();
                if (olduRolePowerConfirmHistory.state == 0)
                {
                    if (olduRolePowerConfirmHistory.limitDate.HasValue && olduRolePowerConfirmHistory.limitDate.Value > DateTime.Now)
                    {
                        checkResult = "覆核已過期，請重新點擊。";
                        olduRolePowerConfirmHistory.state = -1;
                        olduRolePowerConfirmHistory.endDate = DateTime.Now;
                        dburpch.UpdateWithoutNull(olduRolePowerConfirmHistory);
                    }
                    else
                    {
                        checkResult = "覆核已在處理中，開始時間為" + olduRolePowerConfirmHistory.startDate.Value.ToString();
                        if (olduRolePowerConfirmHistory.limitDate.HasValue)
                            checkResult += "，期限時間為" + olduRolePowerConfirmHistory.limitDate.Value;
                        checkResult += "。";
                    }
                }
                else
                {
                    //此處為一次解鎖操作
                    //olduRolePowerConfirmHistory.state = 2;
                    //olduRolePowerConfirmHistory.endDate = DateTime.Now;
                    //dburpch.UpdateWithoutNull(olduRolePowerConfirmHistory);


                    //ExecExecConfirmData()
                }
            }

            return checkResult;
        }

        public int? UserConfirmUserPowerM(string ConfirmUserID, string needConfirmUserID, int needConfirmPowerID, int ConfirmResult)
        {
            int? returnConfirmEndurpchSn = null;
            IEnumerable<uRolePowerConfirmHistoryDetail> urpchDetail = from urpch in db.uRolePowerConfirmHistory
                                                                      join urpchd in db.uRolePowerConfirmHistoryDetail on urpch.urpchSn equals urpchd.uRolePowerConfirmHistorySn
                                                                      where urpch.UserID == needConfirmUserID && urpch.powerID == needConfirmPowerID && urpchd.ConfirmUserID == ConfirmUserID && urpchd.state == 0
                                                                      select urpchd;
            int? urpchSn = urpchDetail.FirstOrDefault().uRolePowerConfirmHistorySn;
            int intResult = -1;
            if (ConfirmResult == 1)
                intResult = 1;
            foreach (var urpchd in urpchDetail)
            {
                urpchd.state = intResult;
                urpchd.ConfirmDate = DateTime.Now;
            }
            db.SaveChanges();
            //檢查覆核完成條件
            if (urpchSn.HasValue)
            {
                uRolePowerConfirmHistory urpchCk = db.uRolePowerConfirmHistory.Where(n => n.urpchSn == urpchSn.Value).First();
                uRolePowerConfirm urpcCk = db.uRolePowerConfirm.Where(n => n.ucrSn == urpchCk.uRolePowerConfirmSn).First();

                if (db.uRolePowerConfirmHistoryDetail.Where(n => n.uRolePowerConfirmHistorySn == urpchCk.urpchSn && n.state == -1).Count() == 0)
                {
                    IEnumerable<uRolePowerConfirmHistoryDetail> urpchdie = db.uRolePowerConfirmHistoryDetail.Where(n => n.uRolePowerConfirmHistorySn == urpchCk.urpchSn && n.state == 1).ToList();
                    List<int> eachRoleNumsRoleIDList = new List<int>();
                    List<int> eachRoleNumsNumsList = new List<int>();
                    int totalNum = 0;
                    foreach (var urpch1 in urpchdie)
                    {
                        if (!eachRoleNumsRoleIDList.Contains(urpch1.ConfirmRoleID))
                        {
                            eachRoleNumsRoleIDList.Add(urpch1.ConfirmRoleID);
                            eachRoleNumsNumsList.Add(1);
                        }
                        else
                        {
                            eachRoleNumsNumsList[eachRoleNumsRoleIDList.IndexOf(urpch1.ConfirmRoleID)]++;
                        }
                        totalNum++;
                    }
                    bool checkEachRoleNums = true;
                    foreach (int ernn in eachRoleNumsNumsList)
                    {
                        if (urpcCk.uEachRoleMinNum.HasValue)
                            if (ernn < urpcCk.uEachRoleMinNum.Value)
                                checkEachRoleNums = false;
                    }
                    bool checkTotalRoleNum = true;
                    if (urpcCk.uTotlaRoleMinNum.HasValue)
                        if (totalNum < urpcCk.uTotlaRoleMinNum.Value)
                            checkTotalRoleNum = false;
                    if (checkEachRoleNums && checkTotalRoleNum)
                    {
                        //此處為一次解鎖操作
                        //urpchCk.state = 1;
                        //db.SaveChanges();

                        //此處為將累積資料執行動作
                        urpchCk.state = 2;
                        urpchCk.endDate = DateTime.Now;
                        db.SaveChanges();
                        //ExecExecConfirmData(urpchCk.urpchSn);

                        //完成覆核並執行結果
                        returnConfirmEndurpchSn = urpchSn.Value;
                    }
                }
                else if (db.uRolePowerConfirmHistoryDetail.Where(n => n.uRolePowerConfirmHistorySn == urpchCk.urpchSn && n.state == -1).Count() > 0)
                {
                    urpchCk.state = -2;
                    urpchCk.endDate = DateTime.Now;
                    db.SaveChanges();
                    //ExecExecConfirmData(urpchCk.urpchSn);
                    //覆核遭否決並執行結果
                    returnConfirmEndurpchSn = urpchSn.Value;
                }
            }
            return returnConfirmEndurpchSn;
        }

        public void AddUserNewsConfirmList(List<int> userNewsSnList, int urpchSn)
        {
            foreach (int userNewsSn in userNewsSnList)
            {
                UserNewsConfirmList uncl = new UserNewsConfirmList
                {
                    UserNewsSn = userNewsSn,
                    uRolePowerConfirmHistorySn = urpchSn
                };
                db.UserNewsConfirmList.Add(uncl);
            }
            db.SaveChanges();
        }

        public void ExecConfirmIsEnd(int urpchSn)
        {
            uRolePowerConfirmHistory urpchCk = db.uRolePowerConfirmHistory.Where(n => n.urpchSn == urpchSn).First();
            IEnumerable<UserNewsConfirmList> unclList = db.UserNewsConfirmList.Where(n => n.uRolePowerConfirmHistorySn == urpchSn).ToList();
            //1.通知欲執行權限者結果
            //2.將所有已覆核/為覆核通知設定結束
            //3.將其通知改為已結束 未讀 附帶defaultAct
            //完成覆核
            int tmpunsSn = unclList.First().UserNewsSn;
            UserNews getOneNews= db.UserNews.Where(n => n.sn == tmpunsSn).FirstOrDefault();           
            string strCResult = urpchCk.state == 2 ? "核准" : "否決";
            string returnUserConfirmResult = "<b>覆核結果為'" + strCResult + "'</b>";            

            string returnUser = getOneNews.fromUser;
            string returnMsgTitle = getOneNews.msgTitle;
            string returnMsgContent= "<div style='border:1px solid black;margin: 2px'>"
                + "<p>" + returnUserConfirmResult + "</p>"
                + "<div style='border:1px solid black;margin: 2px'>"
                + getOneNews.msgContent
                + "</div>"
                + "</div>";
            HubServerMethods returnUserHM = new HubServerMethods();
            returnUserHM.SendMessageToUser(true, returnUser, returnMsgTitle, returnMsgContent, "");

            if (urpchCk.state == 2 || urpchCk.state == -2)
            {
                if (urpchCk.state == 2)
                    ExecExecConfirmData(urpchCk.urpchSn);
                foreach (UserNewsConfirmList uncl in unclList)
                {
                    UserNews un = db.UserNews.Where(n => n.sn == uncl.UserNewsSn).FirstOrDefault();
                    IEnumerable<UserNewsUrls> unuie = db.UserNewsUrls.Where(n => n.UserNewsSn == uncl.UserNewsSn);
                    //設定通知覆核結果
                    un.read = false;
                    //string strCResult = urpchCk.state == 2 ? "核准" : "否決";
                    string confirmResult = "<b>覆核結果為'"+ strCResult+"'，";
                    if (unuie.Count() == 1)
                    {
                        confirmResult += "您已參與此次覆核，您的選擇為'" + unuie.First().UserNewsTitle + "'。";
                    }
                    else
                    {
                        confirmResult += "您未參與此次覆核。";
                    }
                    confirmResult += "</b>";
                    un.msgContent = "<div style='border:1px solid black;margin: 2px'>"
                        + "<p>" + confirmResult + "</p>" 
                        + "<div style='border:1px solid black;margin: 2px'>" 
                        + un.msgContent 
                        + "</div>" 
                        + "</div>";
                    un.noDefaultAction = null;
                    foreach (var unu in unuie)
                    {
                        db.UserNewsUrls.Remove(unu);
                    }
                    
                }
                db.SaveChanges();
            }
        }

        public void SaveExecConfirmData(SaveExecConfirmDataModel cecdm)
        {
            uRolePowerConfirmHistoryConfirmData urpchcd = new uRolePowerConfirmHistoryConfirmData
            {
                state = 0,
                uRolePowerConfirmHistorySn = cecdm.urpchSn,
                cTableName = cecdm.tableName,
                execAction = cecdm.execAction.ToString(),
                primaryColumnName = cecdm.primaryColumnName,
                primaryColumnValue = cecdm.primaryColumnValue,
                primaryColumnType = cecdm.primaryColumnType
            };
            db.uRolePowerConfirmHistoryConfirmData.Add(urpchcd);
            db.SaveChanges();
            if (cecdm.execAction != SaveExecConfirmDataModelActionEnum.Remove)
            {
                if (cecdm.cColumnName.Count() == cecdm.cColumnValue.Count() && cecdm.cColumnName.Count() == cecdm.cColumnType.Count())
                {
                    for (int i = 0; i < cecdm.cColumnName.Count(); i++)
                    {
                        uRolePowerConfirmHistoryConfirmDataDetail urpchcdd = new uRolePowerConfirmHistoryConfirmDataDetail
                        {
                            uRolePowerConfirmHistoryConfirmDataSn = urpchcd.urpchcdSn,
                            cColumnName = cecdm.cColumnName[i],
                            cColumnValue = cecdm.cColumnValue[i],
                            cColumnType = cecdm.cColumnType[i],
                            state = 0
                        };
                        db.uRolePowerConfirmHistoryConfirmDataDetail.Add(urpchcdd);
                    }
                    db.SaveChanges();
                }
            }
        }

        public void SaveExecConfirmData(List<SaveExecConfirmDataModel> cecdmList)
        {
            foreach (SaveExecConfirmDataModel cecdm in cecdmList)
            {
                uRolePowerConfirmHistoryConfirmData urpchcd = new uRolePowerConfirmHistoryConfirmData
                {
                    state = 0,
                    uRolePowerConfirmHistorySn = cecdm.urpchSn,
                    cTableName = cecdm.tableName,
                    execAction = cecdm.execAction.ToString(),
                    primaryColumnName = cecdm.primaryColumnName,
                    primaryColumnValue = cecdm.primaryColumnValue,
                    primaryColumnType = cecdm.primaryColumnType
                };
                db.uRolePowerConfirmHistoryConfirmData.Add(urpchcd);
                db.SaveChanges();
                if (cecdm.execAction != SaveExecConfirmDataModelActionEnum.Remove)
                {
                    if (cecdm.cColumnName.Count() == cecdm.cColumnValue.Count() && cecdm.cColumnName.Count() == cecdm.cColumnType.Count())
                    {
                        for (int i = 0; i < cecdm.cColumnName.Count(); i++)
                        {
                            uRolePowerConfirmHistoryConfirmDataDetail urpchcdd = new uRolePowerConfirmHistoryConfirmDataDetail
                            {
                                uRolePowerConfirmHistoryConfirmDataSn = urpchcd.urpchcdSn,
                                cColumnName = cecdm.cColumnName[i],
                                cColumnValue = cecdm.cColumnValue[i],
                                cColumnType = cecdm.cColumnType[i],
                                state = 0
                            };
                            db.uRolePowerConfirmHistoryConfirmDataDetail.Add(urpchcdd);
                        }
                        db.SaveChanges();
                    }
                }
            }
        }

        public void ExecExecConfirmData(int urpchSn)
        {
            IEnumerable<uRolePowerConfirmHistoryConfirmData> urpchcdie = db.uRolePowerConfirmHistoryConfirmData.Where(n => n.uRolePowerConfirmHistorySn == urpchSn).ToList();

            foreach (var urpchcd in urpchcdie)
            {
                //Table UserNews
                if (urpchcd.cTableName == "UserNews")
                {
                    ConfirmCRUD<UserNews> crudUserNews = new ConfirmCRUD<UserNews>();
                    if (urpchcd.execAction == SaveExecConfirmDataModelActionEnum.Create.ToString())
                    {
                        UserNews newUserNews = new UserNews();
                        crudUserNews.ExecConfirmAction(db,urpchcd, newUserNews);
                    }
                    else if (urpchcd.execAction == SaveExecConfirmDataModelActionEnum.Update.ToString() ||
                        urpchcd.execAction == SaveExecConfirmDataModelActionEnum.Remove.ToString())
                    {
                        int usn = Convert.ToInt32(urpchcd.primaryColumnValue);
                        UserNews newUserNews = db.UserNews.Where(n => n.sn == usn).FirstOrDefault();
                        crudUserNews.ExecConfirmAction(db, urpchcd, newUserNews);
                    }
                    db.SaveChanges();
                }

                //Table UserNewsUrls
                if (urpchcd.cTableName == "UserNewsUrls")
                {
                    ConfirmCRUD<UserNewsUrls> crudUserNewsUrls = new ConfirmCRUD<UserNewsUrls>();
                    if (urpchcd.execAction == SaveExecConfirmDataModelActionEnum.Create.ToString())
                    {
                        UserNewsUrls newUserNewsUrls = new UserNewsUrls();
                        crudUserNewsUrls.ExecConfirmAction(db, urpchcd, newUserNewsUrls);
                    }
                    else if (urpchcd.execAction == SaveExecConfirmDataModelActionEnum.Update.ToString() ||
                        urpchcd.execAction == SaveExecConfirmDataModelActionEnum.Remove.ToString())
                    {
                        int usn = Convert.ToInt32(urpchcd.primaryColumnValue);
                        UserNewsUrls newUserNewsUrls = db.UserNewsUrls.Where(n => n.UserNewsUrlsID == usn).FirstOrDefault();
                        crudUserNewsUrls.ExecConfirmAction(db, urpchcd, newUserNewsUrls);
                    }
                    db.SaveChanges();
                }

                //Table UserNewsUrls
                if (urpchcd.cTableName == "AspNetUsers")
                {
                    ConfirmCRUD<AspNetUsers> crudAspNetUserss = new ConfirmCRUD<AspNetUsers>();
                    if (urpchcd.execAction == SaveExecConfirmDataModelActionEnum.Create.ToString())
                    {
                        AspNetUsers newAspNetUsers = new AspNetUsers();
                        crudAspNetUserss.ExecConfirmAction(db, urpchcd, newAspNetUsers);
                    }
                    else if (urpchcd.execAction == SaveExecConfirmDataModelActionEnum.Update.ToString() ||
                        urpchcd.execAction == SaveExecConfirmDataModelActionEnum.Remove.ToString())
                    {
                        string usn = urpchcd.primaryColumnValue;
                        AspNetUsers newAspNetUsers = db.AspNetUsers.Where(n => n.Id == usn).FirstOrDefault();
                        crudAspNetUserss.ExecConfirmAction(db,urpchcd, newAspNetUsers);
                    }
                    db.SaveChanges();
                }
            }
        }


    }

    public class uRolePowerConfirmDecide
    {
        public uRolePowerConfirm uRolePowerConfirm { get; set; }
        public int roleLevel { get; set; }
        public int roleNums { get; set; }
        public int? firstSurRoleID { get; set; }
        public DateTime roleRegisteredDate { get; set; }
        public int uRoleID { get; set; }
    }

    public class SaveExecConfirmDataModel
    {
        public int urpchSn { get; set; }
        public string tableName { get; set; }
        public string primaryColumnName { get; set; }
        public string primaryColumnValue { get; set; }
        public SaveExecConfirmDataModelActionEnum execAction { get; set; }
        public string primaryColumnType { get; set; }
        public List<string> cColumnName { get; set; }
        public List<string> cColumnValue { get; set; }
        public List<string> cColumnType { get; set; }
    }

    public enum SaveExecConfirmDataModelActionEnum
    {
        Create, Update, Remove
    }

    public class ConfirmCRUD<T> where T : class
    {
        //MSIT116APEXEntities db = new MSIT116APEXEntities();

        public void ExecConfirmAction(MSIT116APEXEntities db, uRolePowerConfirmHistoryConfirmData urpchcd, T _entity)
        {
            IEnumerable<uRolePowerConfirmHistoryConfirmDataDetail> urpchcdds = db.uRolePowerConfirmHistoryConfirmDataDetail.Where(n => n.uRolePowerConfirmHistoryConfirmDataSn == urpchcd.urpchcdSn).ToList();
            if (urpchcd.execAction == SaveExecConfirmDataModelActionEnum.Create.ToString())
            {
                UseConfirmDataToAdd(db,urpchcdds, _entity);
            }
            if (urpchcd.execAction == SaveExecConfirmDataModelActionEnum.Remove.ToString())
            {
                db.Entry(_entity).State = EntityState.Deleted;
                urpchcd.state = 1;
                foreach (var urpchcdd in urpchcdds)
                {
                    urpchcdd.state = 1;
                }
                db.SaveChanges();
            }
            if (urpchcd.execAction == SaveExecConfirmDataModelActionEnum.Update.ToString())
            {
                UseConfirmDataToUpdate(db, urpchcdds, _entity);                
            }
        }

        public void UseConfirmDataToAdd(MSIT116APEXEntities db, IEnumerable<uRolePowerConfirmHistoryConfirmDataDetail> urpchcdds, T _entity)
        {
            db.Entry(_entity).State = EntityState.Added;
            DbEntityEntry<T> entry = db.Entry(_entity);
            foreach (var property in entry.CurrentValues.PropertyNames)
            {
                //var current = entry.CurrentValues.GetValue<object>(property);
                //if (current == null)
                //    entry.Property(property).IsModified = false;
                foreach (var urpchcdd in urpchcdds)
                {
                    if (urpchcdd.cColumnName == property)
                    {
                        entry.Property(property).CurrentValue = Convert.ChangeType(urpchcdd.cColumnValue, Type.GetType(urpchcdd.cColumnType));
                        urpchcdd.state = 1;
                    }
                }
            }
            try
            {
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void UseConfirmDataToUpdate(MSIT116APEXEntities db, IEnumerable<uRolePowerConfirmHistoryConfirmDataDetail> urpchcdds, T _entity)
        {
            db.Entry(_entity).State = EntityState.Modified;
            DbEntityEntry<T> entry = db.Entry(_entity);
            foreach (var property in entry.CurrentValues.PropertyNames)
            {
                //var current = entry.CurrentValues.GetValue<object>(property);
                //if (current == null)
                //    entry.Property(property).IsModified = false;
                foreach (var urpchcdd in urpchcdds)
                {
                    if (urpchcdd.cColumnName == property)
                    {
                        if (urpchcdd.cColumnType == "Byte[]")
                        {
                            string[] ptmps = urpchcdd.cColumnValue.Remove(urpchcdd.cColumnValue.Length - 1, 1).Split(',');
                            byte[] pbtmp = new byte[ptmps.Length];
                            for(int i=0;i< ptmps.Length;i++)
                            {
                                pbtmp[i] = Convert.ToByte(ptmps[i]);
                            }
                            entry.Property(property).CurrentValue = pbtmp;
                        }
                        else
                            entry.Property(property).CurrentValue = Convert.ChangeType(urpchcdd.cColumnValue, Type.GetType(urpchcdd.cColumnType));
                        urpchcdd.state = 1;
                    }
                }
            }
            try
            {
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}