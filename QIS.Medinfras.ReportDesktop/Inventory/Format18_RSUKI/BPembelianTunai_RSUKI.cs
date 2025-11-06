using System;
using System.Data.OleDb;
using System.IO;
using System.Drawing;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using DevExpress.XtraReports.Extensions;
using System.Data;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class BPembelianTunai_RSUKI : BaseDailyPortraitRpt
    {
        public BPembelianTunai_RSUKI()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            base.InitializeReport(param);

            vDirectPurchaseHd entity = BusinessLayer.GetvDirectPurchaseHdList(String.Format("DirectPurchaseID IN (SELECT DirectPurchaseID FROM vDirectPurchaseDt WHERE {0})", param[0]))[0];

            if (entity.GCItemType != Constant.ItemType.BARANG_UMUM)
            {
                lblCreatedByName.Text = entity.CreatedByName;

                string filterExpression = string.Format(" ParameterCode IN ('{0}')", "IM0009");
                List<SettingParameterDt> lstParam = BusinessLayer.GetSettingParameterDtList(filterExpression);
                lblApprovedByName.Text = lstParam.Where(lst => lst.ParameterCode == "IM0009").FirstOrDefault().ParameterValue;

                lblMenyetujui1.Visible = false;
                lblMenyetujui2.Visible = false;
                lblMenyetujui3.Visible = false;
                lblMenyetujui4.Visible = false;

                lblNamaTTD1.Visible = false;
                lblNamaTTD2.Visible = false;
                lblNamaTTD3.Visible = false;
                lblNamaTTD4.Visible = false;

                lblttd1.Visible = false;
                lblttd2.Visible = false;
                lblttd3.Visible = false;
                lblttd4.Visible = false;
            }
            else
            {

                string filterNamattd1 = string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.DIREKTUR_UMUM); //Nama Direktur
                string filterNamattd2 = string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.FN_WAKIL_DIREKTUR_KEUANGAN); // Direktur Keuangan
                string filterNamattd3 = string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.FN_KEPALABAGIAN_KEUANGAN); // Kabag Keuangan
                string filterNamattd4 = string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.KEPALA_LOGISTIK_UMUM); // Ka. Pengadaan & Pembelian

                string filterNamaJabatan1 = string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.IM_NAMA_JABATAN_DIREKTUR); // Direktur

                SettingParameterDt namaTTD1 = BusinessLayer.GetSettingParameterDtList(filterNamattd1).FirstOrDefault();
                SettingParameterDt namaTTD2 = BusinessLayer.GetSettingParameterDtList(filterNamattd2).FirstOrDefault();
                SettingParameterDt namaTTD3 = BusinessLayer.GetSettingParameterDtList(filterNamattd3).FirstOrDefault();
                SettingParameterDt namaTTD4 = BusinessLayer.GetSettingParameterDtList(filterNamattd4).FirstOrDefault();

                SettingParameterDt namaJabatan1 = BusinessLayer.GetSettingParameterDtList(filterNamaJabatan1).FirstOrDefault();

                lblMenyetujui1.Text = "Menyetujui";
                lblMenyetujui2.Text = "Mengetahui";
                lblMenyetujui3.Text = "Mengetahui";
                lblMenyetujui4.Text = "Dibuat Oleh";

                lblNamaTTD1.Text = namaTTD1.ParameterValue;
                lblNamaTTD2.Text = namaTTD2.ParameterValue;
                lblNamaTTD3.Text = namaTTD3.ParameterValue;
                lblNamaTTD4.Text = namaTTD4.ParameterValue;

                lblttd1.Text = namaJabatan1.ParameterValue; // Direktur Utama RSU UKI
                lblttd2.Text = "Dir Keu Adm & Umum";
                lblttd3.Text = "Kabag Keuangan";
                lblttd4.Text = "Ketua Tim Pengadaan & Pembelian";

                xrLine2.Visible = false;
                xrLine2.Visible = false;
                xrLabel5.Visible = false;
                xrLabel6.Visible = false;
                lblCreatedByName.Visible = false;
                lblApprovedByName.Visible = false;
            }
        }
    }
}
