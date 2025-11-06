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
    public partial class BLampiranPemesananBarang : BaseDailyPortraitRpt
    {
        public BLampiranPemesananBarang()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            base.InitializeReport(param);

            vPurchaseOrderHd entity = BusinessLayer.GetvPurchaseOrderHdList(param[0])[0];

            if (entity.IsUrgent)
            {
                lblIsUrgent.Visible = true;
            }
            else
            {
                lblIsUrgent.Visible = false;
            }

            if (entity.GCPurchaseOrderType != Constant.PurchaseOrderType.LOGISTIC && entity.GCPurchaseOrderType != Constant.PurchaseOrderType.ASSET)
            {
                lblReportTitle.Text = "LAMPIRAN SURAT PEMESANAN OBAT DAN ALKES";

                //string filterExpression3 = string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.PHARMACIST);
                //SettingParameterDt lstParam3 = BusinessLayer.GetSettingParameterDtList(filterExpression3).FirstOrDefault();
                //lblApoteker.Text = lstParam3.ParameterValue;

                //string filterExpression4 = string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.PHARMACIST_LICENSE_NO);
                //SettingParameterDt lstParam4 = BusinessLayer.GetSettingParameterDtList(filterExpression4).FirstOrDefault();
                //lblSIK.Text = lstParam4.ParameterValue;

                //lblKepalaLogistik.Text = appSession.UserFullName;
            }
            else
            {
                lblReportTitle.Text = "LAMPIRAN SURAT PEMESANAN BARANG";

                //string filterExpression3 = string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.DIREKTUR_KEUANGAN);
                //SettingParameterDt lstParam3 = BusinessLayer.GetSettingParameterDtList(filterExpression3).FirstOrDefault();
                //lblApoteker.Text = lstParam3.ParameterValue;

                //lblSIK.Visible = false;

                //lblKepalaLogistik.Text = appSession.UserFullName;
            }
        }

        private void xrLabel26_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            String PurchaseRequestNo = GetCurrentColumnValue("PurchaseRequestNo").ToString();
            if (PurchaseRequestNo == "" || PurchaseRequestNo == null)
            {
                xrLabel26.Text = "";
            }
            else
            {
                xrLabel26.Text = "Purchase Request No = " + PurchaseRequestNo;
            }
        }

    }
}
