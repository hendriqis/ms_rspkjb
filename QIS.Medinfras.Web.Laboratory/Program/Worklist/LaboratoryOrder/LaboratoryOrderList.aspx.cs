using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.CommonLibs.MasterPage;
using DevExpress.Web.ASPxCallbackPanel;
using System.Globalization;

namespace QIS.Medinfras.Web.Laboratory.Program
{
    public partial class LaboratoryOrderList : BasePagePatientOrder
    {
        protected int PageCount = 1;
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Laboratory.WORK_LIST_DETAIL;
        }
        private GetUserMenuAccess menu;
        public override bool IsShowRightPanel()
        {
            return true;
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
        private string refreshGridInterval = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                GetSettingParameter();

                refreshGridInterval = BusinessLayer.GetSettingParameter(Constant.SettingParameter.LB_INTERVAL_AUTO_REFRESH).ParameterValue;
                hdnID.Value = AppSession.MedicalDiagnostic.HealthcareServiceUnitID.ToString();
                hdnIDReg.Value = AppSession.MedicalDiagnostic.HealthcareServiceUnitID.ToString();

                hdnLastContentID.Value = "containerByOrder";

                string filterExpression = string.Format("IsUsingRegistration = 1 AND HealthcareID = '{0}' AND IsLaboratoryUnit = 1 AND IsDeleted = 0", AppSession.UserLogin.HealthcareID);

                List<vHealthcareServiceUnitCustom> lstHServiceUnit = BusinessLayer.GetvHealthcareServiceUnitCustomList(filterExpression);
                Methods.SetComboBoxField<vHealthcareServiceUnitCustom>(cboServiceUnit, lstHServiceUnit, "ServiceUnitName", "HealthcareServiceUnitID");
                Methods.SetComboBoxField<vHealthcareServiceUnitCustom>(cboServiceUnitOrder, lstHServiceUnit, "ServiceUnitName", "HealthcareServiceUnitID");
                cboServiceUnit.SelectedIndex = cboServiceUnitOrder.SelectedIndex = 0;

                List<Variable> lstVariable = new List<Variable>();
                lstVariable.Add(new Variable { Code = "0", Value = "Semua" });
                lstVariable.Add(new Variable { Code = "1", Value = "Belum Diproses" });
                lstVariable.Add(new Variable { Code = "2", Value = "Sudah Diproses" });
                Methods.SetComboBoxField<Variable>(cboOrderResultType, lstVariable, "Value", "Code");
                cboOrderResultType.Value = "1";

                txtSearchViewReg.IntellisenseHints = Methods.LoadRegistrationWorklistQuickFilterHints("1");
                txtSearchViewOrder.IntellisenseHints = Methods.LoadLaboratoryWorklistQuickFilterHints("1");

                List<Department> lstDepartment = BusinessLayer.GetDepartmentList(string.Format("IsHasRegistration = 1 AND IsActive = 1 AND DepartmentID != '{0}'", Constant.Facility.PHARMACY));
                List<Department> lstDepartmentOrder = BusinessLayer.GetDepartmentList(string.Format("IsHasRegistration = 1 AND IsActive = 1 AND DepartmentID != '{0}'", Constant.Facility.PHARMACY));
                lstDepartmentOrder.Insert(0, new Department {DepartmentName = string.Format("{0}", GetLabel(" ")) });

                Methods.SetComboBoxField<Department>(cboDepartment, lstDepartment, "DepartmentName", "DepartmentID");
                Methods.SetComboBoxField<Department>(cboDepartmentOrder, lstDepartmentOrder, "DepartmentName", "DepartmentID");
                cboDepartment.SelectedIndex = 0;
                cboDepartmentOrder.SelectedIndex = 0;

                List<StandardCode> lstCheckIn = new List<StandardCode>();
                FillComboBoxForFilter(lstCheckIn);
                Methods.SetComboBoxField<StandardCode>(cboCheckinStatus, lstCheckIn, "StandardCodeName", "StandardCodeID");
                cboCheckinStatus.SelectedIndex = 0;

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
            if (AppSession.LastContentLaboratoryRealization != null)
            {
                LastContentLaboratoryRealization lc = AppSession.LastContentLaboratoryRealization;
                if (lc.LabContentID.Equals("containerByOrder"))
                {
                    hdnLastContentID.Value = "containerByOrder";
                    cboDepartmentOrder.Value = lc.LabPatientFrom;
                    txtTestOrderDate.Text = DateTime.ParseExact(lc.LabDate,
                              Constant.FormatString.DATE_FORMAT_112,
                               CultureInfo.InvariantCulture).ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                    hdnQuickTextOrder.Value = lc.LabQuickFilter.Equals("Search") ? "" : lc.LabQuickFilter;
                    hdnFilterExpressionQuickSearchOrder.Value = lc.LabQuickFilterExpression;
                    cboOrderResultType.Value = lc.LabDisplayResult;
                }
                else
                {
                    hdnLastContentID.Value = "containerDaftar";
                    cboDepartment.Value = lc.LabPatientFrom;
                    txtDate.Text = DateTime.ParseExact(lc.LabDate,
                              Constant.FormatString.DATE_FORMAT_112,
                               CultureInfo.InvariantCulture).ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                    hdnQuickTextReg.Value = lc.LabQuickFilter.Equals("Search") ? "" : lc.LabQuickFilter;
                    hdnFilterExpressionQuickSearchReg.Value = lc.LabQuickFilterExpression;
                }
            }
        }

