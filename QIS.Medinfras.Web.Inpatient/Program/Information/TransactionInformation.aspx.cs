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
using QIS.Data.Core.Dal;
using System.Data;
using System.Web.UI.HtmlControls;

namespace QIS.Medinfras.Web.Inpatient.Program
{
    public partial class TransactionInformation : BasePageTrx
    {
        protected string laboratoryTransactionCode = "";
        protected string GetErrorMsgSelectTransactionFirst()
        {
            return Helper.GetErrorMessageText(this, Constant.ErrorMessage.MSG_SELECT_TRANSACTION_FIRST_VALIDATION);
        }

        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Inpatient.PATIENT_TRANSACTION_PAGE_INFORMATION;
        }

        protected override void InitializeDataControl()
        {
            laboratoryTransactionCode = Constant.TransactionCode.LABORATORY_CHARGES;
            vConsultVisit entityVisit = BusinessLayer.GetvConsultVisitList(string.Format("VisitID = {0}", AppSession.RegisteredPatient.VisitID))[0];

            hdnRegistrationID.Value = entityVisit.RegistrationID.ToString();
            hdnLinkedRegistrationID.Value = entityVisit.LinkedRegistrationID.ToString();
            vRegistration entity = BusinessLayer.GetvRegistrationList(string.Format("RegistrationID = {0}", hdnRegistrationID.Value))[0];
            EntityToControl(entity);

            List<PatientPaymentHd> lstPatientPayment = BusinessLayer.GetPatientPaymentHdList(string.Format(
                        "RegistrationID = {0} AND GCPaymentType = '{1}' AND GCTransactionStatus != '{2}'", 
                        entity.RegistrationID, Constant.PaymentType.DOWN_PAYMENT, Constant.TransactionStatus.VOID));
            txtDownPayment.Text = lstPatientPayment.Sum(x => x.TotalPaymentAmount).ToString("N2");

            if (entityVisit.LinkedRegistrationID != 0)
            {
                //int count = BusinessLayer.GetRegistrationRowCount(string.Format("RegistrationID = {0} AND IsChargesTransfered = 0", entityVisit.LinkedRegistrationID));
                //if (count < 1)
                //    tblInfoOutstandingTransfer.Style.Add("display", "none");

                string filterExpressionLinked = string.Format("(RegistrationID = {0} AND IsChargesTransfered = 0) AND GCTransactionStatus NOT IN ('{1}','{2}') AND IsDeleted = 0", entityVisit.LinkedRegistrationID, Constant.TransactionStatus.CLOSED, Constant.TransactionStatus.VOID);
                int count = BusinessLayer.GetvPatientChargesDtRowCount(filterExpressionLinked);

                if (count < 1)
                {
                    tblInfoOutstandingTransfer.Style.Add("display", "none");
                }
            }
            else
                tblInfoOutstandingTransfer.Style.Add("display", "none");

            decimal coverageLimit = entity.CoverageLimitAmount;
            string filterExpression = string.Format("RegistrationID = {0} AND GCTransactionStatus != '{1}'", hdnRegistrationID.Value, Constant.TransactionStatus.VOID);
            if (entity.IsCoverageLimitPerDay)
                filterExpression += string.Format(" AND BillingDate = '{0}'", DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT_112));

            List<PatientBill> lstPatientBill = BusinessLayer.GetPatientBillList(filterExpression);
            coverageLimit -= lstPatientBill.Sum(p => p.TotalPayerAmount);
            hdnRemainingCoverageAmount.Value = coverageLimit.ToString();
            
