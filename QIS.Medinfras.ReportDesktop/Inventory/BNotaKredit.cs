using System;
using System.Linq;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Data.Service.Helper;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class BNotaKredit : BaseDailyPortraitRpt
    {
        public BNotaKredit()
        {
            InitializeComponent();
        }

        private void lblterbilang_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            Int64 amount = Convert.ToInt64(GetCurrentColumnValue("totalValue"));
            ((XRLabel)sender).Text = Helper.NumberInWords(amount, true);
        }

        public override void InitializeReport(string[] param)
        {
            lblPrintedByName.Text = appSession.UserFullName;

            vSupplierCreditNote entity = BusinessLayer.GetvSupplierCreditNoteList(param[0])[0];
            lblCreatedByName.Text = entity.CreatedByName;

            string filterExpression = string.Format(" ParameterCode IN ('{0}')", "IM0009");
            List<SettingParameterDt> lstParam = BusinessLayer.GetSettingParameterDtList(filterExpression);
            lblApprovedByName.Text = lstParam.Where(lst => lst.ParameterCode == "IM0009").FirstOrDefault().ParameterValue;

            base.InitializeReport(param);
        }
    }
}
