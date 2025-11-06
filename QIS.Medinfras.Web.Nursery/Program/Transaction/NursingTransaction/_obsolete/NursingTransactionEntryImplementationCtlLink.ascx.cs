using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Web.Common;
using System.Web.UI.HtmlControls;

namespace QIS.Medinfras.Web.Nursing.Program
{
    public partial class NursingTransactionEntryImplementationCtlLink : BaseUserControlCtl
    {

        private NursingTransactionEntryLink MainPage
        {
            get { return (NursingTransactionEntryLink)Page; }
        }

        //protected override void OnLoad(EventArgs e)
        //{
        //    //BindGridView();
        //    //BindGridViewDiagnoseItem();

        //    //Helper.SetControlEntrySetting(txtJournalDate, new ControlEntrySetting(true, true, true, DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT)), "mpTrx");
            
        //    base.OnLoad(e);
        //}

        protected string OnGetDiagnoseFilterExpression()
        {
            return String.Format("IsDeleted = 0");
        }

        protected string OnGetDateTimeNowDate()
        {
            return DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
        }

        protected string OnGetDateTimeNowTime()
        {
            return DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
        }

        private void BindGridView()
        {
            string filterExpression = String.Empty;
            if (MainPage.GetNursingTransactionID() != "" && MainPage.GetNursingTransactionID() != "0")
                filterExpression = String.Format("TransactionID = {0}",MainPage.GetNursingTransactionID());
            else
                filterExpression = "1 = 0";
            List<vNursingTransactionInterventionDt> lstEntity = BusinessLayer.GetvNursingTransactionInterventionDtList(filterExpression);
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

        private void BindGridViewDiagnoseItem()
        {
            string filterExpression = String.Empty;
            if (hdnID.Value != "")
                filterExpression = String.Format("NursingTransactionInterventionDtID = {0}",hdnID.Value);
            else
                filterExpression = "1 = 0";
            List<vNursingJournal> lstEntity = BusinessLayer.GetvNursingJournalList(filterExpression);
            grdView1.DataSource = lstEntity;
            grdView1.DataBind();

        }

        protected void grdView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                vNursingTransactionInterventionDt entity = e.Row.DataItem as vNursingTransactionInterventionDt;

                Label lblTotalSelected = (Label)e.Row.FindControl("lblTotalSelected");
                string filterExpression = String.Empty;
                if (entity.ID != 0)
                    filterExpression = String.Format("NursingTransactionInterventionDtID = {0} AND IsDeleted = 0", entity.ID);
                else
                    filterExpression = "1 = 0";
                int count = BusinessLayer.GetNursingJournalList(filterExpression).Count;
                lblTotalSelected.Text = count.ToString();
            }
        }

        #region Callback
        protected void cbpViewImplementation_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');

                BindGridView();
                result = "refresh|" + pageCount;
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        protected void cbpViewImplementation1_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "refresh")
                {
                    BindGridViewDiagnoseItem();
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        protected void cbpProcess_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errMessage = "";
            int OrderID = 0;
            string[] param = e.Parameter.Split('|');
            result = param[0] + "|";
            if (param[0] == "save")
            {
                if (hdnEntryID.Value.ToString() != "")
                {
                    if (OnSaveEditRecordEntityDt(ref errMessage))
                        result += "success";
                    else
                        result += string.Format("fail|{0}", errMessage);
                }
                else
                {
                    if (OnSaveAddRecordEntityDt(ref errMessage, ref OrderID))
                        result += "success";
                    else
                        result += string.Format("fail|{0}", errMessage);
                }
            }
            else if (param[0] == "delete")
            {
                OrderID = Convert.ToInt32(hdnEntryID.Value);
                if (OnDeleteEntityDt(ref errMessage, OrderID))
                    result += "success";
                else
                    result += string.Format("fail|{0}", errMessage);
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
            panel.JSProperties["cpNutritionOrderID"] = OrderID.ToString();
        }
        #endregion

        #region Process Detail
        private void ControlToEntity(NursingJournal entityDt)
        {
            entityDt.VisitID = 0;
            entityDt.HealthcareServiceUnitID = 0;
            entityDt.JournalDate = Helper.GetDatePickerValue(Request.Form[txtJournalDate.UniqueID]);
            entityDt.JournalTime = txtJournalTime.Text;
            entityDt.Remarks = txtRemarks.Text;
            
        }
        private bool OnSaveAddRecordEntityDt(ref string errMessage, ref int NutritionOrderID)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            NursingJournalDao entityDtDao = new NursingJournalDao(ctx);
            try
            {
                NursingJournal entityDt = new NursingJournal();
                ControlToEntity(entityDt);
                ParamedicMaster medic = BusinessLayer.GetParamedicMasterList(String.Format("ParamedicCode = '{0}'", MainPage.GetDefaultParamedicID())).FirstOrDefault();
                entityDt.ParamedicID = medic.ParamedicID;
                entityDt.NursingTransactionInterventionDtID = Convert.ToInt32(hdnID.Value);
                entityDt.LinkField = String.Format("{0}|{1}", MainPage.GetVisitID(), MainPage.GetHealthcareServiceUnitID());
                entityDt.IsDeleted = false;
                entityDt.CreatedBy = AppSession.UserLogin.UserID;
                entityDtDao.Insert(entityDt);
                ctx.CommitTransaction();
            }
            catch (Exception ex)
            {
                result = false;
                errMessage = ex.Message;
                ctx.RollBackTransaction();
            }
            finally
            {
                ctx.Close();
            }
            return result;
        }
        private bool OnSaveEditRecordEntityDt(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            NursingJournalDao entityDtDao = new NursingJournalDao(ctx);
            try
            {
                NursingJournal entityDt = entityDtDao.Get(Convert.ToInt32(hdnEntryID.Value));
                ControlToEntity(entityDt);
                entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                entityDtDao.Update(entityDt);
                ctx.CommitTransaction();
            }
            catch (Exception ex)
            {
                result = false;
                errMessage = ex.Message;
                ctx.RollBackTransaction();
            }
            finally
            {
                ctx.Close();
            }
            return result;
        }
        private bool OnDeleteEntityDt(ref string errMessage, int ID)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            NursingJournalDao entityDtDao = new NursingJournalDao(ctx);
            try
            {
                NursingJournal entityDt = entityDtDao.Get(Convert.ToInt32(hdnEntryID.Value));
                entityDt.IsDeleted = true;
                entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                entityDtDao.Update(entityDt);
                ctx.CommitTransaction();
            }
            catch (Exception ex)
            {
                ctx.RollBackTransaction();
                errMessage = ex.Message;
                result = false;
            }
            finally
            {
                ctx.Close();
            }
            return result;
        }
        #endregion

        #region Public Function
        public void LoadGridViewByHeaderEntity(Int32 transactionID)
        {
            BindGridView();
            //BindGridViewDiagnoseItem();
        }
        #endregion
    }
}