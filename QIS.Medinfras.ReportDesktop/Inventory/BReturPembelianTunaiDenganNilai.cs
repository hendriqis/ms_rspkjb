using System;
using System.Drawing;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Data;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class BReturPembelianTunaiDenganNilai :BaseDailyPortraitRpt
    {
        public BReturPembelianTunaiDenganNilai()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            
            vDirectPurchaseReturnHd entityHd = BusinessLayer.GetvDirectPurchaseReturnHdList(param[0]).FirstOrDefault();
            Decimal discountAmount = (entityHd.TransactionAmount * entityHd.FinalDiscount / 100);
            Decimal totalTransaction = entityHd.TransactionAmount - discountAmount;
            lblFinalDiscount.Text = discountAmount.ToString("N2");
            lblPPN.Text = (totalTransaction * entityHd.VATPercentage / 100).ToString("N2");
            lblTotal.Text = (totalTransaction + (totalTransaction * entityHd.VATPercentage / 100)).ToString("N");
            
            lblCreatedByName.Text = entityHd.CreatedByName;

            txtPPNLabel.Text = string.Format("PPN ({0}%) : ", entityHd.VATPercentage.ToString("0.##"));

            string filterExpression = string.Format(" ParameterCode IN ('{0}')", "IM0009");
            List<SettingParameterDt> lstParam = BusinessLayer.GetSettingParameterDtList(filterExpression);
            lblApprovedByName.Text = lstParam.Where(lst => lst.ParameterCode == "IM0009").FirstOrDefault().ParameterValue;

            base.InitializeReport(param);
        }

        
    }
}