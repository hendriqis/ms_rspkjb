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
    public partial class BMCUOrderRealization : BaseCustomDailyPotraitRpt
    {
        public BMCUOrderRealization()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            vConsultVisit cv = BusinessLayer.GetvConsultVisitList(string.Format("RegistrationID = {0}", param[0]))[0];
//            ServiceOrderHd so = BusinessLayer.GetServiceOrderHdList(string.Format("VisitID = {0}", cv.VisitID))[0];

            #region Header : Per Page
            cHeaderPatient.Text = string.Format("{0} | {1} | {2} yr {3} mnth {4} day",
                cv.PatientName, cv.Gender, cv.AgeInYear, cv.AgeInMonth, cv.AgeInDay);
            cHeaderRegistration.Text = cv.RegistrationNo;
            cHeaderMedicalNo.Text = cv.MedicalNo;
            #endregion

            cMedicalNo.Text = cv.MedicalNo;
            cPatientName.Text = cv.PatientName;
            cDOB.Text = string.Format("{0} ( {1} yr {2} mth {3} day )", cv.DateOfBirthInString, cv.AgeInYear, cv.AgeInMonth, cv.AgeInDay);
            cCorporate.Text = cv.BusinessPartnerName;
//            cServiceOrderNo.Text = so.ServiceOrderNo;
//            cServiceOrderDate.Text = so.ServiceOrderDate.ToString(Constant.FormatString.DATE_FORMAT) + " | " + so.ServiceOrderTime;
            cRegistrationNo.Text = cv.RegistrationNo;
            cRegisteredPhysician.Text = cv.ParamedicName;

            #region Footer
            c1.Text = cv.PatientName;
            c4.Text = appSession.UserFullName;
            #endregion

            base.InitializeReport(param);
        }

    }
}
