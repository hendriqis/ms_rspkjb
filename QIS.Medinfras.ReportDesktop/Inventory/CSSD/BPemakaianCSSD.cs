using System;
using System.Linq;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Data.Service.Helper;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class BPemakaianCSSD : BaseDailyPortraitRpt
    {
        public BPemakaianCSSD()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            vMDServiceRequestConsumption entity = BusinessLayer.GetvMDServiceRequestConsumptionList(param[0])[0];
            lblTransactionNo.Text = entity.TransactionNo;
            lblTransactionDate.Text = entity.cfTransactionDateInString;
            lblLocation.Text = entity.FromLocationName;
            lblRequestNo.Text = entity.RequestNo;
            lblPackage.Text = entity.PackageName;
            lblConsumptionType.Text = entity.ConsumptionType;
            lblHealthcareUnitName.Text = entity.HealthcareUnitName;
            lblRemarks.Text = entity.RemarksHd;
            base.InitializeReport(param);
        }

    }
}
