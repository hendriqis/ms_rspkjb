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
    public partial class BNewPemesananBarangDenganNilai_RSRA : BaseDailyPortraitRpt
    {
        public BNewPemesananBarangDenganNilai_RSRA()
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
            lblApproveBy.Text = string.Format("Approval By: {0} / {1}", entity.ApprovedByName, entity.ApprovedDate.ToString(Constant.FormatString.DATE_FORMAT));
            BusinessPartners bp = BusinessLayer.GetBusinessPartnersList(string.Format("BusinessPartnerID = '{0}'", entity.BusinessPartnerID)).FirstOrDefault();
            Address ad = BusinessLayer.GetAddressList(string.Format("AddressID = '{0}'", bp.AddressID)).FirstOrDefault();
            lblSupplierAddress.Text = ad.StreetName;
            lblPhoneNo.Text = ad.PhoneNo1;
            lblOngkir.Text = entity.ChargesAmount.ToString("N2");
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
            lblTotalPemesanan.Text = totalPemesanan.ToString("N2");

            #endregion

            if (entity.GCPurchaseOrderType != Constant.PurchaseOrderType.LOGISTIC && entity.GCPurchaseOrderType != Constant.PurchaseOrderType.ASSET && entity.LocationID.ToString() == oLocation)
            {
                lblReportTitle.Text = "SURAT PEMESANAN OBAT DAN ALKES";

                //string filterExpressionApprover = string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.KEPALA_LOGISTIK_OBAT, );
                //SettingParameter stafPembelian = BusinessLayer.GetSettingParameterList(filterExpressionApprover).FirstOrDefault();
                string filterExpressionApprover = string.Format("ParameterCode IN ('{0}', '{1}', '{2}', '{3}')", Constant.SettingParameter.MANAGER_FARMASI, Constant.SettingParameter.PHARMACIST_LICENSE_NO, Constant.SettingParameter.KEPALA_LOGISTIK_UMUM, Constant.SettingParameter.IM_BAGIAN_PURCHASING);
                List<SettingParameter> lstSettingParameter = BusinessLayer.GetSettingParameterList(filterExpressionApprover);

                SettingParameter stafPembelian = lstSettingParameter.Where(p => p.ParameterCode == Constant.SettingParameter.MANAGER_FARMASI).FirstOrDefault();
                SettingParameter license = lstSettingParameter.Where(p => p.ParameterCode == Constant.SettingParameter.PHARMACIST_LICENSE_NO).FirstOrDefault();
                SettingParameter pengadaan = lstSettingParameter.Where(p => p.ParameterCode == Constant.SettingParameter.KEPALA_LOGISTIK_UMUM).FirstOrDefault();
                SettingParameter purchasing = lstSettingParameter.Where(p => p.ParameterCode == Constant.SettingParameter.IM_BAGIAN_PURCHASING).FirstOrDefault();

                lblKepadalPengadaan.Text = pengadaan.ParameterValue;
                lblUser.Text = stafPembelian.ParameterValue;
                lblJabatan2.Text = license.ParameterValue;
                //lblPurchasing.Text = purchasing.ParameterValue;

                string filterExpressionDirector = string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.DIREKTUR_YANMED);
                SettingParameter direkturUtama = BusinessLayer.GetSettingParameterList(filterExpressionDirector).FirstOrDefault();

                lblKepalaLogistik.Text = direkturUtama.ParameterValue;
                lblJabatan.Text = direkturUtama.ParameterName;

                lblPemesan.Text = "Kepala Farmasi";

                lblttdKplPengadaan.Visible = true;
                lblKpPenagadaan.Visible = true;
                lblKepalaLogistik.Visible = true;
                lblKepadalPengadaan.Visible = true;
            }
            else
            {
                lblReportTitle.Text = "SURAT PEMESANAN BARANG";

                // string filterExpressionApprover = string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.STAFF_PEMBELIAN);
                /// SettingParameter stafPembelian = BusinessLayer.GetSettingParameterList(filterExpressionApprover).FirstOrDefault();
                string filterExpressionApprover = string.Format("ParameterCode IN ('{0}', '{1}', '{2}', '{3}')", Constant.SettingParameter.DIREKTUR_YANMED, Constant.SettingParameter.DIREKTUR_KEUANGAN, Constant.SettingParameter.KEPALA_LOGISTIK_UMUM, Constant.SettingParameter.IM_BAGIAN_PURCHASING);
                List<SettingParameter> lstSettingParameter = BusinessLayer.GetSettingParameterList(filterExpressionApprover);

                SettingParameter stafPembelian = lstSettingParameter.Where(p => p.ParameterCode == Constant.SettingParameter.KEPALA_LOGISTIK_UMUM).FirstOrDefault();
                SettingParameter dirku = lstSettingParameter.Where(p => p.ParameterCode == Constant.SettingParameter.DIREKTUR_KEUANGAN).FirstOrDefault();
                SettingParameter pengadaan = lstSettingParameter.Where(p => p.ParameterCode == Constant.SettingParameter.KEPALA_LOGISTIK_OBAT).FirstOrDefault();
                SettingParameter purchasing = lstSettingParameter.Where(p => p.ParameterCode == Constant.SettingParameter.IM_BAGIAN_PURCHASING).FirstOrDefault();

                lblUser.Text = stafPembelian.ParameterValue;
                lblJabatan2.Text = stafPembelian.ParameterName;
                //lblPurchasing.Text = purchasing.ParameterValue;

                string filterExpressionDirector = string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.DIREKTUR_YANMED);
                SettingParameter direkturUtama = BusinessLayer.GetSettingParameterList(filterExpressionDirector).FirstOrDefault();

                lblKepalaLogistik.Text = direkturUtama.ParameterValue;
                lblJabatan.Text = direkturUtama.ParameterName;

                lblPemesan.Text = "Kepala Pengadaan";

                lblKpPenagadaan.Text = "Mengetahui";
                lblKepadalPengadaan.Text = dirku.ParameterValue;
                lblttdKplPengadaan.Text = dirku.ParameterName; 
                //// lblttdKplPengadaan.Visible = false;
               //// lblKpPenagadaan.Visible = false;
                ////lblKepalaLogistik.Visible = false;
               /// lblKepadalPengadaan.Visible = false;
            }
        }

        private void lblNotes_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (String.IsNullOrEmpty(lblNotes.Text)) {
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
