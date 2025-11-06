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
    public partial class TransactionVerificationEntry : BasePageTrx
    {
        protected string laboratoryTransactionCode = "";
        protected string GetErrorMsgSelectTransactionFirst()
        {
            return Helper.GetErrorMessageText(this, Constant.ErrorMessage.MSG_SELECT_TRANSACTION_FIRST_VALIDATION);
        }

        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Inpatient.PATIENT_TRANSACTION_PAGE_VERIFICATION;
        }

        protected override void InitializeDataControl()
        {
            laboratoryTransactionCode = Constant.TransactionCode.LABORATORY_CHARGES;
            vConsultVisit entityVisit = BusinessLayer.GetvConsultVisitList(string.Format("VisitID = {0}", AppSession.RegisteredPatient.VisitID))[0];

            hdnRegistrationID.Value = entityVisit.RegistrationID.ToString();
            hdnLinkedRegistrationID.Value = entityVisit.LinkedRegistrationID.ToString();
            vRegistration entity = BusinessLayer.GetvRegistrationList(string.Format("RegistrationID = {0}", hdnRegistrationID.Value))[0];
            EntityToControl(entity);

            if (entityVisit.LinkedRegistrationID != 0)
            {
                int count = BusinessLayer.GetRegistrationRowCount(string.Format("RegistrationID = {0} AND IsChargesTransfered = 0", entityVisit.LinkedRegistrationID));
                if (count < 1)
                    tblInfoOutstandingTransfer.Style.Add("display", "none");
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

            lstVariable = new List<Variable>();
            lstVariable.Add(new Variable { Code = "0", Value = "Semua" });
            lstVariable.Add(new Variable { Code = "1", Value = "Belum Dibuat Tagihan" });
            lstVariable.Add(new Variable { Code = "2", Value = "Sudah Dibuat Tagihan" });
            Methods.SetComboBoxField<Variable>(cboBillingStatus, lstVariable, "Value", "Code");
            cboBillingStatus.Value = "1";

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
                filter = string.Format("(RegistrationID = {0} OR (RegistrationID = {1} AND IsChargesTransfered = 1))", hdnRegistrationID.Value, hdnLinkedRegistrationID.Value);
            else
                filter = string.Format("RegistrationID = {0}", hdnRegistrationID.Value);

            string cboBillingStatusValue = cboBillingStatus.Value.ToString();
            if (cboBillingStatusValue == "1")
                filter += string.Format(" AND GCTransactionStatus IN ('{0}','{1}')", Constant.TransactionStatus.WAIT_FOR_APPROVAL, Constant.TransactionStatus.OPEN);
            else if (cboBillingStatusValue == "2")
                filter += string.Format(" AND GCTransactionStatus = '{0}'", Constant.TransactionStatus.PROCESSED);
            else
                filter += string.Format(" AND GCTransactionStatus IN ('{0}','{1}','{2}')", Constant.TransactionStatus.WAIT_FOR_APPROVAL, Constant.TransactionStatus.OPEN, Constant.TransactionStatus.PROCESSED);

            int healthcareServiceUnitID = Convert.ToInt32(cboServiceUnit.Value);
            if (healthcareServiceUnitID > 0)
                filter += string.Format(" AND HealthcareServiceUnitID = {0}", healthcareServiceUnitID);

            string cboDisplayValue = cboDisplay.Value.ToString();
            if (cboDisplayValue == "1")
                filter += string.Format(" AND IsVerified = 0");
            else if (cboDisplayValue == "2")
                filter += string.Format(" AND IsVerified = 1");
            filter += " ORDER BY ServiceUnitName, TransactionDate";
            List<vPatientChargesHdChargeClass> lst = BusinessLayer.GetvPatientChargesHdChargeClassList(filter);
            lvwView.DataSource = lst;
            lvwView.DataBind();

            if (hdnIsControlCoverageLimit.Value == "0")
            {
                HtmlTableRow trFooterRemainingCoverageLimit = lvwView.FindControl("trFooterRemainingCoverageLimit") as HtmlTableRow;
                HtmlTableRow trFooterRemainingTotalBill = lvwView.FindControl("trFooterRemainingTotalBill") as HtmlTableRow;
                if (trFooterRemainingCoverageLimit != null)
                {
                    trFooterRemainingTotalBill.Style.Add("display", "none");
                    trFooterRemainingCoverageLimit.Style.Add("display", "none");
                }
            }
            if (hdnIsCustomerPersonal.Value == "1")
            {
                HtmlTableRow trFooterAdministrationFee = lvwView.FindControl("trFooterAdministrationFee") as HtmlTableRow;
                HtmlTableRow trFooterServiceFee = lvwView.FindControl("trFooterServiceFee") as HtmlTableRow;
                if (trFooterAdministrationFee != null)
                {
                    trFooterAdministrationFee.Style.Add("display", "none");
                    trFooterServiceFee.Style.Add("display", "none");
                    hdnAdministrationFee.Value = "0";
                    hdnServiceFee.Value = "0";
                }
            }
            else
            {
                TextBox txtAdministrationFee = lvwView.FindControl("txtAdministrationFee") as TextBox;
                TextBox txtServiceFee = lvwView.FindControl("txtServiceFee") as TextBox;
                if (txtAdministrationFee != null)
                {
                    if (hdnAdministrationFee.Attributes["ispercentage"] == "0")
                    {
                        decimal administrationFee = Convert.ToDecimal(hdnAdministrationFee.Value);
                        decimal maxAmount = Convert.ToDecimal(hdnAdministrationFee.Attributes["maxamount"]);
                        decimal minAmount = Convert.ToDecimal(hdnAdministrationFee.Attributes["minamount"]);
                        if (administrationFee < minAmount)
                            administrationFee = minAmount;
                        if (administrationFee > maxAmount)
                            administrationFee = maxAmount;
                        txtAdministrationFee.Text = administrationFee.ToString();
                    }

                    if (hdnServiceFee.Attributes["ispercentage"] == "0")
                    {
                        decimal serviceFee = Convert.ToDecimal(hdnServiceFee.Value);
                        decimal maxAmount = Convert.ToDecimal(hdnServiceFee.Attributes["maxamount"]);
                        decimal minAmount = Convert.ToDecimal(hdnServiceFee.Attributes["minamount"]);
                        if (serviceFee < minAmount)
                            serviceFee = minAmount;
                        if (serviceFee > maxAmount)
                            serviceFee = maxAmount;
                        txtServiceFee.Text = serviceFee.ToString();
                    }
                }
            }
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