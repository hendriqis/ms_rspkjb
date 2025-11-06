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
    public partial class BDraftPermintaanPembelianRSSBB : BaseDailyPortraitRpt
    {
        public BDraftPermintaanPembelianRSSBB()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            vPurchaseRequestHd entity = BusinessLayer.GetvPurchaseRequestHdList(param[0])[0];
            lblPurchaseRequestNo.Text = entity.PurchaseRequestNo;
            lblLocation.Text = string.Format("{0} ({1})", entity.LocationName, entity.LocationCode);
            lblKeterangan.Text = entity.Remarks;
            lblTipePermintaan.Text = entity.PurchaseOrderType;
            lblProductLine.Text = entity.ProductLineName;

            vHealthcare hsu = BusinessLayer.GetvHealthcareList(string.Format("HealthCareID = {0}", appSession.HealthcareID))[0];
            lblTanggal.Text = string.Format("{0}, {1}", hsu.City, DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT));

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

        private void lblKepalaGudang_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            String POType = lblTipePermintaan.Text;

            if (POType == "Obat Rawat Jalan" || POType == "Obat Rawat Inap")
            {
                string filterExpression1 = string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.KEPALA_SUB_BAGIAN_GUDANG_FARMASI);
                string filterExpression2 = string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.MANAGER_FARMASI);
                List<SettingParameterDt> lstParam1 = BusinessLayer.GetSettingParameterDtList(filterExpression1);
                List<SettingParameterDt> lstParam2 = BusinessLayer.GetSettingParameterDtList(filterExpression2);

                lblKepalaGudang.Text = lstParam1.FirstOrDefault().ParameterValue;
                lblJabatan.Text = "Kepala Sub. Bag. Gudang Farmasi";
                lblMengetahui.Text = lstParam2.FirstOrDefault().ParameterValue;
                lblJabatanMengetahui.Text = "Kepala Instalasi Farmasi";
            }
            else if (POType == "Alat Kesehatan" || POType == "Obat dan Alkes")
            {
                string filterExpression1 = string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.KEPALA_SUB_BAGIAN_GUDANG_ALKES);
                string filterExpression2 = string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.MANAGER_FARMASI);
                List<SettingParameterDt> lstParam1 = BusinessLayer.GetSettingParameterDtList(filterExpression1);
                List<SettingParameterDt> lstParam2 = BusinessLayer.GetSettingParameterDtList(filterExpression2);

                lblKepalaGudang.Text = lstParam1.FirstOrDefault().ParameterValue;
                lblJabatan.Text = "Kepala Sub. Bag. Gudang Alkes";
                lblMengetahui.Text = lstParam2.FirstOrDefault().ParameterValue;
                lblJabatanMengetahui.Text = "Kepala Instalasi Farmasi";
            }
            else if (POType == "Bahan Makanan")
            {
                string filterExpression1 = string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.KEPALA_SUB_BAGIAN_GUDANG_BAHAN_MAKANAN);
                string filterExpression2 = string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.KEPALA_INSTALASI_GIZI);
                List<SettingParameterDt> lstParam1 = BusinessLayer.GetSettingParameterDtList(filterExpression1);
                List<SettingParameterDt> lstParam2 = BusinessLayer.GetSettingParameterDtList(filterExpression2);

                lblKepalaGudang.Text = lstParam1.FirstOrDefault().ParameterValue;
                lblJabatan.Text = "Kepala Sub. Bag. Gudang Bahan Makanan";
                lblMengetahui.Text = lstParam2.FirstOrDefault().ParameterValue;
                lblJabatanMengetahui.Text = "Kepala Instalasi Gizi";
            }
            else if (POType == "Barang Umum")
            {
                string filterExpression1 = string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.KEPALA_UNIT_LOGISTIK_UMUM);
                string filterExpression2 = string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.KEPALA_LOGISTIK_UMUM);
                List<SettingParameterDt> lstParam1 = BusinessLayer.GetSettingParameterDtList(filterExpression1);
                List<SettingParameterDt> lstParam2 = BusinessLayer.GetSettingParameterDtList(filterExpression2);

                lblKepalaGudang.Text = lstParam1.FirstOrDefault().ParameterValue;
                lblJabatan.Text = "Kasi Gudang Umum";
                lblMengetahui.Text = lstParam2.FirstOrDefault().ParameterValue;
                lblJabatanMengetahui.Text = "Ka Biro Pengadaan dan Logistik";
            }
            else
            {
                lblKepalaGudang.Text = "";
                lblJabatan.Text = "";
            }
        }
    }
}
