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
    public partial class PatientBillSummaryPayDetailCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            vConsultVisit consultVisit = BusinessLayer.GetvConsultVisitList(string.Format("VisitID = {0}", AppSession.RegisteredPatient.VisitID)).FirstOrDefault();
            vRegistration entity = BusinessLayer.GetvRegistrationList(string.Format("RegistrationID = {0}", consultVisit.RegistrationID))[0];
            if (!entity.IsControlCoverageLimit)
            {
                tdCoverageLimitLabel.Style.Add("display", "none");
                tdCoverageLimitText.Style.Add("display", "none");
            }
            Int32 transactionID = Convert.ToInt32(param);
            PatientPaymentHd chargesHd = BusinessLayer.GetPatientPaymentHd(transactionID);
            txtPaymentNo.Text = chargesHd.PaymentNo;
            txtPaymentDate.Text = chargesHd.PaymentDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtPaymentTime.Text = chargesHd.PaymentTime;

            List<PatientBill> lstPatientBill = BusinessLayer.GetPatientBillList(string.Format("PatientBillingID IN (SELECT PatientBillingID FROM PatientBillPayment WHERE PaymentID = {0})", transactionID));
            txtCoverageLimit.Text = lstPatientBill.Sum(p => p.CoverageAmount).ToString("N");
            txtDiscountPatient.Text = lstPatientBill.Sum(p => p.PatientDiscountAmount).ToString("N");
            txtDiscountPayer.Text = lstPatientBill.Sum(p=> p.PayerDiscountAmount).ToString("N");
            txtGrandTotalPatient.Text = lstPatientBill.Sum(p => p.TotalPatientPaymentAmount).ToString("N");
            txtGrandTotalPayer.Text = lstPatientBill.Sum(p=> p.TotalPayerPaymentAmount).ToString("N");

            BindGrid(transactionID);
        }

        private void BindGrid(int transactionID)
        {
            List<vPatientChargesDt8> lst = BusinessLayer.GetvPatientChargesDt8List(string.Format("LinkedChargesID IS NULL AND PatientBillingID IN (SELECT PatientBillingID FROM PatientBillPayment WHERE PaymentID = {0}) AND IsDeleted = 0", transactionID));

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

            txtTotalPayer.Text = lst.Sum(p => p.PayerAmount).ToString("N");
            txtTotalPatient.Text = lst.Sum(p => p.PatientAmount).ToString("N");
            txtTotal.Text = lst.Sum(p => p.LineAmount).ToString("N");
        }
    }
}