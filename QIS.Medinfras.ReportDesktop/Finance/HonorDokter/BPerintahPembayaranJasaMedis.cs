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
    public partial class BPerintahPembayaranJasaMedis : BaseCustomDailyPotraitRpt
    {
        public BPerintahPembayaranJasaMedis()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            vTransRevenueSharingPaymentHd entity = BusinessLayer.GetvTransRevenueSharingPaymentHdList(param[0]).FirstOrDefault();
            cNoPembayaran.Text = entity.RSPaymentNo;
            cTanggalVerifikasi.Text = entity.cfVerificationDateInString;
            cTanggalPembayaran.Text = entity.cfRSPaymentDateInString;
            cCaraPembayaran.Text = entity.SupplierPaymentMethod;
            cBank.Text = entity.BankName;
            cNoCekGiro.Text = entity.BankReferenceNo;
            cKeterangan.Text = entity.Remarks;

            List<SettingParameter> lstSettingParameter = BusinessLayer.GetSettingParameterList(string.Format("ParameterCode IN ('{0}','{1}')",
                Constant.SettingParameter.SA_VERIFICATION_TREASURY, Constant.SettingParameter.SA_APPROVAL_TREASURY));
            
            cTTDPreparedBy.Text = entity.CreatedByName;
            cTTDReviewedBy.Text = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.SA_VERIFICATION_TREASURY).ParameterValue;
            cTTDPrintedBy.Text = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.SA_APPROVAL_TREASURY).ParameterValue;

            base.InitializeReport(param);
        }
    }
}
