using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Web.Common;


namespace QIS.Medinfras.ReportDesktop
{
    public partial class BTransferKasBank : BaseDailyPortraitRpt
    {
        public BTransferKasBank()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            base.InitializeReport(param);
            lblTgl.Text = string.Format("{0}", DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT));
        }

    }
}
