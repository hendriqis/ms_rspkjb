using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Data.Service;
using System.Globalization;
using System.Linq;


namespace QIS.Medinfras.ReportDesktop
{
    public partial class LAntibiotikPerBulan : BaseCustomDailyLandscapeA3Rpt
    {
        public LAntibiotikPerBulan()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            ItemMaster im = BusinessLayer.GetItemMaster(Convert.ToInt32(param[1].ToString()));
            lblPeriod.Text = string.Format("{0} {1}", GetMonthName(Convert.ToInt32(param[1].ToString())), param[0].ToString());
            base.InitializeReport(param);
        }

        public String GetMonthName(int number)
        {
            String result = "";
            if (number == 1)
            {
                result = "Januari";
            }
            else if (number == 2)
            {
                result = "Februari";
            }
            else if (number == 3)
            {
                result = "Maret";
            }
            else if (number == 4)
            {
                result = "April";
            }
            else if (number == 5)
            {
                result = "Mei";
            }
            else if (number == 6)
            {
                result = "Juni";
            }
            else if (number == 7)
            {
                result = "Juli";
            }
            else if (number == 8)
            {
                result = "Agustus";
            }
            else if (number == 9)
            {
                result = "September";
            }
            else if (number == 10)
            {
                result = "Oktober";
            }
            else if (number == 11)
            {
                result = "November";
            }
            else if (number == 12)
            {
                result = "Desember";
            }
            return result;
        }
    }
}
