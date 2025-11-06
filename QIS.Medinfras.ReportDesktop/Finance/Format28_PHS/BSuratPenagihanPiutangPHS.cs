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
    public partial class BSuratPenagihanPiutangPHS : BaseCustomDailyPotraitRpt
    {
        public BSuratPenagihanPiutangPHS()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            vHealthcare h = BusinessLayer.GetvHealthcareList(string.Format("HealthcareID = {0}", appSession.HealthcareID)).FirstOrDefault();

            ARInvoiceHd invoiceHd = BusinessLayer.GetARInvoiceHdList(param[0]).FirstOrDefault();

            SettingParameter setvar = BusinessLayer.GetSettingParameterList(string.Format(
                    "ParameterCode = '{0}'", Constant.SettingParameter.FN_KEPALABAGIAN_KEUANGAN)).FirstOrDefault();

            List<SettingParameterDt> setvarAR = BusinessLayer.GetSettingParameterDtList(string.Format(
                                                                    "HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}','{3}')",
                                                                    appSession.HealthcareID,
                                                                    Constant.SettingParameter.FN_EMAIL_AR,
                                                                    Constant.SettingParameter.FN_FAX_NO_AR,
                                                                    Constant.SettingParameter.FN_KEPALABAGIAN_KEUANGAN));
            lblEmailPenagihan.Text = setvarAR.Where(a => a.ParameterCode == Constant.SettingParameter.FN_EMAIL_AR).FirstOrDefault().ParameterValue;
            lblContactPenagihan.Text = setvarAR.Where(a => a.ParameterCode == Constant.SettingParameter.FN_FAX_NO_AR).FirstOrDefault().ParameterValue;

            lblHeaderTTD1.Text = string.Format("{0}, {1}", h.City, invoiceHd.DocumentDateInString);
            lblTTD1.Text = setvarAR.Where(a => a.ParameterCode == Constant.SettingParameter.FN_KEPALABAGIAN_KEUANGAN).FirstOrDefault().ParameterValue;
            lblTTD2.Text = setvar.ParameterName;
          
            List<vBusinessPartnerVirtualAccount> lstBPVA = BusinessLayer.GetvBusinessPartnerVirtualAccountList(string.Format("BusinessPartnerID = {0} AND ID = {1} AND IsDeleted = 0", invoiceHd.BusinessPartnerID, invoiceHd.BusinessPartnerVirtualAccountID));

            subBPVA.CanGrow = true;
            bSuratPenagihanPiutangPHSDetailBank.InitializeReport(lstBPVA);
            
            subPiutangDt.CanGrow = true;
            bSuratPenagihanPiutangPHSDt.InitializeReport(invoiceHd.ARInvoiceID);
          
            //ttdDokter.ImageUrl = string.Format("{0}{1}/Signature/{2}.png", AppConfigManager.QISPhysicalDirectory, AppConfigManager.QISParamedicImagePath, entityCV.ParamedicCode);
            
            if (invoiceHd.TotalClaimedAmount < 5000000) {
                ttdpic.ImageUrl = string.Format("{0}/Finance/ttd/ttd_manager_keuangan.png", AppConfigManager.QISPhysicalDirectory);
            }

            base.InitializeReport(param);
        }

    }
}
