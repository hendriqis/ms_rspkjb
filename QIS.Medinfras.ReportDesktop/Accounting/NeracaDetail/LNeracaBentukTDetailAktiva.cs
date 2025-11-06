using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class LNeracaBentukTDetailAktiva : DevExpress.XtraReports.UI.XtraReport
    {
        List<GetGLBalancePerPeriodPerLevel> lstTemp = new List<GetGLBalancePerPeriodPerLevel>();

        public LNeracaBentukTDetailAktiva()
        {
            InitializeComponent();
        }

        public void InitializeReport(List<GetGLBalancePerPeriodPerLevel> lst)
        {
            this.DataSource = lst;
            lstTemp = lst;

        }

        private void cAccountName_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            XRControl control = (XRControl)sender;
            int level = Convert.ToInt32(GetCurrentColumnValue("Level"));
            control.Padding = new DevExpress.XtraPrinting.PaddingInfo(level * 10, 0, 0, 0);
        }

        private void rowTableAktiva_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
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
