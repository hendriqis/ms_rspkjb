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
    public partial class BPermintaanPembelian_RSFM : BaseDailyPortraitRpt
    {
        public BPermintaanPembelian_RSFM()
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

            if (POType == "Obat" || POType == "Alat Kesehatan")
            {
                string filterExpression1 = string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.PJ_PEMBELIAN_ALKES);
                string filterExpression2 = string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.IM_APPROVER_PURCHASE_RECEIVE_MEDIK);
                string filterExpression3 = string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.MANAGER_FARMASI);
                List<SettingParameter> lstParam1 = BusinessLayer.GetSettingParameterList(filterExpression1);
                List<SettingParameter> lstParam2 = BusinessLayer.GetSettingParameterList(filterExpression2);
                List<SettingParameter> lstParam3 = BusinessLayer.GetSettingParameterList(filterExpression3);

                lblKepalaKeu.Text = lstParam1.FirstOrDefault().ParameterValue;
                lblJabatanKepalaKeu.Text = lstParam1.FirstOrDefault().Notes;

                lblPembelian.Text = lstParam2.FirstOrDefault().ParameterValue;
                lblJabatanPembelian.Text = lstParam2.FirstOrDefault().Notes;

                lblMinta.Text = lstParam3.FirstOrDefault().ParameterValue;
                lblJabatanMinta.Text = string.Format("Apoteker");
            }
            else if (POType == "Bahan Makanan")
            {
                string filterExpression1 = string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.PJ_PEMBELIAN_ALKES);
                string filterExpression2 = string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.IM_APPROVER_PURCHASE_RECEIVE_MEDIK);
                string filterExpression3 = string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.KEPALA_UNIT_LOGISTIK_BAHAN_MAKANAN);
                List<SettingParameter> lstParam1 = BusinessLayer.GetSettingParameterList(filterExpression1);
                List<SettingParameter> lstParam2 = BusinessLayer.GetSettingParameterList(filterExpression2);
                List<SettingParameter> lstParam3 = BusinessLayer.GetSettingParameterList(filterExpression3);

                lblKepalaKeu.Text = lstParam1.FirstOrDefault().ParameterValue;
                lblJabatanKepalaKeu.Text = lstParam1.FirstOrDefault().Notes;

                lblPembelian.Text = lstParam2.FirstOrDefault().ParameterValue;
                lblJabatanPembelian.Text = lstParam2.FirstOrDefault().Notes;

                lblMinta.Text = lstParam3.FirstOrDefault().ParameterValue;
                lblJabatanMinta.Text = lstParam3.FirstOrDefault().Notes;
            }
            else if (POType == "Barang Umum")
            {
                string filterExpression1 = string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.PJ_PEMBELIAN_ALKES);
                string filterExpression2 = string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.IM_APPROVER_PURCHASE_RECEIVE_MEDIK);
                string filterExpression3 = string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.KEPALA_UNIT_LOGISTIK_UMUM);
                List<SettingParameter> lstParam1 = BusinessLayer.GetSettingParameterList(filterExpression1);
                List<SettingParameter> lstParam2 = BusinessLayer.GetSettingParameterList(filterExpression2);
                List<SettingParameter> lstParam3 = BusinessLayer.GetSettingParameterList(filterExpression3);

                lblKepalaKeu.Text = lstParam1.FirstOrDefault().ParameterValue;
                lblJabatanKepalaKeu.Text = lstParam1.FirstOrDefault().Notes;

                lblPembelian.Text = lstParam2.FirstOrDefault().ParameterValue;
                lblJabatanPembelian.Text = lstParam2.FirstOrDefault().Notes;

                lblMinta.Text = lstParam3.FirstOrDefault().ParameterValue;
                lblJabatanMinta.Text = lstParam3.FirstOrDefault().Notes;
            }
            else
            {
                lblKepalaKeu.Text = "";
                lblJabatanKepalaKeu.Text = "";

                lblPembelian.Text = "";
                lblJabatanPembelian.Text = "";

                lblMinta.Text = "";
                lblJabatanMinta.Text = "";
            }
        }
    }
}
