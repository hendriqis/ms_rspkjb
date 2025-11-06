using System;
using System.Drawing;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class BPemesananBarangKonsinyasiDenganNilaiRSSC : BaseDailyPortraitRpt
    {
        public BPemesananBarangKonsinyasiDenganNilaiRSSC()
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
            xrLabel15.Text = entity.Remarks;
            lblReferensi.Text = entity.ReferenceNo;
            lblPenerimaan.Text = entity.PurchaseReceiveNo;
            string createdByName = string.Empty;
            string approvedByName = string.Empty;
            string noSIPA = string.Empty;
            if (entity.GCPurchaseOrderType == "X145^001") // Persediaan Farmasi
            {
                string filterExpression = string.Format(" ParameterCode IN ('{0}','{1}','{2}')", "PH0004", "PH0005", "IM0009");
                List<SettingParameter> lstParam = BusinessLayer.GetSettingParameterList(filterExpression);
                createdByName = entity.CreatedByName;
//                approvedByName = lstParam.Where(lst => lst.ParameterCode == "PH0004").FirstOrDefault().ParameterValue;
                approvedByName = lstParam.Where(lst => lst.ParameterCode == "IM0009").FirstOrDefault().ParameterValue;
                noSIPA = lstParam.Where(lst => lst.ParameterCode == "PH0005").FirstOrDefault().ParameterValue;
            }
            else
            {
                string filterExpression = string.Format(" ParameterCode = '{0}'", "IM0009");
                List<SettingParameter> lstParam = BusinessLayer.GetSettingParameterList(filterExpression);
                createdByName = appSession.UserName;
                approvedByName = lstParam.Where(lst => lst.ParameterCode == "IM0009").FirstOrDefault().ParameterValue;
            }

            lblCreatedByName.Text = createdByName;
            lblApprovedByName.Text = approvedByName;
            lblSIPANo.Text = noSIPA;


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

            lblDiskon.Text = totalDiskon.ToString("N2");
            lblPPN.Text = ppn.ToString("N2");

            decimal total = entity.TransactionAmount;
            decimal totalPemesanan = (total - totalDiskon) + ppn;
            lblTotalPemesanan.Text = totalPemesanan.ToString("N2");
        }
    }
}
