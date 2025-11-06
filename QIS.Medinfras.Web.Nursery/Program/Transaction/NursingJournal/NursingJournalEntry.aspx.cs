using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Data.Core.Dal;
using System.Data;
using QIS.Medinfras.Web.CommonLibs.Program;
using System.Web.UI.HtmlControls;

namespace QIS.Medinfras.Web.Nursing.Program
{
    public partial class NursingJournalEntry : BasePageTrx
    {
        protected int PageCount = 1;
        protected bool IsEditable = true;
        private string menuType = "";
        public override string OnGetMenuCode()
        {
            string menuCode;
            switch (menuType)
            {
                case "RI": menuCode = Constant.MenuCode.Nursing.NURSING_JOURNAL_INPATIENT; break;
                case "RD": menuCode = Constant.MenuCode.Nursing.NURSING_JOURNAL_EMERGENCY; break;
                default: menuCode = Constant.MenuCode.Nursing.NURSING_JOURNAL_OUTPATIENT; break;
            }
            return menuCode;
        }

        protected string OnGetMenuCaption()
        {
            return BusinessLayer.GetMenuMasterList(String.Format("MenuCode = '{0}'",OnGetMenuCode())).FirstOrDefault().MenuCaption;
        }
        
        protected override void InitializeDataControl()
        {
            if (Page.Request.QueryString.Count > 0)
            {
                hdnVisitID.Value = Page.Request.QueryString["id"].Split('|')[1];

                menuType = Page.Request.QueryString["id"].Split('|')[0];
                string paramedicCode = "";

                vConsultVisit2 entity = BusinessLayer.GetvConsultVisit2List(string.Format("VisitID = {0}", hdnVisitID.Value))[0];
                hdnHealthcareServiceUnitID.Value = entity.HealthcareServiceUnitID.ToString();
                hdnDefaultChargeClassID.Value = entity.ChargeClassID.ToString();
                hdnRegistrationNo.Value = entity.RegistrationNo;
                paramedicCode = entity.ParamedicCode;
                ctlPatientBanner.InitializePatientBanner(entity);
                

                ParamedicMaster medic = BusinessLayer.GetParamedicMasterList(String.Format("ParamedicCode = '{0}'", paramedicCode)).FirstOrDefault();
                hdnParamedicID.Value = medic.ParamedicID.ToString();
                txtParamedicCode.Text = medic.ParamedicCode;
                txtParamedicName.Text = medic.FullName;
                

                BindGridView();

                Helper.SetControlEntrySetting(txtJournalDate, new ControlEntrySetting(true, true, true,DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT)), "mpTrx");
                Helper.SetControlEntrySetting(txtJournalTime, new ControlEntrySetting(true, true, true, DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT)), "mpTrx");
                Helper.SetControlEntrySetting(txtParamedicCode, new ControlEntrySetting(true, true, true), "mpTrx");
                Helper.SetControlEntrySetting(txtRemarks, new ControlEntrySetting(true, true, true), "mpTrx");
            }
        }

        private string GetFilterExpression()
        {
            string filterExpression = "";
            filterExpression = string.Format("VisitID = {0} AND IsDeleted = 0 AND ISNULL(NursingTransactionInterventionDtID,0) = 0", hdnVisitID.Value);
            return filterExpression;
        }

        private void BindGridView()
        {
            string filterExpression = GetFilterExpression();
            List<vNursingJournal> lstEntity = BusinessLayer.GetvNursingJournalList(filterExpression);
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

        protected void grdView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                vNursingJournal entity = e.Row.DataItem as vNursingJournal;
                HtmlTable tblEditDelete = (HtmlTable)e.Row.FindControl("tblEditDelete");
                if (entity.JournalDateInStringDatePickerFormat != DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT) || entity.NursingTransactionInterventionDtID != 0)
                {
                    tblEditDelete.Visible = false;
                }
            }
        }

        protected string OnGetDateTimeNowDate()
        {
            return DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
        }

        protected string OnGetDateTimeNowTime()
        {
            return DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
        }

        #region Process
        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                BindGridView();
                result = "refresh";
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
            menuType = Page.Request.QueryString["id"].Split('|')[0];
            entityDt.VisitID = Convert.ToInt32(hdnVisitID.Value);
            entityDt.HealthcareServiceUnitID = Convert.ToInt32(hdnHealthcareServiceUnitID.Value);
            entityDt.JournalDate = Helper.GetDatePickerValue(Request.Form[txtJournalDate.UniqueID]);
            entityDt.JournalTime = txtJournalTime.Text;
            entityDt.ParamedicID = Convert.ToInt32(hdnParamedicID.Value);
            entityDt.Remarks = txtRemarks.Text;
            entityDt.LinkField = String.Empty;
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
    }
}