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
    public partial class BFormPermohonanRawatBersama : BaseCustomDailyPotraitRpt
    {
        public BFormPermohonanRawatBersama()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            vHealthcare oHealthcare = BusinessLayer.GetvHealthcareList(string.Format("HealthcareID = '{0}'", AppSession.UserLogin.HealthcareID)).FirstOrDefault();
            vPatientReferralForm oPatientReferral = BusinessLayer.GetvPatientReferralFormList(string.Format(param[0]))[0];

            lblDateHeader.Text = string.Format("{0}, {1}", oHealthcare.City, oPatientReferral.CreatedDate.ToString(Constant.FormatString.DATE_TIME_FORMAT));

            base.InitializeReport(param);
        }

    }
}
