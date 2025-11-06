using System;
using System.Data;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class LNeracaBentukT : BaseDailyPortraitRpt
    {
        public LNeracaBentukT()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            String result = "";
            if (param[2] == "1")
            {
                result = "Januari";
            }
            else if (param[2] == "2")
            {
                result = "Februari";
            }
            else if (param[2] == "3")
            {
                result = "Maret";
            }
            else if (param[2] == "4")
            {
                result = "April";
            }
            else if (param[2] == "5")
            {
                result = "Mei";
            }
            else if (param[2] == "6")
            {
                result = "Juni";
            }
            else if (param[2] == "7")
            {
                result = "Juli";
            }
            else if (param[2] == "8")
            {
                result = "Agustus";
            }
            else if (param[2] == "9")
            {
                result = "September";
            }
            else if (param[2] == "10")
            {
                result = "Oktober";
            }
            else if (param[2] == "11")
            {
                result = "November";
            }
            else if (param[2] == "12")
            {
                result = "Desember";
            }
            else if (param[2] == "13")
            {
                result = "Desember [Audited]";
            }

            Int32 lastDay = 0;
            if (param[2] == "13")
            {
                lastDay = DateTime.DaysInMonth(Convert.ToInt32(param[1]), 12);
            }
            else
            {
                lastDay = DateTime.DaysInMonth(Convert.ToInt32(param[1]), Convert.ToInt32(param[2]));
            }
            lblPeriod.Text = string.Format("Per {0} {1} {2}", lastDay, result, param[1]);

            List<GetGLBalancePerPeriodPerLevel> lstAktiva = BusinessLayer.GetGLBalancePerPeriodPerLevelList(appSession.HealthcareID, Convert.ToInt32(param[1].ToString()), Convert.ToInt32(param[2].ToString()), Convert.ToInt32(param[3].ToString()),1);
            subAktiva.CanGrow = true;
            lNeracaBentukTDetailAktiva1.InitializeReport(lstAktiva);

            List<GetGLBalancePerPeriodPerLevel> lstPassiva = BusinessLayer.GetGLBalancePerPeriodPerLevelList(appSession.HealthcareID, Convert.ToInt32(param[1].ToString()), Convert.ToInt32(param[2].ToString()), Convert.ToInt32(param[3].ToString()), 2);
            subPassiva.CanGrow = true;
            lNeracaBentukTDetailPasiva1.InitializeReport(lstPassiva);

            cGrandTotalAktiva.Text = lstAktiva.Where(a => a.AccountLevel == 1).ToList().Sum(a => a.BalanceEND).ToString(Constant.FormatString.NUMERIC_2);
            cGrandTotalPasiva.Text = lstPassiva.Where(a => a.AccountLevel == 1).ToList().Sum(p => p.BalanceEND).ToString(Constant.FormatString.NUMERIC_2);


            base.InitializeReport(param);
        }     
    }
}
