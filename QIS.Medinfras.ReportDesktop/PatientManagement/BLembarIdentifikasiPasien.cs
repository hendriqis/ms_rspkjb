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
    public partial class BLembarIdentifikasiPasien : BaseCustomDailyPotraitRpt
    {
        public BLembarIdentifikasiPasien()
        {
            InitializeComponent();
        }
        public override void InitializeReport(string[] param)
        {
            Registration r = BusinessLayer.GetRegistrationList(string.Format("RegistrationID = {0}", param[0])).FirstOrDefault();

            if (r.LinkedRegistrationID != null)
            { 
                vConsultVisit cv = BusinessLayer.GetvConsultVisitList(string.Format("RegistrationID = {0} AND IsMainVisit = 1", r.LinkedRegistrationID)).FirstOrDefault();
                lblRujukan.Text = string.Format("{0} | {1}", cv.DepartmentID, cv.ServiceUnitName);
                lblRefererName.Text = string.Format("{0}", cv.ParamedicName);
                lblReferrerAddress.Text = "-";
                lblReferrerPhone.Text = "-";
            }
            else if (r.ReferrerID != null)
            {
                vReferrer rf = BusinessLayer.GetvReferrerList(string.Format("BusinessPartnerID = {0}", r.ReferrerID)).FirstOrDefault();
                lblRujukan.Text = string.Format("{0}",rf.ReferrerGroup);
                lblRefererName.Text = string.Format("{0}", rf.BusinessPartnerName);
                lblReferrerAddress.Text = string.Format("{0}, {1}, {2}", rf.StreetName, rf.District, rf.County);
                lblReferrerPhone.Text = string.Format("{0}",rf.PhoneNo1);
            }
            else
            {
                lblRujukan.Text = "-";
                lblRefererName.Text = "-";
                lblReferrerAddress.Text = "-";
                lblReferrerPhone.Text = "-";
            }

            lblUser.Text = string.Format("{0}", appSession.UserFullName);
            vHealthcare h = BusinessLayer.GetvHealthcareList(string.Format("HealthcareID = {0}", appSession.HealthcareID)).FirstOrDefault();
            lblTanggalTempat.Text = string.Format("{0}, {1}", h.City, DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT));
            
            base.InitializeReport(param);
        }

    }
}
