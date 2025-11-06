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
    public partial class BNew4PemesananBarangTanpaNilai : BaseDailyPortraitRpt
    {
        public BNew4PemesananBarangTanpaNilai()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            vPurchaseOrderHd entity = BusinessLayer.GetvPurchaseOrderHdList(param[0])[0];
            List<PurchaseOrderDt> entityDT = BusinessLayer.GetPurchaseOrderDtList(string.Format("PurchaseOrderID = {0} AND IsDeleted = 0", entity.PurchaseOrderID));
            vHealthcare entityHealthcare = BusinessLayer.GetvHealthcareList(String.Format("HealthcareID = '{0}'", appSession.HealthcareID))[0];

            lblNamaRS.Text = entityHealthcare.HealthcareName;
            lblAlamat.Text = entityHealthcare.StreetName;
            lblTelepon.Text = entityHealthcare.PhoneNo1;
            lblFax.Text = entityHealthcare.FaxNo1;

            lblPurchaseOrderNo.Text = entity.PurchaseOrderNo;
            lblPurchaseOrderDate.Text = entity.OrderDateInString;
            if (entity.Remarks == "")
            {
                lblNotes.Visible = false;
            }
            else
            {
                lblNotes.Text = string.Format("8.{0}", entity.Remarks);
            }
            lblTglExpired.Text = entity.ExpiredDateInString;
            lblStatusPO.Text = entity.ProductLineName;
            lblTermName.Text = entity.TermName;

            vSupplier bp = BusinessLayer.GetvSupplierList(string.Format("BusinessPartnerID = {0}", entity.BusinessPartnerID)).FirstOrDefault();
            lblSupplierName.Text = bp.BusinessPartnerName;
            lblSupplierAddress.Text = bp.Address;
            lblSupplierPhone.Text = string.Format("Telp:{0} Fax:{1}", bp.PhoneNo1, bp.FaxNo1);            

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

            decimal totalPemesanan = (total - totalDiskon) + ppn - entity.DownPaymentAmount;

            #endregion

            lblTanggal.Text = string.Format("{0}, {1}", entityHealthcare.City, DateTime.Now.ToString("dd-MMM-yyyy"));
            lblReportTitle.Text = "SURAT PESANAN";

            SettingParameterDt locationLF = BusinessLayer.GetSettingParameterDt(appSession.HealthcareID, Constant.SettingParameter.IM_DEFAULT_LOCATION_PURCHASEORDER_PHARMACY);
            SettingParameterDt locationBU = BusinessLayer.GetSettingParameterDt(appSession.HealthcareID, Constant.SettingParameter.IM_DEFAULT_LOCATION_PURCHASEORDER_LOGISTIC);

            #region TTD
            string filterExpression0 = string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.IM_APPROVER_PURCHASE_ORDER);
            SettingParameterDt lstParam0 = BusinessLayer.GetSettingParameterDtList(filterExpression0).FirstOrDefault();
            lblApprover.Text = lstParam0.ParameterValue;

            string filterExpression1 = string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.MANAJER_YANMED);
            SettingParameterDt lstParam1 = BusinessLayer.GetSettingParameterDtList(filterExpression1).FirstOrDefault();
            lblDirektur.Text = lstParam1.ParameterValue;

            string filterExpression3 = string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.PHARMACIST);
            SettingParameterDt lstParam3 = BusinessLayer.GetSettingParameterDtList(filterExpression3).FirstOrDefault();
            lblApoteker.Text = lstParam3.ParameterValue;
            #endregion

            base.InitializeReport(param);
        }

        private void lblNotes_BeforePrint_1(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (String.IsNullOrEmpty(lblNotes.Text))
            {
                lblNotes.Text = "-";
            }
        }  
    }
}
