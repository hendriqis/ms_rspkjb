using System;
using System.Linq;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Data.Core;
using QIS.Medinfras.Data;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class BReturPenerimaanPembelianDenganNilai : BaseDailyPortraitRpt
    {
        public BReturPenerimaanPembelianDenganNilai()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {

            vPurchaseReturnHd entityHd = BusinessLayer.GetvPurchaseReturnHdList(param[0]).FirstOrDefault();

            lblTaxInvoiceNo.Text = entityHd.TaxInvoiceNo;
            if (entityHd.TaxInvoiceDate.ToString(Constant.FormatString.DATE_FORMAT) != Constant.ConstantDate.DEFAULT_NULL_DATE_FORMAT)
            {
                lblTaxInvoiceDate.Text = entityHd.TaxInvoiceDate.ToString(Constant.FormatString.DATE_FORMAT);
            }
            lblRemarks.Text = entityHd.Remarks;

            cTransactionAmount.Text = entityHd.TransactionAmount.ToString("N2");
            cPPNAmount.Text = entityHd.VATAmount.ToString("N2");
            cNetAmount.Text = entityHd.GrandTotalTransactionAmount.ToString("N2");

            lblCreatedByName.Text = entityHd.CreatedByName;
            
            txtPPNLabel.Text = string.Format("PPN ({0}%)", entityHd.VATPercentage.ToString("0.##"));

            string filterExpression = string.Format(" ParameterCode IN ('{0}')", "IM0009");
            List<SettingParameterDt> lstParam = BusinessLayer.GetSettingParameterDtList(filterExpression);
            lblApprovedByName.Text = lstParam.Where(lst => lst.ParameterCode == "IM0009").FirstOrDefault().ParameterValue;

            base.InitializeReport(param);
        }

    }

}
