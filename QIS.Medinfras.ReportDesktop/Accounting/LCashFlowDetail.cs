using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Linq;
using System.Globalization;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Data.Service;


namespace QIS.Medinfras.ReportDesktop
{
    public partial class LCashFlowDetail : BaseDailyPortraitRpt
    {
        public LCashFlowDetail()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            string[] temp = param[1].Split(';');
            lblPeriod.Text = string.Format("Periode : {0} s/d {1}", Helper.YYYYMMDDToDate(temp[0]).ToString(Constant.FormatString.DATE_FORMAT), Helper.YYYYMMDDToDate(temp[1]).ToString(Constant.FormatString.DATE_FORMAT));
            base.InitializeReport(param);
        }

        private void xrTableRow2_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            XRTableRow row = (XRTableRow)sender;
            bool isHeader = Convert.ToBoolean(GetCurrentColumnValue("IsHeader"));
            if (isHeader)
                row.Font = new Font("Tahoma", 8, FontStyle.Bold);
            else
                row.Font = new Font("Tahoma", 8, FontStyle.Regular);
        }    
    }
}
