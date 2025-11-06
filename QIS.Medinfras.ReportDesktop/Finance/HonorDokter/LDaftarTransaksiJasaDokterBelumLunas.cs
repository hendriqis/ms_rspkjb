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
    public partial class LDaftarTransaksiJasaDokterBelumLunas : BaseCustomDailyLandscapeRpt
    {
        public LDaftarTransaksiJasaDokterBelumLunas()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            string[] temp = param[0].Split(';');
            string ParamedicID = param[1];
            string Dokter;
            if (ParamedicID == "0" || ParamedicID == "")
            {
                Dokter = "Dokter : SEMUA";
            }
            else
            {
                ParamedicMaster oPM = BusinessLayer.GetParamedicMaster(Convert.ToInt32(temp[1]));
                Dokter = "Dokter : " + oPM.FullName;
            }
            lblPeriod.Text = string.Format("Periode : {0} s/d {1}", Helper.YYYYMMDDToDate(temp[0]).ToString(Constant.FormatString.DATE_FORMAT), Helper.YYYYMMDDToDate(temp[1]).ToString(Constant.FormatString.DATE_FORMAT));
            lblDokter.Text = Dokter;

            base.InitializeReport(param);
        }

    }
}
