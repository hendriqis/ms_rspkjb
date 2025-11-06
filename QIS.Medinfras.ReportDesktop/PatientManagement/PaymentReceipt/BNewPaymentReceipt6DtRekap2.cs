using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class BNewPaymentReceipt6DtRekap2 : BaseRpt
    {
        private int _lineNumber = 0;
        public BNewPaymentReceipt6DtRekap2()
        {
            _lineNumber = 0;
            InitializeComponent();
        }

        public void InitializeReport(List<GetPatientChargesHdDtALLPerPayment> lst, List<vPatientBill> lstBill)
        {
            decimal totalDiscount = 0;
            decimal totalBruto = 0;
            this.DataSource = lst;
            GetPatientChargesHdDtALLPerPayment entity = lst.FirstOrDefault();

            Registration reg = BusinessLayer.GetRegistration(entity.RegistrationID);
            string filterExpression = "";


            List<vPatientChargesDt> lstChargesValid = new List<vPatientChargesDt>();

            if (!String.IsNullOrEmpty(Convert.ToString(reg.LinkedRegistrationID)))
            {
                filterExpression = string.Format("VisitID IN (SELECT VisitID FROM ConsultVisit WHERE RegistrationID IN ('{0}','{1}')) AND GCTransactionStatus != '{2}' AND ISNULL(GCTransactionDetailStatus,'') != '{2}' AND IsDeleted = 0", entity.RegistrationID, reg.LinkedRegistrationID, Constant.TransactionStatus.VOID);
            }
            else
            {
                filterExpression = string.Format("VisitID = {0} AND GCTransactionStatus != '{1}' AND ISNULL(GCTransactionDetailStatus,'') != '{1}' AND IsDeleted = 0", entity.VisitID, Constant.TransactionStatus.VOID);
            }

            List<vPatientChargesDt> lstCharges = BusinessLayer.GetvPatientChargesDtList(filterExpression);

            if (lstBill.Count > 0)
            {
                foreach (vPatientBill e in lstBill)
                {
                    List<vPatientChargesDt> lstTemp = lstCharges.Where(t => t.PatientBillingID == e.PatientBillingID).ToList();
                    lstChargesValid.AddRange(lstTemp);
                }
            }


            totalDiscount = lstChargesValid.Sum(t => t.DiscountAmount) + lstBill.Sum(x => x.DiscountAmount);
            totalBruto = lstChargesValid.Sum(t => t.LineAmount) + totalDiscount;

            cTotalDiscountTransaction.Text = totalDiscount.ToString("N2");
            cTotalTransactionBruto.Text = totalBruto.ToString("N2");
        }

        private void xrTableCell8_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            lblNo.Text = (++_lineNumber).ToString();
        }
    }
}
