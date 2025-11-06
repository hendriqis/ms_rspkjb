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
    public partial class LEvaluasiMakanPasien : BaseCustomDailyPotraitRpt
    {
        public LEvaluasiMakanPasien()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            string hsuID = param[1];
            if (hsuID == null || hsuID == "")
            {
                hsuID = "0";
            }

            List<GetEvaluationMealPatient> lst = BusinessLayer.GetEvaluationMealPatientList(param[0], Convert.ToInt32(hsuID), param[2]);
            decimal pembagi = lst.Where(t => t.AverageValue != 0).Count();
            decimal average = 0;
            if (pembagi != 0)
            {
                average = lst.Sum(p => p.AverageValue) / pembagi;
            }

            lblAverage.Text = average.ToString("N2");

            base.InitializeReport(param);
        }
    }
}
