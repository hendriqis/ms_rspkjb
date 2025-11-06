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
using QIS.Data.Core.Dal;
using System.Web.UI.HtmlControls;
using DevExpress.Web.ASPxEditors;
using System.Text;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class RadiotheraphyReportList : BasePagePatientPageList
    {
        protected int PageCount = 1;
        string menuType = string.Empty;
        string deptType = string.Empty;

        public override string OnGetMenuCode()
        {
            if (menuType == "fo")
            {
                #region Follow-up Pasien Pulang
                switch (hdnDepartmentID.Value)
                {
                    case Constant.Facility.OUTPATIENT:
                        return Constant.MenuCode.Outpatient.FOLLOWUP_ANTENATAL_RECORD;
                    case Constant.Facility.EMERGENCY:
                        return Constant.MenuCode.EmergencyCare.FOLLOWUP_ANTENATAL_RECORD;
                    case Constant.Facility.INPATIENT:
                        return Constant.MenuCode.Inpatient.FOLLOWUP_ANTENATAL_RECORD;
                    case Constant.Facility.DIAGNOSTIC:
                        return Constant.MenuCode.MedicalDiagnostic.FOLLOWUP_ANTENATAL_RECORD;
                    default:
                        return Constant.MenuCode.Outpatient.FOLLOWUP_ANTENATAL_RECORD;
                }
                #endregion
            }
            else
            {
                #region Pasien Dalam Perawatan
                switch (hdnDepartmentID.Value)
                {
                    case Constant.Module.EMR: return Constant.MenuCode.EMR.PATIENT_PAGE_RT_RADIOTHERAPHY_REPORT;
                    case Constant.Module.RADIOTHERAPHY: return Constant.MenuCode.Radiotheraphy.PATIENT_PAGE_RT_RADIOTHERAPHY_REPORT;
                    default: return Constant.MenuCode.Radiotheraphy.PATIENT_PAGE_RT_RADIOTHERAPHY_REPORT;
                }
                #endregion
            }
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        public override void SetCRUDMode(ref bool IsAllowAdd, ref bool IsAllowEdit, ref bool IsAllowDelete)
        {
            IsAllowAdd = IsAllowEdit = IsAllowDelete = false;
        }

        protected override void InitializeDataControl()
        {
            if (Page.Request.QueryString["id"] != null)
            {
                string[] param = Page.Request.QueryString["id"].Split('|');
                if (param.Count() > 1)
                {
                    deptType = param[0];
                    menuType = param[1];
                }
                else
                {
                    hdnDepartmentID.Value = Page.Request.QueryString["id"];
                    deptType = param[0];
                }
            }
            else
            {
                hdnDepartmentID.Value = string.Empty;
            }

            hdnCurrentParamedicID.Value = AppSession.UserLogin.ParamedicID.ToString();
            hdnPageVisitID.Value = AppSession.RegisteredPatient.VisitID.ToString();
            hdnPageMRN.Value = AppSession.RegisteredPatient.MRN.ToString();
            hdnPageMedicalNo.Value = AppSession.RegisteredPatient.MedicalNo;
            hdnPagePatientName.Value = AppSession.RegisteredPatient.PatientName;

            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
            BindGridView(1, true, ref PageCount);
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = ""; //hdnFilterExpression.Value;
            if (filterExpression != "")
                filterExpression += " AND ";
            filterExpression += string.Format("MRN = {0} AND  IsDeleted = 0", AppSession.RegisteredPatient.MRN);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvRadiotheraphyProgramRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vRadiotheraphyProgram> lstEntity = BusinessLayer.GetvRadiotheraphyProgramList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex);
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

        protected override bool OnAddRecord(ref string url, ref string errMessage, ref string queryString, ref int popupWidth, ref int popupHeight, ref string popupHeaderText)
        {
            string param = string.Format("{0}|{1}|{2}|{3}|{4}|{5}", AppSession.RegisteredPatient.MRN, AppSession.RegisteredPatient.MedicalNo, AppSession.RegisteredPatient.PatientName, AppSession.RegisteredPatient.VisitID, "radiotheraphyProgram01", "0");
            url = ResolveUrl("~/Libs/Program/Module/PatientManagement/TransactionPage/Radiotheraphy/RadiotheraphyProgramEntry.ascx");
            queryString = param;
            popupWidth = 800;
            popupHeight = 500;
            popupHeaderText = "Program Radioterapi";
            return true;
        }

        protected override bool OnEditRecord(ref string url, ref string errMessage, ref string queryString, ref int popupWidth, ref int popupHeight, ref string popupHeaderText)
        {
            if (hdnProgramID.Value != "")
            {
                string param = string.Format("{0}|{1}|{2}|{3}|{4}|{5}", AppSession.RegisteredPatient.MRN, AppSession.RegisteredPatient.MedicalNo, AppSession.RegisteredPatient.PatientName, AppSession.RegisteredPatient.VisitID, "radiotheraphyProgram01", hdnProgramID.Value);
                url = ResolveUrl("~/Libs/Program/Module/PatientManagement/TransactionPage/Radiotheraphy/RadiotheraphyProgramEntry.ascx");
                queryString = param;
                popupWidth = 800;
                popupHeight = 500;
                popupHeaderText = "Program Radioterapi";
                return true;
            }
            return false;
        }

        protected override bool OnDeleteRecord(ref string errMessage)
        {
            bool result = true;

            if (hdnProgramID.Value != "")
            {
                if (IsValidToDelete(hdnProgramID.Value, ref errMessage))
                {
                    RadiotherapyProgram entity = BusinessLayer.GetRadiotherapyProgram(Convert.ToInt32(hdnProgramID.Value));
                    entity.IsDeleted = true;
                    entity.LastUpdatedDate = DateTime.Now;
                    BusinessLayer.UpdateRadiotherapyProgram(entity);
                    result = true; 
                }
                else
                {
                    result = false;
                }
            }
            else
            {
                result = false;
            }
            return result;
        }

        private bool IsValidToDelete(string ID, ref string errMessage)
        {
            StringBuilder message = new StringBuilder();

            List<vRadiotheraphyProgramLog> lstMeasurement = BusinessLayer.GetvRadiotheraphyProgramLogList(string.Format("ProgramID = {0} AND IsDeleted = 0", ID));
            if (lstMeasurement.Count > 0)
            {
                message.AppendLine("Data Program Penyinaran tidak bisa dihapus karena masih ada informasi log penyinaran yang terdapat dalam program ini");
            }

            errMessage = message.ToString();

            return string.IsNullOrEmpty(errMessage);
        }

        #region Detail List
        private void BindGridViewDt1(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Format("ProgramID = {0} AND IsDeleted = 0", hdnProgramID.Value);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvRadiotherapyProgramReportRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vRadiotherapyProgramReport> lstEntity = BusinessLayer.GetvRadiotherapyProgramReportList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "ProgramReportID DESC");

            grdViewDt1.DataSource = lstEntity;
            grdViewDt1.DataBind();
        }

        protected void cbpViewDt1_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridViewDt1(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else // refresh
                {

                    BindGridViewDt1(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        protected void cbpDeleteReport_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errMessage = "";
            string param = e.Parameter;
            try
            {
                string retVal = DeleteReport(param);
                string[] retValInfo = retVal.Split('|');
                if (retValInfo[0] == "1")
                    result += string.Format("success|{0}", string.Empty);
                else
                    result += string.Format("fail|{0}", retValInfo[1]);
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                result += string.Format("fail|{0}", errMessage);
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
            panel.JSProperties["cpTransactionID"] = param;
        }

        private string DeleteReport(string recordID)
        {
            string result = string.Empty;

            try
            {
                //Confirm
                int id = Convert.ToInt32(recordID);
                RadiotherapyProgramReport obj = BusinessLayer.GetRadiotherapyProgramReport(id);
                if (obj != null)
                {
                    obj.IsDeleted = true;
                    obj.LastUpdatedBy = AppSession.UserLogin.UserID;
                    obj.LastUpdatedDate = DateTime.Now;
                    BusinessLayer.UpdateRadiotherapyProgramReport(obj);
                    result = string.Format("1|{0}", string.Empty);
                }
            }
            catch (Exception ex)
            {
                result = string.Format("0|{0}", ex.Message);
            }
            finally
            {
            }
            return result;
        }

        #endregion
    }
}