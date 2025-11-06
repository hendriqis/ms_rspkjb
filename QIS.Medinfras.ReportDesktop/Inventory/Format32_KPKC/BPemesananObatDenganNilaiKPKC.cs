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
    public partial class BPemesananObatDenganNilaiKPKC : BaseDailyPortraitRpt
    {
        public BPemesananObatDenganNilaiKPKC()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            base.InitializeReport(param);

            vPurchaseOrderHd entity = BusinessLayer.GetvPurchaseOrderHdList(param[0])[0];
            List<PurchaseOrderDt> entityDT = BusinessLayer.GetPurchaseOrderDtList(string.Format("PurchaseOrderID = {0} AND IsDeleted = 0", entity.PurchaseOrderID));
            vHealthcare entityHealthcare = BusinessLayer.GetvHealthcareList(String.Format("HealthcareID = '{0}'", appSession.HealthcareID))[0];
            lblPurchaseOrderNo.Text = entity.PurchaseOrderNo;
            lblPurchaseOrderDate.Text = entity.OrderDateInString;
            lblNotes.Text = entity.Remarks;
            lblSyarat.Text = entity.PaymentRemarks;
            lblSupplierCode.Text = entity.BusinessPartnerCode;
            lblSupplierName.Text = entity.BusinessPartnerName;
            lblTanggal.Text = string.Format(DateTime.Now.ToString("dd-MMM-yyyy"));

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
            decimal ongkosKirim = entity.ChargesAmount;
            decimal downpayment = entity.DownPaymentAmount;

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

            decimal totalPemesanan = (total - totalDiskon) + ppn - downpayment + ongkosKirim + entity.PPHAmount;

            #endregion

            #region Total Header

            lblTotal.Text = total.ToString("N2");
            lblDiskon.Text = totalDiskon.ToString("N2");
            lblPPN.Text = ppn.ToString("N2");
            lblTotalPemesanan.Text = totalPemesanan.ToString("N2");

            #endregion

            SettingParameter setvarApt = BusinessLayer.GetSettingParameterList(string.Format(
                    "ParameterCode = '{0}'", Constant.SettingParameter.PHARMACIST)).FirstOrDefault();
            SettingParameter setvarSipa = BusinessLayer.GetSettingParameterList(string.Format(
                     "ParameterCode = '{0}'", Constant.SettingParameter.PHARMACIST_LICENSE_NO)).FirstOrDefault();
            lblNamaApoteker.Text = setvarApt.ParameterValue;
            lblNamaKepalaFarmasi.Text = setvarApt.ParameterValue;
            lblNamaKepalaPembelian.Text = setvarApt.ParameterValue;
            lblSipa.Text = string.Format("SIPA {0}",setvarSipa.ParameterValue);
            lblKaFarmasi.Text = string.Format("SIPA {0}", setvarSipa.ParameterValue);
            lblKepalaPembelian.Text = string.Format("SIPA {0}", setvarSipa.ParameterValue);

            SettingParameter setvarPjFar = BusinessLayer.GetSettingParameterList(string.Format(
                    "ParameterCode = '{0}'", Constant.SettingParameter.MANAGER_FARMASI)).FirstOrDefault();
            //lblKaFarmasi.Text = setvarPjFar.ParameterName;
            //lblNamaKepalaFarmasi.Text = setvarPjFar.ParameterValue;

            SettingParameter setvarKaPem = BusinessLayer.GetSettingParameterList(string.Format(
                     "ParameterCode = '{0}'", Constant.SettingParameter.KEPALA_LOGISTIK_UMUM)).FirstOrDefault();
            //lblKepalaPembelian.Text = setvarKaPem.ParameterName;
            //lblNamaKepalaPembelian.Text = setvarKaPem.ParameterValue;

            //lblSipa.Text = setvarSipa.ParameterValue;


            base.InitializeReport(param);
        }

        private void lblNotes_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (String.IsNullOrEmpty(lblNotes.Text)) {
                xrLabel17.Text = "";
                xrLabel16.Text = "";
            }
        }

        private void lblSyarat_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (String.IsNullOrEmpty(lblSyarat.Text)) {
                xrLabel19.Text = "";
                xrLabel22.Text = "";
            }
        }

    }
}
