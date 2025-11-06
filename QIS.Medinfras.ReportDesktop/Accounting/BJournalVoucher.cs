using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Data.Service.Helper;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class BJournalVoucher : BaseDailyPortraitRpt
    {
        public BJournalVoucher()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            vGLTransactionHd entity = BusinessLayer.GetvGLTransactionHdList(param[0])[0];
            
            lblCreatedBy.Text = entity.CreatedByName;
            lblLastUpdatedBy.Text = entity.LastUpdatedByName;
            lblRemarks.Text = entity.Remarks;

            vHealthcare entityHealthcare = BusinessLayer.GetvHealthcareList(String.Format("HealthcareID = {0}", appSession.HealthcareID))[0];

            lblLastUpdatedDate.Text = entityHealthcare.City + ", " + entity.JournalDate.ToString(Constant.FormatString.DATE_FORMAT);

            base.InitializeReport(param);
        }
    }
}
