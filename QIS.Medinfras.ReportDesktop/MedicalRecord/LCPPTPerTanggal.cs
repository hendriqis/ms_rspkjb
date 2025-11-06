using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class LCPPTPerTanggal : BaseCustomDailyLandscapeRpt
    {
        public LCPPTPerTanggal()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            vConsultVisit2 entity = BusinessLayer.GetvConsultVisit2List(String.Format("RegistrationID = {0}", param[0])).FirstOrDefault();
            lblPatientInformation.Text = String.Format("{0} | {1} ({2})", entity.RegistrationNo, entity.PatientName, entity.MedicalNo);

            string[] tempDate = param[1].Split(';');
            lblPeriod.Text = string.Format("{0} s/d {1}", Helper.YYYYMMDDToDate(tempDate[0]).ToString(Constant.FormatString.DATE_FORMAT), Helper.YYYYMMDDToDate(tempDate[1]).ToString(Constant.FormatString.DATE_FORMAT));

            base.InitializeReport(param);
        }

    }
}
