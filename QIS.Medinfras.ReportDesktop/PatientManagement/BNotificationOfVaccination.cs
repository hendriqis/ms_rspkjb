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
    public partial class BNotificationOfVaccination : BaseCustomDailyPotraitRpt
    {
        public BNotificationOfVaccination()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            #region Header : Patient
            vConsultVisit entityCV = BusinessLayer.GetvConsultVisitList(param[0])[0];
            pictPatient.ImageUrl = entityCV.PatientImageUrl;
            #endregion

            base.InitializeReport(param);
        }

    }
}
