using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Web.Common;
using System.Text;


namespace QIS.Medinfras.ReportDesktop
{
    public partial class LPasienKeluarRIDiagnoseProcedure : BaseDailyLandscapeRpt
    {
        public LPasienKeluarRIDiagnoseProcedure()
        {
            InitializeComponent();
        }
        public override void InitializeReport(string[] param)
        {
            string[] temp = param[0].Split(';');
            lblPeriod.Text = string.Format("Periode : {0} s/d {1}", Helper.YYYYMMDDToDate(temp[0]).ToString(Constant.FormatString.DATE_FORMAT), Helper.YYYYMMDDToDate(temp[1]).ToString(Constant.FormatString.DATE_FORMAT));
           
            base.InitializeReport(param);
        }

        private void lblDischargeInfo_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            XRTableCell cell = (XRTableCell) sender;

            Boolean isPlanForFollowupVisit = Convert.ToBoolean(this.GetCurrentColumnValue("IsPlanForFollowupVisit"));
            if (isPlanForFollowupVisit)
            {
                StringBuilder displayText = new StringBuilder();
                displayText.AppendLine(this.GetCurrentColumnValue("KondisiKeluar").ToString());
                displayText.Append(string.Format("Rencana Kontrol : {0}", this.GetCurrentColumnValue("cfFollowupVisitDate").ToString()));
                cell.Text = displayText.ToString();
            }
            else
            {
                cell.Text = this.GetCurrentColumnValue("KondisiKeluar").ToString();
            }
        }
    }
}
