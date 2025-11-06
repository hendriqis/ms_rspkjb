using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using System.Web.UI.HtmlControls;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class PatientBillSummaryDiscountDtCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            vConsultVisit consultVisit = BusinessLayer.GetvConsultVisitList(string.Format("VisitID = {0}", AppSession.RegisteredPatient.VisitID)).FirstOrDefault();
            if (consultVisit.GCCustomerType == Constant.CustomerType.PERSONAL)
            {
                trServiceFee.Style.Add("display", "none");
            }

            vRegistration entity = BusinessLayer.GetvRegistrationList(string.Format("RegistrationID = {0}", consultVisit.RegistrationID)).FirstOrDefault();
            if (!entity.IsControlCoverageLimit)
            {
                tdCoverageLimitLabel.Style.Add("display", "none");
                tdCoverageLimitText.Style.Add("display", "none");
            }

            Int32 transactionID = Convert.ToInt32(param);
            PatientBill patientBill = BusinessLayer.GetPatientBill(transactionID);

            txtBillingNo.Text = patientBill.PatientBillingNo;
            txtBillingDate.Text = patientBill.BillingDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtBillingTime.Text = patientBill.BillingTime;

            txtCoverageLimit.Text = patientBill.CoverageAmount.ToString(Constant.FormatString.NUMERIC_2);

            txtDiscountPatient.Text = patientBill.PatientDiscountAmount.ToString(Constant.FormatString.NUMERIC_2);
            txtDiscountPayer.Text = patientBill.PayerDiscountAmount.ToString(Constant.FormatString.NUMERIC_2);

            txtAdminPatient.Text = patientBill.PatientAdminFeeAmount.ToString(Constant.FormatString.NUMERIC_2);
            txtAdminPayer.Text = patientBill.AdministrationFeeAmount.ToString(Constant.FormatString.NUMERIC_2);

            txtServiceFeeAmount.Text = patientBill.ServiceFeeAmount.ToString(Constant.FormatString.NUMERIC_2);
            txtPatientServiceFeeAmount.Text = patientBill.PatientServiceFeeAmount.ToString(Constant.FormatString.NUMERIC_2);

            txtPatientRoundingAmount.Text = patientBill.PatientRoundingAmount.ToString(Constant.FormatString.NUMERIC_2);
            txtPayerRoundingAmount.Text = patientBill.PayerRoundingAmount.ToString(Constant.FormatString.NUMERIC_2);

            txtGrandTotalPatient.Text = (patientBill.TotalPatientAmount - patientBill.PatientDiscountAmount).ToString(Constant.FormatString.NUMERIC_2);
            txtGrandTotalPayer.Text = (patientBill.TotalPayerAmount - patientBill.PayerDiscountAmount).ToString(Constant.FormatString.NUMERIC_2);

            txtDiffCoverageAmount.Text = patientBill.DiffCoverageAmount.ToString(Constant.FormatString.NUMERIC_2);
            
            BindGrid(transactionID);
        }

        private void BindGrid(int transactionID)
        {
            List<vPatientChargesDt8> lst = BusinessLayer.GetvPatientChargesDt8List(string.Format("PatientBillingID = {0} AND LinkedChargesID IS NULL AND IsDeleted = 0", transactionID));

            List<vPatientChargesDt8> lstService = lst.Where(p => p.GCItemType == Constant.ItemGroupMaster.SERVICE || p.GCItemType == Constant.ItemGroupMaster.MEDICAL_CHECKUP).ToList();
            ((TransactionDtServiceViewCtl)ctlService).BindGrid(lstService);

            List<vPatientChargesDt8> lstDrugMS = lst.Where(p => p.GCItemType == Constant.ItemGroupMaster.DRUGS || p.GCItemType == Constant.ItemGroupMaster.SUPPLIES).ToList();
            ((TransactionDtProductViewCtl)ctlDrugMS).BindGrid(lstDrugMS);

            List<vPatientChargesDt8> lstLogistic = lst.Where(p => p.GCItemType == Constant.ItemGroupMaster.LOGISTIC).ToList();
            ((TransactionDtProductViewCtl)ctlLogistic).BindGrid(lstLogistic);

            List<vPatientChargesDt8> lstLaboratory = lst.Where(p => p.GCItemType == Constant.ItemGroupMaster.LABORATORY).ToList();
            ((TransactionDtServiceViewCtl)ctlLaboratory).BindGrid(lstLaboratory);

            List<vPatientChargesDt8> lstImaging = lst.Where(p => p.GCItemType == Constant.ItemGroupMaster.RADIOLOGY).ToList();
            ((TransactionDtServiceViewCtl)ctlImaging).BindGrid(lstImaging);

            List<vPatientChargesDt8> lstDiagnostic = lst.Where(p => p.GCItemType == Constant.ItemGroupMaster.DIAGNOSTIC).ToList();
            ((TransactionDtServiceViewCtl)ctlMedicalDiagnostic).BindGrid(lstDiagnostic);

            txtTotalPayer.Text = lst.Sum(p => p.PayerAmount).ToString(Constant.FormatString.NUMERIC_2);
            txtTotalPatient.Text = lst.Sum(p => p.PatientAmount).ToString(Constant.FormatString.NUMERIC_2);
            txtTotal.Text = lst.Sum(p => p.LineAmount).ToString(Constant.FormatString.NUMERIC_2);

            //txtTotal.Text = (lst.Sum(p => p.LineAmount) + AdministrationFeeAmount + ServiceFeeAmount).ToString("N");
        }
    }
}