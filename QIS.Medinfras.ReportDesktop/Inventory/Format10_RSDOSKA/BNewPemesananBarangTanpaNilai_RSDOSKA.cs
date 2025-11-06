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
    public partial class BNewPemesananBarangTanpaNilai_RSDOSKA : BaseDailyPortraitRpt
    {
        public BNewPemesananBarangTanpaNilai_RSDOSKA()
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
            string oLocation = lstParam.Where(lst => lst.ParameterCode == Constant.SettingParameter.IM_DEFAULT_LOCATION_PURCHASEORDER_PHARMACY).FirstOrDefault().ParameterValue;

            if (entity.GCPurchaseOrderType != Constant.PurchaseOrderType.LOGISTIC && entity.GCPurchaseOrderType != Constant.PurchaseOrderType.ASSET && entity.LocationID.ToString() == oLocation)
            {
                lblReportTitle.Text = "SURAT PEMESANAN OBAT DAN ALKES";

                //string filterExpressionApprover = string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.MANAGER_FARMASI);
                //SettingParameterDt kepFarmasi = BusinessLayer.GetSettingParameterDtList(filterExpressionApprover).FirstOrDefault();
                lblUser.Text = AppSession.UserLogin.UserFullName;
            }
            else
            {
                lblReportTitle.Text = "SURAT PEMESANAN BARANG";

                //string filterExpressionApprover = string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.KEPALA_LOGISTIK_MEDIK);
                //SettingParameterDt kepLogistik = BusinessLayer.GetSettingParameterDtList(filterExpressionApprover).FirstOrDefault();
                lblUser.Text = AppSession.UserLogin.UserFullName;
            }

        }
    }
}
