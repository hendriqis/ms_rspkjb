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
using QIS.Medinfras.Web.CommonLibs.MasterPage;
using QIS.Medinfras.Web.CommonLibs.Controls;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class ResendImagingOrderList : BasePagePatientOrder
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Imaging.RESEND_ORDER;
        }

        protected string GetRefreshGridInterval()
        {
            return refreshGridInterval;
        }

        public override bool IsShowRightPanel()
        {
            return false;
        }

        private string refreshGridInterval = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                refreshGridInterval = BusinessLayer.GetSettingParameter(Constant.SettingParameter.PATIENT_GRID_REFRESH_INTERVAL).ParameterValue;

                #region Region Registrasi
                txtRealisationDate.Text = DateTime.Today.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                cboMedicalDiagnostic.SelectedIndex = 0;
                Helper.SetControlEntrySetting(txtServiceUnitName, new ControlEntrySetting(false, false, false), "mpServiceUnit");

                if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Imaging)
                {
                    trServiceUnit.Style.Add("display", "none");
                    trServiceUnitOrder.Style.Add("display", "none");
                }
                else
                {
                    string filterExpression = string.Format("IsUsingRegistration = 1 AND HealthcareServiceUnitID NOT IN ({0},{1})", AppSession.MedicalDiagnostic.ImagingHealthcareServiceUnitID, AppSession.MedicalDiagnostic.LaboratoryHealthcareServiceUnitID);
                    if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Nutrition)
                    {
                        filterExpression += " AND IsNutritionUnit = 1";
                    }
                    List<GetServiceUnitUserList> lstServiceUnit = BusinessLayer.GetServiceUnitUserList(AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, Constant.Facility.DIAGNOSTIC, filterExpression);
                    Methods.SetComboBoxField<GetServiceUnitUserList>(cboMedicalDiagnostic, lstServiceUnit, "ServiceUnitName", "HealthcareServiceUnitID");
                    cboMedicalDiagnostic.SelectedIndex = 0;

                    Methods.SetComboBoxField<GetServiceUnitUserList>(cboMedicalDiagnosticOrder, lstServiceUnit, "ServiceUnitName", "HealthcareServiceUnitID");
                    cboMedicalDiagnosticOrder.SelectedIndex = 0;
                }

                List<Department> lstDept = BusinessLayer.GetDepartmentList(string.Format("IsHasRegistration = 1 AND DepartmentID != '{0}' AND IsActive = 1", Constant.Facility.PHARMACY));
                Methods.SetComboBoxField<Department>(cboPatientFrom, lstDept, "DepartmentName", "DepartmentID");
                cboPatientFrom.SelectedIndex = 0;

                SettingControlPropertiesReg();
                #endregion

                #region Region Order
                txtOrderDate.Text = DateTime.Today.ToString(Constant.FormatString.DATE_PICKER_FORMAT);

                List<Variable> lstVariable = new List<Variable>();
                lstVariable.Add(new Variable { Code = "0", Value = "Semua" });
                lstVariable.Add(new Variable { Code = "1", Value = "Belum Diproses" });
                lstVariable.Add(new Variable { Code = "2", Value = "Sudah Diproses" });
                Methods.SetComboBoxField<Variable>(cboOrderResultType, lstVariable, "Value", "Code");
                cboOrderResultType.Value = "1";

                lstDept.Insert(0, new Department { DepartmentID = "", DepartmentName = "" });
                Methods.SetComboBoxField<Department>(cboPatientFromOrder, lstDept, "DepartmentName", "DepartmentID");

                //lstHealthcareServiceUnit.Insert(0, new vHealthcareServiceUnit { HealthcareServiceUnitID = 0, ServiceUnitName = string.Format(" - {0} - ", GetLabel("All")) });
                cboPatientFromOrder.SelectedIndex = 0;

                SettingControlPropertiesOrder();
                #endregion

                ((GridPatientRegOrderCtl)grdRegisteredPatient).InitializeControl();
                ((GridPatientOrderCtl)grdOrderedPatient).InitializeControl();

                Helper.SetControlEntrySetting(cboPatientFrom, new ControlEntrySetting(true, true, false), "mpPatientList");
                Helper.SetControlEntrySetting(cboPatientFromOrder, new ControlEntrySetting(true, true, false), "mpPatientList");
                Helper.SetControlEntrySetting(txtRealisationDate, new ControlEntrySetting(true, true, true), "mpPatientList");
                Helper.SetControlEntrySetting(txtOrderDate, new ControlEntrySetting(true, true, true), "mpPatientList");
            }
        }

        private void SettingControlPropertiesReg()
        {
            txtSearchViewReg.IntellisenseHints = Methods.LoadRegistrationWorklistQuickFilterHints("1");
            if (AppSession.LastContentImagingRealization != null)
            {
                LastContentImagingRealization lc = AppSession.LastContentImagingRealization;
                txtRealisationDate.Text = lc.ImagingDate;
            }
        }

        public override string GetFilterExpression()
        {
            string filterExpression = string.Empty;
            filterExpression = string.Format("GCVisitStatus IN ('{0}')", Constant.VisitStatus.CLOSED);

            if (hdnServiceUnitID.Value != "0" && hdnServiceUnitID.Value != "")
            {
                filterExpression += string.Format(" AND HealthcareServiceUnitID = {0}", hdnServiceUnitID.Value);
            }
            else if (cboPatientFrom.Value != null)
            {
                filterExpression += string.Format(" AND DepartmentID = '{0}'", cboPatientFrom.Value);
            }

            if (cboPatientFrom.Value != null && cboPatientFrom.Value.ToString() != Constant.Facility.INPATIENT)
            {
                if (!chkIsPreviousEpisodePatientReg.Checked)
                {
                    filterExpression += string.Format(" AND VisitDate = '{0}'", Helper.GetDatePickerValue(txtRealisationDate).ToString(Constant.FormatString.DATE_FORMAT_112));
                }
            }

            if (hdnFilterExpressionQuickSearchReg.Value != "")
                filterExpression += string.Format(" AND {0}", hdnFilterExpressionQuickSearchReg.Value);
            return filterExpression;
        }

        public override void LoadAllWords()
        {
            LoadWords();
        }

        public override void OnGrdRowClick(string transactionNo)
        {
            vConsultVisit16 entity = BusinessLayer.GetvConsultVisit16List(string.Format("VisitID = {0}", transactionNo))[0];
            RegisteredPatient pt = new RegisteredPatient();
            pt.MRN = entity.MRN;
            pt.MedicalNo = entity.MedicalNo;
            pt.RegistrationID = entity.RegistrationID;
            pt.VisitID = entity.VisitID;
            pt.VisitDate = entity.VisitDate;
            pt.VisitTime = entity.VisitTime;
            pt.ParamedicID = entity.ParamedicID;
            pt.ParamedicCode = entity.ParamedicCode;
            pt.ParamedicName = entity.ParamedicName;
            pt.IsLockDown = entity.IsLockDown;
            pt.SpecialtyID = entity.SpecialtyID;
            pt.HealthcareServiceUnitID = entity.HealthcareServiceUnitID;
            pt.ServiceUnitName = entity.ServiceUnitName;
            pt.RoomCode = entity.RoomCode;
            pt.BedCode = entity.BedCode;
            pt.DepartmentID = entity.DepartmentID;
            pt.ClassID = entity.ClassID;
            pt.ChargeClassID = entity.ChargeClassID;
            AppSession.RegisteredPatient = pt;
            if (AppSession.MedicalDiagnostic.MedicalDiagnosticType != MedicalDiagnosticType.Imaging)
            {
                AppSession.HealthcareServiceUnitID = cboMedicalDiagnosticOrder.Value.ToString();
            }

            LastContentImagingRealization lc = new LastContentImagingRealization()
            {
                ImagingDate = txtOrderDate.Text
            };
            AppSession.LastContentImagingRealization = lc;

            string url = "";
            url = string.Format("~/Libs/Program/Module/PatientManagement/Process/ResendImagingOrderDetail.aspx?id=pr|{0}|{1}|{2}", transactionNo, AppSession.MedicalDiagnostic.HealthcareServiceUnitID, entity.RegistrationID);

            Response.Redirect(url);
        }

        public override void OnGrdRowClickTestOrder(string transactionNo, string TestOrderID)
        {
            vConsultVisit16 entity = BusinessLayer.GetvConsultVisit16List(string.Format("VisitID = {0}", transactionNo))[0];
            RegisteredPatient pt = new RegisteredPatient();
            pt.MRN = entity.MRN;
            pt.MedicalNo = entity.MedicalNo;
            pt.RegistrationID = entity.RegistrationID;
            pt.VisitID = entity.VisitID;
            pt.VisitDate = entity.VisitDate;
            pt.VisitTime = entity.VisitTime;
            pt.ParamedicID = entity.ParamedicID;
            pt.ParamedicCode = entity.ParamedicCode;
            pt.ParamedicName = entity.ParamedicName;
            pt.IsLockDown = entity.IsLockDown;
            pt.SpecialtyID = entity.SpecialtyID;
            pt.HealthcareServiceUnitID = entity.HealthcareServiceUnitID;
            pt.ServiceUnitName = entity.ServiceUnitName;
            pt.RoomCode = entity.RoomCode;
            pt.BedCode = entity.BedCode;
            pt.DepartmentID = entity.DepartmentID;
            pt.ClassID = entity.ClassID;
            pt.ChargeClassID = entity.ChargeClassID;
            AppSession.RegisteredPatient = pt;
            if (AppSession.MedicalDiagnostic.MedicalDiagnosticType != MedicalDiagnosticType.Imaging)
            {
                AppSession.HealthcareServiceUnitID = cboMedicalDiagnosticOrder.Value.ToString();
            }

            LastContentImagingRealization lc = new LastContentImagingRealization()
            {
                ImagingDate = txtOrderDate.Text
            };
            AppSession.LastContentImagingRealization = lc;

            string url = "";
            url = string.Format("~/Libs/Program/Module/PatientManagement/Process/ResendImagingOrderDetail.aspx?id=to|{0}|{1}|{2}", TestOrderID, transactionNo, entity.RegistrationID);

            Response.Redirect(url);
        }

        private void SettingControlPropertiesOrder()
        {
            txtSearchViewOrder.IntellisenseHints = Methods.LoadDiagnosticWorklistQuickFilterHints("1");
            if (AppSession.LastContentImagingRealization != null)
            {
                LastContentImagingRealization lc = AppSession.LastContentImagingRealization;
                txtOrderDate.Text = lc.ImagingDate;
            }
        }

        public override string GetFilterExpressionTestOrder()
        {
            string filterExpression = "";
            string healthcareServiceUnitID = "";

            if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.OtherMedicalDiagnostic) //|| AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Nutrition)
                healthcareServiceUnitID = cboMedicalDiagnosticOrder.Value.ToString();
            else
                healthcareServiceUnitID = AppSession.MedicalDiagnostic.HealthcareServiceUnitID.ToString();

            filterExpression += string.Format("HealthcareServiceUnitID = {0} AND GCVisitStatus IN ('{1}') AND ",
                    healthcareServiceUnitID, Constant.VisitStatus.CLOSED);

            if (cboOrderResultType.Value.ToString() == "0")
                filterExpression += string.Format("GCTransactionStatus IN ('{0}','{1}','{2}')", Constant.TransactionStatus.PROCESSED, Constant.TransactionStatus.WAIT_FOR_APPROVAL, Constant.TransactionStatus.APPROVED);
            else if (cboOrderResultType.Value.ToString() == "1")
                filterExpression += string.Format("GCTransactionStatus IN ('{0}','{1}')", Constant.TransactionStatus.WAIT_FOR_APPROVAL, Constant.TransactionStatus.APPROVED);
            else
                filterExpression += string.Format("GCTransactionStatus = '{0}'", Constant.TransactionStatus.PROCESSED);

            if (cboPatientFromOrder.Value != null)
                filterExpression += string.Format(" AND DepartmentID = '{0}'", cboPatientFromOrder.Value);

            if (hdnServiceUnitOrderID.Value != "0" && hdnServiceUnitOrderID.Value != "")
                filterExpression += string.Format(" AND VisitHSUID = {0}", hdnServiceUnitOrderID.Value);

            if (!chkIsPreviousEpisodePatientOrder.Checked)
            {
                filterExpression += string.Format(" AND ScheduledDate = '{0}'", Helper.GetDatePickerValue(txtOrderDate).ToString(Constant.FormatString.DATE_FORMAT_112));
            }

            filterExpression += string.Format(" AND IsAIOTransaction = 0");

            if (hdnFilterExpressionQuickSearchOrder.Value != "")
                filterExpression += string.Format(" AND {0}", hdnFilterExpressionQuickSearchOrder.Value);

            return filterExpression;
        }

        public override string GetSortingTestOrder()
        {
            string sortBy = "";

            return sortBy;
        }
    }
}