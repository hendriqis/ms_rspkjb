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

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class ConsentFormList : BasePagePatientPageList
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
            return Constant.MenuCode.EMR.PATIENT_CONSENT_FORM;
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        protected override void InitializeDataControl()
        {
            hdnCurrentSessionID.Value = AppSession.UserLogin.UserID.ToString();
            hdnCurrentParamedicID.Value = AppSession.UserLogin.ParamedicID.ToString();
            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;

            hdnDepartmentID.Value = AppSession.RegisteredPatient.DepartmentID;

            BindGridFormView(1, true, ref PageCount);
            //BindGridView(1, true, ref PageCount);

        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            int cvLinkedID = 0;
            if (AppSession.RegisteredPatient.DepartmentID == Constant.Facility.INPATIENT)
            {
                vConsultVisit4 entityLinkedRegistration = BusinessLayer.GetvConsultVisit4List(string.Format("RegistrationID = (SELECT LinkedRegistrationID FROM Registration WHERE RegistrationID = {0})", AppSession.RegisteredPatient.RegistrationID)).FirstOrDefault();
                if (entityLinkedRegistration != null)
                {
                    cvLinkedID = entityLinkedRegistration.VisitID;
                }
            }

            string filterExpression = ""; //hdnFilterExpression.Value;
            if (filterExpression != "")
                filterExpression += " AND ";
            filterExpression += string.Format("VisitID IN ({0},{1}) AND GCConsentFormGroup = '{2}' AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID, cvLinkedID, hdnConsentFormGroup.Value);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvConsentFormRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vConsentForm> lstEntity = BusinessLayer.GetvConsentFormList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex);
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

        #region Consent List
        private void BindGridFormView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Format("ParentID = '{0}' AND IsActive = 1 AND IsDeleted = 0 ORDER BY StandardCodeID", Constant.StandardCode.PATIENT_CONSENT_FORM_GROUP);

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
            url = ResolveUrl("~/Libs/Program/Module/PatientManagement/TransactionPage/ConsentForm/ConsentFormCtl1.ascx");
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
                url = ResolveUrl("~/Libs/Program/Module/PatientManagement/TransactionPage/ConsentForm/ConsentFormCtl1.ascx");
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
            ConsentFormDao entityDao = new ConsentFormDao(ctx);

            try
            {
                //Confirm
                int id = Convert.ToInt32(paramInfo[0]);
                ConsentForm obj = entityDao.Get(id);
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