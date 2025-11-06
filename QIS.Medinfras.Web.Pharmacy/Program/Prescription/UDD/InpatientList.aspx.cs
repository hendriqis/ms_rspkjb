using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.CommonLibs.MasterPage;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.Pharmacy.Program
{
    public partial class InpatientList : BasePagePatientOrder
    {
        protected int PageCount = 1;
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Pharmacy.UDD_PRESCRIPTION_ENTRY;
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

                string filterExpression = string.Format("IsInpatientDispensary = 1");
                List<GetServiceUnitUserList> lstServiceUnit = BusinessLayer.GetServiceUnitUserList(AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, Constant.Facility.PHARMACY, filterExpression);
                Methods.SetComboBoxField<GetServiceUnitUserList>(cboServiceUnit, lstServiceUnit, "ServiceUnitName", "HealthcareServiceUnitID");
                cboServiceUnit.SelectedIndex = 0;

                string deptFilter = string.Format("IsActive = 1 AND DepartmentID = '{0}'", Constant.Facility.INPATIENT);

                List<Department> lstDepartment = BusinessLayer.GetDepartmentList(deptFilter);
                Methods.SetComboBoxField<Department>(cboDepartment, lstDepartment, "DepartmentName", "DepartmentID");
                cboDepartment.Value = Constant.Facility.INPATIENT.ToString();

                List<StandardCode> lstRegistrationPayer = BusinessLayer.GetStandardCodeList(string.Format("IsActive = 1 AND IsDeleted = 0 AND ParentID = '{0}'", Constant.StandardCode.CUSTOMER_TYPE));
                lstRegistrationPayer.Insert(0, new StandardCode { StandardCodeName = string.Format("{0}", GetLabel(" ")) });
                Methods.SetComboBoxField<StandardCode>(cboRegistrationPayer, lstRegistrationPayer, "StandardCodeName", "StandardCodeID");
                cboRegistrationPayer.SelectedIndex = 0;

                List<Variable> lstRegistrationStatus = new List<Variable>();
                lstRegistrationStatus.Add(new Variable() { Code = "1", Value = "Masih Dirawat" });
                lstRegistrationStatus.Add(new Variable() { Code = "2", Value = "Rencana Pulang" });
                lstRegistrationStatus.Add(new Variable() { Code = "3", Value = "Sudah Pulang" });
                Methods.SetComboBoxField<Variable>(cboCheckinStatus, lstRegistrationStatus, "Value", "Code");
                cboCheckinStatus.SelectedIndex = 0;


                filterExpression = string.Format("ParentID IN ('{0}') AND IsActive = 1 AND IsDeleted = 0", Constant.StandardCode.PRESCRIPTION_TYPE);
                List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(filterExpression);
                lstStandardCode.Insert(0, new StandardCode { StandardCodeID = "", StandardCodeName = "" });

                List<StandardCode> lstPrescriptionType = new List<StandardCode>();
                if (!AppSession.IsUsedInpatientPrescriptionTypeFilter)
                {
                    lstPrescriptionType = lstStandardCode.Where(p => p.ParentID == Constant.StandardCode.PRESCRIPTION_TYPE || p.StandardCodeID == "").ToList();
                }
                else
                {
                    if (!string.IsNullOrEmpty(AppSession.InpatientPrescriptionTypeFilter))
                    {
                        string[] prescriptionType = AppSession.InpatientPrescriptionTypeFilter.Split(',');
                        lstPrescriptionType = lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.PRESCRIPTION_TYPE || sc.StandardCodeID == "").Where(x => !prescriptionType.Contains(x.StandardCodeID)).ToList();
                    }
                    else
                    {
                        lstPrescriptionType = lstStandardCode.Where(p => p.ParentID == Constant.StandardCode.PRESCRIPTION_TYPE || p.StandardCodeID == "").ToList();
                    }
                }

                Methods.SetComboBoxField<StandardCode>(cboPrescriptionType, lstPrescriptionType, "StandardCodeName", "StandardCodeID");

                if (lstStandardCode.Where(p => p.IsDefault).ToList().Count() > 0)
                {
                    cboPrescriptionType.Value = lstPrescriptionType.Where(p => p.IsDefault && p.IsActive && !p.IsDeleted).FirstOrDefault().StandardCodeID;
                }
                else
                {
                    cboPrescriptionType.SelectedIndex = 0;
                }

                List<Variable> lstSortBy = new List<Variable>();
                lstSortBy.Add(new Variable { Code = "0", Value = "No. Order Resep (ASC)" });
                lstSortBy.Add(new Variable { Code = "1", Value = "No. Order Resep (DESC)" });
                Methods.SetComboBoxField<Variable>(cboSortBy, lstSortBy, "Value", "Code");
                cboSortBy.Value = "0";

                SettingControlProperties();

                txtOrderDate.Text = DateTime.Today.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                grdOrderPatient.InitializeControl();
                grdRegisteredPatient.InitializeControl();

                Helper.SetControlEntrySetting(cboDepartment, new ControlEntrySetting(true, true, false), "mpPatientList");

                menu = ((MPMain)Master).ListMenu.FirstOrDefault(p => p.MenuCode == OnGetMenuCode());
            }
        }

        private void SettingControlProperties()
        {
            if (AppSession.LastContentUDDList != null)
            {
                LastContentPrescriptionEntry lc = AppSession.LastContentUDDList;
                hdnLastContentID.Value = lc.ContentID;
                cboServiceUnit.Value = lc.HealthcareServiceUnitID.ToString();
                cboDepartment.Value = lc.FromDepartmentID;
                hdnServiceUnitID.Value = lc.FromHealthcareServiceUnitID.ToString();
                vHealthcareServiceUnit entityHealthcareServiceUnit = BusinessLayer.GetvHealthcareServiceUnitList(string.Format("HealthcareServiceUnitID = {0}", hdnServiceUnitID.Value)).FirstOrDefault();
                txtServiceUnitCode.Text = entityHealthcareServiceUnit == null ? "" : entityHealthcareServiceUnit.ServiceUnitCode;
                txtServiceUnitName.Text = entityHealthcareServiceUnit == null ? "" : entityHealthcareServiceUnit.ServiceUnitName;
                hdnQuickTextReg.Value = lc.QuickText.Equals("Search") ? "" : lc.QuickText;
                hdnFilterExpressionQuickSearchReg.Value = lc.QuickFilterExpression;
            }
            else
            {
                cboDepartment.Value = Constant.Facility.INPATIENT;
            }

            List<Variable> lstVariable1 = new List<Variable>();
            lstVariable1.Add(new Variable { Code = "0", Value = "Semua" });
            lstVariable1.Add(new Variable { Code = "1", Value = "Belum Diproses" });
            lstVariable1.Add(new Variable { Code = "2", Value = "Sudah Diproses" });
            Methods.SetComboBoxField<Variable>(cboOrderResultType, lstVariable1, "Value", "Code");
            cboOrderResultType.Value = "1";

            List<Variable> lstVariable2 = new List<Variable>();
            lstVariable2.Add(new Variable { Code = "0", Value = "Semua" });
            lstVariable2.Add(new Variable { Code = "1", Value = "Belum Dikonfirmasi" });
            lstVariable2.Add(new Variable { Code = "2", Value = "Sudah Dikonfirmasi" });
            Methods.SetComboBoxField<Variable>(cboOrderStatusType, lstVariable2, "Value", "Code");
            cboOrderStatusType.Value = "1";
        }

        public override string GetFilterExpression()
        {
            string filterExpression = hdnFilterExpression.Value;

            if (filterExpression != "")
                filterExpression += " AND ";

            if (cboDepartment.Value != null && cboDepartment.Value.ToString() == "INPATIENT")
            {
                filterExpression += string.Format("DepartmentID = '{0}'", cboDepartment.Value);

                if (cboCheckinStatus.Value.ToString() == "1")
                {
                    if (filterExpression != "")
                    {
                        filterExpression += " AND ";
                    }
                    filterExpression += string.Format("GCVisitStatus NOT IN ('{0}','{1}','{2}','{3}')",
                        Constant.VisitStatus.CANCELLED, Constant.VisitStatus.CLOSED, Constant.VisitStatus.DISCHARGED, Constant.VisitStatus.OPEN);
                }
                else if (cboCheckinStatus.Value.ToString() == "2")
                {
                    if (filterExpression != "")
                    {
                        filterExpression += " AND ";
                    }
                    filterExpression += string.Format("GCVisitStatus NOT IN ('{0}','{1}','{2}','{3}') AND IsPlanDischarge = 1",
                        Constant.VisitStatus.CANCELLED, Constant.VisitStatus.CLOSED, Constant.VisitStatus.DISCHARGED, Constant.VisitStatus.OPEN);
                }
                else if (cboCheckinStatus.Value.ToString() == "3")
                {
                    if (filterExpression != "")
                    {
                        filterExpression += " AND ";
                    }
                    filterExpression += string.Format("GCVisitStatus = '{0}'", Constant.VisitStatus.DISCHARGED);
                }

                if (hdnLastContentID.Value == "containerCPPT")
                {
                    filterExpression += string.Format(" AND VisitID IN (SELECT VisitID FROM PatientVisitNote WITH (NOLOCK) WHERE IsNeedNotification = 1 AND NotificationParamedicID IS NULL AND NotificationUnitID = {0} AND IsDeleted = 0) ", cboServiceUnit.Value);
                }
            }

            if (hdnServiceUnitID.Value != "0" && hdnServiceUnitID.Value != "")
                filterExpression += string.Format(" AND HealthcareServiceUnitID = {0}", hdnServiceUnitID.Value);

            if (hdnRegistrationPayer.Value != "0" && hdnRegistrationPayer.Value != "" && hdnRegistrationPayer.Value != null)
                filterExpression += string.Format(" AND GCCustomerType = '{0}'", hdnRegistrationPayer.Value);

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

                AppSession.HealthcareServiceUnitID = cboServiceUnit.Value.ToString();

                LastContentPrescriptionEntry lc = new LastContentPrescriptionEntry()
                {
                    ContentID = hdnLastContentID.Value,
                    HealthcareServiceUnitID = Convert.ToInt32(cboServiceUnit.Value),
                    FromDepartmentID = cboDepartment.Value != null ? cboDepartment.Value.ToString() : "",
                    FromHealthcareServiceUnitID = hdnServiceUnitID.Value == "" ? 0 : Convert.ToInt32(hdnServiceUnitID.Value),
                    CustomerType = hdnRegistrationPayer.Value,
                    QuickText = txtSearchViewReg.Text,
                    QuickFilterExpression = hdnFilterExpressionQuickSearchReg.Value,
                    VariableDisplay = "0"
                };
                AppSession.LastContentUDDList = lc;
                string url = "";
                url = string.Format("~/Program/Prescription/UDD/MedicationOrder/UDDMedicationOrderEntry.aspx");
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

                LastContentPrescriptionEntry lc = new LastContentPrescriptionEntry()
                {
                    ContentID = hdnLastContentID.Value,
                    HealthcareServiceUnitID = Convert.ToInt32(cboServiceUnit.Value),
                    FromDepartmentID = cboDepartment.Value != null ? cboDepartment.Value.ToString() : "",
                    FromHealthcareServiceUnitID = hdnServiceUnitID.Value == "" ? 0 : Convert.ToInt32(hdnServiceUnitID.Value),
                    CustomerType = hdnRegistrationPayer.Value,
                    QuickText = txtSearchViewReg.Text,
                    QuickFilterExpression = hdnFilterExpressionQuickSearchReg.Value,
                    VariableDisplay = "0"
                };
                AppSession.LastContentUDDList = lc;
                string url = "";
                string[] orderInfo = prescriptionOrderID.Split('|');
                url = string.Format("~/Program/Prescription/UDD/MedicationOrder/UDDMedicationOrderEntry.aspx?id=to|{0}|{1}", transactionNo, orderInfo[0]);
                Response.Redirect(url);
            }
        }

        public override string GetFilterExpressionTestOrder()
        {
            string filterExpression = hdnFilterExpression.Value;

            if (filterExpression != "")
                filterExpression += " AND ";

            if (cboDepartment.Value != null && cboDepartment.Value.ToString() == "INPATIENT")
                filterExpression += string.Format("DepartmentID = '{0}'", cboDepartment.Value);

            filterExpression += string.Format(" AND DispensaryServiceUnitID = {0}", cboServiceUnit.Value);

            if (cboCheckinStatus.Value.ToString() == "1")
            {
                if (filterExpression != "")
                {
                    filterExpression += " AND ";
                }
                filterExpression += string.Format("GCVisitStatus NOT IN ('{0}','{1}','{2}','{3}')",
                    Constant.VisitStatus.CANCELLED, Constant.VisitStatus.CLOSED, Constant.VisitStatus.DISCHARGED, Constant.VisitStatus.OPEN);
            }
            else if (cboCheckinStatus.Value.ToString() == "2")
            {
                if (filterExpression != "")
                {
                    filterExpression += " AND ";
                }
                filterExpression += string.Format("GCVisitStatus NOT IN ('{0}','{1}','{2}','{3}') AND IsPlanDischarge = 1",
                    Constant.VisitStatus.CANCELLED, Constant.VisitStatus.CLOSED, Constant.VisitStatus.DISCHARGED, Constant.VisitStatus.OPEN);
            }
            else if (cboCheckinStatus.Value.ToString() == "3")
            {
                if (filterExpression != "")
                {
                    filterExpression += " AND ";
                }
                filterExpression += string.Format("GCVisitStatus = '{0}'", Constant.VisitStatus.DISCHARGED);
            }

            if (hdnServiceUnitID.Value != "0" && hdnServiceUnitID.Value != "")
                filterExpression += string.Format(" AND VisitHSUID = {0}", hdnServiceUnitID.Value);

            if (hdnRegistrationPayer.Value != "0" && hdnRegistrationPayer.Value != "" && hdnRegistrationPayer.Value != null)
                filterExpression += string.Format(" AND GCCustomerType = '{0}'", hdnRegistrationPayer.Value);

            if (hdnFilterExpressionQuickSearchReg.Value != "")
                filterExpression += string.Format(" AND {0}", hdnFilterExpressionQuickSearchReg.Value);

            if (cboPrescriptionType.Value != null)
            {
                filterExpression += string.Format(" AND GCPrescriptionType = '{0}'", cboPrescriptionType.Value.ToString());
            }

            if (cboOrderResultType.Value.ToString() == "0")
            {
                filterExpression += string.Format(" AND PrescriptionDate = '{0}' AND GCOrderStatus IN ('{1}','{2}','{3}')",
                    Helper.GetDatePickerValue(txtOrderDate).ToString(Constant.FormatString.DATE_FORMAT_112), Constant.OrderStatus.RECEIVED, Constant.OrderStatus.IN_PROGRESS, Constant.OrderStatus.COMPLETED);
            }
            else if (cboOrderResultType.Value.ToString() == "1")
            {
                filterExpression += string.Format(" AND PrescriptionDate = '{0}' AND GCOrderStatus = '{1}'",
                    Helper.GetDatePickerValue(txtOrderDate).ToString(Constant.FormatString.DATE_FORMAT_112), Constant.OrderStatus.RECEIVED);
            }
            else
            {
                filterExpression += string.Format(" AND PrescriptionDate = '{0}' AND GCOrderStatus IN ('{1}','{2}')",
                    Helper.GetDatePickerValue(txtOrderDate).ToString(Constant.FormatString.DATE_FORMAT_112), Constant.OrderStatus.IN_PROGRESS, Constant.OrderStatus.COMPLETED);
            }

            if (cboOrderStatusType.Value.ToString() == "0")
            {
                filterExpression += string.Format(" AND PrescriptionDate = '{0}' AND GCTransactionStatus IN ('{1}','{2}','{3}')",
                    Helper.GetDatePickerValue(txtOrderDate).ToString(Constant.FormatString.DATE_FORMAT_112), Constant.TransactionStatus.WAIT_FOR_APPROVAL, Constant.TransactionStatus.APPROVED, Constant.TransactionStatus.PROCESSED);
            }
            else if (cboOrderStatusType.Value.ToString() == "1")
            {
                filterExpression += string.Format(" AND PrescriptionDate = '{0}' AND GCTransactionStatus = '{1}'",
                    Helper.GetDatePickerValue(txtOrderDate).ToString(Constant.FormatString.DATE_FORMAT_112), Constant.TransactionStatus.WAIT_FOR_APPROVAL);
            }
            else
            {
                filterExpression += string.Format(" AND PrescriptionDate = '{0}' AND GCTransactionStatus IN ('{1}','{2}')",
                    Helper.GetDatePickerValue(txtOrderDate).ToString(Constant.FormatString.DATE_FORMAT_112), Constant.TransactionStatus.APPROVED, Constant.TransactionStatus.PROCESSED);
            }

            return filterExpression;
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