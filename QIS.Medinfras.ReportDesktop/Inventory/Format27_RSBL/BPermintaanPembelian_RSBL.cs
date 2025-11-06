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
    public partial class BPermintaanPembelian_RSBL : BaseDailyPortraitRpt
    {
        public BPermintaanPembelian_RSBL()
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
            lblPrintDate.Text = string.Format("{0}, {1}", hsu.City, DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT));

            base.InitializeReport(param);
            if (entity.IsUrgent)
            {
                lblIsUrgent.Visible = true;
            }
            else
            {
                lblIsUrgent.Visible = false;
            }

            String POType = lblTipePermintaan.Text;

            if (POType == "Obat" || POType == "Alat Kesehatan")
            {
                string filterExpressionApprover = string.Format("ParameterCode IN ('{0}', '{1}', '{2}', '{3}', '{4}','{5}')", Constant.SettingParameter.DIREKTUR_YANMED, Constant.SettingParameter.IM_NAMA_JABATAN_DIREKTUR, Constant.SettingParameter.FN_KEPALABAGIAN_KEUANGAN,
                     Constant.SettingParameter.STAFF_PEMBELIAN, Constant.SettingParameter.APOTEKER_PENANGGUNG_JAWAB, Constant.SettingParameter.PHARMACIST_LICENSE_NO);

                List<SettingParameter> lstSettingParameter = BusinessLayer.GetSettingParameterList(filterExpressionApprover);

                SettingParameter direktur = lstSettingParameter.Where(p => p.ParameterCode == Constant.SettingParameter.DIREKTUR_YANMED).FirstOrDefault();
                SettingParameter jabatanDirektur = lstSettingParameter.Where(p => p.ParameterCode == Constant.SettingParameter.IM_NAMA_JABATAN_DIREKTUR).FirstOrDefault();

                SettingParameter kepalaKeuanganAkuntansi = lstSettingParameter.Where(p => p.ParameterCode == Constant.SettingParameter.FN_KEPALABAGIAN_KEUANGAN).FirstOrDefault();
                SettingParameter jabatanKepalaKeuanganAkuntansi = lstSettingParameter.Where(p => p.ParameterCode == Constant.SettingParameter.FN_KEPALABAGIAN_KEUANGAN).FirstOrDefault();

                SettingParameter kepalaPembelian = lstSettingParameter.Where(p => p.ParameterCode == Constant.SettingParameter.STAFF_PEMBELIAN).FirstOrDefault();
                SettingParameter jabatanKepalaPembelian = lstSettingParameter.Where(p => p.ParameterCode == Constant.SettingParameter.STAFF_PEMBELIAN).FirstOrDefault();

                SettingParameter farmasi = lstSettingParameter.Where(p => p.ParameterCode == Constant.SettingParameter.APOTEKER_PENANGGUNG_JAWAB).FirstOrDefault();
                SettingParameter jabatanFarmasi = lstSettingParameter.Where(p => p.ParameterCode == Constant.SettingParameter.APOTEKER_PENANGGUNG_JAWAB).FirstOrDefault();

                SettingParameter sipaFarmasi = lstSettingParameter.Where(p => p.ParameterCode == Constant.SettingParameter.PHARMACIST_LICENSE_NO).FirstOrDefault();

                lblDirektur.Text = direktur.ParameterValue;
                lblJabatanDirektur.Text = jabatanDirektur.ParameterValue;

                lblKabagKeu.Text = kepalaKeuanganAkuntansi.ParameterValue;
                lblJabatanKabagKeu.Text = jabatanKepalaKeuanganAkuntansi.Notes;

                lblPembelian.Text = kepalaPembelian.ParameterValue;
                lblJabatanPembelian.Text = jabatanKepalaPembelian.Notes;

                lblMinta.Text = farmasi.ParameterValue;
                lblJabatanMinta.Text = farmasi.Notes;

                lblSIPA.Text = sipaFarmasi.ParameterValue;

                lblKabagKeu2.Visible = false;
                lblJabatanKabagKeu2.Visible = false;

                lblPembelian2.Visible = false;
                lblJabatanPembelian2.Visible = false;

                lblMinta2.Visible = false;
                lblJabatanMinta2.Visible = false;

                lblDisetujui3.Visible = false;
                lblDibuatOleh2.Visible = false;
                lblDimintaOleh2.Visible = false;
            }
            else
            {
                string filterExpressionApprover = string.Format("ParameterCode IN ('{0}', '{1}', '{2}', '{3}', '{4}')", Constant.SettingParameter.DIREKTUR_YANMED, Constant.SettingParameter.IM_NAMA_JABATAN_DIREKTUR, Constant.SettingParameter.FN_KEPALABAGIAN_KEUANGAN,
                     Constant.SettingParameter.STAFF_PEMBELIAN, Constant.SettingParameter.KEPALA_UNIT_LOGISTIK_UMUM);

                List<SettingParameter> lstSettingParameter = BusinessLayer.GetSettingParameterList(filterExpressionApprover);

                SettingParameter direktur = lstSettingParameter.Where(p => p.ParameterCode == Constant.SettingParameter.DIREKTUR_YANMED).FirstOrDefault();
                SettingParameter jabatanDirektur = lstSettingParameter.Where(p => p.ParameterCode == Constant.SettingParameter.IM_NAMA_JABATAN_DIREKTUR).FirstOrDefault();

                SettingParameter kepalaKeuanganAkuntansi = lstSettingParameter.Where(p => p.ParameterCode == Constant.SettingParameter.FN_KEPALABAGIAN_KEUANGAN).FirstOrDefault();
                SettingParameter jabatanKepalaKeuanganAkuntansi = lstSettingParameter.Where(p => p.ParameterCode == Constant.SettingParameter.FN_KEPALABAGIAN_KEUANGAN).FirstOrDefault();

                SettingParameter kepalaPembelian = lstSettingParameter.Where(p => p.ParameterCode == Constant.SettingParameter.STAFF_PEMBELIAN).FirstOrDefault();
                SettingParameter jabatanKepalaPembelian = lstSettingParameter.Where(p => p.ParameterCode == Constant.SettingParameter.STAFF_PEMBELIAN).FirstOrDefault();

                SettingParameter logistik = lstSettingParameter.Where(p => p.ParameterCode == Constant.SettingParameter.KEPALA_UNIT_LOGISTIK_UMUM).FirstOrDefault();
                SettingParameter jabatanLogistik = lstSettingParameter.Where(p => p.ParameterCode == Constant.SettingParameter.KEPALA_UNIT_LOGISTIK_UMUM).FirstOrDefault();

                lblKabagKeu2.Text = kepalaKeuanganAkuntansi.ParameterValue;
                lblJabatanKabagKeu2.Text = jabatanKepalaKeuanganAkuntansi.Notes;

                lblPembelian2.Text = kepalaPembelian.ParameterValue;
                lblJabatanPembelian2.Text = kepalaPembelian.Notes;

                lblMinta2.Text = logistik.ParameterValue;
                lblJabatanMinta2.Text = logistik.Notes;

                lblDirektur.Visible = false;
                lblJabatanDirektur.Visible = false;

                lblKabagKeu.Visible = false;
                lblJabatanKabagKeu.Visible = false;

                lblPembelian.Visible = false;
                lblJabatanPembelian.Visible = false;

                lblMinta.Visible = false;
                lblJabatanMinta.Visible = false;

                lblDisetujui1.Visible = false;
                lblDisetujui2.Visible = false;

                lblDibuatOleh1.Visible = false;
                lblDimintaOleh1.Visible = false;
            }

        }
    }
}
