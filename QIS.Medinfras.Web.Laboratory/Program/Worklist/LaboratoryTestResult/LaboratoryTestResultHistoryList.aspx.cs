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
using QIS.Medinfras.Web.CommonLibs.Controls;

namespace QIS.Medinfras.Web.Laboratory.Program
{
    public partial class LaboratoryTestResultHistoryList : BasePageRegisteredPatient
    {
        protected int PageCount = 1;

        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Laboratory.LAB_RESULT;
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
                List<Variable> lstVariable = new List<Variable>();
                lstVariable.Add(new Variable { Code = "0", Value = "Semua" });
                lstVariable.Add(new Variable { Code = "1", Value = "Belum Ada Hasil" });
                lstVariable.Add(new Variable { Code = "2", Value = "Sudah Ada Hasil" });
                Methods.SetComboBoxField<Variable>(cboOrderResultType, lstVariable, "Value", "Code");
                cboOrderResultType.Value = "1";

                txtTransactionDateFrom.Text = DateTime.Today.AddMonths(-1).ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                txtTransactionDateTo.Text = DateTime.Today.ToString(Constant.FormatString.DATE_PICKER_FORMAT);

                Helper.SetControlEntrySetting(txtMRN, new ControlEntrySetting(true, true, false), "mpPatientList");
                Helper.SetControlEntrySetting(txtPatientName, new ControlEntrySetting(true, true, false), "mpPatientList");

                ((GridPatientResultCtl)grdPatientResult).InitializeControl();
                menu = ((MPMain)Master).ListMenu.FirstOrDefault(p => p.MenuCode == OnGetMenuCode());

                refreshGridInterval = BusinessLayer.GetSettingParameter(Constant.SettingParameter.PATIENT_GRID_REFRESH_INTERVAL).ParameterValue;
            }
        }

        public override string GetFilterExpression()
        {
            string filterExpression = string.Format("HealthcareServiceUnitID = {0} AND GCTransactionStatus NOT IN ('{1}','{2}') AND TransactionDate BETWEEN '{3}' AND '{4}'",
                AppSession.MedicalDiagnostic.HealthcareServiceUnitID, Constant.TransactionStatus.OPEN, Constant.TransactionStatus.VOID,
                Helper.GetDatePickerValue(txtTransactionDateFrom).ToString(Constant.FormatString.DATE_FORMAT_112),
                Helper.GetDatePickerValue(txtTransactionDateTo).ToString(Constant.FormatString.DATE_FORMAT_112));

            if (hdnMRN.Value != "")
                filterExpression += string.Format(" AND MRN = {0}", hdnMRN.Value);
            else
                filterExpression += string.Format(" AND PatientName LIKE '%{0}%'", txtPatientName.Text);

            if (cboOrderResultType.Value.ToString() == "1")
                filterExpression += " AND TransactionID NOT IN (SELECT ChargeTransactionID FROM ImagingResultHd WITH(NOLOCK) WHERE IsDeleted = 0)";
            else if (cboOrderResultType.Value.ToString() == "2")
                filterExpression += " AND TransactionID IN (SELECT ChargeTransactionID FROM ImagingResultHd WITH(NOLOCK) WHERE IsDeleted = 0)";

            return filterExpression;
        }

        public override void LoadAllWords()
        {
            LoadWords();
        }

        public override void OnGrdRowClick(string oTransactionID)
        {
            vPatientChargesHd entity = BusinessLayer.GetvPatientChargesHdList(string.Format("TransactionID = {0}", oTransactionID)).FirstOrDefault();
            RegisteredPatient pt = new RegisteredPatient();
            pt.MRN = entity.MRN;
            pt.MedicalNo = entity.MedicalNo;
            pt.PatientName = entity.PatientName;
            pt.RegistrationID = entity.RegistrationID;
            pt.VisitID = entity.VisitID;
            pt.RegistrationNo = entity.RegistrationNo;
            pt.BusinessPartnerID = entity.BusinessPartnerID;
            pt.LinkedRegistrationID = entity.LinkedRegistrationID;
            pt.LinkedToRegistrationID = entity.LinkedToRegistrationID;
            pt.HealthcareServiceUnitID = entity.VisitHealthcareServiceUnitID;
            AppSession.RegisteredPatient = pt;

            string url = "";
            url = string.Format("~/Program/Worklist/LaboratoryTestResult/LaboratoryTestResultDetailReadOnly.aspx?id=hs|{0}", oTransactionID);
            Response.Redirect(url);
        }
    }
}