using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class BRekapRevenueSharing : BaseCustomDailyPotraitRpt
    {
        public BRekapRevenueSharing()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {

            subAdjustmentPlus.CanGrow = true;
            bRekapRevenueSharingAdjPlus.InitializeReport(param[0]);

            subAdjusmentMinus.CanGrow = true;
            bRekapRevenueSharingAdjMinus.InitializeReport(param[0]);

            base.InitializeReport(param);
        }

    }
}
