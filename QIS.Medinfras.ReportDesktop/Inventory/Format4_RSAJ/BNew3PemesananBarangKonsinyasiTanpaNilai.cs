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
    public partial class BNew3PemesananBarangKonsinyasiTanpaNilai : BaseLegalRpt
    {
        public BNew3PemesananBarangKonsinyasiTanpaNilai()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            base.InitializeReport(param);

            vPurchaseOrderHd entity = BusinessLayer.GetvPurchaseOrderHdList(param[0])[0];
            lblPurchaseOrderNo.Text = entity.PurchaseOrderNo;
            lblPurchaseOrderDate.Text = entity.OrderDateInString;
            lblSupplierCode.Text = entity.BusinessPartnerCode;
            lblSupplierName.Text = entity.BusinessPartnerName;

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
            decimal total = entity.TransactionAmount;
            decimal totalPemesanan = (total - totalDiskon) + ppn;

            string filterExpression = string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.IM_MAX_TRANSACTION_AMOUNT_PURCHASE_ORDER);
            SettingParameterDt lstParam = BusinessLayer.GetSettingParameterDtList(filterExpression).FirstOrDefault();
            Decimal amount = Convert.ToDecimal(lstParam.ParameterValue);

            if (entity.GCPurchaseOrderType != Constant.PurchaseOrderType.LOGISTIC && entity.GCPurchaseOrderType != Constant.PurchaseOrderType.ASSET)
            {
                lblReportTitle.Text = "SURAT PEMESANAN OBAT DAN ALKES";

                string filterExpression3 = string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.PHARMACIST);
                SettingParameterDt lstParam3 = BusinessLayer.GetSettingParameterDtList(filterExpression3).FirstOrDefault();
                lblPenanggungJawab.Text = lstParam3.ParameterValue;

                string filterExpression4 = string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.PHARMACIST_LICENSE_NO);
                SettingParameterDt lstParam4 = BusinessLayer.GetSettingParameterDtList(filterExpression4).FirstOrDefault();
                lblSIK.Text = lstParam4.ParameterValue;

                string filterExpression5 = string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.IM_NAME_CREATE_PO_PHARMACY);
                SettingParameter lstParam5 = BusinessLayer.GetSettingParameterList(filterExpression5).FirstOrDefault();
                lblCreatedByName.Text = lstParam5.ParameterValue;
                lblCreatedByRemark.Text = lstParam5.Notes;

                xrLine3.Visible = false;
            }
            else
            {
                lblReportTitle.Text = "SURAT PEMESANAN BARANG";

                lblMengetahui2.Visible = false;
                lblPenanggungJawabCaption.Visible = false;
                lblPenanggungJawab.Visible = false;
                lblSIK.Visible = false;

                string filterExpression5 = string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.IM_NAME_CREATE_PO_PHARMACY);
                SettingParameter lstParam5 = BusinessLayer.GetSettingParameterList(filterExpression5).FirstOrDefault();
                lblCreatedByName.Text = lstParam5.ParameterValue;
                lblCreatedByRemark.Text = lstParam5.Notes;
            }
        }
    }
}
