using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class LPasienPerPenjaminBayar : BaseDailyLandscapeRpt
    {
        public LPasienPerPenjaminBayar()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            string[] temp = param[0].Split(';');
            lblPeriod.Text = string.Format("Periode : {0} s/d {1}", Helper.YYYYMMDDToDate(temp[0]).ToString(Constant.FormatString.DATE_FORMAT), Helper.YYYYMMDDToDate(temp[1]).ToString(Constant.FormatString.DATE_FORMAT));
            BusinessPartners entity = BusinessLayer.GetBusinessPartners(Convert.ToInt32(param[1]));
            lblBusinessPartnerName.Text = entity.BusinessPartnerName;
            base.InitializeReport(param);
        }

        private void lblGroup1SubTotal_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            lblGroup1SubTotal.Text = string.Format("Sub Total {0}",lblDepartmentName.Text);
        }

        private void GroupFooter1_AfterPrint(object sender, EventArgs e)
        {
            if (this.GetNextColumnValue("RegistrationNo") != null)
            {
                this.GroupFooter1.PageBreak = DevExpress.XtraReports.UI.PageBreak.AfterBand;
            }
            else
            {
                this.GroupFooter1.PageBreak = DevExpress.XtraReports.UI.PageBreak.None;
            }
        }
    }
}
