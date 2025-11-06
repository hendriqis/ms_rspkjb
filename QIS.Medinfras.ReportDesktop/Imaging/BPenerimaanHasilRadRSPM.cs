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
    public partial class BPenerimaanHasilRadRSPM : BaseDailyPortraitRpt
    {
        public BPenerimaanHasilRadRSPM()
        {
            InitializeComponent();

            UserAttribute ua = BusinessLayer.GetUserAttribute(AppSession.UserLogin.UserID);
            lblUSerFullName.Text = ua.FullName;
        }
    }
}
