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
    public partial class BPemesananBarangTanpaNilai : BaseDailyPortraitRpt
    {
        public BPemesananBarangTanpaNilai()
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
            lblNotes.Text = entity.Remarks;
            lblSupplierCode.Text = entity.BusinessPartnerCode;
            lblSupplierName.Text = entity.BusinessPartnerName;
            
            if (entity.GCPurchaseOrderType != Constant.PurchaseOrderType.LOGISTIC && entity.GCPurchaseOrderType != Constant.PurchaseOrderType.ASSET)
            {
                lblReportTitle.Text = "SURAT PEMESANAN OBAT DAN ALKES";

                string filterExpression1 = string.Format("ParameterCode IN ('{0}')", "SA0020");
                List<SettingParameterDt> lstParam1 = BusinessLayer.GetSettingParameterDtList(filterExpression1);
                lblDirektur.Text = lstParam1.Where(lst => lst.ParameterCode == "SA0020").FirstOrDefault().ParameterValue;

                string filterExpression2 = string.Format("ParameterCode IN ('{0}')", "SA0021");
                List<SettingParameterDt> lstParam2 = BusinessLayer.GetSettingParameterDtList(filterExpression2);
                lblKeuangan.Text = lstParam2.Where(lst => lst.ParameterCode == "SA0021").FirstOrDefault().ParameterValue;

                string filterExpression3 = string.Format("ParameterCode IN ('{0}')", "PH0004");
                List<SettingParameterDt> lstParam3 = BusinessLayer.GetSettingParameterDtList(filterExpression3);
                lblApoteker.Text = lstParam3.Where(lst => lst.ParameterCode == "PH0004").FirstOrDefault().ParameterValue;

                string filterExpression4 = string.Format("ParameterCode IN ('{0}')", "PH0005");
                List<SettingParameterDt> lstParam4 = BusinessLayer.GetSettingParameterDtList(filterExpression4);
                lblSIK.Text = lstParam4.Where(lst => lst.ParameterCode == "PH0005").FirstOrDefault().ParameterValue;

                string filterExpression5 = string.Format("ParameterCode IN ('{0}')", "IM0009");
                List<SettingParameterDt> lstParam5 = BusinessLayer.GetSettingParameterDtList(filterExpression5);
                lblKepalaLogistik.Text = lstParam5.Where(lst => lst.ParameterCode == "IM0009").FirstOrDefault().ParameterValue;

            }
            else
            {
                lblReportTitle.Text = "SURAT PEMESANAN BARANG";

                string filterExpression1 = string.Format("ParameterCode IN ('{0}')", "SA0020");
                List<SettingParameterDt> lstParam1 = BusinessLayer.GetSettingParameterDtList(filterExpression1);
                lblDirektur.Text = lstParam1.Where(lst => lst.ParameterCode == "SA0020").FirstOrDefault().ParameterValue;

                string filterExpression2 = string.Format("ParameterCode IN ('{0}')", "SA0021");
                List<SettingParameterDt> lstParam2 = BusinessLayer.GetSettingParameterDtList(filterExpression2);
                lblKeuangan.Text = lstParam2.Where(lst => lst.ParameterCode == "SA0021").FirstOrDefault().ParameterValue;

                lblMengetahui2.Visible = false;
                lblPenanggungJawab.Visible = false;
                lblApoteker.Visible = false;
                lblSIK.Visible = false;
                lnApoteker.Visible = false;

                string filterExpression5 = string.Format("ParameterCode IN ('{0}')", "IM0009");
                List<SettingParameterDt> lstParam5 = BusinessLayer.GetSettingParameterDtList(filterExpression5);
                lblKepalaLogistik.Text = lstParam5.Where(lst => lst.ParameterCode == "IM0009").FirstOrDefault().ParameterValue;
            }

            if (entity.IsUrgent)
            {
                lblIsUrgent.Visible = true;
            }
            else
            {
                lblIsUrgent.Visible = false;
            }
        }
    }
}
