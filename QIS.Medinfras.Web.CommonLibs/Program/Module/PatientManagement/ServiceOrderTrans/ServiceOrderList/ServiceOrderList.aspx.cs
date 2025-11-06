using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.CommonLibs.MasterPage;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class ServiceOrderList : BasePagePatientOrder
    {
        protected int PageCount = 1;
        public override string OnGetMenuCode()
        {
            string id = Page.Request.QueryString["id"];
            switch (id)
            {
                case "eo": return Constant.MenuCode.EmergencyCare.SERVICE_ORDER_TRANS;
                case "op": return Constant.MenuCode.Outpatient.SERVICE_ORDER_TRANS;
                default: return Constant.MenuCode.EmergencyCare.SERVICE_ORDER_TRANS;
            }
            
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
        private string refreshGridInterval = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                SettingParameterDt setvarRealisasi = BusinessLayer.GetSettingParameterDt(AppSession.UserLogin.HealthcareID, Constant.SettingParameter.MENU_REALISASI_TAMPIL_HARGA_TIDAK);
                hdnSetVarMenuRealisasi.Value = setvarRealisasi.ParameterValue;

                refreshGridInterval = BusinessLayer.GetSettingParameter(Constant.SettingParameter.PATIENT_GRID_REFRESH_INTERVAL).ParameterValue;

                string id = Page.Request.QueryString["id"];

                #region Cbo To ServiceUnit
                if (id == "eo")
                {
                    trServiceUnit.Style.Add("display", "none");
                    trServiceUnitOrder.Style.Add("display", "none");
                }
                else
                {
                    List<GetServiceUnitUserList> lstServiceUnit = BusinessLayer.GetServiceUnitUserList(AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, Constant.Facility.OUTPATIENT, "IsUsingRegistration = 1");

                    Methods.SetComboBoxField<GetServiceUnitUserList>(cboToServiceUnit, lstServiceUnit, "ServiceUnitName", "HealthcareServiceUnitID");
                    Methods.SetComboBoxField<GetServiceUnitUserList>(cboToServiceUnitOrder, lstServiceUnit, "ServiceUnitName", "HealthcareServiceUnitID");
                    cboToServiceUnit.SelectedIndex = 1;
                    cboToServiceUnitOrder.SelectedIndex = 1;
                }
                #endregion

                List<Department> lstDepartment = null;
                if (id == "eo")
                {
                    vHealthcareServiceUnit HSU = BusinessLayer.GetvHealthcareServiceUnitList(string.Format("IsUsingRegistration = 1 AND DepartmentID = '{0}' AND HealthcareID = '{1}' AND IsDeleted = 0", Constant.Facility.EMERGENCY, AppSession.UserLogin.HealthcareID)).FirstOrDefault();
                    hdnToServiceUnitID.Value = HSU.HealthcareServiceUnitID.ToString();
                    lstDepartment = BusinessLayer.GetDepartmentList(string.Format("IsHasRegistration = 1 AND DepartmentID IN ('{0}','{1}','{2}','{3}')", Constant.Facility.OUTPATIENT, Constant.Facility.INPATIENT, Constant.Facility.DIAGNOSTIC, Constant.Facility.MEDICAL_CHECKUP)); 
                }
                else
                    lstDepartment = BusinessLayer.GetDepartmentList(string.Format("IsHasRegistration = 1 AND DepartmentID IN ('{0}','{1}','{2}','{3}')", Constant.Facility.EMERGENCY, Constant.Facility.INPATIENT, Constant.Facility.DIAGNOSTIC, Constant.Facility.MEDICAL_CHECKUP));
                Methods.SetComboBoxField<Department>(cboDepartment, lstDepartment, "DepartmentName", "DepartmentID");

                lstDepartment.Insert(0, new Department { DepartmentID = "", DepartmentName = "" });
                Methods.SetComboBoxField<Department>(cboDepartmentOrder, lstDepartment, "DepartmentName", "DepartmentID");
                cboDepartment.SelectedIndex = 0;
                cboDepartmentOrder.SelectedIndex = 0;

                List<StandardCode> lstCheckIn = new List<StandardCode>();
                FillComboBoxForFilter(lstCheckIn);
                Methods.SetComboBoxField<StandardCode>(cboCheckinStatus, lstCheckIn, "StandardCodeName", "StandardCodeID");
                cboCheckinStatus.SelectedIndex = 0;

                List<Variable> lstVariable = new List<Variable>();
                lstVariable.Add(new Variable { Code = "0", Value = "Semua" });
                lstVariable.Add(new Variable { Code = "1", Value = "Belum Diproses" });
                lstVariable.Add(new Variable { Code = "2", Value = "Sudah Diproses" });
                Methods.SetComboBoxField<Variable>(cboOrderResultType, lstVariable, "Value", "Code");
                cboOrderResultType.Value = "1";

                txtDate.Text = txtTestOrderDate.Text = DateTime.Today.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                grdRegisteredPatient.InitializeControl();
                grdOrderPatient.InitializeControl();

                Helper.SetControlEntrySetting(txtDate, new ControlEntrySetting(true, true, false), "mpPatientList");
                Helper.SetControlEntrySetting(cboDepartment, new ControlEntrySetting(true, true, false), "mpPatientList");
                Helper.SetControlEntrySetting(txtTestOrderDate, new ControlEntrySetting(true, true, false), "mpPatientListOrder");
                Helper.SetControlEntrySetting(cboDepartmentOrder, new ControlEntrySetting(true, true, false), "mpPatientListOrder");

                menu = ((MPMain)Master).ListMenu.FirstOrDefault(p => p.MenuCode == OnGetMenuCode());
            }
        }

        private void FillComboBoxForFilter(List<StandardCode> lstCheckIn)
        {
            lstCheckIn.Add(new StandardCode() { StandardCodeID = "1", StandardCodeName = "Masih Dirawat" });
            string id = Page.Request.QueryString["id"];
            lstCheckIn.Add(new StandardCode() { StandardCodeID = "2", StandardCodeName = "Sudah Pulang" });
        }

        public override void LoadAllWords()
        {
            LoadWords();
        }

        #region Registration
        public override string GetFilterExpression()
        {
            string filterExpression = "";
            //string filterExpression = string.Format("GCVisitStatus NOT IN ('{0}','{1}','{2}','{3}')",
            //    Constant.VisitStatus.OPEN, Constant.VisitStatus.CANCELLED, Constant.VisitStatus.DISCHARGED, Constant.VisitStatus.CLOSED);

            if (cboDepartment.Value.ToString() != Constant.Facility.INPATIENT)
            {
                filterExpression += string.Format("GCVisitStatus NOT IN ('{0}','{1}','{2}')",
                    Constant.VisitStatus.OPEN, Constant.VisitStatus.CANCELLED, Constant.VisitStatus.CLOSED);
            }
            else
            {
                if (cboCheckinStatus.Value.ToString() == "1")
                {
                    filterExpression = string.Format("GCVisitStatus NOT IN ('{0}','{1}','{2}','{3}')",
                        Constant.VisitStatus.CANCELLED, Constant.VisitStatus.CLOSED, Constant.VisitStatus.DISCHARGED, Constant.VisitStatus.OPEN);
                }
                else if (cboCheckinStatus.Value.ToString() == "2")
                {
                    filterExpression += string.Format("GCVisitStatus = '{0}'", Constant.VisitStatus.DISCHARGED);
                }
                else
                {
                    filterExpression += string.Format("GCVisitStatus NOT IN ('{0}','{1}','{2}')",
                        Constant.VisitStatus.OPEN, Constant.VisitStatus.CANCELLED, Constant.VisitStatus.CLOSED);
                }
            }

            if (cboDepartment.Value != null && cboDepartment.Value.ToString() != Constant.Facility.INPATIENT)
            {
                if (!chkIsPreviousEpisodePatientReg.Checked)
                {
                    filterExpression += string.Format(" AND VisitDate = '{0}'", Helper.GetDatePickerValue(txtDate).ToString(Constant.FormatString.DATE_FORMAT_112));
                }
            }

            if (hdnFromServiceUnitID.Value != "0" && hdnFromServiceUnitID.Value != "")
                filterExpression += string.Format(" AND HealthcareServiceUnitID = {0}", hdnFromServiceUnitID.Value);
            else if (cboDepartment.Value != null && cboDepartment.Value.ToString() != "")
                filterExpression += string.Format(" AND DepartmentID = '{0}'", cboDepartment.Value);
            else
            {
                string id = Page.Request.QueryString["id"];
                switch (id)
                {
                    case "eo": filterExpression += string.Format(" AND DepartmentID IN ('{0}','{1}')", Constant.Facility.INPATIENT, Constant.Facility.OUTPATIENT); break;
                    case "op": filterExpression += string.Format(" AND DepartmentID IN ('{0}','{1}')", Constant.Facility.INPATIENT, Constant.Facility.EMERGENCY); break;
                }
            }

            if (hdnFilterExpressionQuickSearchReg.Value != "")
                filterExpression += string.Format(" AND {0}", hdnFilterExpressionQuickSearchReg.Value);
            return filterExpression;
        }

        public override void OnGrdRowClick(string transactionNo)
        {
            string id = Page.Request.QueryString["id"];
            string serviceUnitID = "";
            vConsultVisit entity = BusinessLayer.GetvConsultVisitList(string.Format("VisitID = {0}", transactionNo))[0];
            RegisteredPatient pt = new RegisteredPatient();
            pt.MRN = entity.MRN;
            pt.MedicalNo = entity.MedicalNo;
            pt.RegistrationID = entity.RegistrationID;
            pt.VisitID = entity.VisitID;
            pt.VisitDate = entity.VisitDate;
            pt.VisitTime = entity.VisitTime;
            pt.ParamedicID = entity.ParamedicID;
            pt.SpecialtyID = entity.SpecialtyID;
            pt.HealthcareServiceUnitID = entity.HealthcareServiceUnitID;
            pt.ServiceUnitName = entity.ServiceUnitName;
            pt.RoomCode = entity.RoomCode;
            pt.BedCode = entity.BedCode;
            pt.DepartmentID = entity.DepartmentID;
            pt.ClassID = entity.ClassID;
            AppSession.RegisteredPatient = pt;
            if (id == "eo")
                serviceUnitID = hdnToServiceUnitID.Value;
            else
                serviceUnitID = cboToServiceUnit.Value.ToString();

            string url = "";

            if (hdnSetVarMenuRealisasi.Value == "0")
            {
                url = string.Format("~/Libs/Program/Module/PatientManagement/TransactionPage/PatientUse/PatientManagementTransactionUseDetail.aspx?id={0}|{1}", transactionNo, serviceUnitID);
            }
            else
            {
                url = string.Format("~/Libs/Program/Module/PatientManagement/Transaction/PatientManagementTransactionDetail.aspx?id={0}|{1}", transactionNo, serviceUnitID);
            }

            Response.Redirect(url);
        }
        #endregion

        #region Order
        public override void OnGrdRowClickTestOrder(string transactionNo, string serviceOrderID)
        {
            string id = Page.Request.QueryString["id"];
            vConsultVisit entity = BusinessLayer.GetvConsultVisitList(string.Format("VisitID = {0}", transactionNo))[0];
            RegisteredPatient pt = new RegisteredPatient();
            pt.MRN = entity.MRN;
            pt.MedicalNo = entity.MedicalNo;
            pt.RegistrationID = entity.RegistrationID;
            pt.VisitID = entity.VisitID;
            pt.VisitDate = entity.VisitDate;
            pt.VisitTime = entity.VisitTime;
            pt.ParamedicID = entity.ParamedicID;
            pt.SpecialtyID = entity.SpecialtyID;
            pt.HealthcareServiceUnitID = entity.HealthcareServiceUnitID;
            pt.ServiceUnitName = entity.ServiceUnitName;
            pt.RoomCode = entity.RoomCode;
            pt.BedCode = entity.BedCode;
            pt.DepartmentID = entity.DepartmentID;
            pt.ClassID = entity.ClassID;
            AppSession.RegisteredPatient = pt;
            string type = "";
            if (id == "eo")
                type = "soer";
            else
                type = "soop";

            string url = "";

            if (hdnSetVarMenuRealisasi.Value == "0")
            {
                url = string.Format("~/Libs/Program/Module/PatientManagement/TransactionPage/PatientUse/PatientManagementTransactionUseDetail.aspx?id={0}|{1}|{2}", type, serviceOrderID, transactionNo);
            }
            else
            {
                url = string.Format("~/Libs/Program/Module/PatientManagement/Transaction/PatientManagementTransactionDetail.aspx?id={0}|{1}|{2}", type, serviceOrderID, transactionNo);
            }

            Response.Redirect(url);
        }

        public override string GetFilterExpressionTestOrder()
        {
            string id = Page.Request.QueryString["id"];
            string filterExpression = hdnFilterExpression.Value;   
         
            if (filterExpression != "")
                filterExpression += " AND ";

            //filterExpression += string.Format("GCVisitStatus NOT IN ('{0}','{1}','{2}','{3}','{4}')",
            //    Constant.VisitStatus.OPEN, Constant.VisitStatus.CANCELLED, Constant.VisitStatus.DISCHARGED, Constant.VisitStatus.PHYSICIAN_DISCHARGE, Constant.VisitStatus.CLOSED);

            filterExpression += string.Format("GCVisitStatus NOT IN ('{0}','{1}','{2}')",
                Constant.VisitStatus.OPEN, Constant.VisitStatus.CANCELLED, Constant.VisitStatus.CLOSED);

            if (id == "eo")
            {
                filterExpression += string.Format(" AND HealthcareServiceUnitID = {0}", hdnToServiceUnitID.Value);
            }
            else
            {
                filterExpression += string.Format(" AND HealthcareServiceUnitID = {0}", cboToServiceUnitOrder.Value);
            }

            if (!chkIsPreviousEpisodePatientOrder.Checked)
            {
                filterExpression += string.Format(" AND ServiceOrderDate = '{0}'", Helper.GetDatePickerValue(txtTestOrderDate).ToString(Constant.FormatString.DATE_FORMAT_112));
            }

            string orderResultType = cboOrderResultType.Value.ToString();
            if (orderResultType == "1")
            {
                filterExpression += string.Format(" AND GCTransactionStatus = '{0}'", Constant.TransactionStatus.WAIT_FOR_APPROVAL);
            }
            else if (orderResultType == "2")
            {
                filterExpression += string.Format(" AND GCTransactionStatus = '{0}'", Constant.TransactionStatus.PROCESSED);
            }
            else
            {
                filterExpression += string.Format(" AND GCTransactionStatus IN ('{0}','{1}')", Constant.TransactionStatus.WAIT_FOR_APPROVAL, Constant.TransactionStatus.PROCESSED);
            }

            if (hdnServiceUnitIDOrder.Value != "0" && hdnServiceUnitIDOrder.Value != "")
            {
                filterExpression += string.Format(" AND VisitHSUID = {0}", hdnServiceUnitIDOrder.Value);
            }
            else if (cboDepartmentOrder.Value != null && cboDepartmentOrder.Value.ToString() != "")
            {
                filterExpression += string.Format(" AND DepartmentID = '{0}'", cboDepartmentOrder.Value);
            }
            else
            {
                switch (id)
                {
                    case "eo": filterExpression += string.Format(" AND DepartmentID IN ('{0}','{1}','{2}','{3}')", Constant.Facility.INPATIENT, Constant.Facility.OUTPATIENT, Constant.Facility.DIAGNOSTIC, Constant.Facility.MEDICAL_CHECKUP); break;
                    case "op": filterExpression += string.Format(" AND DepartmentID IN ('{0}','{1}','{2}','{3}')", Constant.Facility.INPATIENT, Constant.Facility.EMERGENCY, Constant.Facility.DIAGNOSTIC, Constant.Facility.MEDICAL_CHECKUP); break;
                }
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
        #endregion
    }
}