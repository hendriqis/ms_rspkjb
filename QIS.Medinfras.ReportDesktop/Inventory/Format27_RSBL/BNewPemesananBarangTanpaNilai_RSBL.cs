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
    public partial class BNewPemesananBarangTanpaNilai_RSBL : BaseDailyPortraitRpt
    {
        public BNewPemesananBarangTanpaNilai_RSBL()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            base.InitializeReport(param);

            vHealthcare oHealthcare = BusinessLayer.GetvHealthcareList(string.Format("HealthcareID = '{0}'", AppSession.UserLogin.HealthcareID)).FirstOrDefault();

            vPurchaseOrderHd entity = BusinessLayer.GetvPurchaseOrderHdList(param[0]).FirstOrDefault();
            lblPurchaseOrderNo.Text = entity.PurchaseOrderNo;
            lblPurchaseOrderDate.Text = entity.OrderDateInString;
            lblNotes.Text = entity.Remarks;
            lblSupplierCode.Text = entity.BusinessPartnerCode;
            lblSupplierName.Text = entity.BusinessPartnerName;
            lblTerm.Text = entity.TermName;
            lblProductLine.Text = entity.ProductLineName;

            BusinessPartners bp = BusinessLayer.GetBusinessPartnersList(string.Format("BusinessPartnerID = '{0}'", entity.BusinessPartnerID)).FirstOrDefault();
            Address ad = BusinessLayer.GetAddressList(string.Format("AddressID = '{0}'", bp.AddressID)).FirstOrDefault();
            lblSupplierAddress.Text = ad.StreetName;
            lblPhoneNo.Text = ad.PhoneNo1;

            lblPOType.Text = entity.PurchaseOrderType;
            lblSyarat.Text = entity.PaymentRemarks;
            lblDeliveryDate.Text = entity.DeliveryDate.ToString(Constant.FormatString.DATE_FORMAT);
            lblPrintDate.Text = string.Format("{0}, {1}", oHealthcare.City, DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT));

            if (entity.IsUrgent == true)
            {
                lblIsUrgent.Visible = true;
            }
            else
            {
                lblIsUrgent.Visible = false;
            }

            string filterExpression = string.Format("ParameterCode IN ('{0}', '{1}')", Constant.SettingParameter.IM_MAX_TRANSACTION_AMOUNT_PURCHASE_ORDER,
                        Constant.SettingParameter.IM_DEFAULT_LOCATION_PURCHASEORDER_PHARMACY);
            List<SettingParameterDt> lstParam = BusinessLayer.GetSettingParameterDtList(filterExpression);
            //Decimal amount = Convert.ToDecimal(lstParam.ParameterValue[0]);
            Decimal amount = Convert.ToDecimal(lstParam.Where(lst => lst.ParameterCode == Constant.SettingParameter.IM_MAX_TRANSACTION_AMOUNT_PURCHASE_ORDER).FirstOrDefault().ParameterValue);
            string oLocation = lstParam.Where(lst => lst.ParameterCode == Constant.SettingParameter.IM_DEFAULT_LOCATION_PURCHASEORDER_PHARMACY).FirstOrDefault().ParameterValue;
            
            #region Hitung Total

            decimal total = entity.TransactionAmount;

            decimal totalDiskon = 0;
            if (entity.FinalDiscount > 0)
            {
                totalDiskon = (entity.FinalDiscount / 100 * entity.TransactionAmount);
            }

            decimal ppn = 0;
            if (entity.IsIncludeVAT)
            {
                ppn = ((entity.VATPercentage * (entity.TransactionAmount - totalDiskon)) / 100);
            }
            else
            {
                ppn = 0;
            }

            decimal totalPemesanan = (total - totalDiskon) + ppn;

            #endregion

            if (entity.GCPurchaseOrderType != Constant.PurchaseOrderType.LOGISTIC && entity.GCPurchaseOrderType != Constant.PurchaseOrderType.ASSET && entity.LocationID.ToString() == oLocation)
            {
                lblReportTitle.Text = "SURAT PEMESANAN OBAT DAN ALKES";

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

                lblDirektur2.Visible = false;
                lblJabatanDirektur2.Visible = false;

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
                lblReportTitle.Text = "SURAT PEMESANAN BARANG";

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

                lblDirektur2.Text = direktur.ParameterValue;
                lblJabatanDirektur2.Text = jabatanDirektur.ParameterValue;

                lblKabagKeu2.Text = kepalaKeuanganAkuntansi.ParameterValue;
                lblJabatanKabagKeu2.Text = jabatanKepalaKeuanganAkuntansi.Notes;

                lblPembelian2.Text = kepalaPembelian.ParameterValue;
                lblJabatanPembelian2.Text = kepalaPembelian.Notes;

                lblMinta2.Text = logistik.ParameterValue;
                lblJabatanMinta2.Text = logistik.Notes;

                lblSIPA.Visible = false;

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

        private void lblNotes_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (String.IsNullOrEmpty(lblNotes.Text))
            {
                xrLabel17.Text = "";
                xrLabel16.Text = "";
            }
        }

        private void lblRemarks_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            String PO = lblPOType.Text;
            if (PO == "Bahan Makanan")
            {
                lblRemarks.Text = "Sebelum pukul 08:00";
            }
            else
            {
                lblRemarks.Text = "Sebelum pukul 12:00";
            }
        }

    }
}
