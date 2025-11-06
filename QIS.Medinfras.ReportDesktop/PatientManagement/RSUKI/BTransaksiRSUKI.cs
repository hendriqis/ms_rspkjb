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
    public partial class BTransaksiRSUKI : BaseDailyPortrait2Rpt
    {
        public BTransaksiRSUKI()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            vHealthcare entityHealthcare = BusinessLayer.GetvHealthcareList(String.Format("HealthcareID = {0}", appSession.HealthcareID)).FirstOrDefault();
            vPatientChargesHd entity = BusinessLayer.GetvPatientChargesHdList(string.Format("TransactionID = {0}", param[0])).FirstOrDefault();
            vRegistration entityReg = BusinessLayer.GetvRegistrationList(string.Format("RegistrationID = {0}", entity.RegistrationID)).FirstOrDefault();
            vRegistrationBPJS entityRegB = BusinessLayer.GetvRegistrationBPJSList(string.Format("RegistrationID = {0}", entityReg.RegistrationID)).FirstOrDefault();

            string visitID = AppSession.RegisteredPatient.VisitID.ToString();
            vConsultVisit entityVisit = BusinessLayer.GetvConsultVisitList(string.Format("VisitID = {0}", visitID)).FirstOrDefault();

            cParamedicName.Text = entityVisit.ParamedicName;
            cBusinessPartnerName.Text = entityReg.BusinessPartnerName;
            cTransactionDate.Text = entity.TransactionDateInString;
            cVisitServiceUnitName.Text = string.Format("{0}", entityReg.ServiceUnitName);
            cServiceUnitName.Text = entityVisit.ServiceUnitName;
            cDateOfBirth.Text = string.Format("{0} / {1} Tahun / {2}", entityReg.DateOfBirthInString, entityReg.PatientAgeInYear, entityReg.cfGenderInitial);

            if (entityReg.DepartmentID == Constant.Facility.INPATIENT)
                lblServiceUnit.Text = "RUANG";
            else if (entityReg.DepartmentID == Constant.Facility.OUTPATIENT)
                lblServiceUnit.Text = "KLINIK";
            else if (entityReg.DepartmentID == Constant.Facility.EMERGENCY)
                lblServiceUnit.Text = "UNIT PELAYANAN";
            else
                lblServiceUnit.Text = "PENUNJANG";

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

            lblTTDParamedic.Text = entityVisit.ParamedicName;
            lblLastUpdatedBy.Text = appSession.UserFullName;
            lblLastUpdatedDate.Text = string.Format("{0}, {1} {2}", entityHealthcare.City, entity.cfDateForSignInString, DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT));

            base.InitializeReport(param);
        }
    }
}
