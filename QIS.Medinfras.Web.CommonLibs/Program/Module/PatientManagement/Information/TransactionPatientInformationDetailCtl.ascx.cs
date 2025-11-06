using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class TransactionPatientInformationDetailCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            hdnRegistrationID.Value = param;

            string filterExpression = string.Format("RegistrationID = {0}", hdnRegistrationID.Value);
            vRegistration9 entityRegistration = BusinessLayer.GetvRegistration9List(filterExpression).FirstOrDefault();
            txtRegistrationNo.Text = entityRegistration.RegistrationNo;
            txtPatientName.Text = string.Format("({0}) {1}", entityRegistration.MedicalNo, entityRegistration.PatientName);

            bindGrdOutstandingPosting(filterExpression);
            bindGrdTestOrder(filterExpression);
            bindGrdServiceOrder(filterExpression);
            bindGrdCharges(filterExpression);
            bindGrdChargesNotBilling(filterExpression);
            bindGrdBillingNotPayment(filterExpression);
        }

        private void bindGrdOutstandingPosting(String filterExpression)
        {
            filterExpression += string.Format(" AND IsDeleted = 0 AND IsApproved = 0 AND GCTransactionStatus != '{0}' AND GCItemType IN ('{1}','{2}','{3}','{4}') ORDER BY TransactionDate, TransactionID, ItemName1", Constant.TransactionStatus.VOID, Constant.ItemType.OBAT_OBATAN, Constant.ItemType.BARANG_MEDIS, Constant.ItemType.BARANG_UMUM, Constant.ItemType.BAHAN_MAKANAN);
            List<vPatientChargesDt4> lstCharges4 = BusinessLayer.GetvPatientChargesDt4List(filterExpression);
            grdViewChargesNotPosting.DataSource = lstCharges4;
            grdViewChargesNotPosting.DataBind();
        }

        private void bindGrdTestOrder(String filterExpression) 
        {
            filterExpression += string.Format(" AND GCTransactionStatus NOT IN ('{0}') ORDER BY ServiceUnitName, TestOrderDate, TestOrderTime, TestOrderNo", Constant.TransactionStatus.VOID);
            List<vTestOrderHd> lstTestOrderHd = BusinessLayer.GetvTestOrderHdList(filterExpression);
            grdViewTestOrder.DataSource = lstTestOrderHd;
            grdViewTestOrder.DataBind();
        }

        private void bindGrdServiceOrder(String filterExpression)
        {
            filterExpression += string.Format(" AND GCTransactionStatus NOT IN ('{0}') ORDER BY ServiceUnitName, ServiceOrderDate, ServiceOrderTime, ServiceOrderNo", Constant.TransactionStatus.VOID);
            List<vServiceOrderHd> lstServiceOrderHd = BusinessLayer.GetvServiceOrderHdList(filterExpression);
            grdViewServiceOrder.DataSource = lstServiceOrderHd;
            grdViewServiceOrder.DataBind();
        }

        private void bindGrdCharges(String filterExpression)
        {
            filterExpression += string.Format(" AND GCTransactionStatus NOT IN ('{0}') ORDER BY TransactionDate, TransactionTime, TransactionNo", Constant.TransactionStatus.VOID);
            List<vPatientChargesHd> lstPatientChargesHd = BusinessLayer.GetvPatientChargesHdList(filterExpression);
            grdViewCharges.DataSource = lstPatientChargesHd;
            grdViewCharges.DataBind();
        }

        private void bindGrdChargesNotBilling(String filterExpression)
        {
            filterExpression += string.Format(" AND GCTransactionStatus NOT IN ('{0}') AND PatientBillingID IS NULL ORDER BY TransactionDate, TransactionTime, TransactionNo", Constant.TransactionStatus.VOID);
            List<vPatientChargesHd> lstPatientChargesHd = BusinessLayer.GetvPatientChargesHdList(filterExpression);
            grdViewChargesNotBilling.DataSource = lstPatientChargesHd;
            grdViewChargesNotBilling.DataBind();
        }

        private void bindGrdBillingNotPayment(String filterExpression)
        {
            filterExpression += string.Format(" AND GCTransactionStatus NOT IN ('{0}') AND PaymentID IS NULL ORDER BY BillingDate, BillingTime, PatientBillingNo", Constant.TransactionStatus.VOID);
            List<PatientBill> lstPatientBillHd = BusinessLayer.GetPatientBillList(filterExpression);
            grdViewChargesNotPayment.DataSource = lstPatientBillHd;
            grdViewChargesNotPayment.DataBind();
        }
    }
}