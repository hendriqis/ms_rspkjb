using System;
using System.Linq;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.ReportDesktop;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Data.Service.Helper;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class BPenerimaanBarangKonsinyasi : BaseDailyPortraitRpt
    {
        public BPenerimaanBarangKonsinyasi()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            base.InitializeReport(param);
            vPurchaseReceiveHd entityHd = BusinessLayer.GetvPurchaseReceiveHdList(param[0])[0];
            
            lblPurchaseReceiveNo.Text = entityHd.PurchaseReceiveNo;
            lblPurchaseReceiveDate.Text = entityHd.ReceivedDate.ToString(Constant.FormatString.DATE_FORMAT);
            lblSupplierCode.Text = String.Format("{0} - {1}",entityHd.SupplierCode,entityHd.SupplierName);

            decimal ppn = ((entityHd.TransactionAmount - entityHd.FinalDiscount) * entityHd.VATPercentage / 100);

            lblTotal.Text = entityHd.TransactionAmount.ToString("N");
            lblPPN.Text = ppn.ToString("N");
            lblDiskon.Text = entityHd.FinalDiscount.ToString("N");
            lblChargesType.Text = entityHd.ChargesType;
            lblCharges.Text = entityHd.ChargesAmount.ToString("N");
            Decimal totalPPN = entityHd.TransactionAmount - entityHd.FinalDiscount + ppn + entityHd.ChargesAmount;
            lblTotalPPN.Text = totalPPN.ToString("N");

            lblCreatedByName.Text = entityHd.CreatedByName;

            string filterExpression = string.Format(" ParameterCode IN ('{0}')", "IM0009");
            List<SettingParameterDt> lstParam = BusinessLayer.GetSettingParameterDtList(filterExpression);
            lblApprovedByName.Text = lstParam.Where(lst => lst.ParameterCode == "IM0009").FirstOrDefault().ParameterValue;
        }
    }
}
