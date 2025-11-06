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
    public partial class BNewPemesananBarangDenganNilai_RSDOSOBA : BaseDailyPortraitRpt
    {
        public BNewPemesananBarangDenganNilai_RSDOSOBA()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            base.InitializeReport(param);

            #region ReportHeader
            string filterExpressionHeader = string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.IM_NOMOR_IZIN_RUMAH_SAKIT_PO);
            SettingParameterDt noIzinRS = BusinessLayer.GetSettingParameterDtList(filterExpressionHeader).FirstOrDefault();
            lblNoIzin.Text = string.Format("No Izin RS : {0}", noIzinRS.ParameterValue);
            #endregion

            vPurchaseOrderHd entity = BusinessLayer.GetvPurchaseOrderHdList(param[0])[0];
            List<PurchaseOrderDt> entityDT = BusinessLayer.GetPurchaseOrderDtList(string.Format("PurchaseOrderID = {0} AND IsDeleted = 0", entity.PurchaseOrderID));
            lblPurchaseOrderNo.Text = entity.PurchaseOrderNo;
            lblPurchaseOrderDate.Text = entity.OrderDateInString;
            lblSupplierName.Text = String.Format("{0} {1}", entity.BusinessPartnerCode, entity.BusinessPartnerName);
            lblProductLine.Text = entity.ProductLineName;

            BusinessPartners bp = BusinessLayer.GetBusinessPartnersList(string.Format("BusinessPartnerID = '{0}'", entity.BusinessPartnerID)).FirstOrDefault();
            Address ad = BusinessLayer.GetAddressList(string.Format("AddressID = '{0}'", bp.AddressID)).FirstOrDefault();
            lblSupplierAddress.Text = ad.StreetName;

            StandardCode sc = BusinessLayer.GetStandardCodeList(string.Format("StandardCodeID = '{0}'", entity.GCPurchaseOrderType)).FirstOrDefault();
            lblPOType.Text = sc.StandardCodeName;

            string filterExpression = string.Format("ParameterCode IN ('{0}', '{1}')", Constant.SettingParameter.IM_MAX_TRANSACTION_AMOUNT_PURCHASE_ORDER,
                        Constant.SettingParameter.IM_DEFAULT_LOCATION_PURCHASEORDER_PHARMACY);
            List<SettingParameterDt> lstParam = BusinessLayer.GetSettingParameterDtList(filterExpression);
            //Decimal amount = Convert.ToDecimal(lstParam.ParameterValue[0]);
            Decimal amount = Convert.ToDecimal(lstParam.Where(lst => lst.ParameterCode == Constant.SettingParameter.IM_MAX_TRANSACTION_AMOUNT_PURCHASE_ORDER).FirstOrDefault().ParameterValue);
            String location = lstParam.Where(lst => lst.ParameterCode == Constant.SettingParameter.IM_DEFAULT_LOCATION_PURCHASEORDER_PHARMACY).FirstOrDefault().ParameterValue;

            if (entity.IsUrgent)
                lblIsUrgent.Visible = true;
            else
                lblIsUrgent.Visible = false;

            if (entity.PaymentRemarks == "")
                lblPaymentRemarks.Text = "";
            else
                lblPaymentRemarks.Text = String.Format("Syarat Pembayaran : {0}", entity.PaymentRemarks);

            if (entity.Remarks == "")
                lblRemarks.Text = "";
            else
                lblRemarks.Text = String.Format("Catatan : {0}", entity.Remarks);

            #region Hitung Total

            decimal total = entity.TransactionAmount;
            decimal downpayment = entity.DownPaymentAmount;

            decimal totalDiskon = 0;
            if (!entity.IsFinalDiscountInPercentage)
            {
                totalDiskon = (entity.FinalDiscountAmount);
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

            decimal charges = entity.ChargesAmount;

            decimal totalPemesanan = total + ppn + pph - totalDiskon - downpayment + charges;

            #endregion

            #region Total Header

            lblTotal.Text = total.ToString("N2");
            //lblDiskon.Text = totalDiskon.ToString("N2");
            lblPPN.Text = ppn.ToString("N2");
            lblPPH.Text = pph.ToString("N2");
            lblCharges.Text = charges.ToString("N2");
            lblTotalPemesanan.Text = totalPemesanan.ToString("N2");

            #endregion

            if ((entity.GCPurchaseOrderType == Constant.PurchaseOrderType.DRUGMS || entity.GCPurchaseOrderType == Constant.PurchaseOrderType.ALKES
            || entity.GCPurchaseOrderType == Constant.PurchaseOrderType.OBAT_RANAP || entity.GCPurchaseOrderType == Constant.PurchaseOrderType.OBAT_RAJAL) && entity.LocationCode == location)
            {
                lblReportTitle.Text = "SURAT PEMESANAN OBAT DAN ALKES";

                string filterExpressionApprover = string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.MANAGER_FARMASI);
                SettingParameterDt kepFarmasi = BusinessLayer.GetSettingParameterDtList(filterExpressionApprover).FirstOrDefault();
                lblPJ.Text = kepFarmasi.ParameterValue;
                lblUser.Text = AppSession.UserLogin.UserFullName;
            }
            else
            {
                lblReportTitle.Text = "SURAT PEMESANAN BARANG";

                string filterExpressionApprover = string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.KEPALA_LOGISTIK_MEDIK);
                SettingParameterDt kepLogistik = BusinessLayer.GetSettingParameterDtList(filterExpressionApprover).FirstOrDefault();
                lblPJ.Text = kepLogistik.ParameterValue;
                lblUser.Text = AppSession.UserLogin.UserFullName;
            }

            lblFooterInfo.Text = String.Format("MEDINFRAS - {0}, Print Date/Time:{1}, User ID:{2}", reportMaster.ReportCode, DateTime.Now.ToString("dd-MMM-yyyy/HH:mm:ss"), AppSession.UserLogin.UserName);
        }
    }
}
