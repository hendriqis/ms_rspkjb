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
    public partial class BTransaksiMCURSUKI : BaseDailyPortrait2Rpt
    {
        public BTransaksiMCURSUKI()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            vHealthcare entityHealthcare = BusinessLayer.GetvHealthcareList(String.Format("HealthcareID = {0}", appSession.HealthcareID)).FirstOrDefault();
            vConsultVisitItemPackage entityCV = BusinessLayer.GetvConsultVisitItemPackageList(param[0]).FirstOrDefault();
            vRegistration entityReg = BusinessLayer.GetvRegistrationList(string.Format("RegistrationID = {0}", entityCV.RegistrationID)).FirstOrDefault();
            vRegistrationBPJS entityRegB = BusinessLayer.GetvRegistrationBPJSList(string.Format("RegistrationID = {0}", entityReg.RegistrationID)).FirstOrDefault();

            cDateOfBirth.Text = string.Format("{0} / {1} Tahun / {2}", entityReg.DateOfBirthInString, entityReg.PatientAgeInYear, entityReg.cfGenderInitial);
            if (entityRegB != null)
            {
                if (entityRegB.NoPeserta != null || entityRegB.NoPeserta != "")
                {
                    lblNoJaminan.Text = entityRegB.NoPeserta;
                }
                else
                {
                    lblNoJaminan.Text = "";
                }
            }
            else
            {
                lblNoJaminan.Text = "";
            }

            lblTTDParamedic.Text = entityCV.FullName;
            lblLastUpdatedBy.Text = appSession.UserFullName;
            lblLastUpdatedDate.Text = string.Format("{0}, {1} {2}", entityHealthcare.City, entityCV.cfDateForSignInString, DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT));

            base.InitializeReport(param);
        }
    }
}
