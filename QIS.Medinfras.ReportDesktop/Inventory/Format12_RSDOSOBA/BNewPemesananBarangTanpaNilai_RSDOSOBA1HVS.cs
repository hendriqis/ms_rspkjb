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
    public partial class BNewPemesananBarangTanpaNilai_RSDOSOBA1HVS : BaseDailyPortraitRpt
    {
        public BNewPemesananBarangTanpaNilai_RSDOSOBA1HVS()
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
            lblSupplierName.Text = String.Format("{0} {1}", entity.BusinessPartnerCode, entity.BusinessPartnerName);

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
