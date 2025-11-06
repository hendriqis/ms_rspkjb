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
    public partial class BSuratPermintaanKonsultasi : BaseCustomDailyPotraitRpt
    {
        public BSuratPermintaanKonsultasi()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            ConsultVisit entityCV = BusinessLayer.GetConsultVisitList(param[0])[0];
            ParamedicMaster entityPM = BusinessLayer.GetParamedicMasterList(string.Format("ParamedicID = {0}", entityCV.ParamedicID))[0];

            lblDokter.Text = entityPM.FullName;
            lblTanggalKonsul.Text = "Tanggal Konsul : " + DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT);

            base.InitializeReport(param);
        }

    }
}
