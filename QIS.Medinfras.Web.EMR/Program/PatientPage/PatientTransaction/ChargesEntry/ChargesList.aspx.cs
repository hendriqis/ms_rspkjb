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
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.EMR.Program
{
    public partial class ChargesList : BasePagePatientPageList
    {
        protected int PageCount = 1;
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.EMR.PATIENT_CHARGES_ENTRY;
        }

        public override bool IsEntryUsePopup()
        {
            return false;
        }

        protected override void InitializeDataControl()
        {
            BindGridView(1, true, ref PageCount);
            BindGridViewDt(1, true, ref PageCount);
        }

        #region Patient Charges Hd
        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = hdnFilterExpression.Value;
            if (filterExpression != "")
                filterExpression += " AND ";

            string code = ddlViewType.SelectedValue;

            if (code == "0")
                filterExpression += string.Format("VisitID = {0} AND ParamedicID = {1}",
                  AppSession.RegisteredPatient.VisitID, AppSession.UserLogin.ParamedicID);
            if (code == "1")
                filterExpression += string.Format("VisitID = {0} AND ParamedicID = {1} AND IsEntryByPhysician = 1",
                    AppSession.RegisteredPatient.VisitID, AppSession.UserLogin.ParamedicID);
            else if (code == "2")
                filterExpression += string.Format("VisitID = {0} AND ParamedicID = {1} AND IsEntryByPhysician = 0",
                    AppSession.RegisteredPatient.VisitID, AppSession.UserLogin.ParamedicID);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvPatientChargesHd8RowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vPatientChargesHd8> lstEntity = BusinessLayer.GetvPatientChargesHd8List(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "TransactionDate DESC");
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

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
        #endregion

        #region Patient Charges Dt
        private void BindGridViewDt(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = "1 = 0";
            if (hdnID.Value != "")
            {
                filterExpression = string.Format("TransactionID = {0} AND IsDeleted = 0", hdnID.Value);

                if (isCountPageCount)
                {
                    int rowCount = BusinessLayer.GetvPatientChargesDtRowCount(filterExpression);
                    pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
                }
            }
            List<vPatientChargesDt> lstEntity = BusinessLayer.GetvPatientChargesDtList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "ID ASC");
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

        protected override bool OnBeforeDeleteRecord(ref string errMessage)
        {
            bool result = true;
            try
            {
                PatientChargesHd entity = BusinessLayer.GetPatientChargesHd(Convert.ToInt32(hdnID.Value));
                if (entity.GCTransactionStatus != Constant.TransactionStatus.OPEN)
                {
                    errMessage = "Cannot delete transaction because has already been approved.";
                    result = false;
                }
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                result = false;
            }
            return result;
        }

        protected override bool OnBeforeEditRecord(ref string errMessage)
        {
            bool result = true;
            try
            {
                PatientChargesHd entity = BusinessLayer.GetPatientChargesHd(Convert.ToInt32(hdnID.Value));
                if (entity.GCTransactionStatus != Constant.TransactionStatus.OPEN)
                {
                    errMessage = "Cannot edit transaction because has already been approved.";
                    result = false;
                }
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                result = false;
            }
            return result;
        }

        protected override bool OnAddRecord(ref string url, ref string errMessage)
        {
            url = ResolveUrl("~/Program/PatientPage/PatientTransaction/ChargesEntry/ChargesEntry.aspx");
            return true;
        }

        protected override bool OnEditRecord(ref string url, ref string errMessage)
        {
            //if (hdnID.Value != "")
            //{
            //    url = ResolveUrl(string.Format("~/Program/PatientPage/Planning/TestOrder/TestOrderEntry.aspx?id={0}", hdnID.Value));
            //    return true;
            //}
            errMessage = "This feature is currently under development.";
            return false;
        }

        protected override bool OnDeleteRecord(ref string errMessage)
        {
            if (hdnItemID.Value != "")
            {
                bool result = true;
                int id = Convert.ToInt32(hdnItemID.Value);
                IDbContext ctx = DbFactory.Configure(true);
                PatientChargesDtDao entityDtDao = new PatientChargesDtDao(ctx);
                try
                {
                    PatientChargesDt entityDt = BusinessLayer.GetPatientChargesDtList(string.Format("ID = {0}", id), ctx).FirstOrDefault();
                    if (entityDt != null)
                    {
                        entityDt.GCTransactionDetailStatus = Constant.TransactionStatus.VOID;
                        entityDt.IsDeleted = true;
                        entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                        entityDtDao.Update(entityDt);
                    }
                    ctx.CommitTransaction();
                }
                catch (Exception ex)
                {
                    result = false;
                    errMessage = ex.Message;
                    Helper.InsertErrorLog(ex);
                    ctx.RollBackTransaction();
                }
                finally
                {
                    ctx.Close();
                }
                return result;
            }
            return false;
        }

        protected override bool OnCustomButtonClick(string type, ref string errMessage)
        {
            bool result = true;
            if (type == "Void")
            {
                if (hdnID.Value != "")
                {
                    IDbContext ctx = DbFactory.Configure(true);
                    PatientChargesHdDao entityHdDao = new PatientChargesHdDao(ctx);
                    PatientChargesDtDao entityDtDao = new PatientChargesDtDao(ctx);
                    try
                    {
                        PatientChargesHd entityHd = entityHdDao.Get(Convert.ToInt32(hdnID.Value));
                        entityHd.GCTransactionStatus = Constant.TransactionStatus.VOID;
                        entityHd.VoidBy = AppSession.UserLogin.UserID;
                        entityHd.VoidDate = DateTime.Now;
                        entityHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                        entityHdDao.Update(entityHd);

                        List<PatientChargesDt> lstEntityDt = BusinessLayer.GetPatientChargesDtList(string.Format("TransactionID = {0} AND IsDeleted = 0", entityHd.TransactionID), ctx);
                        foreach (PatientChargesDt entityDt in lstEntityDt)
                        {
                            entityDt.GCTransactionDetailStatus = Constant.TransactionStatus.VOID;
                            entityDt.IsDeleted = true;
                            entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                            entityDtDao.Update(entityDt);
                        }

                        ctx.CommitTransaction();
                    }
                    catch (Exception ex)
                    {
                        result = false;
                        errMessage = ex.Message;
                        Helper.InsertErrorLog(ex);
                        ctx.RollBackTransaction();
                    }
                    finally
                    {
                        ctx.Close();
                    }
                    return result;
                }
                return result;
            }
            return false;
        }
    }
}