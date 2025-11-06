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
    public partial class BSuratKeteranganButaWarna : BaseDailyPortraitRpt
    {
        public BSuratKeteranganButaWarna()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            vConsultVisit9 entityVisit = BusinessLayer.GetvConsultVisit9List(param[0]).FirstOrDefault();
            Registration entityReg = BusinessLayer.GetRegistrationList(String.Format("RegistrationID = '{0}'", entityVisit.RegistrationID)).FirstOrDefault();
            vPatient entityPatient = BusinessLayer.GetvPatientList(String.Format("MRN = '{0}'", entityReg.MRN)).FirstOrDefault();
            vHealthcare entityHealthcare = BusinessLayer.GetvHealthcareList(String.Format("HealthcareID = '{0}'", appSession.HealthcareID))[0];
            DateTime dateNow = DateTime.Now;

            lblGender.Text = entityPatient.cfGender;

            lblMataKananTanpaKacamata.Text = param[1];
            lblMataKiriTanpaKacamata.Text = param[2];
            lblMataKananDenganKacamata.Text = param[3];
            lblMataKiriDenganKacamata.Text = param[4];

            String type = param[5];
            if (type == "0")
            {
                lblType.Text = "Tidak";
            }
            else
            {
                lblType.Text = "Ya";
            }

            String type2 = param[6];
            if (type2 == "undefined")
            {
                lblType2.Text = "";
            }
            else if (type2 == "0")
            {
                lblType2.Text = "Total";
            }
            else
            {
                lblType2.Text = "Parsial";
            }
            lblPrintDate.Text = string.Format("{0}, {1}",entityHealthcare.City, dateNow.ToString(Constant.FormatString.DATE_FORMAT));
            lblParamedicName.Text = entityVisit.ParamedicName;
            base.InitializeReport(param);
        }
    }
}
