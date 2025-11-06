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
    public partial class BPermintaanPembelianRSCK : BaseDailyPortraitRpt
    {
        public BPermintaanPembelianRSCK()
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

            string filterExpression1 = string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.SA0134);
            string filterExpression2 = string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.SA0135);
            string filterExpression3 = string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.DIREKTUR_UMUM);
            List<SettingParameter> lstParam1 = BusinessLayer.GetSettingParameterList(filterExpression1);
            List<SettingParameter> lstParam2 = BusinessLayer.GetSettingParameterList(filterExpression2);
            List<SettingParameter> lstParam3 = BusinessLayer.GetSettingParameterList(filterExpression3);

            //lblSektretarisRS.Text = lstParam2.FirstOrDefault().ParameterValue;
            //lblJabatan1.Text = lstParam2.FirstOrDefault().Notes;
            lblWadirUmum.Text = lstParam1.FirstOrDefault().ParameterValue;
            lblJabatan2.Text = lstParam1.FirstOrDefault().Notes;

            string filterExp = string.Format("PurchaseRequestID = {0}", entity.PurchaseRequestID);
            List<vPurchaseRequestDt> lstDt = BusinessLayer.GetvPurchaseRequestDtList(filterExp);
            decimal totalPrice = lstDt.Sum(p => p.CustomTotalPrice);
            decimal payment = 10000000;

            if (totalPrice >= payment)
            {
                lblDirekturUmum.Text = lstParam3.FirstOrDefault().ParameterValue;
                lblJabatan3.Text = lstParam3.FirstOrDefault().Notes;
            }
            else
            {
                lblDirekturUmum.Visible = false;
                lblJabatan3.Visible = false;
                lblLine.Visible = false;
                xrLabel41.Visible = false;
                xrLabel38.Visible = false;
                xrLabel39.Visible = false;
                xrLabel37.Visible = false;
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

        private void lblKepalaGudang_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            String POType = lblTipePermintaan.Text;

            //if (POType == "Obat Rawat Jalan" || POType == "Obat Rawat Inap")
            //{
            //    string filterExpression1 = string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.KEPALA_SUB_BAGIAN_GUDANG_FARMASI);
            //    string filterExpression2 = string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.MANAGER_FARMASI);
            //    List<SettingParameterDt> lstParam1 = BusinessLayer.GetSettingParameterDtList(filterExpression1);
            //    List<SettingParameterDt> lstParam2 = BusinessLayer.GetSettingParameterDtList(filterExpression2);

            //    lblKepalaGudang.Text = lstParam1.FirstOrDefault().ParameterValue;
            //    lblJabatan.Text = "Kepala Sub. Bag. Gudang Farmasi";
            //    lblMengetahui.Text = lstParam2.FirstOrDefault().ParameterValue;
            //    lblJabatanMengetahui.Text = "Kepala Instalasi Farmasi";
            //}
            //else if (POType == "Alat Kesehatan" || POType == "Obat dan Alkes")
            //{
            //    string filterExpression1 = string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.KEPALA_SUB_BAGIAN_GUDANG_ALKES);
            //    string filterExpression2 = string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.MANAGER_FARMASI);
            //    List<SettingParameterDt> lstParam1 = BusinessLayer.GetSettingParameterDtList(filterExpression1);
            //    List<SettingParameterDt> lstParam2 = BusinessLayer.GetSettingParameterDtList(filterExpression2);

            //    lblKepalaGudang.Text = lstParam1.FirstOrDefault().ParameterValue;
            //    lblJabatan.Text = "Kepala Sub. Bag. Gudang Alkes";
            //    lblMengetahui.Text = lstParam2.FirstOrDefault().ParameterValue;
            //    lblJabatanMengetahui.Text = "Kepala Instalasi Farmasi";
            //}
            //else if (POType == "Bahan Makanan")
            //{
            //    string filterExpression1 = string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.KEPALA_SUB_BAGIAN_GUDANG_BAHAN_MAKANAN);
            //    string filterExpression2 = string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.KEPALA_INSTALASI_GIZI);
            //    List<SettingParameterDt> lstParam1 = BusinessLayer.GetSettingParameterDtList(filterExpression1);
            //    List<SettingParameterDt> lstParam2 = BusinessLayer.GetSettingParameterDtList(filterExpression2);

            //    lblKepalaGudang.Text = lstParam1.FirstOrDefault().ParameterValue;
            //    lblJabatan.Text = "Kepala Sub. Bag. Gudang Bahan Makanan";
            //    lblMengetahui.Text = lstParam2.FirstOrDefault().ParameterValue;
            //    lblJabatanMengetahui.Text = "Kepala Instalasi Gizi";
            //}
            //else if (POType == "Barang Umum")
            //{
            //    string filterExpression1 = string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.KEPALA_UNIT_LOGISTIK_UMUM);
            //    string filterExpression2 = string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.KEPALA_LOGISTIK_UMUM);
            //    List<SettingParameterDt> lstParam1 = BusinessLayer.GetSettingParameterDtList(filterExpression1);
            //    List<SettingParameterDt> lstParam2 = BusinessLayer.GetSettingParameterDtList(filterExpression2);

            //    lblKepalaGudang.Text = lstParam1.FirstOrDefault().ParameterValue;
            //    lblJabatan.Text = "Kasi Gudang Umum";
            //    lblMengetahui.Text = lstParam2.FirstOrDefault().ParameterValue;
            //    lblJabatanMengetahui.Text = "Ka Biro Pengadaan dan Logistik";
            //}
            //else
            //{
            //    lblKepalaGudang.Text = "";
            //    lblJabatan.Text = "";
            //}
        }
    }
}
