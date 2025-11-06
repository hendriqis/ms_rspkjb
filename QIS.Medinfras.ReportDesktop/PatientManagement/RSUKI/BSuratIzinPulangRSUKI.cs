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
    public partial class BSuratIzinPulangRSUKI : BaseCustomDailyPotraitRpt
    {
        public BSuratIzinPulangRSUKI()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            vRegistration entity = BusinessLayer.GetvRegistrationList(string.Format("{0}", param[0]))[0];
            vConsultVisit entityVisit = BusinessLayer.GetvConsultVisitList(string.Format("RegistrationID = {0}", entity.RegistrationID))[0];
            vPatient entityP = BusinessLayer.GetvPatientList(string.Format("MRN = {0}", entity.MRN))[0];
            vHealthcare oHealthcare = BusinessLayer.GetvHealthcareList(string.Format("HealthcareID = '{0}'", appSession.HealthcareID))[0];

            lblNamaPasien.Text = entityP.PatientName;
            lblNoRM.Text = entityP.MedicalNo;
            lblNoReg.Text = entity.RegistrationNo;

            lblBusinessPartner.Text = entity.BusinessPartnerName;
            if (entityVisit.PhysicianDischargeOrderDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT) != Constant.ConstantDate.DEFAULT_NULL)
            {
                lblDate.Text = string.Format("{0} s/d {1}", entityVisit.ActualVisitDateInString, entityVisit.cfPhysicianDischargedDateOrderInString);
            }
            else
            {
                if (entityVisit.DischargeDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT) != Constant.ConstantDate.DEFAULT_NULL)
                {
                    lblDate.Text = string.Format("{0} s/d {1}", entityVisit.ActualVisitDateInString, entityVisit.DischargeDateInString);
                }
                else
                {
                    lblDate.Text = string.Format("{0} s/d {1}", entityVisit.ActualVisitDateInString, DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT));
                }
            }
            lblServiceUnit.Text = entityVisit.ServiceUnitName;
            lblRoom.Text = entityVisit.RoomName;
            lblPrintDate.Text = string.Format("Jakarta, {0}", DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT));
            lblPenanggungJawab.Text = appSession.UserFullName;

            base.InitializeReport(param);
        }

    }
}
