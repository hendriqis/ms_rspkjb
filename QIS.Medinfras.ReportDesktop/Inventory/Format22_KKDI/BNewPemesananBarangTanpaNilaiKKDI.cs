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
    public partial class BNewPemesananBarangTanpaNilaiKKDI : BaseDailyPortraitRpt
    {
        public BNewPemesananBarangTanpaNilaiKKDI()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            base.InitializeReport(param);

            vPurchaseOrderHd entity = BusinessLayer.GetvPurchaseOrderHdList(param[0])[0];
            lblPurchaseOrderNo.Text = entity.PurchaseOrderNo;
            lblPurchaseOrderDate.Text = entity.OrderDateInString;
            lblNotes.Text = entity.Remarks;
            lblSyarat.Text = entity.PaymentRemarks;
            lblSupplierCode.Text = entity.BusinessPartnerCode;
            lblSupplierName.Text = entity.BusinessPartnerName;

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

            decimal totalPemesanan = (total - totalDiskon) + ppn;

            #endregion

            if (entity.GCPurchaseOrderType != Constant.PurchaseOrderType.LOGISTIC && entity.GCPurchaseOrderType != Constant.PurchaseOrderType.ASSET)
            {
                lblReportTitle.Text = "SURAT PEMESANAN OBAT DAN ALKES";


                string filterparam = string.Format(" ParameterCode IN ('{0}', '{1}','{2}')", Constant.SettingParameter.KEPALA_LOGISTIK_OBAT,
                    Constant.SettingParameter.KEPALA_LOGISTIK_UMUM, Constant.SettingParameter.PHARMACIST);
                List<SettingParameterDt> lstSetPar = BusinessLayer.GetSettingParameterDtList(filterparam);
                lblPemesan.Text = entity.CreatedByName;
                if (entity.GCItemType == Constant.ItemType.BARANG_UMUM)
                {
                    lblMengetahui.Text = lstSetPar.Where(lst => lst.ParameterCode == Constant.SettingParameter.KEPALA_LOGISTIK_UMUM).FirstOrDefault().ParameterValue;
                }
                else
                {
                    lblMengetahui.Text = lstSetPar.Where(lst => lst.ParameterCode == Constant.SettingParameter.PHARMACIST).FirstOrDefault().ParameterValue;
                }
                lblMenyetujui.Text = "dr. SUSI ANGGRAINI, MM";
            }
            else
            {
                lblReportTitle.Text = "SURAT PEMESANAN BARANG";

                string filterparam = string.Format(" ParameterCode IN ('{0}', '{1}','{2}')", Constant.SettingParameter.KEPALA_LOGISTIK_OBAT,
                    Constant.SettingParameter.KEPALA_LOGISTIK_UMUM, Constant.SettingParameter.PHARMACIST);
                List<SettingParameterDt> lstSetPar = BusinessLayer.GetSettingParameterDtList(filterparam);
                lblPemesan.Text = entity.CreatedByName;
                if (entity.GCItemType == Constant.ItemType.BARANG_UMUM)
                {
                    lblMengetahui.Text = lstSetPar.Where(lst => lst.ParameterCode == Constant.SettingParameter.KEPALA_LOGISTIK_UMUM).FirstOrDefault().ParameterValue;
                }
                else
                {
                    lblMengetahui.Text = lstSetPar.Where(lst => lst.ParameterCode == Constant.SettingParameter.PHARMACIST).FirstOrDefault().ParameterValue;
                }
                lblMenyetujui.Text = "dr. SUSI ANGGRAINI, MM";
            }
        }

        private void lblNotes_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (String.IsNullOrEmpty(lblNotes.Text))
            {
                xrLabel17.Text = "";
                xrLabel16.Text = "";
            }
        }

        private void lblSyarat_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (String.IsNullOrEmpty(lblSyarat.Text))
            {
                xrLabel1.Text = "";
                xrLabel2.Text = "";
            }
        }
    }
}
