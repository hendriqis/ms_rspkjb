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
    public partial class LHargaSatuanHNANew : BaseCustomDailyPotraitRpt
    {
        public LHargaSatuanHNANew()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            lblParamDate.Text = string.Format("Date : {0}", Helper.YYYYMMDDToDate(param[0]).ToString(Constant.FormatString.DATE_FORMAT));
            base.InitializeReport(param);
        }

    }
}
