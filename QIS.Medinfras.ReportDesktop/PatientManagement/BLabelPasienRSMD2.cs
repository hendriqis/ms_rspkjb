using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using DevExpress.XtraReports.UI;
//cetakan ini pakai kertas kecil
namespace QIS.Medinfras.ReportDesktop
{
    public partial class BLabelPasienRSMD2 : BaseRpt
    {
        public BLabelPasienRSMD2()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            int RegistrationID = Convert.ToInt32(param[0]);
            List<vConsultVisit2> entityCV = BusinessLayer.GetvConsultVisit2List(String.Format("RegistrationID = '{0}' AND IsMainVisit = 1", RegistrationID));
            this.DataSource = entityCV;
        }
    }
}
