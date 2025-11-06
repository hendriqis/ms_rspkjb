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
    public partial class BKeteranganDirawatRSSES : BaseCustomDailyPotraitRpt
    {
        public BKeteranganDirawatRSSES()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            Registration entity = BusinessLayer.GetRegistrationList(string.Format("RegistrationID = {0}", param[0])).FirstOrDefault();

            if (entity.MRN != null && entity.MRN != 0)
            {
                vPatient entityP = BusinessLayer.GetvPatientList(string.Format("MRN = {0}", entity.MRN)).FirstOrDefault();

                cPatient.Text = string.Format("({0}) {1}", entityP.MedicalNo, entityP.PatientName);
                cPatientAddress.Text = string.Format("{0} {1} {2} {3} {4} {5}", entityP.StreetName, entityP.County, entityP.District, entityP.City, entityP.State, entityP.ZipCode);
                cPatientTelp.Text = string.Format("{0} / {1}", entityP.PhoneNo1, entityP.MobilePhoneNo1);
                cDOBGender.Text = string.Format("{0} / {1}", entityP.DateOfBirth.ToString(Constant.FormatString.DATE_FORMAT), entityP.Gender);
                cOccupation.Text = string.Format("{0} {1}", entityP.Occupation, entityP.Company);

                if (param[4] != null && param[4] != "")
                {
                    string startDate = Helper.GetDatePickerValue(param[3]).ToString(Constant.FormatString.DATE_FORMAT);
                    string endDate = Helper.GetDatePickerValue(param[4]).ToString(Constant.FormatString.DATE_FORMAT);

                    if (endDate != "" && endDate != Constant.ConstantDate.DEFAULT_NULL_DATE_FORMAT)
                    {
                        cVisitDuration.Text = string.Format("{0} s/d {1}", startDate, endDate);
                    }
                    else
                    {
                        cVisitDuration.Text = string.Format("{0} dan sampai sekarang masih dalam perawatan.", startDate);
                    }
                }
                else
                {
                    cVisitDuration.Text = string.Format("{0} dan sampai sekarang masih dalam perawatan.", entity.RegistrationDate.ToString(Constant.FormatString.DATE_FORMAT));
                }
            }

            vHealthcare healthcare = BusinessLayer.GetvHealthcareList(string.Format("HealthcareID = '{0}'", AppSession.UserLogin.HealthcareID)).FirstOrDefault();

            lblFooterDate.Text = string.Format("{0}, {1}", healthcare.City, DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT));
            lblFooterName.Text = AppSession.UserLogin.UserFullName;
            
            base.InitializeReport(param);
        }
    }
}
