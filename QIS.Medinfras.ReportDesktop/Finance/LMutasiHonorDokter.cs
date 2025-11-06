using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Data.Service;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class LMutasiHonorDokter : BaseCustomDailyPotraitRpt
    {
        public LMutasiHonorDokter()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            string[] temp = param[0].Split(';');
            string paramedicID = param[1].ToString();

            lblPeriod.Text = string.Format("Periode : {0} s/d {1}", Helper.YYYYMMDDToDate(temp[0]).ToString(Constant.FormatString.DATE_FORMAT), Helper.YYYYMMDDToDate(temp[1]).ToString(Constant.FormatString.DATE_FORMAT));

            if (!String.IsNullOrEmpty(paramedicID))
            {
                ParamedicMaster pm = BusinessLayer.GetParamedicMaster(Convert.ToInt32(param[1]));
                xrLabel2.Text = String.Format("Dokter : {0}", pm.FullName);
            }
            else
            {
                xrLabel2.Text = String.Format("Dokter : Semua");
            }
            base.InitializeReport(param);
        }

    }
}
