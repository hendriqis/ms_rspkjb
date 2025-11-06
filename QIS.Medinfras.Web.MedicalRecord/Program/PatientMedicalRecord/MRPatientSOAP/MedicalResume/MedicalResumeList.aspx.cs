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

namespace QIS.Medinfras.Web.MedicalRecord.Program
{
    public partial class MedicalResumeList : BasePagePatientPageList
    {
        protected int PageCount = 1;
        string menuType = string.Empty;
        protected static bool _isMedicalResumeExists = false;

        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.MedicalRecord.PATIENT_SOAP;
        }

        public override bool IsEntryUsePopup()
        {
            return false;
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        public override void SetCRUDMode(ref bool IsAllowAdd, ref bool IsAllowEdit, ref bool IsAllowDelete)
        {
            IsAllowAdd = IsAllowDelete = false;
        }

        protected override void InitializeDataControl()
        {
            ctlToolbar.SetSelectedMenu(12);
            if (Page.Request.QueryString["id"] != null)
            {
                hdnModuleID.Value = Page.Request.QueryString["id"];
            }
            hdnCurrentSessionID.Value = AppSession.UserLogin.UserID.ToString();
            hdnCurrentParamedicID.Value = AppSession.UserLogin.ParamedicID.ToString();
            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
            hdnRegistrationID.Value = AppSession.RegisteredPatient.RegistrationID.ToString();
            hdnVisitID.Value = AppSession.RegisteredPatient.VisitID.ToString();

            Healthcare oHealthcare = BusinessLayer.GetHealthcareList(string.Format("HealthcareID = {0}", AppSession.UserLogin.HealthcareID)).FirstOrDefault();
            string initial = oHealthcare.Initial;
            if (oHealthcare.Initial == "RSUKI")
            {
                hdnReportCode.Value = "PM-90009";
            }
            else if (oHealthcare.Initial == "BROS")
            {
                hdnReportCode.Value = "PM-90010";
            } 
            else if (oHealthcare.Initial == "QISJKT" || oHealthcare.Initial == "RSCK" || oHealthcare.Initial == "rsdo-soba" || oHealthcare.Initial == "RSRT"
                    || oHealthcare.Initial == "RSRTH" || oHealthcare.Initial == "RSRA" || oHealthcare.Initial == "NHS" || oHealthcare.Initial == "RSMD"
                    || oHealthcare.Initial == "kkdi" || oHealthcare.Initial == "rsdo-ska" || oHealthcare.Initial == "RSPBT" || oHealthcare.Initial == "DEMO")
            {
                hdnReportCode.Value = "PM-90011";
            }
            else if (oHealthcare.Initial == "RSUKRIDA")
            {
                hdnReportCode.Value = "PM-90012";
            }
            else if (oHealthcare.Initial == "RSSC")
            {
                hdnReportCode.Value = "PM-90013";
            }
            else if (oHealthcare.Initial == "RSSBB")
            {
                hdnReportCode.Value = "PM-90014";
            }
            else
            {
                hdnReportCode.Value = "PM-90011";
            }

            BindGridView(1, true, ref PageCount);
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Format("VisitID = {0} AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvChiefComplaintRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vMedicalResume> lstEntity = BusinessLayer.GetvMedicalResumeList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "ID DESC");
            _isMedicalResumeExists = lstEntity.Count > 0;
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


        protected override bool OnAddRecord(ref string url, ref string errMessage)
        {
            bool result = true;

            if (_isMedicalResumeExists)
            {
                errMessage = "Hanya boleh ada 1 (satu) resume medis.";
                result = false;
            }
            else
            {
                url = ResolveUrl("~/Program/PatientMedicalRecord/MRPatientSOAP/MedicalResume/MedicalResumeEntry.aspx?id=");
                result = true;
            }

            return result;
        }

        protected override bool OnEditRecord(ref string url, ref string errMessage)
        {
            bool result = true;

            url = ResolveUrl("~/Program/PatientMedicalRecord/MRPatientSOAP/MedicalResume/MedicalResumeEntry.aspx?id=" + hdnID.Value);
            result = true;
            
            return result;
        }

        protected void grdView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
        }

        protected void cbpCompleted_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errMessage = "";
            string param = e.Parameter;
            try
            {
                string retVal = CompletedAssessment(param);
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

        private string CompletedAssessment(string recordID)
        {
            string result = string.Empty;

            try
            {
                //Confirm
                int id = Convert.ToInt32(recordID);
                NurseChiefComplaint obj = BusinessLayer.GetNurseChiefComplaint(id);
                if (obj != null)
                {
                    if (string.IsNullOrEmpty(obj.GCAssessmentStatus) || obj.GCAssessmentStatus == Constant.AssessmentStatus.OPEN )
                    {
                        obj.GCAssessmentStatus = Constant.AssessmentStatus.COMPLETED;
                        obj.LastUpdatedBy = AppSession.UserLogin.UserID;
                        obj.LastUpdatedDate = DateTime.Now;
                        BusinessLayer.UpdateNurseChiefComplaint(obj);
                        result = string.Format("1|{0}", string.Empty);
                    }
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
        protected override bool OnDeleteRecord(ref string errMessage)
        {
            if (hdnID.Value != "")
            {
                try
                {
                    MedicalResume entity = BusinessLayer.GetMedicalResume(Convert.ToInt32(hdnID.Value));
                    entity.IsDeleted = true;
                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                    BusinessLayer.UpdateMedicalResume(entity);
                    return true;
                }
                catch (Exception ex)
                {
                    errMessage = ex.Message;
                    return false;
                }
            }
            return false;
        }
    }
}