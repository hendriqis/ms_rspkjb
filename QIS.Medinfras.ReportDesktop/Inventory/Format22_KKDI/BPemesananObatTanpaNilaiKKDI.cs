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
    public partial class BPemesananObatTanpaNilaiKKDI : BaseDailyPortraitRpt
    {
        public BPemesananObatTanpaNilaiKKDI()
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
