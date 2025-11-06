using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using System.Linq;


namespace QIS.Medinfras.ReportDesktop
{
    public partial class BSuratKeteranganPendampingPasien : BaseDailyPortrait2Rpt
    {
        public BSuratKeteranganPendampingPasien()
        {
            InitializeComponent();
        }
        public override void InitializeReport(string[] param)
        {
            vConsultVisit entityCV = BusinessLayer.GetvConsultVisitList(param[0]).FirstOrDefault();
            vHealthcare oHealthcare = BusinessLayer.GetvHealthcareList(string.Format("HealthcareID = '{0}'", appSession.HealthcareID))[0];

            string dateNow = "";

            if (entityCV.cfPhysicianDischargedDateOrderInString == "" || entityCV.cfPhysicianDischargedDateOrderInString == null)
            {
                dateNow = DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT);
            }
            else
            {
                dateNow = entityCV.cfPhysicianDischargedDateOrderInString;
            }

            lblKeterangan1.Text = string.Format("Yang bertanda tangan dibawah ini, Dokter {0} menerangkan bahwa : ", oHealthcare.HealthcareName);
            lblKeterangan2.Text = string.Format("Ayah/Ibu/Suami/Istri* yang bersangkutan benar-benar menjalani perawatan di {0} sejak tanggal {1} s/d {2} dan selama perawatan perlu didampingi oleh ................", oHealthcare.HealthcareName, entityCV.ActualVisitDateInString, dateNow);

            lblPrintDate.Text = string.Format("{0}, {1}", oHealthcare.City, DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT));
            lblHealthcareName2.Text = oHealthcare.HealthcareName;

            base.InitializeReport(param);
        }
    }
}