        private void FillComboBoxForFilter(List<StandardCode> lstCheckIn)
        {
            lstCheckIn.Add(new StandardCode() { StandardCodeID = "1", StandardCodeName = "Masih Dirawat" });
            string id = Page.Request.QueryString["id"];
            lstCheckIn.Add(new StandardCode() { StandardCodeID = "2", StandardCodeName = "Sudah Pulang" });
        }

        private void GetSettingParameter()
        {
            List<SettingParameterDt> lstSetvarDt = BusinessLayer.GetSettingParameterDtList(string.Format("ParameterCode IN ('{0}', '{1}')", Constant.SettingParameter.MENU_REALISASI_TAMPIL_HARGA_TIDAK, Constant.SettingParameter.MD_IS_REGISTRATION_OPENED_SHOW_IN_REGISTRATION_LIST));
            hdnSetVarMenuRealisasi.Value = lstSetvarDt.Where(w => w.ParameterCode == Constant.SettingParameter.MENU_REALISASI_TAMPIL_HARGA_TIDAK).FirstOrDefault().ParameterValue;
            hdnIsRegistrationOpenedAllowed.Value = lstSetvarDt.Where(w => w.ParameterCode == Constant.SettingParameter.MD_IS_REGISTRATION_OPENED_SHOW_IN_REGISTRATION_LIST).FirstOrDefault().ParameterValue;
        }

