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
    public partial class BPenerimaanPembelian_RSDOSKA_OLD : BaseDailyPortraitRpt
    {
        public BPenerimaanPembelian_RSDOSKA_OLD()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            base.InitializeReport(param);
            vPurchaseReceiveHd entityHd = BusinessLayer.GetvPurchaseReceiveHdList(param[0])[0];
            
            //lblPurchaseReceiveNo.Text = entityHd.PurchaseReceiveNo;
            //lblPurchaseReceiveDate.Text = entityHd.ReceivedDate.ToString(Constant.FormatString.DATE_FORMAT);
            lblSupplierCode.Text = String.Format("{0} - {1}",entityHd.SupplierCode,entityHd.SupplierName);


            lblTotal.Text = entityHd.TransactionAmount.ToString(Constant.FormatString.NUMERIC_2);

            decimal ppn = ((entityHd.TransactionAmount - entityHd.FinalDiscount + entityHd.ChargesAmount + entityHd.StampAmount) * entityHd.VATPercentage / 100);
            lblPPN.Text = ppn.ToString(Constant.FormatString.NUMERIC_2);

            lblDiskon.Text = entityHd.FinalDiscount.ToString(Constant.FormatString.NUMERIC_2);

            lblChargesType.Text = entityHd.ChargesType;
            lblCharges.Text = entityHd.ChargesAmount.ToString(Constant.FormatString.NUMERIC_2);

            lblTotalPPN.Text = entityHd.NetTransactionAmount.ToString(Constant.FormatString.NUMERIC_2);

            lblCreatedByName.Text = entityHd.CreatedByName;

            string filterExpression = string.Format(" ParameterCode IN ('{0}')", "IM0009");
            List<SettingParameterDt> lstParam = BusinessLayer.GetSettingParameterDtList(filterExpression);
            lblApprovedByName.Text = lstParam.Where(lst => lst.ParameterCode == "IM0009").FirstOrDefault().ParameterValue;
        }
    }
}
