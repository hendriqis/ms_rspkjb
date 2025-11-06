using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using System.Linq;
using System.Data;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class LMacamDiagnosaPenyakitIGD : BaseDailyPortraitRpt
    {
        decimal totSummary = 0;
        int ct = 0; 
        public LMacamDiagnosaPenyakitIGD()
        {
            
            InitializeComponent();
        }
        public override void InitializeReport(string[] param)
        {
            string[] date = param[0].Split(';');
            string startDate = date[0];
            string endDate = date[1];
            string filterParameter = string.Format("DifferentialDate BETWEEN '{0}' AND '{1}' AND DepartmentID = 'EMERGENCY' AND GCRegistrationStatus != 'X121^006'", startDate, endDate);
            List<vPatientDiagnosis> lst = BusinessLayer.GetvPatientDiagnosisList(filterParameter);
            totSummary = lst.Count();
            lblPeriod.Text = string.Format("Periode : {0} s/d {1}", Helper.YYYYMMDDToDate(date[0]).ToString(Constant.FormatString.DATE_FORMAT), Helper.YYYYMMDDToDate(date[1]).ToString(Constant.FormatString.DATE_FORMAT));
            base.InitializeReport(param);
        }

        private void xrTableCell8_SummaryGetResult(object sender, SummaryGetResultEventArgs e)
        {
            if (totSummary == 0)
            {
                totSummary = totSummary + 1;
            }
            e.Result = (ct / totSummary);
            e.Handled = true;
        }

        private void xrTableCell8_SummaryReset(object sender, EventArgs e)
        {
            ct = 0;
        }

        private void xrTableCell8_SummaryRowChanged(object sender, EventArgs e)
        {
            ct++;
        }

        private void Detail_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            ct++;
        }

    }
}
