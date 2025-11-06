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
    public partial class LPerubahanEkuitas : BaseDailyPortraitRpt
    {
        public LPerubahanEkuitas()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            String month = "";

            if (param[1] == "1")
            {
                month = "Januari";
            }
            else if (param[1] == "2")
            {
                month = "Februari";
            }
            else if (param[1] == "3")
            {
                month = "Maret";
            }
            else if (param[1] == "4")
            {
                month = "April";
            }
            else if (param[1] == "5")
            {
                month = "Mei";
            }
            else if (param[1] == "6")
            {
                month = "Juni";
            }
            else if (param[1] == "7")
            {
                month = "Juli";
            }
            else if (param[1] == "8")
            {
                month = "Agustus";
            }
            else if (param[1] == "9")
            {
                month = "September";
            }
            else if (param[1] == "10")
            {
                month = "Oktober";
            }
            else if (param[1] == "11")
            {
                month = "November";
            }
            else if (param[1] == "12")
            {
                month = "Desember";
            }
            else if (param[1] == "13")
            {
                month = "Desember [Audited]";
            }

            lblPeriod.Text = string.Format("Periode : {0} {1}", param[0], month);

            base.InitializeReport(param);
        }

        private void xrTableRow2_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            XRTableRow row = (XRTableRow)sender;
            Int32 isHeader = Convert.ToInt32(GetCurrentColumnValue("isHeader"));
            if (isHeader == 1)
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
