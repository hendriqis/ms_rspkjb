using System;
using System.Linq;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class BPermintaanPembelianRSSK : BaseDailyPortraitRpt
    {
        public BPermintaanPembelianRSSK()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            vPurchaseRequestHd entity = BusinessLayer.GetvPurchaseRequestHdList(param[0])[0];
            
            lblPurchaseRequestNo.Text = entity.PurchaseRequestNo;
            lblLocation.Text = string.Format("{0} ({1})", entity.LocationName, entity.LocationCode);
            lblTipePermintaan.Text = entity.PurchaseOrderType;
            lblKeterangan.Text = entity.Remarks;
            lblProductLine.Text = entity.ProductLineName;

            vHealthcare hsu = BusinessLayer.GetvHealthcareList(string.Format("HealthCareID = {0}", appSession.HealthcareID))[0];
            lblTanggal.Text = string.Format("{0}, {1}", hsu.City, DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT));

            String POType = lblTipePermintaan.Text;
            String CreatedByName = entity.CreatedByName;

            if (POType == "Obat Rawat Jalan" || POType == "Obat Rawat Inap" || POType == "Alkes Rawat Inap" || POType == "Alkes Rawat Jalan" || POType == "Alat Kesehatan")
            {
                string filterExpression1 = string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.SA0134);
                string filterExpression2 = string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.DIREKTUR_UMUM);
                string filterExpression3 = string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.FN_KA_SIE_AKUN_PEMBELIAN);
                string filterExpression4 = string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.DIREKTUR_KEUANGAN);
                string filterExpression5 = string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.PHARMACIST);

                List<SettingParameter> lstParam1 = BusinessLayer.GetSettingParameterList(filterExpression1);
                List<SettingParameter> lstParam2 = BusinessLayer.GetSettingParameterList(filterExpression2);
                List<SettingParameter> lstParam3 = BusinessLayer.GetSettingParameterList(filterExpression3);
                List<SettingParameter> lstParam4 = BusinessLayer.GetSettingParameterList(filterExpression4);
                List<SettingParameter> lstParam5 = BusinessLayer.GetSettingParameterList(filterExpression5);

                string[] paramInfo = lstParam1.FirstOrDefault().ParameterValue.Split('|');
                string[] paramNotes = lstParam1.FirstOrDefault().Notes.Split('|');

                lblCreatedBy.Text = CreatedByName;

                //lblNamaWakilDirektur.Text = paramInfo[1];
                //lblWakilDirektur.Text = paramNotes[1];

                //lblNamaDirektur.Text = lstParam2.FirstOrDefault().ParameterValue;
                //lblDirektur.Text = lstParam2.FirstOrDefault().Notes;

                lblNamaKasieAkun.Text = lstParam3.FirstOrDefault().ParameterValue;
                lblKasieAkun.Text = lstParam3.FirstOrDefault().Notes;

                //lblNamaKasieKeu.Text = lstParam4.FirstOrDefault().ParameterValue;
                //lblKasieKeu.Text = lstParam4.FirstOrDefault().Notes;

                //lblNamaKasieRumahTangga.Text = lstParam5.FirstOrDefault().ParameterValue;
                //lblKasieRumahTangga.Text = lstParam5.FirstOrDefault().Notes;
            }
            else
            {
                string filterExpression1 = string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.SA0134);
                string filterExpression2 = string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.DIREKTUR_UMUM);
                string filterExpression3 = string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.FN_KA_SIE_AKUN_PEMBELIAN);
                string filterExpression4 = string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.DIREKTUR_KEUANGAN);
                string filterExpression5 = string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.IM_SPV_RT_LOGISTIK);

                List<SettingParameter> lstParam1 = BusinessLayer.GetSettingParameterList(filterExpression1);
                List<SettingParameter> lstParam2 = BusinessLayer.GetSettingParameterList(filterExpression2);
                List<SettingParameter> lstParam3 = BusinessLayer.GetSettingParameterList(filterExpression3);
                List<SettingParameter> lstParam4 = BusinessLayer.GetSettingParameterList(filterExpression4);
                List<SettingParameter> lstParam5 = BusinessLayer.GetSettingParameterList(filterExpression5);

                string[] paramInfo = lstParam1.FirstOrDefault().ParameterValue.Split('|');
                string[] paramNotes = lstParam1.FirstOrDefault().Notes.Split('|');

                lblCreatedBy.Text = CreatedByName;

                //lblNamaWakilDirektur.Text = paramInfo[0];
                //lblWakilDirektur.Text = paramNotes[0];

                //lblNamaDirektur.Text = lstParam2.FirstOrDefault().ParameterValue;
                //lblDirektur.Text = lstParam2.FirstOrDefault().Notes;

                lblNamaKasieAkun.Text = lstParam3.FirstOrDefault().ParameterValue;
                lblKasieAkun.Text = lstParam3.FirstOrDefault().Notes;

                //lblNamaKasieKeu.Text = lstParam4.FirstOrDefault().ParameterValue;
                //lblKasieKeu.Text = lstParam4.FirstOrDefault().Notes;

                //lblNamaKasieRumahTangga.Text = lstParam5.FirstOrDefault().ParameterValue;
                //lblKasieRumahTangga.Text = lstParam5.FirstOrDefault().Notes;
            }

            base.InitializeReport(param);
            if (entity.IsUrgent)
            {
                lblIsUrgent.Visible = true;
            }
            else
            {
                lblIsUrgent.Visible = false;
            }
        }
    }
}
