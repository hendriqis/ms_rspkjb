using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Data.Service;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class LPelayananPerUnit : BaseDailyPortraitRpt
    {
        public LPelayananPerUnit()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            vHealthcareServiceUnit entity = BusinessLayer.GetvHealthcareServiceUnitList(String.Format("HealthcareServiceUnitID = {0}", param[0]))[0];
            lblUnit.Text = string.Format("Unit : {0}", entity.ServiceUnitName);

            base.InitializeReport(param);
        }

    }
}
