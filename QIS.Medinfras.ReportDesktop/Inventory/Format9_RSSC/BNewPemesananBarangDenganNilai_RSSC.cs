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
    public partial class BNewPemesananBarangDenganNilai_RSSC : BaseDailyPortraitRpt
    {
        public BNewPemesananBarangDenganNilai_RSSC()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            base.InitializeReport(param);

            vPurchaseOrderHd entity = BusinessLayer.GetvPurchaseOrderHdList(param[0])[0];
            List<PurchaseOrderDt> entityDT = BusinessLayer.GetPurchaseOrderDtList(string.Format("PurchaseOrderID = {0} AND IsDeleted = 0", entity.PurchaseOrderID));
            lblPurchaseOrderNo.Text = entity.PurchaseOrderNo;
            lblPurchaseOrderDate.Text = entity.OrderDateInString;
            lblNotes.Text = entity.Remarks;
            lblSupplierCode.Text = entity.BusinessPartnerCode;
            lblSupplierName.Text = entity.BusinessPartnerName;

            BusinessPartners bp = BusinessLayer.GetBusinessPartnersList(string.Format("BusinessPartnerID = '{0}'", entity.BusinessPartnerID)).FirstOrDefault();
            Address ad = BusinessLayer.GetAddressList(string.Format("AddressID = '{0}'", bp.AddressID)).FirstOrDefault();
            lblSupplierAddress.Text = ad.StreetName;

            StandardCode sc = BusinessLayer.GetStandardCodeList(string.Format("StandardCodeID = '{0}'", entity.GCPurchaseOrderType)).FirstOrDefault();
            lblPOType.Text = sc.StandardCodeName;
            lblPrintDate.Text = DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT);

            if (entity.IsUrgent)
            {
                lblIsUrgent.Visible = true;
            }
            else 
            {
                lblIsUrgent.Visible = false;            
            }

            string filterExpression = string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.IM_MAX_TRANSACTION_AMOUNT_PURCHASE_ORDER);
            SettingParameterDt lstParam = BusinessLayer.GetSettingParameterDtList(filterExpression).FirstOrDefault();
            Decimal amount = Convert.ToDecimal(lstParam.ParameterValue);

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

            if (entity.GCPurchaseOrderType != Constant.PurchaseOrderType.LOGISTIC && entity.GCPurchaseOrderType != Constant.PurchaseOrderType.ASSET)
            {
                lblReportTitle.Text = "SURAT PEMESANAN OBAT DAN ALKES";

                string filterExpressionApprover = string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.KEPALA_LOGISTIK_UMUM);
                SettingParameterDt kepLogistik = BusinessLayer.GetSettingParameterDtList(filterExpressionApprover).FirstOrDefault();
                SettingParameter setvar = BusinessLayer.GetSettingParameter(kepLogistik.ParameterCode);
                lblKepalaLogistik.Text = kepLogistik.ParameterValue;
                lblUser.Text = AppSession.UserLogin.UserFullName;
                xrLabel34.Text = setvar.ParameterName;
            }
            else
            {
                lblReportTitle.Text = "SURAT PEMESANAN BARANG";

                string filterExpressionApprover = string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.KEPALA_LOGISTIK_UMUM);
                SettingParameterDt kepLogistik = BusinessLayer.GetSettingParameterDtList(filterExpressionApprover).FirstOrDefault();
                SettingParameter setvar = BusinessLayer.GetSettingParameter(kepLogistik.ParameterCode);
                lblKepalaLogistik.Text = kepLogistik.ParameterValue;
                lblUser.Text = AppSession.UserLogin.UserFullName;
                xrLabel34.Text = setvar.ParameterName;
            }

        }

        private void lblNotes_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (String.IsNullOrEmpty(lblNotes.Text)) {
                xrLabel17.Text = "";
                xrLabel16.Text = "";
            }
        }

    }
}