        public override string GetFilterExpression()
        {
            string filterExpression = hdnFilterExpression.Value;
            if (filterExpression != "")
                filterExpression += " AND ";

            if (hdnIsRegistrationOpenedAllowed.Value == "1")
            {
                if (cboDepartment.Value.ToString() != Constant.Facility.INPATIENT)
                {
                    filterExpression += string.Format("GCVisitStatus NOT IN ('{0}','{1}')",
                    Constant.VisitStatus.CANCELLED, Constant.VisitStatus.CLOSED);
                }
                else
                {
                    if (cboCheckinStatus.Value.ToString() == "1")
                    {
                        filterExpression += string.Format("GCVisitStatus NOT IN ('{0}','{1}','{2}')",
                            Constant.VisitStatus.CANCELLED, Constant.VisitStatus.CLOSED, Constant.VisitStatus.DISCHARGED);
                    }
                    else if (cboCheckinStatus.Value.ToString() == "2")
                    {
                        filterExpression += string.Format("GCVisitStatus = '{0}'", Constant.VisitStatus.DISCHARGED);
                    }
                    else
                    {
                        filterExpression = string.Format("GCVisitStatus NOT IN ('{0}','{1}')",
                        Constant.VisitStatus.CANCELLED, Constant.VisitStatus.CLOSED);
                    }
                }
            }
            else
            {
                if (cboDepartment.Value.ToString() != Constant.Facility.INPATIENT)
                {
                    filterExpression += string.Format("GCVisitStatus NOT IN ('{0}','{1}')",
                    Constant.VisitStatus.CANCELLED, Constant.VisitStatus.CLOSED);
                }
                else
                {
                    if (cboCheckinStatus.Value.ToString() == "1")
                    {
                        filterExpression += string.Format("GCVisitStatus NOT IN ('{0}','{1}','{2}')",
                            Constant.VisitStatus.CANCELLED, Constant.VisitStatus.CLOSED, Constant.VisitStatus.DISCHARGED);
                    }
                    else if (cboCheckinStatus.Value.ToString() == "2")
                    {
                        filterExpression += string.Format("GCVisitStatus = '{0}'", Constant.VisitStatus.DISCHARGED);
                    }
                    else
                    {
                        filterExpression = string.Format("GCVisitStatus NOT IN ('{0}','{1}','{2}')",
                        Constant.VisitStatus.OPEN, Constant.VisitStatus.CANCELLED, Constant.VisitStatus.CLOSED);
                    }
                }
            }
            
            if (cboDepartment.Value != null && cboDepartment.Value.ToString() != "INPATIENT")
            {
                if (!chkIsIgnoreDate.Checked)
                {
                    filterExpression += string.Format(" AND VisitDate = '{0}'", Helper.GetDatePickerValue(txtDate).ToString(Constant.FormatString.DATE_FORMAT_112));
                }
            }

            if (cboDepartment.Value != null)
            {
                if (hdnIsRegistrationOpenedAllowed.Value == "1")
                {
                    filterExpression += string.Format(" AND DepartmentID = '{0}'", Constant.Facility.OUTPATIENT);
                }
                else
                {
                    filterExpression += string.Format(" AND DepartmentID = '{0}'", cboDepartment.Value);
                }
            }
            else
            {
                filterExpression += string.Format(" AND DepartmentID = '{0}'", cboDepartment.Value);
            }

            if (hdnServiceUnitID.Value != "0" && hdnServiceUnitID.Value != "")
                filterExpression += string.Format(" AND HealthcareServiceUnitID = {0}", hdnServiceUnitID.Value);

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
            LastContentLaboratoryRealization lc = new LastContentLaboratoryRealization()
            {
                LabContentID = hdnLastContentID.Value,
                LabDate = Helper.GetDatePickerValue(txtDate.Text).ToString(Constant.FormatString.DATE_FORMAT_112),
                LabDisplayResult = "1",
                LabPatientFrom = cboDepartment.Value.ToString(),
                LabQuickFilter = txtSearchViewReg.Text,
                LabQuickFilterExpression = hdnFilterExpressionQuickSearchReg.Value
            };
            AppSession.LastContentLaboratoryRealization = lc;

            vConsultVisit4 entity = BusinessLayer.GetvConsultVisit4List(string.Format("VisitID = {0}", transactionNo)).FirstOrDefault();
            RegisteredPatient pt = new RegisteredPatient();
            pt.MRN = entity.MRN;
            pt.MedicalNo = entity.MedicalNo;
            pt.RegistrationID = entity.RegistrationID;
            pt.BusinessPartnerID = entity.BusinessPartnerID;
            pt.VisitID = entity.VisitID;
            pt.VisitDate = entity.VisitDate;
            pt.VisitTime = entity.VisitTime;
            pt.DischargeDate = entity.DischargeDate;
            pt.DischargeTime = entity.DischargeTime;
            pt.ParamedicID = entity.ParamedicID;
            pt.ParamedicCode = entity.ParamedicCode;
            pt.ParamedicName = entity.ParamedicName;
            pt.SpecialtyID = entity.SpecialtyID;
            pt.HealthcareServiceUnitID = entity.HealthcareServiceUnitID;
            pt.DepartmentID = entity.DepartmentID;
            pt.ClassID = entity.ClassID;
            pt.ChargeClassID = entity.ChargeClassID;
            pt.IsBillingReopen = entity.IsBillingReopen;
            pt.IsLockDown = entity.IsLockDown;
            AppSession.RegisteredPatient = pt;

            string url = "";
            if (hdnSetVarMenuRealisasi.Value == "0")
            {
                url = string.Format("~/Program/Worklist/LaboratoryOrder/LaboratoryOrderUseDetail.aspx?id={0}|{1}|{2}", transactionNo, hdnServiceUnitID.Value, hdnIDReg.Value);
            }
            else
            {
                url = string.Format("~/Program/Worklist/LaboratoryOrder/LaboratoryOrderDetail.aspx?id={0}|{1}|{2}", transactionNo, hdnServiceUnitID.Value, hdnIDReg.Value);
            }
            Response.Redirect(url);
        }

