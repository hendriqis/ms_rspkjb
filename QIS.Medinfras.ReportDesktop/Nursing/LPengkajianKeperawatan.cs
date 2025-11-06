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
    public partial class LPengkajianKeperawatan : BaseCustomDailyPotraitRpt
    {
        public LPengkajianKeperawatan()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            string[] temp = param[0].Split(';');
            lblPeriode.Text = string.Format("Periode : {0} s/d {1}", Helper.YYYYMMDDToDate(temp[0]).ToString(Constant.FormatString.DATE_FORMAT), Helper.YYYYMMDDToDate(temp[1]).ToString(Constant.FormatString.DATE_FORMAT));
            // vConsultVisit2 entity = BusinessLayer.GetvConsultVisit2List(String.Format(param[0].ToString())).FirstOrDefault();
            // lblPatientInformation.Text = String.Format("Registrasi : {0} | {1} ({2})" , entity.RegistrationNo, entity.PatientName, entity.MedicalNo);
            base.InitializeReport(param);
        }

    }
}
