using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Data.Service;
using System.Linq;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class LPermintaanDanRealisasiKasbon : BaseCustomDailyPotraitRpt
    {
        public LPermintaanDanRealisasiKasbon()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            string[] temp = param[0].Split(';');
            lblPeriod.Text = string.Format("Periode : {0} s/d {1}", Helper.YYYYMMDDToDate(temp[0]).ToString(Constant.FormatString.DATE_FORMAT), Helper.YYYYMMDDToDate(temp[1]).ToString(Constant.FormatString.DATE_FORMAT));

            string oParamDate = param[0];

            #region Total
            List<GetPermintaanDanRealisasiKasbonSubReport> lstRealisasi = BusinessLayer.GetPermintaanDanRealisasiKasbonSubReportList(oParamDate);

            subTotal.CanGrow = true;
            mrSubReportTotal.InitializeReport(lstRealisasi);
            #endregion

            base.InitializeReport(param);
        }
    }
}
