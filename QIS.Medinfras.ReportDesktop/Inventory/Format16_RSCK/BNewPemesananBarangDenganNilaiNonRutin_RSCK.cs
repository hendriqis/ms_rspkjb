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
    public partial class BNewPemesananBarangDenganNilaiNonRutin_RSCK : BaseDailyPortraitRpt
    {
        public BNewPemesananBarangDenganNilaiNonRutin_RSCK()
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
            lblTanggal.Text = string.Format("{0}, {1}", oHealthcare.City, DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT));

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
            }
            else
            {
                lblReportTitle.Text = "SURAT PEMESANAN BARANG";
            }

            string filterExpressionApprover = string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.KEPALA_LOGISTIK_MEDIK);
            SettingParameterDt kepLogistik = BusinessLayer.GetSettingParameterDtList(filterExpressionApprover).FirstOrDefault();

            lblPengadaan.Text = kepLogistik.ParameterValue;
            lblJabatan1.Text = "Pengadaan";

            string filterExpression1 = string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.SA0134);
            string filterExpression2 = string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.DIREKTUR_UMUM);
            //string filterExpression3 = string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.SA0136);
            List<SettingParameter> lstParam1 = BusinessLayer.GetSettingParameterList(filterExpression1);
            List<SettingParameter> lstParam2 = BusinessLayer.GetSettingParameterList(filterExpression2);
            //List<SettingParameter> lstParam3 = BusinessLayer.GetSettingParameterList(filterExpression3);

            lblWadirUmum.Text = lstParam1.FirstOrDefault().ParameterValue;
            lblJabatan2.Text = lstParam1.FirstOrDefault().Notes;

            //lblWadirMedis.Text = lstParam3.FirstOrDefault().ParameterValue;
            //lblJabatan4.Text = lstParam3.FirstOrDefault().Notes;

            string filterExp = string.Format("PurchaseOrderID = {0}", entity.PurchaseOrderID);
            List<vPurchaseOrderDt> lstDt = BusinessLayer.GetvPurchaseOrderDtList(filterExp);
            decimal payment = 10000000;

            if (totalPemesanan >= payment)
            {
                lblDirekturUmum.Text = lstParam2.FirstOrDefault().ParameterValue;
                lblJabatan3.Text = lstParam2.FirstOrDefault().Notes;
            }
            else
            {
                lblDirekturUmum.Visible = false;
                lblJabatan3.Visible = false;
                lblLine.Visible = false;
                xrLabel53.Visible = false;
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
