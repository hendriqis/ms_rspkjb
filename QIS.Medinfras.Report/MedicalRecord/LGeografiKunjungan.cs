using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

namespace QIS.Medinfras.Report
{
    public partial class LGeografiKunjungan : BaseDailyPortraitRpt
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

        private void GroupHeader1_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            recordNo = 0;
        }

        private void GroupHeader3_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            recordNo++;
        }

        private void xrTableCell10_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            ((XRLabel)sender).Text = recordNo.ToString();
        }

        private void xrTableCell12_SummaryReset(object sender, EventArgs e)
        {
            maleByDistrict = 0;
        }


        private void xrTableCell12_SummaryGetResult(object sender, SummaryGetResultEventArgs e)
        {
            e.Result = maleByDistrict;
            e.Handled = true;
        }


        private void xrTableCell14_SummaryGetResult(object sender, SummaryGetResultEventArgs e)
        {
            e.Result = femaleByDistrict;
            e.Handled = true;
        }

        private void GroupFooter1_AfterPrint(object sender, EventArgs e)
        {
            lastRegistrationNo = string.Empty;
        }

        private void xrTableCell14_SummaryReset(object sender, EventArgs e)
        {
            femaleByDistrict = 0;
        }

        private void xrTableCell18_SummaryReset(object sender, EventArgs e)
        {
            maleByCity = 0;
        }

        private void xrTableCell18_SummaryGetResult(object sender, SummaryGetResultEventArgs e)
        {
            e.Result = maleByCity;
            e.Handled = true;
        }

        private void xrTableCell19_SummaryReset(object sender, EventArgs e)
        {
            femaleByCity = 0;
        }

        private void xrTableCell19_SummaryGetResult(object sender, SummaryGetResultEventArgs e)
        {
            e.Result = femaleByCity;
            e.Handled = true;
        }

        private void xrTableCell13_SummaryRowChanged(object sender, EventArgs e)
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


    }
}
