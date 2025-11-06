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
    public partial class LPasienIGD : BaseCustomDailyPotraitA3Rpt
    {
        public LPasienIGD()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            StandardCode sc = BusinessLayer.GetStandardCode(param[1]);
            string start = sc.TagProperty.Substring(0, 5);
            string end = sc.TagProperty.Substring(6, 5);
            DateTime tanggal = Helper.YYYYMMDDToDate(param[0]);
            DateTime tanggalBaru = Helper.YYYYMMDDToDate(param[0]).AddDays(1);

            if (param[1] != Constant.Shift.MALAM)
            {
                lblPeriod.Text = string.Format("Periode : {0} ({1} - {2})",
                    Helper.YYYYMMDDToDate(param[0]).ToString(Constant.FormatString.DATE_FORMAT),
                    start,
                    end
                    );
            }
            else
            {
                lblPeriod.Text = string.Format("Periode : {0} {1} s/d {2} {3}",
                    tanggal.ToString(Constant.FormatString.DATE_FORMAT),
                    start,
                    tanggalBaru.ToString(Constant.FormatString.DATE_FORMAT),
                    end
                    );
            }
            base.InitializeReport(param);
        }
    }
}
