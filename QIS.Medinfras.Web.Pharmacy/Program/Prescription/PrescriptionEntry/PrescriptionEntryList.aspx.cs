using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.CommonLibs.Controls;
using QIS.Medinfras.Web.CommonLibs.MasterPage;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.Pharmacy.Program
{
    public partial class PrescriptionEntryList : BasePagePatientOrder
    {
        protected int PageCount = 1;
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Pharmacy.PRESCRIPTION_ENTRY;
        }
        private GetUserMenuAccess menu;
        public override bool IsShowRightPanel()
        {
            return false;
        }
        protected String GetMenuCaption()
        {
            if (menu != null)
                return GetLabel(menu.MenuCaption);
            return "";
        }

        protected string GetRefreshGridInterval()
        {
            return refreshGridInterval;
        }
        private string refreshGridInterval = "10";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                string paramFilter = string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}')", AppSession.UserLogin.HealthcareID, Constant.SettingParameter.FM_PATIENT_GRID_REFRESH_INTERVAL, Constant.SettingParameter.FM_USING_UDD_FOR_INPATIENT);

                List<SettingParameterDt> lstParam = BusinessLayer.GetSettingParameterDtList(paramFilter);
                if (lstParam.Count > 0)
                {
                    hdnIsUsingUDD.Value = lstParam.Where(lst => lst.ParameterCode == Constant.SettingParameter.FM_USING_UDD_FOR_INPATIENT).FirstOrDefault().ParameterValue;
                    refreshGridInterval = lstParam.Where(lst => lst.ParameterCode == Constant.SettingParameter.FM_PATIENT_GRID_REFRESH_INTERVAL).FirstOrDefault().ParameterValue;
                }

                string filterExpression = string.Format("ParentID = '{0}' AND IsActive = 1 AND IsDeleted = 0", Constant.StandardCode.PRESCRIPTION_TYPE);
                List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(filterExpression);
                Methods.SetComboBoxField<StandardCode>(cboPrescriptionType, lstStandardCode, "StandardCodeName", "StandardCodeID");
                cboPrescriptionType.SelectedIndex = 0;

                List<StandardCode> lstRegistrationPayer = BusinessLayer.GetStandardCodeList(string.Format("IsActive = 1 AND IsDeleted = 0 AND ParentID = '{0}'", Constant.StandardCode.CUSTOMER_TYPE));
                lstRegistrationPayer.Insert(0, new StandardCode { StandardCodeName = string.Format("{0}", GetLabel(" ")) });
                Methods.SetComboBoxField<StandardCode>(cboRegistrationPayer, lstRegistrationPayer, "StandardCodeName", "StandardCodeID");
                Methods.SetComboBoxField<StandardCode>(cboRegistrationPayerOrder, lstRegistrationPayer, "StandardCodeName", "StandardCodeID");
                cboRegistrationPayer.SelectedIndex = 0;
                cboRegistrationPayerOrder.SelectedIndex = 0;

                filterExpression = string.Format("HealthcareID = '{0}' AND IsDeleted = 0 AND DepartmentID = '{1}' AND IsUsingRegistration = 1 ", AppSession.UserLogin.HealthcareID, Constant.Facility.PHARMACY);
                if (hdnIsUsingUDD.Value == "1")
                    filterExpression = string.Format("HealthcareID = '{0}' AND IsDeleted = 0 AND DepartmentID = '{1}' AND IsInpatientDispensary = 0 AND IsUsingRegistration = 1 ", AppSession.UserLogin.HealthcareID, Constant.Facility.PHARMACY);

                List<vHealthcareServiceUnit> lstHServiceUnit = BusinessLayer.GetvHealthcareServiceUnitList(filterExpression);
                Methods.SetComboBoxField<vHealthcareServiceUnit>(cboServiceUnit, lstHServiceUnit, "ServiceUnitName", "HealthcareServiceUnitID");
                Methods.SetComboBoxField<vHealthcareServiceUnit>(cboServiceUnitOrder, lstHServiceUnit, "ServiceUnitName", "HealthcareServiceUnitID");

                string ipAddress = HttpContext.Current.Request.UserHostAddress;
                string filterExp = string.Format("IPAddress = '{0}' AND IsDeleted=0", ipAddress);
                List<IPPharmacyUnit> lstDispensary = BusinessLayer.GetIPPharmacyUnitList(filterExp);
                if (lstDispensary.Count > 0)
                {
                    cboServiceUnit.SelectedIndex = lstDispensary.FirstOrDefault().PharmacyHealthcareServiceUnitID;
                    cboServiceUnitOrder.SelectedIndex = lstDispensary.FirstOrDefault().PharmacyHealthcareServiceUnitID;
                }
                else
                {
                    cboServiceUnit.SelectedIndex = 1;
                    cboServiceUnitOrder.SelectedIndex = 1;
                }

                string deptFilter = string.Format("IsHasRegistration = 1 AND IsActive = 1 AND DepartmentID != '{0}'", Constant.Facility.PHARMACY);
                if (hdnIsUsingUDD.Value == "1")
                    deptFilter = string.Format("IsHasRegistration = 1 AND IsActive = 1 AND DepartmentID NOT IN ('{0}','{1}')", Constant.Facility.PHARMACY, Constant.Facility.INPATIENT);

                List<Department> lstDepartment = BusinessLayer.GetDepartmentList(deptFilter);
                List<Department> lstDepartmentOrder = BusinessLayer.GetDepartmentList(deptFilter);
                lstDepartmentOrder.Insert(0, new Department { DepartmentName = string.Format("{0}", GetLabel(" ")) });
                Methods.SetComboBoxField<Department>(cboDepartment, lstDepartment, "DepartmentName", "DepartmentID");
                Methods.SetComboBoxField<Department>(cboDepartmentOrder, lstDepartmentOrder, "DepartmentName", "DepartmentID");
                cboDepartment.SelectedIndex = 0;
                cboDepartmentOrder.SelectedIndex = 0;

                List<Variable> lstVariable = new List<Variable>();
                lstVariable.Add(new Variable { Code = "0", Value = "Semua" });
                lstVariable.Add(new Variable { Code = "1", Value = "Belum Diproses" });
                lstVariable.Add(new Variable { Code = "2", Value = "Sudah Diproses" });
                lstVariable.Add(new Variable { Code = "3", Value = "Belum Proposed" });
                Methods.SetComboBoxField<Variable>(cboOrderResultType, lstVariable, "Value", "Code");
                cboOrderResultType.Value = "1";

                List<Variable> lstSortBy = new List<Variable>();
                lstSortBy.Add(new Variable { Code = "0", Value = "No. Order Resep (ASC)" });
                lstSortBy.Add(new Variable { Code = "1", Value = "No. Order Resep (DESC)" });
                Methods.SetComboBoxField<Variable>(cboSortBy, lstSortBy, "Value", "Code");
                cboSortBy.Value = "0";


                hdnLastContentID.Value = "containerByOrder";
                txtDate.Text = txtTestOrderDate.Text = DateTime.Today.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                SettingControlProperties();
                grdRegisteredPatient.InitializeControl();
                grdOrderPatient.InitializeControl();

                Helper.SetControlEntrySetting(txtDate, new ControlEntrySetting(true, true, false), "mpPatientList");
                Helper.SetControlEntrySetting(cboDepartment, new ControlEntrySetting(true, true, false), "mpPatientList");
                Helper.SetControlEntrySetting(txtTestOrderDate, new ControlEntrySetting(true, true, false), "mpPatientListOrder");
                Helper.SetControlEntrySetting(cboDepartmentOrder, new ControlEntrySetting(true, true, false), "mpPatientListOrder");

                menu = ((MPMain)Master).ListMenu.FirstOrDefault(p => p.MenuCode == OnGetMenuCode());
            }
        }

        private void SettingControlProperties()
        {
            txtSearchViewReg.IntellisenseHints = Methods.LoadRegistrationWorklistQuickFilterHints("1");
            txtSearchViewOrder.IntellisenseHints = Methods.LoadPharmacyWorklistQuickFilterHints("1");

            if (AppSession.LastContentPrescriptionEntry != null)
            {
                LastContentPrescriptionEntry lc = AppSession.LastContentPrescriptionEntry;
                if (lc.ContentID.Equals("containerByOrder"))
                {
                    hdnLastContentID.Value = "containerByOrder";
                    cboServiceUnitOrder.Value = lc.HealthcareServiceUnitID.ToString();
                    cboDepartmentOrder.Value = lc.FromDepartmentID;
                    hdnServiceUnitIDOrder.Value = lc.FromHealthcareServiceUnitID.ToString();
                    vHealthcareServiceUnit entityHealthcareServiceUnit = BusinessLayer.GetvHealthcareServiceUnitList(string.Format("HealthcareServiceUnitID = {0}", hdnServiceUnitIDOrder.Value)).FirstOrDefault();
                    txtServiceUnitCodeOrder.Text = entityHealthcareServiceUnit == null ? "" : entityHealthcareServiceUnit.ServiceUnitCode;
                    txtServiceUnitNameOrder.Text = entityHealthcareServiceUnit == null ? "" : entityHealthcareServiceUnit.ServiceUnitName;
                    cboRegistrationPayerOrder.Text = lc.CustomerType;
                    txtTestOrderDate.Text = lc.Date;
                    hdnQuickTextOrder.Value = lc.QuickText.Equals("Search") ? "" : lc.QuickText;
                    hdnFilterExpressionQuickSearchOrder.Value = lc.QuickFilterExpression;
                    cboOrderResultType.Value = lc.VariableDisplay;
                }
                else if (lc.ContentID.Equals("containerDaftar"))
                {
                    hdnLastContentID.Value = "containerDaftar";
                    cboServiceUnit.Value = lc.HealthcareServiceUnitID.ToString();
                    cboDepartment.Value = lc.FromDepartmentID;
                    hdnServiceUnitID.Value = lc.FromHealthcareServiceUnitID.ToString();
                    vHealthcareServiceUnit entityHealthcareServiceUnit = BusinessLayer.GetvHealthcareServiceUnitList(string.Format("HealthcareServiceUnitID = {0}", hdnServiceUnitID.Value)).FirstOrDefault();
                    txtServiceUnitCode.Text = entityHealthcareServiceUnit == null ? "" : entityHealthcareServiceUnit.ServiceUnitCode;
                    txtServiceUnitName.Text = entityHealthcareServiceUnit == null ? "" : entityHealthcareServiceUnit.ServiceUnitName;
                    cboRegistrationPayer.Text = lc.CustomerType;
                    hdnQuickTextReg.Value = lc.QuickText.Equals("Search") ? "" : lc.QuickText;
                    txtDate.Text = lc.Date;
                    hdnFilterExpressionQuickSearchReg.Value = lc.QuickFilterExpression;
                    cboPrescriptionType.Value = lc.PrescriptionType;
                }
            }
        }

        public override string GetFilterExpression()
        {
            string filterExpression = hdnFilterExpression.Value;

            if (filterExpression != "")
                filterExpression += " AND ";

            //if (cboDepartment.Value != null && cboDepartment.Value.ToString() == "INPATIENT")
            //    filterExpression += string.Format("GCVisitStatus NOT IN ('{0}','{1}','{2}','{3}')", Constant.VisitStatus.CANCELLED, Constant.VisitStatus.DISCHARGED, Constant.VisitStatus.CLOSED, Constant.VisitStatus.OPEN);
            if (cboDepartment.Value != null && cboDepartment.Value.ToString() == "INPATIENT")
                filterExpression += string.Format("GCVisitStatus NOT IN ('{0}','{1}','{2}')", Constant.VisitStatus.CANCELLED, Constant.VisitStatus.CLOSED, Constant.VisitStatus.OPEN);
            else
                filterExpression += string.Format("GCVisitStatus NOT IN ('{0}','{1}','{2}')",
                    Constant.VisitStatus.CANCELLED, Constant.VisitStatus.CLOSED, Constant.VisitStatus.OPEN);
            if (cboDepartment.Value != null)
                filterExpression += string.Format(" AND DepartmentID = '{0}'", cboDepartment.Value);
            if (hdnServiceUnitID.Value != "0" && hdnServiceUnitID.Value != "")
                filterExpression += string.Format(" AND HealthcareServiceUnitID = {0}", hdnServiceUnitID.Value);
            if (hdnRegistrationPayer.Value != "0" && hdnRegistrationPayer.Value != "" && hdnRegistrationPayer.Value != null)
                filterExpression += string.Format(" AND GCCustomerType = '{0}'", hdnRegistrationPayer.Value);
                

            if (!chkIsPreviousEpisodePatientReg.Checked)
            {
                if (cboDepartment.Value != null)
                {
                    if (cboDepartment.Value.ToString() != Constant.Facility.INPATIENT)
                    {
                        filterExpression += string.Format(" AND VisitDate = '{0}'", Helper.GetDatePickerValue(txtDate).ToString(Constant.FormatString.DATE_FORMAT_112));
                    }
                }
                else
                {
                    filterExpression += string.Format(" AND VisitDate = '{0}'", Helper.GetDatePickerValue(txtDate).ToString(Constant.FormatString.DATE_FORMAT_112));
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
            vConsultVisit4 entity = BusinessLayer.GetvConsultVisit4List(string.Format("VisitID = {0}", transactionNo)).FirstOrDefault();
            if (entity != null)
            {
                RegisteredPatient pt = new RegisteredPatient();
                pt.MRN = entity.MRN;
                pt.MedicalNo = entity.MedicalNo;
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

                AppSession.HealthcareServiceUnitID = cboServiceUnit.Value.ToString();

                LastContentPrescriptionEntry lc = new LastContentPrescriptionEntry()
                {
                    ContentID = hdnLastContentID.Value,
                    HealthcareServiceUnitID = Convert.ToInt32(cboServiceUnit.Value),
                    FromDepartmentID = cboDepartment.Value != null ? cboDepartment.Value.ToString() : "",
                    FromHealthcareServiceUnitID = hdnServiceUnitID.Value == "" ? 0 : Convert.ToInt32(hdnServiceUnitID.Value),
                    CustomerType = hdnRegistrationPayer.Value,
                    Date = txtDate.Text,
                    QuickText = txtSearchViewReg.Text,
                    QuickFilterExpression = hdnFilterExpressionQuickSearchReg.Value,
                    VariableDisplay = "0",
                    PrescriptionType = cboPrescriptionType.Value.ToString()
                };
                AppSession.LastContentPrescriptionEntry = lc;

                string url = "";
                if (entity.DepartmentID == Constant.Facility.INPATIENT && hdnIsUsingUDD.Value == "1" && cboPrescriptionType.Value.ToString() == Constant.PrescriptionType.MEDICATION_ORDER)
                {
                    url = string.Format("~/Program/Prescription/UDD/MedicationOrder/MedicationOrderEntry.aspx?id=pr|{0}|{1}", transactionNo, cboServiceUnit.Value);
                }
                else
                {
                    url = string.Format("~/Program/Prescription/PrescriptionEntry/PrescriptionEntryDetail.aspx?id=pr|{0}|{1}", transactionNo, cboServiceUnit.Value);
                }
                Response.Redirect(url);
            }
        }

        public override void OnGrdRowClickTestOrder(string transactionNo, string prescriptionOrderID)
        {
            vConsultVisit4 entity = BusinessLayer.GetvConsultVisit4List(string.Format("VisitID = {0}", transactionNo)).FirstOrDefault();
            if (entity != null)
            {
                RegisteredPatient pt = new RegisteredPatient();
                pt.MRN = entity.MRN;
                pt.MedicalNo = entity.MedicalNo;
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

                string test = txtSearchViewOrder.Text;
                LastContentPrescriptionEntry lc = new LastContentPrescriptionEntry()
                {
                    ContentID = hdnLastContentID.Value,
                    HealthcareServiceUnitID = Convert.ToInt32(cboServiceUnitOrder.Value),
                    FromDepartmentID = cboDepartmentOrder.Value == null ? "" : cboDepartmentOrder.Value.ToString(),
                    FromHealthcareServiceUnitID = hdnServiceUnitIDOrder.Value == "" ? 0 : Convert.ToInt32(hdnServiceUnitIDOrder.Value),
                    CustomerType = hdnRegistrationPayerOrder.Value,
                    Date = txtTestOrderDate.Text,
                    QuickText = txtSearchViewOrder.Text,
                    QuickFilterExpression = hdnFilterExpressionQuickSearchOrder.Value,
                    VariableDisplay = cboOrderResultType.Value.ToString()
                };
                AppSession.LastContentPrescriptionEntry = lc;
                string url = "";
                string[] orderInfo = prescriptionOrderID.Split('|');

                if (entity.DepartmentID == Constant.Facility.INPATIENT && orderInfo[1] == Constant.PrescriptionType.MEDICATION_ORDER && hdnIsUsingUDD.Value == "1")
                {
                    url = string.Format("~/Program/Prescription/UDD/MedicationOrder/MedicationOrderEntry.aspx?id=to|{0}|{1}", transactionNo, orderInfo[0]);
                }
                else
                {
                    url = string.Format("~/Program/Prescription/PrescriptionEntry/PrescriptionEntryDetail.aspx?id=to|{0}|{1}", transactionNo, orderInfo[0]);
                }
                Response.Redirect(url);
            }
        }

        #region GetFilterExpressionTestOrder OLD
        //WR : di comment karena saat pilih belum di proses order2 yang sudah di proses juga muncul
        //public override string GetFilterExpressionTestOrder()
        //{
        //    string filterExpression = hdnFilterExpression.Value;

        //    if (filterExpression != "")
        //        filterExpression += " AND ";

        //    if (cboOrderResultType.Value.ToString() == "0")
        //    {
        //        filterExpression += string.Format("GCVisitStatus NOT IN ('{0}','{1}','{2}') AND GCTransactionStatus IN ('{3}','{4}')", Constant.VisitStatus.CANCELLED, Constant.VisitStatus.CLOSED, Constant.VisitStatus.OPEN, Constant.TransactionStatus.WAIT_FOR_APPROVAL, Constant.TransactionStatus.PROCESSED);
        //    }
        //    else if (cboOrderResultType.Value.ToString() == "1")
        //    {
        //        filterExpression += string.Format("GCVisitStatus NOT IN ('{0}','{1}','{2}') AND GCTransactionStatus IN ('{3}')", Constant.VisitStatus.CANCELLED, Constant.VisitStatus.CLOSED, Constant.VisitStatus.OPEN, Constant.TransactionStatus.WAIT_FOR_APPROVAL);
        //    }
        //    else if (cboOrderResultType.Value.ToString() == "2")
        //    {
        //        filterExpression += string.Format(" GCVisitStatus NOT IN ('{0}','{1}','{2}') AND GCTransactionStatus IN ('{3}','{4}')", Constant.VisitStatus.CANCELLED, Constant.VisitStatus.CLOSED, Constant.VisitStatus.OPEN, Constant.TransactionStatus.APPROVED, Constant.TransactionStatus.PROCESSED);
        //    }
        //    else if (cboOrderResultType.Value.ToString() == "3")
        //    {
        //        filterExpression += string.Format(" GCVisitStatus NOT IN ('{0}','{1}','{2}') AND GCTransactionStatus IN ('{3}')", Constant.VisitStatus.CANCELLED, Constant.VisitStatus.CLOSED, Constant.VisitStatus.OPEN, Constant.TransactionStatus.WAIT_FOR_APPROVAL);
        //    }
        //    else
        //    {
        //        filterExpression += string.Format("PrescriptionDate = '{0}' AND GCVisitStatus NOT IN ('{2}','{3}','{4}') AND GCTransactionStatus IN ('{5}') AND GCOrderStatus IN ('{6}') ",
        //            Helper.GetDatePickerValue(txtTestOrderDate).ToString(Constant.FormatString.DATE_FORMAT_112),
        //            cboServiceUnitOrder.Value, Constant.VisitStatus.CANCELLED, Constant.VisitStatus.CLOSED, Constant.VisitStatus.OPEN, Constant.TransactionStatus.PROCESSED, Constant.TestOrderStatus.IN_PROGRESS);
        //    }

        //    if (cboServiceUnitOrder.Value.ToString() != "")
        //        filterExpression += string.Format(" AND DispensaryServiceUnitID = {0}", cboServiceUnitOrder.Value);
        //    if (cboDepartmentOrder.Value != null)
        //        filterExpression += string.Format(" AND DepartmentID = '{0}'", cboDepartmentOrder.Value);
        //    if (hdnServiceUnitIDOrder.Value != "0" && hdnServiceUnitIDOrder.Value != "")
        //        filterExpression += string.Format(" AND VisitHSUID = '{0}'", hdnServiceUnitIDOrder.Value);
        //    if (!chkIsPreviousEpisodePatientOrder.Checked)
        //    {
        //        filterExpression += string.Format(" AND PrescriptionDate = '{0}'", Helper.GetDatePickerValue(txtTestOrderDate).ToString(Constant.FormatString.DATE_FORMAT_112));
        //    }
        //    if (hdnFilterExpressionQuickSearchOrder.Value != "")
        //        filterExpression += string.Format(" AND {0}", hdnFilterExpressionQuickSearchOrder.Value);
        //    return filterExpression;
        //    //return "1 = 0";
        //}
        #endregion

        public override string GetFilterExpressionTestOrder()
        {
            string filterExpression = hdnFilterExpression.Value;

            if (filterExpression != "")
                filterExpression += " AND ";

            if (cboOrderResultType.Value.ToString() == "0")
            {
                filterExpression += string.Format("GCVisitStatus NOT IN ('{0}','{1}','{2}') AND GCTransactionStatus IN ('{3}','{4}')", Constant.VisitStatus.CANCELLED, Constant.VisitStatus.CLOSED, Constant.VisitStatus.OPEN, Constant.TransactionStatus.WAIT_FOR_APPROVAL, Constant.TransactionStatus.PROCESSED);
            }
            else if (cboOrderResultType.Value.ToString() == "1")
            {
                filterExpression += string.Format("GCVisitStatus NOT IN ('{0}','{1}','{2}') AND GCTransactionStatus = '{3}' AND ChargesGCTransactionStatus IS NULL", Constant.VisitStatus.CANCELLED, Constant.VisitStatus.CLOSED, Constant.VisitStatus.OPEN, Constant.TransactionStatus.WAIT_FOR_APPROVAL);
            }
            else if (cboOrderResultType.Value.ToString() == "2")
            {
                filterExpression += string.Format(" GCVisitStatus NOT IN ('{0}','{1}','{2}') AND GCTransactionStatus = '{3}' AND ChargesGCTransactionStatus = '{4}'", Constant.VisitStatus.CANCELLED, Constant.VisitStatus.CLOSED, Constant.VisitStatus.OPEN, Constant.TransactionStatus.PROCESSED, Constant.TransactionStatus.WAIT_FOR_APPROVAL);
            }
            else if (cboOrderResultType.Value.ToString() == "3")
            {
                filterExpression += string.Format(" GCVisitStatus NOT IN ('{0}','{1}','{2}') AND GCTransactionStatus = '{3}' AND ChargesGCTransactionStatus = '{4}'", Constant.VisitStatus.CANCELLED, Constant.VisitStatus.CLOSED, Constant.VisitStatus.OPEN, Constant.TransactionStatus.PROCESSED, Constant.TransactionStatus.OPEN);
            }
            else
            {
                filterExpression += string.Format("PrescriptionDate = '{0}' AND GCVisitStatus NOT IN ('{2}','{3}','{4}') AND GCTransactionStatus IN ('{5}') AND GCOrderStatus IN ('{6}') ",
                    Helper.GetDatePickerValue(txtTestOrderDate).ToString(Constant.FormatString.DATE_FORMAT_112),
                    cboServiceUnitOrder.Value, Constant.VisitStatus.CANCELLED, Constant.VisitStatus.CLOSED, Constant.VisitStatus.OPEN, Constant.TransactionStatus.PROCESSED, Constant.TestOrderStatus.IN_PROGRESS);
            }

            if (cboServiceUnitOrder.Value.ToString() != "")
                filterExpression += string.Format(" AND DispensaryServiceUnitID = {0}", cboServiceUnitOrder.Value);
            if (cboDepartmentOrder.Value != null && cboDepartmentOrder.Value.ToString() != "")
                filterExpression += string.Format(" AND DepartmentID = '{0}'", cboDepartmentOrder.Value);
            if (hdnServiceUnitIDOrder.Value != "0" && hdnServiceUnitIDOrder.Value != "")
                filterExpression += string.Format(" AND VisitHSUID = '{0}'", hdnServiceUnitIDOrder.Value);
            if (hdnRegistrationPayerOrder.Value != "0" && hdnRegistrationPayerOrder.Value != "")
                filterExpression += string.Format(" AND GCCustomerType = '{0}'", hdnRegistrationPayerOrder.Value);
            if (!chkIsPreviousEpisodePatientOrder.Checked)
            {
                filterExpression += string.Format(" AND PrescriptionDate = '{0}'", Helper.GetDatePickerValue(txtTestOrderDate).ToString(Constant.FormatString.DATE_FORMAT_112));
            }
            if (hdnFilterExpressionQuickSearchOrder.Value != "")
                filterExpression += string.Format(" AND {0}", hdnFilterExpressionQuickSearchOrder.Value);
            return filterExpression;
            //return "1 = 0";
        }            

        public override string GetSortingTestOrder()
        {
            string sortBy = "TagProperty ASC";

            if (cboSortBy.Value.ToString() == "0")
            {
                sortBy += ", PrescriptionOrderID ASC";
            }
            else
            {
                sortBy += ", PrescriptionOrderID DESC";
            }

            return sortBy;
        }
    }
}