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
    public partial class BPemesananObatTanpaNilaiRSSC : BaseDailyPortraitRpt
    {
        public BPemesananObatTanpaNilaiRSSC()
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


            SettingParameter setvarApt = BusinessLayer.GetSettingParameterList(string.Format(
                    "ParameterCode = '{0}'", Constant.SettingParameter.PHARMACIST)).FirstOrDefault();
            lblApoteker.Text = setvarApt.ParameterName;
            lblNamaApoteker.Text = setvarApt.ParameterValue;

            SettingParameter setvarPjFar = BusinessLayer.GetSettingParameterList(string.Format(
                    "ParameterCode = '{0}'", Constant.SettingParameter.MANAGER_FARMASI)).FirstOrDefault();
            lblKaFarmasi.Text = setvarPjFar.ParameterName;
            lblNamaKepalaFarmasi.Text = setvarPjFar.ParameterValue;

            SettingParameter setvarKaPem = BusinessLayer.GetSettingParameterList(string.Format(
                     "ParameterCode = '{0}'", Constant.SettingParameter.KEPALA_LOGISTIK_UMUM)).FirstOrDefault();
            lblKepalaPembelian.Text = setvarKaPem.ParameterName;
            lblNamaKepalaPembelian.Text = setvarKaPem.ParameterValue;

            SettingParameter setvarSipa = BusinessLayer.GetSettingParameterList(string.Format(
                     "ParameterCode = '{0}'", Constant.SettingParameter.PHARMACIST_LICENSE_NO)).FirstOrDefault();
            lblSipa.Text = setvarSipa.ParameterValue;


            base.InitializeReport(param);
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
