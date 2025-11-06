using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;

namespace QIS.Medinfras.Report
{
    public partial class LaporanItemTariff : BaseDailyPortraitRpt
    {
        public LaporanItemTariff()
        {
            InitializeComponent();
        }
        public override void InitializeReport(string[] param)
        {
            List<ClassCare> lstClassCare = BusinessLayer.GetClassCareList("IsDeleted = 0");
            int count = lstClassCare.Count;
            float width = 1000 / count;
            int idx = 0;
            int max = 8;
            XRLabel lbl;

            // fill header column text
            for (idx = 0; idx < count; idx++)
            {
                lbl = (XRLabel)FindControl(string.Format("hdCol{0}", idx + 1), true);
                lbl.Text = lstClassCare[idx].ClassName;
                lbl.WidthF = width * (idx + 1);
                // detail column
                lbl = (XRLabel)FindControl(string.Format("Col{0}", idx + 1), true);
                lbl.WidthF = width * (idx + 1);
                lbl.Text = string.Format("[Tariff{0}]", idx + 1);
            }
            // hide useless column
            for (; idx < max; idx++)
            {
                // header column
                lbl = (XRLabel)FindControl(string.Format("hdCol{0}", idx + 1), true);
                lbl.Text = "";
                lbl.WidthF = 2;
                // detail column
                lbl = (XRLabel)FindControl(string.Format("Col{0}", idx + 1), true);
                lbl.Visible = false;
                lbl.WidthF = 2;
            }

            base.InitializeReport(param);
        }

    }
}
