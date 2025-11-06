using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using DevExpress.Web.ASPxCallbackPanel;
using DevExpress.Web.ASPxEditors;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class MedicalAssessmentForm : BasePagePatientPageList
    {
        string deptType = string.Empty;
        string menuType = string.Empty;
        protected int PageCount = 1;
        protected List<vVitalSignDt> lstVitalSignDt = null;

        public override string OnGetMenuCode()
        {
            if (menuType == "fo")
            {
                #region Follow-up Pasien Pulang
                switch (deptType)
                {
                    case Constant.Facility.INPATIENT: return Constant.MenuCode.Inpatient.FOLLOWUP_MEDICAL_ASSESSMENT_FORM;
                    default: return Constant.MenuCode.Inpatient.FOLLOWUP_MEDICAL_ASSESSMENT_FORM;
                }
                #endregion
            }
            else
            {
                #region Pasien Dalam Perawatan
                switch (deptType)
                {
                    case Constant.Facility.INPATIENT: return Constant.MenuCode.Inpatient.MEDICAL_ASSESSMENT_FORM;
                    default: return Constant.MenuCode.Inpatient.MEDICAL_ASSESSMENT_FORM;
                }
                #endregion
            }   
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        protected override void InitializeDataControl()
        {
            string[] param = Page.Request.QueryString["id"].Split('|');
            if (param.Count() > 1)
            {
                deptType = param[0];
                menuType = param[1];
            }
            else
            {
                deptType = param[0];
            }

            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
            hdnMRN.Value = AppSession.RegisteredPatient.MRN.ToString();
            hdnDepartmentID.Value = AppSession.RegisteredPatient.DepartmentID;

            txtFromDate.Text = DateTime.Today.AddDays(-1).ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtToDate.Text = DateTime.Today.ToString(Constant.FormatString.DATE_PICKER_FORMAT);

            BindGridView(1, true, ref PageCount);
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Format("ParentID = '{0}' ORDER BY StandardCodeID", Constant.StandardCode.PATIENT_ADDITIONAL_ASSESSMENT_FORM);
            List<StandardCode> lstEntity = BusinessLayer.GetStandardCodeList(filterExpression);
            grdFormList.DataSource = lstEntity;
            grdFormList.DataBind(); 
        }

        private void BindGridViewDt(int pageIndex, bool isCountPageCount, ref int pageCount)
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

            filterExpression += string.Format("VisitID IN ({0},{3}) AND (ObservationDate BETWEEN '{1}' AND '{2}') AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID, Helper.GetDatePickerValue(txtFromDate).ToString(Constant.FormatString.DATE_FORMAT_112), Helper.GetDatePickerValue(txtToDate).ToString(Constant.FormatString.DATE_FORMAT_112), cvLinkedID);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvVitalSignHdRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vVitalSignHd> lstEntity = BusinessLayer.GetvVitalSignHdList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "ID DESC");
            lstVitalSignDt = BusinessLayer.GetvVitalSignDtList(string.Format("VisitID IN ({0},{1}) ORDER BY DisplayOrder", AppSession.RegisteredPatient.VisitID, cvLinkedID));
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

        protected void grdView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                vVitalSignHd obj = (vVitalSignHd)e.Row.DataItem;
                Repeater rptVitalSignDt = (Repeater)e.Row.FindControl("rptVitalSignDt");
                rptVitalSignDt.DataSource = GetVitalSignDt(obj.ID);
                rptVitalSignDt.DataBind();
            }
        }

        protected List<vVitalSignDt> GetVitalSignDt(Int32 ID)
        {
            return lstVitalSignDt.Where(p => p.ID == ID).ToList();
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
                    //BindGridViewHd(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else // refresh
                {

                    //BindGridViewHd(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
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

        protected override bool OnAddRecord(ref string url, ref string errMessage, ref string queryString, ref int popupWidth, ref int popupHeight, ref string popupHeaderText)
        {
            url = ResolveUrl("~/Libs/Program/Module/PatientManagement/TransactionPage/Assessment/MedicalForm/MedicalAssessmentFormEntry.ascx");
            queryString = "";
            popupWidth = 1200;
            popupHeight = 600;
            popupHeaderText = "Formulir Pengkajian Pasien";
            return true;
        }

        protected override bool OnEditRecord(ref string url, ref string errMessage, ref string queryString, ref int popupWidth, ref int popupHeight, ref string popupHeaderText)
        {
            if (hdnID.Value != "")
            {
                url = ResolveUrl("~/Libs/Program/Module/PatientManagement/TransactionPage/Assessment/MedicalForm/MedicalAssessmentFormEntry.ascx");
                queryString = hdnID.Value;
                popupWidth = 1200;
                popupHeight = 600;
                popupHeaderText = "Formulir Pengkajian Pasien";
                return true;
            }
            return false;
        }

        protected override bool OnDeleteRecord(ref string errMessage)
        {
            if (hdnID.Value != "")
            {
                VitalSignHd entity = BusinessLayer.GetVitalSignHd(Convert.ToInt32(hdnID.Value));
                entity.IsDeleted = true;
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateVitalSignHd(entity);
                return true;
            }
            return false;
        }
    }
}