            if (hdnLinkedRegistrationID.Value != "" && hdnLinkedRegistrationID.Value != "0")
                filterExpression = string.Format("(RegistrationID = {0} OR (RegistrationID = {1} AND IsChargesTransfered = 1))", hdnRegistrationID.Value, hdnLinkedRegistrationID.Value);
            else
                filterExpression = string.Format("RegistrationID = {0}", hdnRegistrationID.Value);
            List<vHealthcareServiceUnit> lstHSU = BusinessLayer.GetvHealthcareServiceUnitList(string.Format("HealthcareServiceUnitID IN (SELECT HealthcareServiceUnitID FROM vPatientChargesHd WHERE {0}) AND IsDeleted = 0", filterExpression));
            lstHSU.Insert(0, new vHealthcareServiceUnit { HealthcareServiceUnitID = 0, ServiceUnitName = "" });
            Methods.SetComboBoxField<vHealthcareServiceUnit>(cboServiceUnit, lstHSU, "ServiceUnitName", "HealthcareServiceUnitID");
            cboServiceUnit.SelectedIndex = 0;

            List<Variable> lstVariable = new List<Variable>();
            lstVariable.Add(new Variable { Code = "0", Value = "Semua" });
            lstVariable.Add(new Variable { Code = "1", Value = "Belum Diverifikasi" });
            lstVariable.Add(new Variable { Code = "2", Value = "Sudah Diverifikasi" });
            Methods.SetComboBoxField<Variable>(cboDisplay, lstVariable, "Value", "Code");
            cboDisplay.Value = "1";

            txtTransactionDateFrom.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtTransactionDateTo.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);            

            BindGridDetail();
        }

        protected string GetRemainingCoverageAmount()
        {
            return Convert.ToDecimal(hdnRemainingCoverageAmount.Value).ToString("N");
        }

        private void BindGridDetail()
        {
            string filter;
            if (hdnLinkedRegistrationID.Value != "" && hdnLinkedRegistrationID.Value != "0")
                filter = string.Format("(RegistrationID = {0} OR (RegistrationID = {1} AND IsChargesTransfered = 1)) AND GCTransactionStatus IN ('{2}','{3}')", hdnRegistrationID.Value, hdnLinkedRegistrationID.Value, Constant.TransactionStatus.WAIT_FOR_APPROVAL, Constant.TransactionStatus.OPEN);
            else
                filter = string.Format("RegistrationID = {0} AND GCTransactionStatus != '{1}'", hdnRegistrationID.Value, Constant.TransactionStatus.VOID);

            int healthcareServiceUnitID = Convert.ToInt32(cboServiceUnit.Value);
            if (healthcareServiceUnitID > 0)
                filter += string.Format(" AND HealthcareServiceUnitID = {0}", healthcareServiceUnitID);

            string cboDisplayValue = cboDisplay.Value.ToString();
            if (cboDisplayValue == "1")
                filter += string.Format(" AND IsVerified = 0");
            else if (cboDisplayValue == "2")
                filter += string.Format(" AND IsVerified = 1");

            string valueRB = rblFilterDate.SelectedValue.ToString();
            if(valueRB == "true") {
                string transactionDateFrom = Helper.GetDatePickerValue(txtTransactionDateFrom).ToString(Constant.FormatString.DATE_FORMAT_112);
                string transactionDateTo = Helper.GetDatePickerValue(txtTransactionDateTo).ToString(Constant.FormatString.DATE_FORMAT_112);
                filter += string.Format(" AND TransactionDate BETWEEN '{0}' AND '{1}'", transactionDateFrom, transactionDateTo);
            }

            filter += " ORDER BY ServiceUnitName, TransactionDate";

            List<vPatientChargesHd> lst = BusinessLayer.GetvPatientChargesHdList(filter);
            lvwView.DataSource = lst;
            lvwView.DataBind();
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            BindGridDetail();
        }

        private void EntityToControl(vRegistration entity)
        {
            hdnDepartmentID.Value = entity.DepartmentID;
            hdnHealthcareServiceUnitID.Value = entity.HealthcareServiceUnitID.ToString();
            hdnIsCustomerPersonal.Value = entity.GCCustomerType == Constant.CustomerType.PERSONAL ? "1" : "0";
        }

        public override void SetToolbarVisibility(ref bool IsAllowAdd, ref bool IsAllowSave, ref bool IsAllowVoid, ref bool IsAllowNextPrev)
        {
            IsAllowAdd = IsAllowSave = IsAllowVoid = IsAllowNextPrev = false;
        }
    }
}