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
    public partial class BNewPemesananBarangDenganNilai_RSUKI : BaseDailyPortraitRpt
    {
        public BNewPemesananBarangDenganNilai_RSUKI()
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
            lblRCC.Text = entity.RevenueCostCenterName;
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
            lblMenyetujui4.Text = string.Format("{0}, {1}", oHealthcare.City, DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT));

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

            decimal totalDiskon = 0;
            decimal total = entity.TransactionAmount;
            decimal charges = entity.ChargesAmount;
            decimal downpayment = entity.DownPaymentAmount;

            if (entity.FinalDiscountAmount > 0)
            {
                totalDiskon = entity.FinalDiscountAmount;
                total = entity.TotalDtAmount;
            }
            else
            {
                totalDiskon = (entity.FinalDiscount / 100 * entity.TransactionAmount);
            }

            //if (entity.FinalDiscount > 0)
            //{
            //    totalDiskon = (entity.FinalDiscount / 100 * entity.TransactionAmount);
            //}

            decimal ppn = 0;
            if (entity.IsIncludeVAT)
            {
                ppn = ((entity.VATPercentage * (entity.TransactionAmount - totalDiskon)) / 100);
            }
            else
            {
                ppn = 0;
            }

            decimal pph = 0;
            if (entity.IsPPHInPercentage)
            {
                pph = entity.TransactionAmount * entity.PPHPercentage / 100;
            }
            else
            {
                pph = entity.PPHAmount;
            }

            decimal totalPemesanan = total + ppn + pph - totalDiskon - downpayment + charges;

            #endregion

            #region Total Header

            lblTotal.Text = total.ToString("N2");
            lblDiskon.Text = totalDiskon.ToString("N2");
            lblPPN.Text = ppn.ToString("N2");
            lblCharges.Text = charges.ToString("N2");
            lblTotalPemesanan.Text = totalPemesanan.ToString("N2");

            #endregion

            if (entity.GCPurchaseOrderType != Constant.PurchaseOrderType.LOGISTIC && entity.GCPurchaseOrderType != Constant.PurchaseOrderType.ASSET && entity.LocationID.ToString() == oLocation)
            {
                lblReportTitle.Text = "SURAT PEMESANAN OBAT DAN ALKES";

                string filterNamattd1 = string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.SA0136); // Dir.Pelayanan dan Penunjang Medis
                string filterNamattd2 = string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.IM_APPROVER_PURCHASE_RECEIVE_MEDIK); // Ka.Bid Penunjang Medis
                string filterNamattd3 = string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.MANAGER_FARMASI); // Ka.Instalasi Farmasi
                string filterNamattd4 = string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.KEPALA_LOGISTIK_UMUM); // Ka. Pengadaan & Pembelian
                SettingParameterDt namaTTD1 = BusinessLayer.GetSettingParameterDtList(filterNamattd1).FirstOrDefault();
                SettingParameterDt namaTTD2 = BusinessLayer.GetSettingParameterDtList(filterNamattd2).FirstOrDefault();
                SettingParameterDt namaTTD3 = BusinessLayer.GetSettingParameterDtList(filterNamattd3).FirstOrDefault();
                SettingParameterDt namaTTD4 = BusinessLayer.GetSettingParameterDtList(filterNamattd4).FirstOrDefault();

                lblMenyetujui1.Text = "";
                lblMenyetujui2.Text = "";
                lblMenyetujui3.Text = "";
                lblMenyetujui4.Text = "";

                lblNamaTTD1.Text = namaTTD1.ParameterValue;
                lblNamaTTD2.Text = namaTTD2.ParameterValue;
                lblNamaTTD3.Text = namaTTD3.ParameterValue;
                lblNamaTTD4.Text = namaTTD4.ParameterValue;

                lblttd1.Text = "Dir.Pelayanan dan Penunjang Medis";
                lblttd2.Text = "Ka.Bid Penunjang Medis";
                lblttd3.Text = "Ka. Instalasi Farmasi";
                lblttd4.Text = "Ka.Tim. Pengadaan & Pembelian";
            }
            else
            {
                lblReportTitle.Text = "SURAT PEMESANAN BARANG";

                string filterNamattd1 = string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.DIREKTUR_UMUM); //Nama Direktur
                string filterNamattd2 = string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.FN_WAKIL_DIREKTUR_KEUANGAN); // Direktur Keuangan
                string filterNamattd3 = string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.FN_KEPALABAGIAN_KEUANGAN); // PLT Kabag Keuangan
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
                lblttd3.Text = "PLT Kabag Keuangan";
                lblttd4.Text = "Ketua Tim Pengadaan & Pembelian";
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
