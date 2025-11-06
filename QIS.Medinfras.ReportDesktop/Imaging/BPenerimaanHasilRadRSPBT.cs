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
    public partial class BPenerimaanHasilRadRSPBT : BaseDailyPortraitRpt
    {
        public BPenerimaanHasilRadRSPBT()
        {
            InitializeComponent();
        }
        public override void InitializeReport(string[] param)
        {
            Healthcare healthcare = BusinessLayer.GetHealthcare(appSession.HealthcareID);


            lblUser.Text = appSession.UserFullName;
            base.InitializeReport(param);
        }
    }
}
