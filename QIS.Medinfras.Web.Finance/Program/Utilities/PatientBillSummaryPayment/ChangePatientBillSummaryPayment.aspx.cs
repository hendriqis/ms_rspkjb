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
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Medinfras.Web.CommonLibs.Controls;

namespace QIS.Medinfras.Web.Finance.Program
{
    public partial class ChangePatientBillSummaryPayment : BasePageRegisteredPatient
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Finance.FN_CHANGE_PAYMENT;
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
                #region Region Registrasi
                txtRealisationDate.Text = DateTime.Today.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                //Helper.SetControlEntrySetting(txtServiceUnitName, new ControlEntrySetting(false, false, false), "mpServiceUnit");

                List<Department> lstDept = BusinessLayer.GetDepartmentList(string.Format("IsActive = 1 AND IsHasRegistration = 1"));
                lstDept = lstDept.OrderBy(lst => lst.TabOrder).ToList();
                Methods.SetComboBoxField<Department>(cboDepartment, lstDept, "DepartmentName", "DepartmentID");
                cboDepartment.SelectedIndex = 0;

                string moduleName = Helper.GetModuleName();
                string ModuleID = Helper.GetModuleID(moduleName);
                hdnModuleID.Value = ModuleID;
                #endregion

                refreshGridInterval = BusinessLayer.GetSettingParameter(Constant.SettingParameter.PATIENT_GRID_REFRESH_INTERVAL).ParameterValue;
                ((GridPatientRegOrderCtl)grdRegisteredPatient).InitializeControl();
            }
        }

        public override string GetFilterExpression()
        {
            string departmentID = cboDepartment.Value.ToString();
            string filterExpression = "";

            if (chkIsClosedRegistration.Checked)
            {
                filterExpression = string.Format("GCVisitStatus NOT IN ('{0}') AND DepartmentID = '{1}'", Constant.VisitStatus.CANCELLED, departmentID);
            }
            else
            {
                filterExpression = string.Format("GCVisitStatus NOT IN ('{0}','{1}','{2}') AND DepartmentID = '{3}'", Constant.VisitStatus.CANCELLED, Constant.VisitStatus.DISCHARGED, Constant.VisitStatus.CLOSED, departmentID);
            }

            if (hdnServiceUnitID.Value != "0" && hdnServiceUnitID.Value != "")
                filterExpression += string.Format(" AND HealthcareServiceUnitID = {0}", hdnServiceUnitID.Value);

            if (departmentID != Constant.Facility.INPATIENT)
            {
                filterExpression += string.Format(" AND VisitDate = '{0}'", Helper.GetDatePickerValue(txtRealisationDate).ToString(Constant.FormatString.DATE_FORMAT_112));
            }

            if (hdnFilterExpressionQuickSearchReg.Value != "") {
                filterExpression += string.Format(" AND {0}", hdnFilterExpressionQuickSearchReg.Value);
            }

            filterExpression += string.Format(" AND RegistrationID IN (SELECT RegistrationID FROM PatientPaymentHd WHERE GCTransactionStatus = '{0}' AND PaymentID IN (SELECT PaymentID FROM PatientPaymentDt WHERE IsDeleted = 0 AND GCPaymentMethod IN ('{1}','{2}')))",
                                                        Constant.TransactionStatus.OPEN,
                                                        Constant.PaymentMethod.DEBIT_CARD,
                                                        Constant.PaymentMethod.CREDIT_CARD
                                            );

            return filterExpression;
        }

        public override void LoadAllWords()
        {
            LoadWords();
        }

        public override void OnGrdRowClick(string visitID)
        {
            vConsultVisit4 entity = BusinessLayer.GetvConsultVisit4List(string.Format("VisitID = {0}", visitID)).FirstOrDefault();
            string parameter = string.Format("{0}|{1}", visitID, entity.HealthcareServiceUnitID);
            string url = "";

            RegisteredPatient pt = new RegisteredPatient();
            pt.MRN = entity.MRN;
            pt.PatientName = entity.PatientName;
            pt.MedicalNo = entity.MedicalNo;
            pt.RegistrationID = entity.RegistrationID;
            pt.BusinessPartnerID = entity.BusinessPartnerID;
            pt.RegistrationNo = entity.RegistrationNo;
            pt.VisitID = entity.VisitID;
            pt.VisitDate = entity.VisitDate;
            pt.VisitTime = entity.VisitTime;
            pt.ParamedicID = entity.ParamedicID;
            pt.ParamedicCode = entity.ParamedicCode;
            pt.ParamedicName = entity.ParamedicName;
            pt.SpecialtyID = entity.SpecialtyID;
            pt.HealthcareServiceUnitID = entity.HealthcareServiceUnitID;
            pt.ServiceUnitName = entity.ServiceUnitName;
            pt.RoomCode = entity.RoomCode;
            pt.BedCode = entity.BedCode;
            pt.DepartmentID = entity.DepartmentID;
            pt.ClassID = entity.ClassID;
            pt.ChargeClassID = entity.ChargeClassID;
            pt.GCRegistrationStatus = entity.GCVisitStatus;
            pt.IsLockDown = entity.IsLockDown;
            pt.LinkedRegistrationID = entity.LinkedRegistrationID;
            pt.GCRegistrationStatus = entity.GCRegistrationStatus;
            pt.DepartmentID = entity.DepartmentID;
            pt.StartServiceDate = entity.StartServiceDate;
            pt.StartServiceTime = entity.StartServiceTime;
            pt.DateOfBirth = entity.DateOfBirth;
            pt.IsBillingReopen = entity.IsBillingReopen;
            AppSession.RegisteredPatient = pt;


            url = string.Format("~/Program/Utilities/PatientBillSummaryPayment/ChangePatientBillSummaryPaymentEntry.aspx?id={0}", parameter);
            Response.Redirect(url);
        }
    }
}