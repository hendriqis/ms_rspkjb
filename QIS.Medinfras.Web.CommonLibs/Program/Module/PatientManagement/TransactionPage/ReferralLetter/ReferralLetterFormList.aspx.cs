using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using DevExpress.Web.ASPxCallbackPanel;
using System.Globalization;
using System.Web.UI.HtmlControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Web.CommonLibs.Service;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class ReferralLetterFormList : BasePagePatientPageList
    {
        string menuType = string.Empty;
        string deptType = string.Empty;
        protected int PageCount = 1;

        public override void SetCRUDMode(ref bool IsAllowAdd, ref bool IsAllowEdit, ref bool IsAllowDelete)
        {
            IsAllowAdd = IsAllowEdit = IsAllowDelete = false;
        }

        public override string OnGetMenuCode()
        {
            switch (hdnDepartmentID.Value)
            {
                case Constant.Facility.INPATIENT: return Constant.MenuCode.Inpatient.IP031904;
                case Constant.Facility.EMERGENCY: return Constant.MenuCode.EmergencyCare.ER021904;
                case Constant.Facility.OUTPATIENT: return Constant.MenuCode.Outpatient.OP031804;
                case Constant.Module.MEDICAL_DIAGNOSTIC: return Constant.MenuCode.MedicalDiagnostic.MD035803;
                default: return Constant.MenuCode.EMR.EM09623;
            }
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        protected override void InitializeDataControl()
        {
            hdnCurrentParamedicID.Value = AppSession.UserLogin.ParamedicID.ToString();
            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
            hdnDepartmentID.Value = Page.Request.QueryString["id"];

            BindGridFormView(1, true, ref PageCount);
            //BindGridView(1, true, ref PageCount);
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Format("VisitID = {0} AND StandardCodeID = '{1}' AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID, hdnConsentFormGroup.Value);
            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvRegistrationReferralLetterRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vRegistrationReferralLetter> lstEntity = BusinessLayer.GetvRegistrationReferralLetterList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex);
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

        private string processWordFile(int id)
        {
            MedinfrasAPIService oServis = new MedinfrasAPIService();
            string oRequest = oServis.GenerateReferralLetter(id);
            string[] result = oRequest.Split('|');

            RegistrationReferralLetter data = BusinessLayer.GetRegistrationReferralLetter(id);
            data.PDFStream = result[1];
            data.PrintNumber = data.PrintNumber + 1;
            data.LastPrintedBy = AppSession.UserLogin.UserID;
            data.LastPrintedDate = DateTime.Now;
            BusinessLayer.UpdateRegistrationReferralLetter(data);

            return result[1];
        }

        #region Consent List
        private void BindGridFormView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Format("ParentID = '{0}' AND IsActive = 1 AND IsDeleted = 0 ORDER BY StandardCodeID", Constant.StandardCode.REFERRAL_LETTER);

            List<StandardCode> lstEntity = BusinessLayer.GetStandardCodeList(filterExpression);
            grdFormList.DataSource = lstEntity;
            grdFormList.DataBind();
        }

        protected void cbpFormList_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridFormView(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else // refresh
                {

                    BindGridFormView(1, true, ref pageCount);
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
                else if (param[0] == "view")
                {
                    result = "view|" + processWordFile(Convert.ToInt32(hdnIDForView.Value));
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

        protected void grdView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //if (e.Row.RowType == DataControlRowType.DataRow)
            //{
            //    vPatientConsentForm obj = (vPatientEducationHd)e.Row.DataItem;
            //    Repeater rptPatientEducationDt = (Repeater)e.Row.FindControl("rptPatientEducationDt");
            //    rptPatientEducationDt.DataSource = GetPatientEducationDt(obj.ID);
            //    rptPatientEducationDt.DataBind();
            //}
        }


        protected override bool OnAddRecord(ref string url, ref string errMessage, ref string queryString, ref int popupWidth, ref int popupHeight, ref string popupHeaderText)
        {
            url = ResolveUrl("~/Libs/Program/Module/PatientManagement/TransactionPage/ReferralLetter/ReferralLetterFormCtl.ascx");
            queryString = string.Format("{0}|{1}", hdnConsentFormGroup.Value, hdnID.Value);
            popupWidth = 750;
            popupHeight = 500;
            popupHeaderText = "Informed Consent";
            return true;
        }

        protected override bool OnEditRecord(ref string url, ref string errMessage, ref string queryString, ref int popupWidth, ref int popupHeight, ref string popupHeaderText)
        {
            if (hdnID.Value != "")
            {
                url = ResolveUrl("~/Libs/Program/Module/PatientManagement/TransactionPage/ReferralLetter/ReferralLetterFormCtl.ascx");
                queryString = hdnID.Value;
                popupWidth = 750;
                popupHeight = 500;
                popupHeaderText = "Informed Consent";
                return true;
            }
            return false;
        }

        protected override bool OnDeleteRecord(ref string errMessage)
        {
            return false;
        }

        protected void cbpDelete_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errMessage = "";
            string param = e.Parameter;
            try
            {
                string[] paramInfo = param.Split('|');
                string retVal = DeleteRecord(paramInfo);
                string[] retValInfo = retVal.Split('|');
                if (retValInfo[0] == "1")
                    result += string.Format("success|{0}", string.Empty);
                else
                    result += string.Format("fail|{0}", retValInfo[1]);
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
                result += string.Format("fail|{0}", errMessage);
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
            panel.JSProperties["cpTransactionID"] = param;
        }

        private string DeleteRecord(string[] paramInfo)
        {
            string result = string.Empty;
            IDbContext ctx = DbFactory.Configure(true);
            RegistrationReferralLetterDao entityDao = new RegistrationReferralLetterDao(ctx);

            try
            {
                //Confirm
                int id = Convert.ToInt32(paramInfo[0]);
                RegistrationReferralLetter obj = entityDao.Get(id);
                if (obj != null)
                {
                    obj.IsDeleted = true;
                    obj.LastUpdatedBy = AppSession.UserLogin.UserID;
                    obj.LastUpdatedDate = DateTime.Now;
                    entityDao.Update(obj);

                    result = string.Format("1|{0}|{1}", string.Empty, paramInfo[0]);
                }

                ctx.CommitTransaction();
            }
            catch (Exception ex)
            {
                result = string.Format("0|{0}|{1}", ex.Message, paramInfo[0]);
                ctx.RollBackTransaction();
            }
            finally
            {
                ctx.Close();
            }
            return result;
        }
    }
}