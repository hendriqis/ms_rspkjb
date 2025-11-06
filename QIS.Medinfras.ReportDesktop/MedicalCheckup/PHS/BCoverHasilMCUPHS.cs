using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using System.Linq;


namespace QIS.Medinfras.ReportDesktop
{
    public partial class BCoverHasilMCUPHS : BaseCustomDailyPotraitRptRSRT
    {
        public BCoverHasilMCUPHS()
        {
            InitializeComponent();
        }
        public override void InitializeReport(string[] param)
        {

            xrPictureBox2.ImageUrl = ResolveUrl("~/SystemSetup/Libs/Images/healthcarelogo.png");
            base.InitializeReport(param);
            lblReportTitle.Text = ""; 
        }
    }
}
