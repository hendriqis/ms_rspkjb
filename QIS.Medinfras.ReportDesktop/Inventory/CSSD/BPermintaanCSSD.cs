using System;
using System.Linq;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class BPermintaanCSSD : BaseDailyLandscapeRpt
    {
        public BPermintaanCSSD()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            vMDServiceRequestDistribution entity = BusinessLayer.GetvMDServiceRequestDistributionList(param[0])[0];
            lblRequestNo.Text = entity.RequestNo;
            lblLocationNameFrom.Text = entity.FromLocationName;
            lblPackage.Text = entity.PackageName;
            lblRequestType.Text = entity.ServiceType;
            lblDateTime.Text = entity.cfRequestDateInString;
            lblLocationNameTo.Text = entity.ToLocationName;
            lblRemarks.Text = entity.Remarks;

            lblCreatedByName.Text = entity.CreatedByName;
            
            base.InitializeReport(param);
        }

    }
}
