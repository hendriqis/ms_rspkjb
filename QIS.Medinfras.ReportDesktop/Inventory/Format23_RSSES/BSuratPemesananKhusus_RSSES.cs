using System;
using System.Drawing;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class BSuratPemesananKhusus_RSSES : BaseDailyPortraitRpt
    {
        public BSuratPemesananKhusus_RSSES()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
           
            base.InitializeReport(param);

            ReportMaster oReport = BusinessLayer.GetReportMasterList(string.Format("ReportCode='{0}' ", param[1])).FirstOrDefault();

            if (oReport.ReportCode == "IM-00162")
            {
                lblPengajuan.Text = string.Format("Mengajukan pesanan obat mengandung Psikotropika Farmasi kepada :");
                lblDigunakan.Text = string.Format("Psikotropika tersebut akan digunakan untuk  : ");
            }
            else if (oReport.ReportCode == "IM-00163") {
                lblPengajuan.Text = string.Format("Mengajukan pesanan obat mengandung Prekursor Farmasi kepada :");
                lblDigunakan.Text = string.Format("Prekursor tersebut akan digunakan untuk  : ");
            }
            else if (oReport.ReportCode == "IM-00164") {
                lblPengajuan.Text = string.Format("Mengajukan pesanan obat mengandung Obat-Obat tertentu Farmasi kepada :");
                lblDigunakan.Text = string.Format("Obat-Obat tertentu  tersebut akan digunakan untuk  : ");
            }
            else if (oReport.ReportCode == "IM-00165")
            {
                lblPengajuan.Text = string.Format("Mengajukan pesanan obat mengandung Obat-Obat tertentu Farmasi kepada :");
                lblDigunakan.Text = string.Format("Obat Narkotika tersebut akan digunakan untuk  keperluan: ");
            }
            lblReportTitle.Text = oReport.ReportTitle1.ToUpper();
            
            vPurchaseOrderHd entity = BusinessLayer.GetvPurchaseOrderHdList(param[0])[0];
            
            List<SettingParameterDt> lstSetpar = BusinessLayer.GetSettingParameterDtList(string.Format("HealthcareID='001' AND ParameterCode IN ('{0}','{1}', '{2}')", Constant.SettingParameter.PHARMACIST_LICENSE_NO, Constant.SettingParameter.PHARMACIST, Constant.SettingParameter.IM_NOMOR_IZIN_RUMAH_SAKIT_PO));

            string PHARMACIST = lstSetpar.Where(p => p.ParameterCode == Constant.SettingParameter.PHARMACIST).FirstOrDefault().ParameterValue;
            string PHARMACIST_LICENSE_NO = lstSetpar.Where(p => p.ParameterCode == Constant.SettingParameter.PHARMACIST_LICENSE_NO).FirstOrDefault().ParameterValue;
            string nomorizin = lstSetpar.Where(p => p.ParameterCode == Constant.SettingParameter.IM_NOMOR_IZIN_RUMAH_SAKIT_PO).FirstOrDefault().ParameterValue; 
            lbldate.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            lblSIPA.Text = lblSIK.Text = PHARMACIST_LICENSE_NO;
            lblApotekerName.Text =  lblApoteker.Text = PHARMACIST;
            lblNomorIzin.Text = nomorizin; 
            if (entity.IsUrgent)
            {
                lblIsUrgent.Visible = true;
            }
            else
            {
                lblIsUrgent.Visible = false;
            }
          
            Healthcare oHc = BusinessLayer.GetHealthcare(appSession.HealthcareID);
            if (oHc != null) 
            {
                Address oAddress = BusinessLayer.GetAddress(Convert.ToInt32( oHc.AddressID)) ;
                lblHealthCare.Text = oHc.HealthcareName;
                lblHealthCareAddress.Text = oAddress.StreetName;
                lblHealthcarePhone.Text = oAddress.PhoneNo1;
            }
            
        }

       
    }
}
