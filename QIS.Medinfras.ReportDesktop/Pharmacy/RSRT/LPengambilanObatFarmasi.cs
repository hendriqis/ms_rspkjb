using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class LPengambilanObatFarmasi : BaseDailyPortraitRpt
    {
        public LPengambilanObatFarmasi()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            if (param[2] == "")
            {
                Col58.Checked = true;
                xrCheckBox1.Checked = true;
            }
            else
            {
                if (param[2] == "1")
                {
                    Col58.Checked = true;
                    xrCheckBox1.Checked = false;
                }
                else
                {
                    Col58.Checked = false;
                }

                if (param[2] == "2")
                {
                    xrCheckBox1.Checked = true;
                    Col58.Checked = false;
                }
                else
                {
                    xrCheckBox1.Checked = false;
                }
            }


            base.InitializeReport(param);

            lblPeriod.Text = string.Format("Periode   : {0} Sequence-{1} s/d {2} Sequence-{3}",
                Helper.YYYYMMDDToDate(param[3]).ToString(Constant.FormatString.DATE_FORMAT),
                param[4],
                Helper.YYYYMMDDToDate(param[5]).ToString(Constant.FormatString.DATE_FORMAT),
                param[6]
                );
        }

    }
}
