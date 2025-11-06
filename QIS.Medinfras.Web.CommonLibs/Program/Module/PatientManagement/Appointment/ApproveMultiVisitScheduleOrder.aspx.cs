using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.CommonLibs.MasterPage;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class ApproveMultiVisitScheduleOrder : BasePageTrx
    {
        protected int PageCount = 1;
        protected string filterExpressionLocation = "";
        protected string filterExpressionLocationTo = "";
        
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.MedicalDiagnostic.APPROVE_MULTI_VISIT_SCHEDULE_ORDER;
        }

        protected string DateTimeNowDatePicker()
        {
            return DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
        }

        protected string DateTimeNowAddedDatePicker()
        {
            return DateTime.Now.AddDays(7).ToString(Constant.FormatString.DATE_PICKER_FORMAT);
        }

        protected void grdViewDt_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                GetApprovalMultiVisitScheduleOrder entity = e.Row.DataItem as GetApprovalMultiVisitScheduleOrder;
                TextBox txtStartScheduleDate = e.Row.FindControl("txtStartScheduleDate") as TextBox;
                TextBox txtEndScheduleDate = e.Row.FindControl("txtEndScheduleDate") as TextBox;
                TextBox txtInterval = e.Row.FindControl("txtInterval") as TextBox;
                TextBox txtItemQty = e.Row.FindControl("txtItemQty") as TextBox;
                txtStartScheduleDate.Text = entity.TestOrderDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                txtEndScheduleDate.Text = entity.TestOrderDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                txtInterval.Text = 1.ToString();
                txtItemQty.Text = entity.cfItemQtyInString;
            }
        }

        private GetUserMenuAccess menu;

        protected String GetMenuCaption()
        {
            if (menu != null)
                return GetLabel(menu.MenuCaption);
            return "";
        }

        public override bool IsShowRightPanel()
        {
            return false;
        }

        public override void SetCRUDMode(ref bool IsAllowAdd, ref bool IsAllowEdit, ref bool IsAllowDelete)
        {
            IsAllowAdd = IsAllowEdit = IsAllowDelete = false;
        }

        public override void SetToolbarVisibility(ref bool IsAllowAdd, ref bool IsAllowSave, ref bool IsAllowVoid, ref bool IsAllowNextPrev)
        {
            IsAllowAdd = false;
            IsAllowSave = false;
        }
        protected override void InitializeDataControl()
        {
            if (lstOrderMultiVisit != null)
            {
                lstOrderMultiVisit.Clear();
            }
            MPTrx master = (MPTrx)Master;
            menu = ((MPMain)master.Master).ListMenu.FirstOrDefault(p => p.MenuCode == OnGetMenuCode());

            txtFromOrderDate.Text = DateTime.Now.AddMonths(-1).ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtToOrderDate.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);

            List<vHealthcareServiceUnitCustom> lstHSU = BusinessLayer.GetvHealthcareServiceUnitCustomList(string.Format("DepartmentID = '{0}' AND IsDeleted = 0 AND IsAllowMultiVisitSchedule = 1", Constant.Facility.DIAGNOSTIC));
            Methods.SetComboBoxField<vHealthcareServiceUnitCustom>(cboServiceUnit, lstHSU, "ServiceUnitName", "HealthcareServiceUnitID");
            cboServiceUnit.SelectedIndex = 0;

            BindGridView(1, true, ref PageCount);
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            lstOrderMultiVisit = new List<GetApprovalMultiVisitScheduleOrder>();
            string periodeDate = string.Format("{0};{1}", Helper.GetDatePickerValue(txtFromOrderDate.Text).ToString(Constant.FormatString.DATE_FORMAT_112), Helper.GetDatePickerValue(txtToOrderDate).ToString(Constant.FormatString.DATE_FORMAT_112));
            lstOrderMultiVisit = BusinessLayer.GetApprovalMultiVisitScheduleOrder(periodeDate, Convert.ToInt32(cboServiceUnit.Value.ToString()));

            if (isCountPageCount)
            {
                int rowCount = lstOrderMultiVisit.GroupBy(g => g.TestOrderID).Select(s => s.FirstOrDefault()).ToList().Count;
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_PATIENT_LIST);
            }

            grdView.DataSource = lstOrderMultiVisit.GroupBy(g => g.TestOrderID).Select(s => s.FirstOrDefault()).OrderBy(o => o.TestOrderNo).ToList();
            grdView.DataBind();
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            string errMessage = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridView(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else if (param[0] == "add")
                {
                    result = "add|";
                    int orderHdID = Convert.ToInt32(param[1]);
                    int itemId = Convert.ToInt32(param[2]);
                    string[] resultInfo = OnAddDetailOrder(orderHdID, itemId, ref errMessage).Split('|');
                    if (resultInfo[0] == "1")
                    {
                        result += "success";
                    }
                    else
                    {
                        result += "fail";
                    }
                    BindGridView(1, false, ref pageCount);
                }
                else if (param[0] == "edit")
                {
                    result = "edit|";
                    int orderHdID = Convert.ToInt32(param[1]);
                    int orderDtId = Convert.ToInt32(param[2]);
                    int itemId = Convert.ToInt32(param[3]);
                    string[] resultInfo = OnEditDetailOrder(orderHdID, orderDtId, itemId, ref errMessage).Split('|');
                    if (resultInfo[0] == "1")
                    {
                        result += "success";
                    }
                    else
                    {
                        result += "fail";
                    }
                    BindGridView(1, false, ref pageCount);
                }
                else if (param[0] == "delete")
                {
                    result = "delete|";
                    int orderDtId = Convert.ToInt32(param[1]);
                    string[] resultInfo = OnDeleteDetailOrder(orderDtId, ref errMessage).Split('|');
                    if (resultInfo[0] == "1")
                    {
                        result += "success";
                    }
                    else
                    {
                        result += "fail";
                    }
                    BindGridView(1, false, ref pageCount);
                }
                else if (param[0] == "approve")
                {
                    result = "approve|";
                    string[] resultInfo = OnApproveOrder(param[1], ref errMessage).Split('|');
                    if (resultInfo[0] == "1")
                    {
                        result += "success";
                    }
                    else
                    {
                        result += "fail";
                    }
                    BindGridView(1, false, ref pageCount);
                }
                else if (param[0] == "decline")
                {
                    result = "decline|";
                    string[] resultInfo = OnDeclineOrder(param[1], ref errMessage).Split('|');
                    if (resultInfo[0] == "1")
                    {
                        result += "success";
                    }
                    else
                    {
                        result += "fail";
                    }
                    BindGridView(1, false, ref pageCount);
                }
                else // refresh
                {
                    BindGridView(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private string OnApproveOrder(string param, ref string errMessage)
        {
            string result = "1|";

            string orderHdID = param.Split('^')[0];
            string[] lstParam = param.Split('^')[1].Split('&');

            TestOrderHd orderHd = BusinessLayer.GetTestOrderHd(Convert.ToInt32(orderHdID));
            List<TestOrderDt> lstOrderDt = BusinessLayer.GetTestOrderDtList(string.Format("TestOrderID = {0} AND GCTestOrderStatus = '{1}' AND IsDeleted = 0", orderHdID, Constant.TestOrderStatus.OPEN));

            IDbContext ctx = DbFactory.Configure(true);
            TestOrderHdDao orderHdDao = new TestOrderHdDao(ctx);
            TestOrderDtDao orderDtDao = new TestOrderDtDao(ctx);
            DiagnosticVisitScheduleDao visitScheduleDao = new DiagnosticVisitScheduleDao(ctx);

            try
            {
                for (int i = 0; i < lstParam.Length; i++)
                {
                    if (!string.IsNullOrEmpty(lstParam[i]))
                    {
                        string[] paramDetail = lstParam[i].Split('~');
                        int orderDtID = Convert.ToInt32(paramDetail[0]);
                        DateTime startDate = Helper.GetDatePickerValue(paramDetail[2]);
                        DateTime endDate = Helper.GetDatePickerValue(paramDetail[3]);
                        int interval = Convert.ToInt32(paramDetail[4]);

                        int qty = Convert.ToInt32(paramDetail[1]);
                        //int qty = Convert.ToInt32(((endDate - startDate).TotalDays + 1) / interval);

                        #region Update Test Order Hd
                        orderHd.RealizationDate = DateTime.Now;
                        orderHd.RealizationTime = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
                        orderHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                        orderHd.LastUpdatedDate = DateTime.Now;
                        orderHdDao.Update(orderHd);
                        #endregion

                        #region Update Test Order Dt
                        TestOrderDt orderDt = lstOrderDt.Where(w => w.ID == orderDtID).FirstOrDefault();
                        if (orderDt != null)
                        {
                            orderDt.IntervalByDay = interval;
                            orderDt.StartDateSchedule = startDate;
                            orderDt.EndDateSchedule = endDate;
                            orderDt.GCTestOrderStatus = Constant.TestOrderStatus.COMPLETED;
                            orderDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                            orderDt.LastUpdatedDate = DateTime.Now;

                            orderDtDao.Update(orderDt);
                        }
                        #endregion

                        #region Insert Diagnostic Visit Schedule
                        if (qty > 0)
                        {
                            DateTime date = new DateTime();
                            int intervalTotal = interval;
                            for (int a = 1; a <= qty; a++)
                            {
                                DiagnosticVisitSchedule schedule = new DiagnosticVisitSchedule();
                                schedule.TestOrderID = Convert.ToInt32(orderHdID);
                                schedule.ItemID = orderDt.ItemID;
                                schedule.SequenceNo = a.ToString();
                                schedule.GCDiagnosticScheduleStatus = Constant.DiagnosticVisitScheduleStatus.OPEN;
                                //if (a == 1)
                                //{
                                //    schedule.ScheduledDate = startDate;
                                //    date = startDate;
                                //}
                                //else if (a > 1)
                                //{
                                //    if (endDate >= date.AddDays(interval))
                                //    {
                                //        schedule.ScheduledDate = date.AddDays(interval);
                                //        date = date.AddDays(interval);
                                //    }
                                //    else
                                //    {
                                //        errMessage = "Melebihi jadwal rencana multi kunjungan";
                                //        ctx.RollBackTransaction();
                                //        break;
                                //    }
                                //}
                                schedule.CreatedBy = AppSession.UserLogin.UserID;
                                schedule.CreatedDate = DateTime.Now;

                                visitScheduleDao.Insert(schedule);

                                GetApprovalMultiVisitScheduleOrder orderRemove = lstOrderMultiVisit.Where(w => w.TestOrderID == orderDt.TestOrderID && w.TestOrderDtID == orderDt.ID).FirstOrDefault();
                                if (orderRemove != null)
                                {
                                    lstOrderMultiVisit.Remove(orderRemove);
                                }
                            }
                        }
                        #endregion
                    }
                }

                ctx.CommitTransaction();
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;

                result = "0|" + errMessage;

                ctx.RollBackTransaction();
            }
            finally
            {
                ctx.Close();
            }

            return result;
        }

        private string OnDeclineOrder(string param, ref string errMessage)
        {
            string result = "1|";

            TestOrderHd entityHd = BusinessLayer.GetTestOrderHd(Convert.ToInt32(param));

            IDbContext ctx = DbFactory.Configure(true);
            TestOrderHdDao entityHdDao = new TestOrderHdDao(ctx);
            
            try
            {
                entityHd.GCOrderStatus = Constant.TestOrderStatus.RECEIVED;
                entityHd.GCTransactionStatus = Constant.TransactionStatus.WAIT_FOR_APPROVAL;
                entityHd.IsMultiVisitScheduleOrder = false;
                entityHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                entityHd.LastUpdatedDate = DateTime.Now;

                entityHdDao.Update(entityHd);

                ctx.CommitTransaction();
            }
            catch (Exception ex)
            {
                Helper.InsertErrorLog(ex);
                errMessage = ex.Message;

                ctx.RollBackTransaction();

                result = "0|" + ex.Message;
            }
            finally
            {
                ctx.Close();
            }

            return result;
        }

        private string OnAddDetailOrder(int orderHdID, int itemID, ref string errMessage)
        {
            string result = "1|";

            TestOrderHd orderHd = BusinessLayer.GetTestOrderHd(orderHdID);
            ItemMaster entityIM = BusinessLayer.GetItemMaster(itemID);

            if (orderHd != null)
            {
                IDbContext ctx = DbFactory.Configure(true);
                TestOrderDtDao entityDao = new TestOrderDtDao(ctx);
                DiagnosticVisitScheduleDao visitScheduleDao = new DiagnosticVisitScheduleDao(ctx);

                try
                {
                    TestOrderDt entity = new TestOrderDt();
                    entity.TestOrderID = orderHdID;
                    entity.ItemID = itemID;
                    entity.GCTestOrderStatus = Constant.OrderStatus.OPEN;
                    entity.ItemQty = 1;
                    entity.ItemUnit = entityIM.GCItemUnit;
                    entity.Remarks = orderHd.Remarks;
                    entity.IsDeleted = false;
                    entity.CreatedBy = AppSession.UserLogin.UserID;
                    entity.CreatedDate = DateTime.Now;
                    entity.ID = entityDao.InsertReturnPrimaryKeyID(entity);

                    ctx.CommitTransaction();

                    GetApprovalMultiVisitScheduleOrder orderAdd = new GetApprovalMultiVisitScheduleOrder();
                    orderAdd.TestOrderID = orderHdID;
                    orderAdd.TransactionCode = orderHd.TransactionCode;
                    orderAdd.TestOrderNo = orderHd.TestOrderNo;
                    orderAdd.TestOrderDate = orderHd.TestOrderDate;
                    orderAdd.FromVisitID = orderHd.VisitID;
                    orderAdd.TestOrderDtID = entity.ID;
                    orderAdd.ItemID = itemID;
                    orderAdd.ItemCode = entityIM.ItemCode;
                    orderAdd.ItemName1 = entityIM.ItemName1;
                    orderAdd.ItemQty = 1;
                    lstOrderMultiVisit.Add(orderAdd);
                }
                catch (Exception ex)
                {
                    errMessage = ex.Message;

                    result = "0|" + errMessage;

                    ctx.RollBackTransaction();
                }
                finally
                {
                    ctx.Close();
                }
            }
            else
            {
                errMessage = "Order tidak ditemukan";
                result = "0|" + errMessage;
            }

            return result;
        }

        private string OnEditDetailOrder(int orderHdID, int testOrderDtID, int itemID, ref string errMessage)
        {
            string result = "1|";

            TestOrderHd orderHd = BusinessLayer.GetTestOrderHd(orderHdID);

            if (orderHd != null)
            {
                IDbContext ctx = DbFactory.Configure(true);
                try
                {
                    TestOrderDt orderDt = BusinessLayer.GetTestOrderDt(testOrderDtID);
                    orderDt.ItemID = Convert.ToInt32(itemID);
                    orderDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                    orderDt.LastUpdatedDate = DateTime.Now;
                    BusinessLayer.UpdateTestOrderDt(orderDt);
                }
                catch (Exception ex)
                {
                    errMessage = ex.Message;

                    result = "0|" + errMessage;

                    ctx.RollBackTransaction();
                }
                finally
                {
                    ctx.Close();
                }
            }
            else
            {
                errMessage = "Order tidak ditemukan";
                result = "0|" + errMessage;
            }

            return result;
        }

        private string OnDeleteDetailOrder(int testOrderDtID, ref string errMessage)
        {
            string result = "1|";

            TestOrderDt entityDt = BusinessLayer.GetTestOrderDt(testOrderDtID);

            if (entityDt != null)
            {
                IDbContext ctx = DbFactory.Configure(true);
                TestOrderDtDao entityDao = new TestOrderDtDao(ctx);
                DiagnosticVisitScheduleDao visitScheduleDao = new DiagnosticVisitScheduleDao(ctx);

                try
                {
                    entityDt.IsDeleted = true;
                    entityDt.GCTestOrderStatus = Constant.TestOrderStatus.CANCELLED;
                    entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entityDt.LastUpdatedDate = DateTime.Now;
                    entityDao.Update(entityDt);

                    ctx.CommitTransaction();

                    GetApprovalMultiVisitScheduleOrder orderDelete = lstOrderMultiVisit.Where(w => w.TestOrderDtID == testOrderDtID).FirstOrDefault();
                    if (orderDelete != null)
                    {
                        lstOrderMultiVisit.Remove(orderDelete);
                    }
                }
                catch (Exception ex)
                {
                    errMessage = ex.Message;

                    result = "0|" + errMessage;

                    ctx.RollBackTransaction();
                }
                finally
                {
                    ctx.Close();
                }
            }
            else
            {
                errMessage = "Order tidak ditemukan";
                result = "0|" + errMessage;
            }

            return result;
        }

        #region List Order Detail
        private void BindGridViewDt(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            grdViewDt.DataSource = lstOrderMultiVisit.Where(w => w.TestOrderID == Convert.ToInt32(hdnID.Value)).ToList();
            grdViewDt.DataBind();
        }
        protected void cbpViewDt_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridViewDt(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else // refresh
                {

                    BindGridViewDt(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }
        #endregion

        public List<GetApprovalMultiVisitScheduleOrder> lstOrderMultiVisit
        {
            get
            {
                if (Session["__lstOrderMultiVisit"] == null)
                    Session["__lstOrderMultiVisit"] = new List<GetApprovalMultiVisitScheduleOrder>();

                return (List<GetApprovalMultiVisitScheduleOrder>)Session["__lstOrderMultiVisit"];
            }
            set { Session["__lstOrderMultiVisit"] = value; }
        }
    }
}