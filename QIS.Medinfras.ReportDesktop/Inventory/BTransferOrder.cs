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
    public partial class BTransferOrder : BaseCustomDailyPotraitRpt
    {
        public BTransferOrder()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            vItemDistributionHd entityHD = BusinessLayer.GetvItemDistributionHdList(param[0])[0];
            vLocation entityLocationFrom = BusinessLayer.GetvLocationList(string.Format("LocationID = {0}", entityHD.FromLocationID))[0];
            Healthcare entityHealthcareFrom = BusinessLayer.GetHealthcareList(string.Format("HealthcareID = '{0}'", entityLocationFrom.HealthcareID))[0];
            vLocation entityLocationTo = BusinessLayer.GetvLocationList(string.Format("LocationID = {0}", entityHD.ToLocationID))[0];
            Healthcare entityHealthcareTo = BusinessLayer.GetHealthcareList(string.Format("HealthcareID = '{0}'", entityLocationTo.HealthcareID))[0];

            #region Header
            cSiteFrom.Text = entityHealthcareFrom.HealthcareName;
            cSiteTo.Text = entityHealthcareTo.HealthcareName;
            cDistributionDate.Text = entityHD.DeliveryDateTimeInString;
            cNotes.Text = entityHD.DeliveryRemarks;
            #endregion

            #region Footer
            cTTDReviewedBy.Text = entityHD.LastUpdatedByName;
            cTTDPrintedBy.Text = appSession.UserFullName;
            #endregion

            base.InitializeReport(param);
        }
    }
}
