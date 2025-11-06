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
    public partial class BSlipDokterRSSBB : BaseCustomDailyPotraitRpt
    {
        public BSlipDokterRSSBB()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            vTransRevenueSharingHd entity = BusinessLayer.GetvTransRevenueSharingHdList(param[0]).FirstOrDefault();
            cSubTotalRevenue.Text = entity.TotalRevenueSharingAmount.ToString(Constant.FormatString.NUMERIC_2);
            cReductionPercentage.Text = entity.ReductionPercentage.ToString(Constant.FormatString.NUMERIC_2);
            cReductionAmount.Text = entity.ReductionAmount.ToString(Constant.FormatString.NUMERIC_2);
            cTotalRevenue.Text = entity.TotalRevenueSharingReductionAmount.ToString(Constant.FormatString.NUMERIC_2);

            subAdjustmentPlus.CanGrow = true;
            bRekapRevenueSharingAdjPlus.InitializeReport(param[0]);

            subAdjusmentMinus.CanGrow = true;
            bRekapRevenueSharingAdjMinus.InitializeReport(param[0]);

            base.InitializeReport(param);
        }

    }
}