        public override void OnGrdRowClickTestOrder(string transactionNo, string TestOrderID)
        {
            string cboDepartmentOrderValue = "";

            if (cboDepartmentOrder.Value != null)
            {
                cboDepartmentOrderValue = cboDepartmentOrder.Value.ToString();
            }
            else
            {
                cboDepartmentOrderValue = "";
            }

            LastContentLaboratoryRealization lc = new LastContentLaboratoryRealization()
            {
                LabContentID = hdnLastContentID.Value,
                LabDate = Helper.GetDatePickerValue(txtTestOrderDate.Text).ToString(Constant.FormatString.DATE_FORMAT_112),
                LabDisplayResult = cboOrderResultType.Value.ToString(),
                LabPatientFrom = cboDepartmentOrderValue,
                LabQuickFilter = txtSearchViewOrder.Text,
                LabQuickFilterExpression = hdnFilterExpressionQuickSearchOrder.Value
            };
            AppSession.LastContentLaboratoryRealization = lc;

            vConsultVisit4 entity = BusinessLayer.GetvConsultVisit4List(string.Format("VisitID = {0}", transactionNo)).FirstOrDefault();
            RegisteredPatient pt = new RegisteredPatient();
            pt.MRN = entity.MRN;
            pt.MedicalNo = entity.MedicalNo;
            pt.RegistrationID = entity.RegistrationID;
            pt.BusinessPartnerID = entity.BusinessPartnerID;
            pt.VisitID = entity.VisitID;
            pt.VisitDate = entity.VisitDate;
            pt.VisitTime = entity.VisitTime;
            pt.DischargeDate = entity.DischargeDate;
            pt.DischargeTime = entity.DischargeTime;
            pt.ParamedicID = entity.ParamedicID;
            pt.ParamedicCode = entity.ParamedicCode;
            pt.ParamedicName = entity.ParamedicName;
            pt.SpecialtyID = entity.SpecialtyID;
            pt.HealthcareServiceUnitID = entity.HealthcareServiceUnitID;
            pt.DepartmentID = entity.DepartmentID;
            pt.ClassID = entity.ClassID;
            pt.ChargeClassID = entity.ChargeClassID;
            pt.IsBillingReopen = entity.IsBillingReopen;
            pt.IsLockDown = entity.IsLockDown;
            AppSession.RegisteredPatient = pt;

            string url = "";
            if (hdnSetVarMenuRealisasi.Value == "0")
            {
                url = string.Format("~/Program/Worklist/LaboratoryOrder/LaboratoryOrderUseDetail.aspx?id=to|{0}|{1}|{2}", transactionNo, TestOrderID, hdnID.Value);
            }
            else
            {
                url = string.Format("~/Program/Worklist/LaboratoryOrder/LaboratoryOrderDetail.aspx?id=to|{0}|{1}|{2}", transactionNo, TestOrderID, hdnID.Value);
            }
            Response.Redirect(url);
        }

        public override string GetFilterExpressionTestOrder()
        {
            string filterExpression = hdnFilterExpression.Value;
            if (filterExpression != "")
                filterExpression += " AND ";

            filterExpression += string.Format("GCVisitStatus NOT IN ('{0}','{1}','{2}')",
                Constant.VisitStatus.OPEN, Constant.VisitStatus.CANCELLED, Constant.VisitStatus.CLOSED);

            if (cboOrderResultType.Value.ToString() == "0")
            {
                filterExpression += string.Format(" AND GCTransactionStatus IN ('{0}','{1}','{2}')",
                    Constant.TransactionStatus.WAIT_FOR_APPROVAL, Constant.TransactionStatus.PROCESSED, Constant.TransactionStatus.CLOSED);
            }
            else if (cboOrderResultType.Value.ToString() == "1")
            {
                filterExpression += string.Format(" AND GCTransactionStatus = '{0}'",
                    Constant.TransactionStatus.WAIT_FOR_APPROVAL);
            }
            else
            {
                filterExpression += string.Format(" AND GCTransactionStatus IN ('{0}','{1}')",
                    Constant.TransactionStatus.PROCESSED, Constant.TransactionStatus.CLOSED);
            }

            if (hdnID.Value != "0" && hdnID.Value != "") filterExpression += String.Format(" AND HealthcareServiceUnitID = {0}", hdnID.Value);

            if (cboDepartmentOrder.Value != null)
                filterExpression += string.Format(" AND DepartmentID = '{0}'", cboDepartmentOrder.Value);

            if (hdnServiceUnitIDOrder.Value != "0" && hdnServiceUnitIDOrder.Value != "")
                filterExpression += string.Format(" AND VisitHSUID = '{0}'", hdnServiceUnitIDOrder.Value);

            if (hdnFilterExpressionQuickSearchOrder.Value != "")
                filterExpression += string.Format(" AND {0}", hdnFilterExpressionQuickSearchOrder.Value);

            if (!chkIgnoreDateOrder.Checked)
            {
                filterExpression += string.Format(" AND ScheduledDate = '{0}'", Helper.GetDatePickerValue(txtTestOrderDate).ToString(Constant.FormatString.DATE_FORMAT_112));
            }

            if (chkIsPathologicalAnatomyTest.Checked)
            {
                filterExpression += string.Format(" AND IsPathologicalAnatomyTest = 1");
            }

            filterExpression += string.Format(" AND IsAIOTransaction = 0");

            return filterExpression;
        }

        public override string GetSortingTestOrder()
        {
            string sortBy = "";

            return sortBy;
        }
    }
}