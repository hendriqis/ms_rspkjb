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
    public partial class ImagingResultRpt : BaseDailyPortraitRpt
    {
        public ImagingResultRpt()
        {
            InitializeComponent();
        }

        private void lblTestOrderInfo_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (lblTestOrderInfo.Text.Contains(";"))
            {
                lblOrderParamedic.Text = lblTestOrderInfo.Text.Split(';')[3];                
            }
        }

    }
}
