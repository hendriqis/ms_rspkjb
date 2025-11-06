using System;
using System.Drawing;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class BSuratKeteranganPemeriksaanMata1 : BaseDailyPortrait2Rpt
    {
        public BSuratKeteranganPemeriksaanMata1()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            vConsultVisit9 entityVisit = BusinessLayer.GetvConsultVisit9List(param[0])[0];
            vPatient entityPatient = BusinessLayer.GetvPatientList(string.Format("MRN = {0}" , entityVisit.MRN))[0];
            vHealthcare entityHealthcare = BusinessLayer.GetvHealthcareList(String.Format("HealthcareID = '{0}'", appSession.HealthcareID))[0];
            DateTime dateNow = DateTime.Now;

            lblMataKanan.Text = param[1];
            lblMataKananKoreksi.Text = param[2];
            lblMataKiri.Text = param[3];
            lblMataKiriKoreksi.Text = param[4];

            lblGender.Text = entityPatient.cfGender;

            String type = param[5];
            if (type == "0")
            {
                lblType.Text = "2. Tidak Buta Warna";
            }
            else
            {
                lblType.Text = "2. Buta Warna";
            }
            lblLainlain.Text = param[6];
            lblPrintDate.Text = string.Format("{0}, {1}",entityHealthcare.City, dateNow.ToString(Constant.FormatString.DATE_FORMAT));
            lblParamedicName.Text = entityVisit.ParamedicName;
            base.InitializeReport(param);
        }
    }
}
