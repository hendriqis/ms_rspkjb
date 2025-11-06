using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxCallbackPanel;
using DevExpress.Web.ASPxEditors;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class AttendingNotesInformationDetail : BasePagePatientPageList
    {
        protected int PageCount = 1;
        protected bool IsLockDown = false;

        public override string OnGetMenuCode()
        {
            switch (hdnDepartmentID.Value)
            {
                case Constant.Facility.EMERGENCY: return Constant.MenuCode.EmergencyCare.BILL_SUMMARY_PHYSICIAN_VISIT_LIST;
                case Constant.Facility.INPATIENT: return Constant.MenuCode.Inpatient.BILL_SUMMARY_PHYSICIAN_VISIT_LIST;
                case Constant.Facility.IMAGING: return Constant.MenuCode.Imaging.BILL_SUMMARY_PHYSICIAN_VISIT_LIST;
                case Constant.Facility.DIAGNOSTIC: return Constant.MenuCode.Inpatient.BILL_SUMMARY_PHYSICIAN_VISIT_LIST;
                case Constant.Facility.MEDICAL_CHECKUP: return Constant.MenuCode.Imaging.BILL_SUMMARY_PHYSICIAN_VISIT_LIST;
                case Constant.Facility.OUTPATIENT: return Constant.MenuCode.Imaging.BILL_SUMMARY_PHYSICIAN_VISIT_LIST;
                default: return Constant.MenuCode.Inpatient.BILL_SUMMARY_PHYSICIAN_VISIT_LIST;
            }
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        public override void SetCRUDMode(ref bool IsAllowAdd, ref bool IsAllowEdit, ref bool IsAllowDelete)
        {
            IsAllowAdd = false;
            IsAllowEdit = false;
            IsAllowDelete = false;
        }

        protected override void InitializeDataControl()
        {
            txtFromVisitDate.Text = AppSession.RegisteredPatient.VisitDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtToVisitDate.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);

            string filterExpression = string.Format("RegistrationID = {0} AND GCParamedicRole IN ('{1}','{2}') AND IsDeleted = 0 ORDER BY GCParamedicRole, ParamedicCode", AppSession.RegisteredPatient.RegistrationID, Constant.ParamedicRole.DPJP_UTAMA, Constant.ParamedicRole.DPJP_KONSUL);
            List<vParamedicTeam> lstPhysician = BusinessLayer.GetvParamedicTeamList(filterExpression);

            if (lstPhysician.Count() > 0)
            {
                Methods.SetComboBoxField(cboPhysicianList, lstPhysician, "ParamedicName", "ParamedicID");
            }
            else
            {
                filterExpression = string.Format("HealthcareServiceUnitID = {0} AND IsDeleted = 0 ORDER BY ParamedicCode", AppSession.RegisteredPatient.HealthcareServiceUnitID);
                List<vServiceUnitParamedic> lstPhysician2 = BusinessLayer.GetvServiceUnitParamedicList(filterExpression);
                Methods.SetComboBoxField(cboPhysicianList, lstPhysician2, "ParamedicName", "ParamedicID");
            }

            cboPhysicianList.Value = AppSession.RegisteredPatient.ParamedicID.ToString();
            hdnDepartmentID.Value = AppSession.RegisteredPatient.DepartmentID;
            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;

            Registration oReg = BusinessLayer.GetRegistration(AppSession.RegisteredPatient.RegistrationID);
            IsLockDown = oReg.IsLockDown;

            ConsultVisit oCV = BusinessLayer.GetConsultVisitList(string.Format("RegistrationID = {0}", oReg.RegistrationID)).FirstOrDefault();
            hdnRegistrationID.Value = oCV.RegistrationID.ToString();
            hdnVisitID.Value = oCV.VisitID.ToString();
            hdnParamedicID.Value = oCV.ParamedicID.ToString();
            hdnHealthcareServiceUnitID.Value = oCV.HealthcareServiceUnitID.ToString();
            hdnChargeClassID.Value = oCV.ChargeClassID.ToString();


            BindGridView(1, true, ref PageCount);
        }


        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Format("VisitID = {0} AND GCPatientNoteType = '{1}' AND IsDeleted = 0", hdnVisitID.Value, Constant.PatientVisitNotes.SOAP_SUMMARY_NOTES);

            if (!string.IsNullOrEmpty(hdnParamedicID.Value))
                filterExpression += string.Format(" AND ParamedicID = {0}", hdnParamedicID.Value);

            filterExpression += string.Format(" AND NoteDate >= '{0}' and NoteDate <= '{1}' ", Helper.GetDatePickerValue(txtFromVisitDate.Text).ToString("yyyyMMdd"), Helper.GetDatePickerValue(txtToVisitDate.Text).ToString("yyyyMMdd"));

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvPatientVisitNote1RowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vPatientVisitNote1> lstEntity = BusinessLayer.GetvPatientVisitNote1List(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex);
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
    }
}