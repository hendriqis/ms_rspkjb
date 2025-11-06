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
    public partial class BJadwalPemeriksaanFisioNonBPJS_RSSES : BaseCustomDailyPotraitA5Rpt
    {

        public BJadwalPemeriksaanFisioNonBPJS_RSSES()
        {
            InitializeComponent();
        }
        public override void InitializeReport(string[] param)
        {
            String appointmentID = param[0];
            String dateFrom = param[1];
            String dateTo = param[2];
            vHealthcare entityHealthcare = BusinessLayer.GetvHealthcareList(String.Format("HealthcareID = '{0}'", appSession.HealthcareID))[0];
            lblHealthcareName.Text = string.Format("{0}", entityHealthcare.HealthcareName);
            lblAddress.Text = string.Format("{0} {1} {2} Telp.{3},{4} Fax.{5}", entityHealthcare.StreetName, entityHealthcare.City, entityHealthcare.ZipCode, entityHealthcare.PhoneNo1, entityHealthcare.PhoneNo2, entityHealthcare.FaxNo1);
            lblContact.Text = string.Format("Email : {0} Webiste : {1}", entityHealthcare.Email, entityHealthcare.Website);
            string filterExpression = string.Format("AppointmentID = '{0}' AND StartDate BETWEEN '{1}' AND '{2}' AND GCCustomerType IN('{3}')", appointmentID, dateFrom, dateTo, param[3]);
            vAppointment entity = BusinessLayer.GetvAppointmentList(filterExpression).FirstOrDefault();
            if (entity != null)
            {
                List<vDraftPatientChargesFisio> lstResultFisio = BusinessLayer.GetvDraftPatientChargesFisioList(string.Format("AppointmentID = '{0}' AND StartDate BETWEEN '{1}' AND '{2}'", appointmentID, dateFrom, dateTo));
                #region Fisio Detail
                subDetail.CanGrow = true;
                bPemeriksaanFisioNonBPJSDetail.InitializeReport(lstResultFisio.FirstOrDefault().AppointmentID, dateFrom, dateTo);
                #endregion
            }
            base.InitializeReport(param);
        }

    }
}
