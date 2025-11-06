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
    public partial class BQCPemeriksaanGRANOSTICJenis : BaseCustomDailyPotraitA5Rpt
    {
        public BQCPemeriksaanGRANOSTICJenis()
        {
            InitializeComponent();
        }
        public void InitializeReport(int RegistrationID)
        {
            vRegistration entity = BusinessLayer.GetvRegistrationList(string.Format("RegistrationID = {0}", RegistrationID)).FirstOrDefault();
            List<vPatientChargesDt> lstResultLab = BusinessLayer.GetvPatientChargesDtList(string.Format("RegistrationID IN ({0})", entity.RegistrationID));
            this.DataSource = lstResultLab;
        }
        
    }
}
