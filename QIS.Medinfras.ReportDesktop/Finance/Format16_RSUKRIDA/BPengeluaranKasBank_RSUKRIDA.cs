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
    public partial class BPengeluaranKasBank_RSUKRIDA : BaseCustomDailyLandscapeA5Rpt
    {
        decimal subTotalPembayaran = 0;

        public BPengeluaranKasBank_RSUKRIDA()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            vSupplierPaymentHd entity = BusinessLayer.GetvSupplierPaymentHdList(param[0]).FirstOrDefault();
            cNoPembayaran1.Text = entity.SupplierPaymentNo;
            cTanggalVerifikasi1.Text = entity.VerificationDateInString;
            cNoReferensi1.Text = entity.ReferenceNo;
            cTanggalReferensi1.Text = entity.ReferenceDateInString;
            cKeterangan.Text = entity.Remarks;

            if (entity.BankName != "" && entity.BankName != null)
            {
                lblBank.Text = string.Format("Bank : {0}", entity.BankName);
            }
            else
            {
                lblBank.Text = string.Format("Bank : -");
            }

            base.InitializeReport(param);
        }

        private void lblPaymentAmountText_SummaryGetResult(object sender, SummaryGetResultEventArgs e)
        {
            Decimal paymentAmount = Convert.ToDecimal(subTotalPembayaran);
            string text = Function.NumberInWords(Convert.ToInt64(paymentAmount), true);
            e.Result = text;
            e.Handled = true;

            List<SettingParameter> lstentity = BusinessLayer.GetSettingParameterList(String.Format("ParameterCode IN ('{0}','{1}','{2}')", Constant.SettingParameter.FN_WAKIL_DIREKTUR_KEUANGAN, Constant.SettingParameter.MANAGER_KEUANGAN, Constant.SettingParameter.FN_BAGIAN_KEUANGAN));
            
            lblDirectorName.Text = lstentity.Where(t => t.ParameterCode == Constant.SettingParameter.FN_WAKIL_DIREKTUR_KEUANGAN).FirstOrDefault().ParameterValue;
            lblDirectorCaption.Text = lstentity.Where(t => t.ParameterCode == Constant.SettingParameter.FN_WAKIL_DIREKTUR_KEUANGAN).FirstOrDefault().ParameterName;
            lblManagerName.Text = lstentity.Where(t => t.ParameterCode == Constant.SettingParameter.MANAGER_KEUANGAN).FirstOrDefault().ParameterValue;
            lblManagerCaption.Text = lstentity.Where(t => t.ParameterCode == Constant.SettingParameter.MANAGER_KEUANGAN).FirstOrDefault().ParameterName;
            lblSupervisorName.Text = lstentity.Where(t => t.ParameterCode == Constant.SettingParameter.FN_BAGIAN_KEUANGAN).FirstOrDefault().ParameterValue;
            lblSupervisorCaption.Text = lstentity.Where(t => t.ParameterCode == Constant.SettingParameter.FN_BAGIAN_KEUANGAN).FirstOrDefault().ParameterName;
        }

        private void lblPaymentAmountText_SummaryReset(object sender, EventArgs e)
        {
            subTotalPembayaran = 0;
        }

        private void lblPaymentAmountText_SummaryRowChanged(object sender, EventArgs e)
        {
            subTotalPembayaran += Convert.ToDecimal(GetCurrentColumnValue("PaymentAmount"));
        }

    }
}
