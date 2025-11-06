using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Data.Core.Dal;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.Pharmacy.Program
{
    public partial class MedicationOrderProcess : BasePageTrx
    {
        protected int PageCount = 1;
        private string pageTitle = string.Empty;
        private string[] lstSelectedMember = null;

        protected override void InitializeDataControl()
        {
            BindGridView(1, true, ref PageCount);
        }

        private string GetFilterExpression()
        {
            string filterExpression = "";
           
            return filterExpression;
        }

        protected string GetPageTitle()
        {
            return pageTitle;
        }

        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Pharmacy.PRESCRIPTION_ENTRY_INPATIENT;
        }

        protected override bool OnCustomButtonClick(string type, ref string errMessage)
        {
            bool result = true;
            if (type == "process")
            {
                String[] paramID = hdnSelectedMember.Value.Substring(1).Split('|');
                IDbContext ctx = DbFactory.Configure(true);
                MedicationScheduleDao entityMedicationScheduleDao = new MedicationScheduleDao(ctx);
                PrescriptionOrderDtDao entityPrescriptionOrderDtDao = new PrescriptionOrderDtDao(ctx);
                PrescriptionOrderHdDao entityPrescriptionOrderHdDao = new PrescriptionOrderHdDao(ctx);
                try
                {
                    for (int ct = 0; ct < paramID.Length; ct++)
                    {
                        vPrescriptionOrderDt1 entityPrescriptionOrderDt1 = BusinessLayer.GetvPrescriptionOrderDt1List(string.Format("PrescriptionOrderDetailID = {0}", paramID[ct])).FirstOrDefault();
                        DateTime StartDate = entityPrescriptionOrderDt1.StartDate;
                        for (int i = 0; i < entityPrescriptionOrderDt1.DosingDuration; i++)
                        {
                            DateTime CurrentDate = StartDate.AddDays(i);
                            int SequenceNo = 1;
                            for (int j = 0; j < entityPrescriptionOrderDt1.Frequency; j++)
                            {
                                MedicationSchedule entityMedicationSchedule = new MedicationSchedule();
                                entityMedicationSchedule.VisitID = entityPrescriptionOrderDt1.VisitID;
                                entityMedicationSchedule.PrescriptionOrderID = entityPrescriptionOrderDt1.PrescriptionOrderID;
                                entityMedicationSchedule.PrescriptionOrderDetailID = entityPrescriptionOrderDt1.PrescriptionOrderDetailID;
                                entityMedicationSchedule.ItemID = entityPrescriptionOrderDt1.ItemID;
                                if (!entityPrescriptionOrderDt1.IsCompound)
                                    entityMedicationSchedule.ItemName = entityPrescriptionOrderDt1.DrugName;
                                else
                                    entityMedicationSchedule.ItemName = entityPrescriptionOrderDt1.MedicationLine;
                                entityMedicationSchedule.MedicationDate = CurrentDate;
                                entityMedicationSchedule.SequenceNo = SequenceNo.ToString();
                                entityMedicationSchedule.MedicationTime = "00:00";
                                entityMedicationSchedule.NumberOfDosage = entityPrescriptionOrderDt1.NumberOfDosage;
                                entityMedicationSchedule.NumberOfDosageInString = entityPrescriptionOrderDt1.NumberOfDosageInString;
                                entityMedicationSchedule.GCDosingUnit = entityPrescriptionOrderDt1.GCDosingUnit;
                                entityMedicationSchedule.ConversionFactor = entityPrescriptionOrderDt1.ConversionFactor;
                                entityMedicationSchedule.ResultQuantity = 0;
                                entityMedicationSchedule.ChargeQuantity = 0;
                                entityMedicationSchedule.IsAsRequired = entityPrescriptionOrderDt1.IsAsRequired;
                                entityMedicationSchedule.IsMorning = entityMedicationSchedule.IsNoon = entityMedicationSchedule.IsEvening = entityMedicationSchedule.IsNight = false;
                                if (j == 0) entityMedicationSchedule.IsMorning = true;
                                if (j == 1) entityMedicationSchedule.IsNoon = true;
                                if (j == 2) entityMedicationSchedule.IsEvening = true;
                                if (j == 3) entityMedicationSchedule.IsNight = true;
                                entityMedicationSchedule.GCRoute = entityPrescriptionOrderDt1.GCRoute;
                                entityMedicationSchedule.GCCoenamRule = entityPrescriptionOrderDt1.GCCoenamRule;
                                entityMedicationSchedule.MedicationAdministration = entityPrescriptionOrderDt1.MedicationAdministration;
                                if (entityPrescriptionOrderDt1.IsUsingUDD)
                                    entityMedicationSchedule.GCMedicationStatus = Constant.MedicationStatus.OPEN;
                                else
                                    entityMedicationSchedule.GCMedicationStatus = Constant.MedicationStatus.DIPROSES_FARMASI;
                                entityMedicationSchedule.IsInternalMedication = true;
                                entityMedicationSchedule.CreatedBy = AppSession.UserLogin.UserID;
                                PrescriptionOrderDt entityPrescriptionOrderDt = entityPrescriptionOrderDtDao.Get(entityPrescriptionOrderDt1.PrescriptionOrderDetailID);
                                entityPrescriptionOrderDt.GCPrescriptionOrderStatus = Constant.TestOrderStatus.IN_PROGRESS;
                                PrescriptionOrderHd entityPrescriptionOrderHd = entityPrescriptionOrderHdDao.Get(Convert.ToInt32(entityPrescriptionOrderDt1.PrescriptionOrderID));
                                entityPrescriptionOrderHd.GCTransactionStatus = Constant.TransactionStatus.PROCESSED;
                                entityPrescriptionOrderHd.GCOrderStatus = Constant.TestOrderStatus.IN_PROGRESS;
                                entityPrescriptionOrderHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                                SequenceNo++;
                                entityMedicationScheduleDao.Insert(entityMedicationSchedule);
                                entityPrescriptionOrderHdDao.Update(entityPrescriptionOrderHd);
                                entityPrescriptionOrderDtDao.Update(entityPrescriptionOrderDt);
                            }
                        }
                    }
                    ctx.CommitTransaction();
                }
                catch (Exception ex)
                {
                    errMessage = ex.Message;
                    result = false;
                    ctx.RollBackTransaction();
                }
                finally
                {
                    ctx.Close();
                }
            }
            else if (type=="void")
            {
                string[] param = hdnVoidReason.Value.Split(';');
                String[] detailID = hdnSelectedMember.Value.Substring(1).Split('|');
                string gcDeleteReason = param[0];
                string reason = param[1];

                IDbContext ctx = DbFactory.Configure(true);
                PrescriptionOrderHdDao entityHdDao = new PrescriptionOrderHdDao(ctx);
                PrescriptionOrderDtDao entityDtDao = new PrescriptionOrderDtDao(ctx);
                try
                {
                    for (int ct = 0; ct < detailID.Length; ct++)
                    {
                        PrescriptionOrderDt entityOrderDt = BusinessLayer.GetPrescriptionOrderDt(Convert.ToInt32(detailID[ct]));
                        if (entityOrderDt != null)
                        {
                            entityOrderDt.GCPrescriptionOrderStatus = Constant.TestOrderStatus.CANCELLED;
                            entityOrderDt.GCVoidReason = gcDeleteReason;
                            entityOrderDt.VoidReason = reason;
                            entityOrderDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                            entityDtDao.Update(entityOrderDt);
                        }
                    }
                    int dtOpenCount = BusinessLayer.GetPrescriptionOrderDtRowCount(string.Format("PrescriptionOrderID = {0} AND IsRFlag = 1 AND GCPrescriptionOrderStatus = '{1}' AND IsDeleted = 0", hdnPrescriptionOrderHdID.Value, Constant.TransactionStatus.OPEN), ctx);
                    int dtProcessedCount = BusinessLayer.GetPrescriptionOrderDtRowCount(string.Format("PrescriptionOrderID = {0} AND IsRFlag = 1 AND GCPrescriptionOrderStatus = '{1}' AND IsDeleted = 0", hdnPrescriptionOrderHdID.Value, Constant.TransactionStatus.PROCESSED), ctx);
                    if (dtOpenCount == 0 && dtProcessedCount == 0)
                    {
                        PrescriptionOrderHd orderHd = entityHdDao.Get(Convert.ToInt32(hdnPrescriptionOrderHdID.Value));
                        orderHd.GCTransactionStatus = Constant.TransactionStatus.VOID;
                        orderHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                        entityHdDao.Update(orderHd);
                    }
                    ctx.CommitTransaction();
                }
                catch (Exception ex)
                {
                    errMessage = ex.Message;
                    result = false;
                    ctx.RollBackTransaction();
                }
                finally
                {
                    ctx.Close();
                }
            }
            return result;
        }

        protected void grdViewDt_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                vPrescriptionOrderDt1 entity = e.Row.DataItem as vPrescriptionOrderDt1;
                CheckBox chkIsSelected = (CheckBox)e.Row.FindControl("chkIsSelected");
                if (entity.GCPrescriptionOrderStatus == Constant.TestOrderStatus.OPEN)
                {
                    if (lstSelectedMember.Contains(entity.PrescriptionOrderDetailID.ToString()))
                    {
                        int idx = Array.IndexOf(lstSelectedMember, entity.PrescriptionOrderDetailID.ToString());
                        chkIsSelected.Checked = true;
                    }
                }
                else chkIsSelected.Style.Add("display", "none");
            }
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = "";//hdnFilterExpression.Value;
            if (filterExpression != "")
                filterExpression += " AND ";
            String TransactionStatus = String.Format("'{0}','{1}'", Constant.TransactionStatus.CLOSED, Constant.TransactionStatus.VOID);
            filterExpression += string.Format("VisitID = {0} AND GCTransactionStatus NOT IN ({1}) AND IsOutstandingOrder=1", AppSession.RegisteredPatient.VisitID, TransactionStatus);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvPrescriptionOrderHd1RowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vPrescriptionOrderHd1> lstEntity = BusinessLayer.GetvPrescriptionOrderHd1List(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "PrescriptionOrderID DESC");
            grdView.DataSource = lstEntity;
            grdView.DataBind();

            if (lstEntity.Count == 0)
                hdnPrescriptionOrderHdID.Value = "";
        }

        #region Prescription Dt
        private void BindGridViewDt(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = "1 = 0";
            if (hdnPrescriptionOrderHdID.Value != "")
            {
                filterExpression = string.Format("PrescriptionOrderID = {0} AND OrderIsDeleted = 0", hdnPrescriptionOrderHdID.Value);

                if (isCountPageCount)
                {
                    int rowCount = BusinessLayer.GetvPrescriptionOrderDt1RowCount(filterExpression);
                    pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
                }
            }
            lstSelectedMember = hdnSelectedMember.Value.Split('|');
            List<vPrescriptionOrderDt1> lstEntity = BusinessLayer.GetvPrescriptionOrderDt1List(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "PrescriptionOrderDetailID DESC");
            grdViewDt.DataSource = lstEntity;
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

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridView(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
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
    }
}