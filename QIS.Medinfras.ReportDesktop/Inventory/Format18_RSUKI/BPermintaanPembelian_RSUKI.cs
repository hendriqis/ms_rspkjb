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
    public partial class BPermintaanPembelian_RSUKI : BaseDailyPortraitRpt
    {
        public BPermintaanPembelian_RSUKI()
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

            string productLineCode = entity.ProductLineCode;

            vHealthcare hsu = BusinessLayer.GetvHealthcareList(string.Format("HealthCareID = {0}", appSession.HealthcareID))[0];
            lblTanggal.Text = string.Format("{0}, {1}", hsu.City, DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT));

            String POType = lblTipePermintaan.Text;
            if (entity.IsUrgent)
            {
                lblIsUrgent.Visible = true;
            }
            else
            {
                lblIsUrgent.Visible = false;
            }

            #region Obat/Alkes
            if (POType == "Obat" || POType == "Alat Kesehatan" || POType == "COVID")
            {
                string filterNamattd1 = string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.SA0177); // Ka.Bid Penunjang Medis
                string filterNamattd2 = string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.MANAGER_FARMASI); // Ka.Instalasi Farmasi
                string filterNamattd3 = string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.KEPALA_LOGISTIK_OBAT); // KaSub Logistik Farmasi
                SettingParameterDt namaTTD1 = BusinessLayer.GetSettingParameterDtList(filterNamattd1).FirstOrDefault();
                SettingParameterDt namaTTD2 = BusinessLayer.GetSettingParameterDtList(filterNamattd2).FirstOrDefault();
                SettingParameterDt namaTTD3 = BusinessLayer.GetSettingParameterDtList(filterNamattd3).FirstOrDefault();

                lblMenyetujui6.Text = "Diperiksa Oleh,";
                lblMenyetujui7.Text = "Diajukan Oleh,";

                lblNamaTTD6.Text = namaTTD1.ParameterValue;
                lblNamaTTD7.Text = namaTTD3.ParameterValue;

                lblttd6.Text = "Kabid. Penunjang Medis";
                lblttd7.Text = "Plt. Kasubag Logistik Farmasi";

                lblMenyetujui1.Visible = false;
                lblMenyetujui5.Visible = false;
                lblMenyetujui4.Visible = false;

                lblNamaTTD1.Visible = false;
                lblNamaTTD2.Visible = false;
                lblNamaTTD3.Visible = false;
                lblNamaTTD4.Visible = false;
                lblNamaTTD5.Visible = false;

                lblttd1.Visible = false;
                lblttd2.Visible = false;
                lblttd3.Visible = false;
                lblttd4.Visible = false;
                lblttd5.Visible = false;
            }
            #endregion

            #region Bahan Makanan
            else if (POType == "Bahan Makanan")
            {
                string filterNamattd1 = string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.SA0177); // Ka.Bid Penunjang Medis
                string filterNamattd2 = string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.KEPALA_INSTALASI_GIZI); // Kepala Instalasi Gizi

                SettingParameterDt namaTTD1 = BusinessLayer.GetSettingParameterDtList(filterNamattd1).FirstOrDefault();
                SettingParameterDt namaTTD2 = BusinessLayer.GetSettingParameterDtList(filterNamattd2).FirstOrDefault();

                lblMenyetujui1.Visible = false;
                lblMenyetujui4.Visible = false;
                lblMenyetujui5.Visible = false;

                lblNamaTTD1.Visible = false;
                lblNamaTTD2.Visible = false;
                lblNamaTTD3.Visible = false;
                lblNamaTTD4.Visible = false;
                lblNamaTTD5.Visible = false;

                lblttd1.Visible = false;
                lblttd2.Visible = false;
                lblttd3.Visible = false;
                lblttd4.Visible = false;
                lblttd5.Visible = false;

                lblMenyetujui6.Text = "Diperiksa oleh.";
                lblMenyetujui7.Text = "Diajukan oleh.";

                lblNamaTTD6.Text = namaTTD1.ParameterValue;
                lblNamaTTD7.Text = namaTTD2.ParameterValue;

                lblttd6.Text = "Kabid Penunjang Medik";
                lblttd7.Text = "Koordinator Gizi";
            }
            #endregion

            #region Barang Umum
            else if (POType == "Barang Umum")
            {
                string filterNamattd1 = string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.FN_WAKIL_DIREKTUR_KEUANGAN); // Direktur Keuangan
                string filterNamattd2 = string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.FN_KEPALABAGIAN_KEUANGAN); // Kabag Keuangan
                string filterNamattd3 = string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.PJ_PEMBELIAN_BARANG); // Kabag Umum
                string filterNamattd4 = string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.KEPALA_UNIT_LOGISTIK_UMUM); // KaSub Logistik Umum
                string filterNamattd5 = string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.IM_BAGIAN_PURCHASING); // Sarana
                string filterNamattd6 = string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.SA0177); // Ka.Bid Penunjang Medis
                string filterNamattd7 = string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.KEPALA_LOGISTIK_OBAT); // Linen

                SettingParameterDt namaTTD1 = BusinessLayer.GetSettingParameterDtList(filterNamattd1).FirstOrDefault();
                SettingParameterDt namaTTD2 = BusinessLayer.GetSettingParameterDtList(filterNamattd2).FirstOrDefault();
                SettingParameterDt namaTTD3 = BusinessLayer.GetSettingParameterDtList(filterNamattd3).FirstOrDefault();
                SettingParameterDt namaTTD4 = BusinessLayer.GetSettingParameterDtList(filterNamattd4).FirstOrDefault();
                SettingParameterDt namaTTD5 = BusinessLayer.GetSettingParameterDtList(filterNamattd5).FirstOrDefault();
                SettingParameterDt namaTTD6 = BusinessLayer.GetSettingParameterDtList(filterNamattd6).FirstOrDefault();
                SettingParameterDt namaTTD7 = BusinessLayer.GetSettingParameterDtList(filterNamattd7).FirstOrDefault();

                if (productLineCode == "SN" || productLineCode == "TS" || productLineCode == "CTK")
                {
                    lblMenyetujui1.Visible = false;
                    lblMenyetujui4.Visible = false;
                    lblMenyetujui5.Visible = false;

                    lblNamaTTD4.Visible = false;
                    lblNamaTTD5.Visible = false;

                    lblMenyetujui6.Text = "Diperiksa Oleh.";
                    lblMenyetujui7.Text = "Diajukan Oleh.";

                    lblNamaTTD1.Visible = false;
                    lblNamaTTD2.Visible = false;
                    lblNamaTTD3.Visible = false;

                    lblttd1.Visible = false;
                    lblttd2.Visible = false;
                    lblttd3.Visible = false;
                    lblttd4.Visible = false;
                    lblttd5.Visible = false;

                    lblNamaTTD6.Text = namaTTD3.ParameterValue;
                    lblNamaTTD7.Text = namaTTD4.ParameterValue;

                    lblttd6.Text = "Kabag. Umum";
                    lblttd7.Text = "Kasubag Logistik Umum & Aset";
                }
                else if (productLineCode == "SC" || productLineCode == "SC2" || productLineCode == "BPG" || productLineCode == "BB")
                {
                    lblMenyetujui1.Visible = false;
                    lblMenyetujui4.Visible = false;
                    lblMenyetujui5.Visible = false;

                    lblNamaTTD4.Visible = false;
                    lblNamaTTD5.Visible = false;

                    lblMenyetujui6.Text = "Diperiksa Oleh.";
                    lblMenyetujui7.Text = "Diajukan Oleh.";

                    lblNamaTTD1.Visible = false;
                    lblNamaTTD2.Visible = false;
                    lblNamaTTD3.Visible = false;

                    lblttd1.Visible = false;
                    lblttd2.Visible = false;
                    lblttd3.Visible = false;
                    lblttd4.Visible = false;
                    lblttd5.Visible = false;

                    lblNamaTTD6.Text = namaTTD3.ParameterValue;
                    lblNamaTTD7.Text = namaTTD5.ParameterValue;

                    lblttd6.Text = "Kabag. Umum";
                    lblttd7.Text = "Instalasi Sarana";
                }
                else if (productLineCode == "LN")
                {
                    lblMenyetujui1.Visible = false;
                    lblMenyetujui4.Visible = false;
                    lblMenyetujui5.Visible = false;

                    lblNamaTTD4.Visible = false;
                    lblNamaTTD5.Visible = false;

                    lblMenyetujui6.Text = "Diperiksa Oleh.";
                    lblMenyetujui7.Text = "Diajukan Oleh.";

                    lblNamaTTD1.Visible = false;
                    lblNamaTTD2.Visible = false;
                    lblNamaTTD3.Visible = false;

                    lblttd1.Visible = false;
                    lblttd2.Visible = false;
                    lblttd3.Visible = false;
                    lblttd4.Visible = false;
                    lblttd5.Visible = false;

                    lblNamaTTD6.Text = namaTTD6.ParameterValue;
                    lblNamaTTD7.Text = namaTTD7.ParameterValue;

                    lblttd6.Text = "Kabid. Penunjang Medis";
                    lblttd7.Text = "Instalasi Linen";
                }
                else
                {
                    lblMenyetujui6.Visible = false;
                    lblMenyetujui7.Visible = false;

                    lblNamaTTD6.Visible = false;
                    lblNamaTTD5.Visible = false;
                    lblNamaTTD7.Visible = false;

                    lblttd6.Visible = false;
                    lblttd5.Visible = false;
                    lblttd7.Visible = false;

                    lblMenyetujui1.Text = "Disetujui oleh.";
                    lblMenyetujui5.Text = "Diperiksa oleh.";
                    lblMenyetujui4.Text = "Diajukan oleh.";

                    lblNamaTTD1.Text = namaTTD1.ParameterValue;
                    lblNamaTTD2.Text = namaTTD2.ParameterValue;
                    lblNamaTTD3.Text = namaTTD3.ParameterValue;
                    lblNamaTTD4.Text = namaTTD4.ParameterValue;

                    lblttd1.Text = "Dir.Keuangan";
                    lblttd2.Text = "Pjs Kabag. Keu";
                    lblttd3.Text = "Kabag. Umum";
                    lblttd4.Text = "Kasubag. Log.Um & Aset";

                }
            }
            #endregion

            else
            {
                string filterNamattd1 = string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.FN_WAKIL_DIREKTUR_KEUANGAN); // Direktur Keuangan
                string filterNamattd2 = string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.FN_KEPALABAGIAN_KEUANGAN); // Kabag Keuangan
                string filterNamattd3 = string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.PJ_PEMBELIAN_BARANG); // Kabag Umum
                string filterNamattd4 = string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.KEPALA_UNIT_LOGISTIK_UMUM); // KaSub Logistik Umum

                SettingParameterDt namaTTD1 = BusinessLayer.GetSettingParameterDtList(filterNamattd1).FirstOrDefault();
                SettingParameterDt namaTTD2 = BusinessLayer.GetSettingParameterDtList(filterNamattd2).FirstOrDefault();
                SettingParameterDt namaTTD3 = BusinessLayer.GetSettingParameterDtList(filterNamattd3).FirstOrDefault();
                SettingParameterDt namaTTD4 = BusinessLayer.GetSettingParameterDtList(filterNamattd4).FirstOrDefault();

                lblMenyetujui6.Visible = false;
                lblMenyetujui7.Visible = false;

                lblNamaTTD6.Visible = false;
                lblNamaTTD5.Visible = false;
                lblNamaTTD7.Visible = false;

                lblttd6.Visible = false;
                lblttd5.Visible = false;
                lblttd7.Visible = false;

                lblMenyetujui1.Text = "Disetujui oleh.";
                lblMenyetujui5.Text = "Diperiksa oleh.";
                lblMenyetujui4.Text = "Diajukan oleh.";

                lblNamaTTD1.Text = namaTTD1.ParameterValue;
                lblNamaTTD2.Text = namaTTD2.ParameterValue;
                lblNamaTTD3.Text = namaTTD3.ParameterValue;
                lblNamaTTD4.Text = namaTTD4.ParameterValue;

                lblttd1.Text = "Dir.Keuangan";
                lblttd2.Text = "Pjs Kabag. Keu";
                lblttd3.Text = "Kabag. Umum";
                lblttd4.Text = "Kasubag. Log.Um & Aset";
            }
            base.InitializeReport(param);
        }
    }
}
