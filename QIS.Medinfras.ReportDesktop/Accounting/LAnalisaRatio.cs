using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using System.Data;
using System.Linq;
using System.Globalization;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class LAnalisaRatio : BaseDailyPortraitRpt
    {
        public LAnalisaRatio()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            string bulan = "";
            if (param[1] == "1")
            {
                bulan = "Januari";
            }
            else if (param[1] == "2")
            {
                bulan = "Februari";
            }
            else if (param[1] == "3")
            {
                bulan = "Maret";
            }
            else if (param[1] == "4")
            {
                bulan = "April";
            }
            else if (param[1] == "5")
            {
                bulan = "Mei";
            }
            else if (param[1] == "6")
            {
                bulan = "Juni";
            }
            else if (param[1] == "7")
            {
                bulan = "Juli";
            }
            else if (param[1] == "8")
            {
                bulan = "Agustus";
            }
            else if (param[1] == "9")
            {
                bulan = "September";
            }
            else if (param[1] == "10")
            {
                bulan = "Oktober";
            }
            else if (param[1] == "11")
            {
                bulan = "November";
            }
            else if (param[1] == "12")
            {
                bulan = "Desember";
            }
            else if (param[1] == "13")
            {
                bulan = "Desember [Audited]";
            }

            lblPeriod.Text = string.Format("Periode : {0} {1}", bulan, param[0]);

            base.InitializeReport(param);
        }

        private void xrTableRow2_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            XRTableRow row = (XRTableRow)sender;
            String isHeader = Convert.ToString(GetCurrentColumnValue("ReportDefinitionDTCode"));
            if (isHeader != "")
            {
                row.Font = new Font("Tahoma", 8, FontStyle.Bold);
            }
            else
            {
                row.Font = new Font("Tahoma", 8, FontStyle.Regular);
            }
        }        
    }
}
