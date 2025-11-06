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
    public partial class BSuratKeteranganSehat2 : BaseDailyPortraitRpt
    {
        public BSuratKeteranganSehat2()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            String TBBB = "";
            String Wt = "";
            String Type = "";

            vConsultVisit9 entityVisit = BusinessLayer.GetvConsultVisit9List(param[0])[0];
            vPatient entityPatient = BusinessLayer.GetvPatientList(string.Format("MRN = {0}", entityVisit.MRN))[0];
            vHealthcare entityHealthcare = BusinessLayer.GetvHealthcareList(String.Format("HealthcareID = '{0}'", appSession.HealthcareID))[0];
            vVitalSignDt VitalSignW = BusinessLayer.GetvVitalSignDtList(string.Format("RegistrationID = {0} AND VitalSignLabel = 'WEIGHT' AND IsDeleted = 0", entityVisit.RegistrationID)).FirstOrDefault();
            vVitalSignDt VitalSignH = BusinessLayer.GetvVitalSignDtList(string.Format("RegistrationID = {0} AND VitalSignLabel = 'HEIGHT' AND IsDeleted = 0", entityVisit.RegistrationID)).FirstOrDefault();

            DateTime dateNow = DateTime.Now;
            if (VitalSignH != null)
            {
                TBBB = string.Format("TB {0} cm", VitalSignH.VitalSignValue);
            }
            else
            {
                TBBB =  string.Format("TB - ");
            }
            if (VitalSignW != null)
            {
                Wt = string.Format("BB {0} kg", VitalSignW.VitalSignValue);
            }
            else
            {
                Wt = string.Format("BB - ");
            }

            txtDateOfBirth.Text = string.Format("{0} /{1} tahun      {2}", entityVisit.DateOfBirthInString, entityVisit.AgeInYear, entityVisit.Gender);
            txtPekerjaan.Text = string.Format("{0}", entityPatient.Occupation);
            lblPrintDate.Text = string.Format("{0}, {1}",entityHealthcare.City, dateNow.ToString(Constant.FormatString.DATE_FORMAT));
            lblParamedicName.Text = entityVisit.ParamedicName;
            lblStatement.Text = param[1];
            String type = param[2];
            if (type == "0")
            {
                Type = "Buta Warna : Tidak Buta Warna";
            }
            else 
            {
                Type = "Buta Warna : Buta Warna";
            }
            lblKeterangan.Text = String.Format("{0}   {1}   {2}", TBBB, Wt, Type);
            base.InitializeReport(param);

        }
    }
}
