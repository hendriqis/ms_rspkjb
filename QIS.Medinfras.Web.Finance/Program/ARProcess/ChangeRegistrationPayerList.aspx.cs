using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.CommonLibs.MasterPage;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.Finance.Controls;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.Finance.Program
{
    public partial class ChangeRegistrationPayerList : BasePageRegisteredPatient
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Finance.CHANGE_REGISTRATION_PAYER;
        }

        protected int PageCount = 1;
        protected int CurrPage = 1;
        protected string GetRefreshGridInterval()
        {
            return refreshGridInterval;
        }
        private string refreshGridInterval = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                txtRealisationDate.Text = DateTime.Today.ToString(Constant.FormatString.DATE_PICKER_FORMAT);

                List<Department> lstDept = BusinessLayer.GetDepartmentList(string.Format("IsActive = 1 AND IsHasRegistration = 1"));
                lstDept = lstDept.OrderBy(lst => lst.TabOrder).ToList();
                lstDept.Add(new Department{DepartmentID = "ALL", DepartmentName = "ALL"});
                Methods.SetComboBoxField<Department>(cboDepartment, lstDept, "DepartmentName", "DepartmentID");
                cboDepartment.SelectedIndex = 0;

                string moduleName = Helper.GetModuleName();
                string ModuleID = Helper.GetModuleID(moduleName);
                hdnModuleID.Value = ModuleID;

                refreshGridInterval = BusinessLayer.GetSettingParameter(Constant.SettingParameter.PATIENT_GRID_REFRESH_INTERVAL).ParameterValue;
                ((GridChangeRegistrationPayerCtl)grdRegisteredPatient).InitializeControl();
            }
        }

        public override string GetFilterExpression()
        {
            string filterExpression = "";

            string oDepartmentID = "%%";
            int oHealthcareServiceUnitID = 0;
            string oVisitDate = "19000101";
            string oQuickSearch = "|";

            if (cboDepartment.Value.ToString() != "ALL")
            {
                oDepartmentID = cboDepartment.Value.ToString();
            }

            if (hdnHealthcareServiceUnitID.Value != "0" && hdnHealthcareServiceUnitID.Value != "")
            {
                oHealthcareServiceUnitID = Convert.ToInt32(hdnHealthcareServiceUnitID.Value);
            }

            if (oDepartmentID != Constant.Facility.INPATIENT)
            {
                oVisitDate = Helper.GetDatePickerValue(txtRealisationDate).ToString(Constant.FormatString.DATE_FORMAT_112);
            }

            if (hdnFilterExpressionQuickSearchReg.Value != "")
            {
                oQuickSearch = hdnFilterExpressionQuickSearchReg.Value;
            }

            filterExpression = oDepartmentID + "|" + oHealthcareServiceUnitID + "|" + oVisitDate + "|" + oQuickSearch;

            return filterExpression;
        }

        public override void LoadAllWords()
        {
            LoadWords();
        }

        public override void OnGrdRowClick(string visitID)
        {
            vConsultVisit4 entity = BusinessLayer.GetvConsultVisit4List(string.Format("VisitID = {0}", visitID)).FirstOrDefault();
            RegisteredPatient pt = new RegisteredPatient();
            pt.MRN = entity.MRN;
            pt.MedicalNo = entity.MedicalNo;
            pt.PatientName = entity.PatientName;
            pt.GCGender = entity.GCGender;
            pt.GCSex = entity.GCSex;
            pt.DateOfBirth = entity.DateOfBirth;
            pt.RegistrationID = entity.RegistrationID;
            pt.RegistrationNo = entity.RegistrationNo;
            pt.RegistrationDate = entity.RegistrationDate;
            pt.RegistrationTime = entity.RegistrationTime;
            pt.VisitID = entity.VisitID;
            pt.VisitDate = entity.VisitDate;
            pt.VisitTime = entity.VisitTime;
            pt.StartServiceDate = entity.StartServiceDate;
            pt.StartServiceTime = entity.StartServiceTime;
            pt.DischargeDate = entity.DischargeDate;
            pt.DischargeTime = entity.DischargeTime;
            pt.GCCustomerType = entity.GCCustomerType;
            pt.BusinessPartnerID = entity.BusinessPartnerID;
            pt.ParamedicID = entity.ParamedicID;
            pt.ParamedicCode = entity.ParamedicCode;
            pt.ParamedicName = entity.ParamedicName;
            pt.SpecialtyID = entity.SpecialtyID;
            pt.HealthcareServiceUnitID = entity.HealthcareServiceUnitID;
            pt.DepartmentID = entity.DepartmentID;
            pt.ServiceUnitName = entity.ServiceUnitName;
            pt.RoomCode = entity.RoomCode;
            pt.BedCode = entity.BedCode;
            pt.DepartmentID = entity.DepartmentID;
            pt.ChargeClassID = entity.ChargeClassID;
            pt.ClassID = entity.ClassID;
            pt.GCRegistrationStatus = entity.GCVisitStatus;
            pt.IsLockDown = entity.IsLockDown;
            pt.IsBillingReopen = entity.IsBillingReopen;
            pt.LinkedRegistrationID = entity.LinkedRegistrationID;
            pt.LinkedToRegistrationID = entity.LinkedToRegistrationID;
            AppSession.RegisteredPatient = pt;

            string url = "";
            url = string.Format("~/Program/ARProcess/ChangeRegistrationPayerDetail.aspx?id={0}", visitID);
            Response.Redirect(url);
        }
    }
}