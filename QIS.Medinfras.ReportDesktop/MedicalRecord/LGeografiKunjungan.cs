using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Data.Service;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class LGeografiKunjungan : BaseCustomDailyLandscapeRpt
    {
        public LGeografiKunjungan()
        {
            InitializeComponent();
        }

        int recordNo = 0;
        int maleByDistrict;
        int femaleByDistrict;
        int maleByCity;
        int femaleByCity;
        string lastRegistrationNo = string.Empty;

        private void GroupHeader6_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            recordNo++;
        }

        private void xrTableCell32_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            ((XRLabel)sender).Text = recordNo.ToString();
        }

        private void xrTableCell25_SummaryReset(object sender, EventArgs e)
        {
            maleByDistrict = 0;
        }

        private void xrTableCell25_SummaryGetResult(object sender, SummaryGetResultEventArgs e)
        {
            e.Result = maleByDistrict;
            e.Handled = true;
        }

        private void xrTableCell26_SummaryGetResult(object sender, SummaryGetResultEventArgs e)
        {
            e.Result = femaleByDistrict;
            e.Handled = true;
        }

        private void GroupFooter1_AfterPrint(object sender, EventArgs e)
        {
            lastRegistrationNo = string.Empty;
        }

        private void xrTableCell26_SummaryReset(object sender, EventArgs e)
        {
            femaleByDistrict = 0;
        }

        private void xrTableCell19_SummaryRowChanged(object sender, EventArgs e)
        {
            string regNo = GetCurrentColumnValue("RegistrationNo").ToString();
            string gender = GetCurrentColumnValue("Gender").ToString();
            if (lastRegistrationNo != regNo)
            {
                if (gender == "L")
                {
                    maleByDistrict++;
                    maleByCity++;
                }
                else
                {
                    femaleByDistrict++;
                    femaleByCity++;
                }
                lastRegistrationNo = regNo;

            }
        }


        public override void InitializeReport(string[] param)
        {
            string[] temp = param[0].Split(';');
            lblPeriod.Text = string.Format("Periode : {0} s/d {1}", Helper.YYYYMMDDToDate(temp[0]).ToString(Constant.FormatString.DATE_FORMAT), Helper.YYYYMMDDToDate(temp[1]).ToString(Constant.FormatString.DATE_FORMAT));
            base.InitializeReport(param);
        }
    }
}
