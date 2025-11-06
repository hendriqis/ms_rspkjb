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
    public partial class LLabaRugiRekap : BaseDailyPortraitRpt
    {
        public LLabaRugiRekap()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
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
