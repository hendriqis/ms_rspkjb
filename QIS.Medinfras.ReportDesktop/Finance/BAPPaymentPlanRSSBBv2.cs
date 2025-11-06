using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class BAPPaymentPlanRSSBBv2 : BaseCustomDailyPotraitRpt
    {
        private decimal totalAmount = 0, stampAmount = 0, roundingAmount = 0, grandTotalAmount = 0;

        public BAPPaymentPlanRSSBBv2()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            vSupplierPaymentHd entity = BusinessLayer.GetvSupplierPaymentHdList(param[0]).FirstOrDefault();
            cNoPembayaran.Text = entity.SupplierPaymentNo;
            cTanggalVerifikasi.Text = entity.VerificationDateInString;
            cNoReferensi.Text = entity.ReferenceNo;
            cTanggalReferensi.Text = entity.ReferenceDateInString;
            //cSupplier.Text = entity.BusinessPartnerName;
            if (entity.BankID != null && entity.BankID != 0)
            {
                cCaraPembayaran.Text = entity.PaymentMethod + " | " + entity.BankName;
            }
            else
            {
                cCaraPembayaran.Text = entity.PaymentMethod;
            }
            cKeterangan.Text = entity.Remarks;

            List<SupplierPaymentDt> lstEntityDt = BusinessLayer.GetSupplierPaymentDtList(string.Format("SupplierPaymentID = {0}", entity.SupplierPaymentID));
            foreach (SupplierPaymentDt entityDt in lstEntityDt)
            {
                List<PurchaseInvoiceHd> lstPCIHD = BusinessLayer.GetPurchaseInvoiceHdList(string.Format("PurchaseInvoiceID = {0} AND GCTransactionStatus != '{1}'", entityDt.PurchaseInvoiceID, Constant.TransactionStatus.VOID));
                stampAmount += lstPCIHD.Sum(a => a.StampAmount);
            }

            totalAmount = lstEntityDt.Sum(b => b.PaymentAmount);
            stampAmount += entity.StampAmount;
            roundingAmount = entity.RoundingAmount;
            grandTotalAmount = entity.TotalPaymentAmount;

            cStampAmount.Text = stampAmount.ToString(Constant.FormatString.NUMERIC_2);
            cRoundingAmount.Text = roundingAmount.ToString(Constant.FormatString.NUMERIC_2);
            cGrandTotal.Text = grandTotalAmount.ToString(Constant.FormatString.NUMERIC_2);

            List<SettingParameter> lstSettingParameter = BusinessLayer.GetSettingParameterList(string.Format("ParameterCode IN ('{0}','{1}')",
                Constant.SettingParameter.SA_VERIFICATION_TREASURY, Constant.SettingParameter.SA_APPROVAL_TREASURY));
            
            cTTDPreparedBy.Text = entity.CreatedByName;
            cTTDReviewedBy.Text = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.SA_VERIFICATION_TREASURY).ParameterValue;
            //cTTDPrintedBy.Text = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.SA_APPROVAL_TREASURY).ParameterValue;

            base.InitializeReport(param);
        }
    }
}
