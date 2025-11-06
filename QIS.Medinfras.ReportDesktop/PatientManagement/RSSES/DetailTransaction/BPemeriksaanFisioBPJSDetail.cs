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
    public partial class BPemeriksaanFisioBPJSDetail : BaseCustomDailyPotraitA5Rpt
    {
        public BPemeriksaanFisioBPJSDetail()
        {
            InitializeComponent();
        }
        public void InitializeReport(int AppointmentID, string DateFrom, string DateTo)
        {
            vAppointment entity = BusinessLayer.GetvAppointmentList(string.Format("AppointmentID = {0}", AppointmentID)).FirstOrDefault();
            List<vDraftPatientChargesFisio> lstResultLab = BusinessLayer.GetvDraftPatientChargesFisioList(string.Format("MedicalNo = '{0}' AND StartDate BETWEEN '{1}' AND '{2}' ORDER BY StartDate ASC", entity.MedicalNo, DateFrom, DateTo));
            this.DataSource = lstResultLab;
        }
    }
}
