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
    public partial class BNew4PemesananBarangKonsinyasiDenganNilai : BaseDailyPortraitRpt
    {
        public BNew4PemesananBarangKonsinyasiDenganNilai()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            vPurchaseOrderHd entity = BusinessLayer.GetvPurchaseOrderHdList(param[0])[0];
            List<PurchaseOrderDt> entityDT = BusinessLayer.GetPurchaseOrderDtList(string.Format("PurchaseOrderID = {0} AND IsDeleted = 0", entity.PurchaseOrderID));
            vHealthcare entityHealthcare = BusinessLayer.GetvHealthcareList(String.Format("HealthcareID = '{0}'", appSession.HealthcareID))[0];

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

            #region Header

            lblNamaRS.Text = entityHealthcare.HealthcareName;
            lblAlamat.Text = entityHealthcare.StreetName;
            lblTelepon.Text = entityHealthcare.PhoneNo1;
            lblFax.Text = entityHealthcare.FaxNo1;

            lblTotal.Text = total.ToString("N2");
            lblDiskon.Text = totalDiskon.ToString("N2");
            lblPPN.Text = ppn.ToString("N2");
            lblTotalHargaPemesanan.Text = (totalPemesanan + entity.DownPaymentAmount).ToString("N2");
            lblUangMuka.Text = entity.DownPaymentAmount.ToString("N2");
            lblSisaHargaPemesanan.Text = totalPemesanan.ToString("N2");
            lblTerbilang.Text = string.Format("Terbilang : #{0}#", Function.NumberInWords(Convert.ToInt32(totalPemesanan), true));
            lblTanggal.Text = string.Format("{0}, {1}", entityHealthcare.City, DateTime.Now.ToString("dd-MMM-yyyy"));
            lblReportTitle.Text = "SURAT PESANAN";

            //SettingParameterDt locationLF = BusinessLayer.GetSettingParameterDt(appSession.HealthcareID, Constant.SettingParameter.IM_DEFAULT_LOCATION_PURCHASEORDER_PHARMACY);
            //SettingParameterDt locationBU = BusinessLayer.GetSettingParameterDt(appSession.HealthcareID, Constant.SettingParameter.IM_DEFAULT_LOCATION_PURCHASEORDER_LOGISTIC);

            string filterExpression1 = string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.PHARMACIST);
            SettingParameterDt lstParam1 = BusinessLayer.GetSettingParameterDtList(filterExpression1).FirstOrDefault();

            string filterExpression2 = string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.PHARMACIST_LICENSE_NO);
            SettingParameterDt lstParam2 = BusinessLayer.GetSettingParameterDtList(filterExpression2).FirstOrDefault();

            //if (entity.LocationCode == locationLF.ParameterValue)
            //{
            //    lblHormatKami.Text = "Supplier";
            //    lnHormatKami.Visible = true;

            //    lblMengetahui2.Text = "Apoteker";
            lblApoteker.Text = lstParam1.ParameterValue;
            lblSIPA.Text = lstParam2.ParameterValue;
            //    lnApoteker.Visible = true;
            //}
            //else
            //{
            //    lblHormatKami.Text = string.Empty;
            //    lnHormatKami.Visible = false;

            //    lblMengetahui2.Text = "Supplier";
            //    lblApoteker.Text = string.Empty;
            //    lblSIPA.Text = string.Empty;
            //}

            #endregion

            base.InitializeReport(param);
        }

        private void lblNotes_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (String.IsNullOrEmpty(lblNotes.Text))
            {
                lblNotes.Text = "-";
            }
        }

    }
}
